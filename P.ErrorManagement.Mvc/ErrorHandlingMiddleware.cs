using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace P.ErrorManagement.Mvc
{
    //rule buraya error olsun olmasin gelinir burasi aslinda context'te hem exp firlatildi mi kontrolu
    //hemde 200 olmadi mi kontrolu yapilir
    //mesela hic exp firlatilmayip ApiAuthorizationHandler'da result 401 set edilebilir
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                var url = context.Request.Path;//todo sil

                await _next(context);//eger daha icerdeki middleware'larda veya action'a kadar ulasip da bir hata firlatilirsa catch'e.


                //note 304 icin oku
                //https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/ETag
                //esp_client iframe'de bu sonuc olabiliyor
                //if (context.Response.StatusCode == StatusCodes.Status304NotModified)
                //    return;
                

            }
            catch (System.Exception ex)
            {
                if (context.Response.StatusCode == StatusCodes.Status200OK)//exp firlatilmistir fakat context.result forbid/unauth gibi birseye ayarlanmamissa deyu (orn:authorizationhandler throw'da 403'dur fakat action throw'da henuz 200'dur)
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var error = new Error(context.Response.StatusCode, ex);
                if (!context.Response.HasStarted)
                    ClearCacheHeaders(context.Response);
                context.Items.Clear();
                context.Items["error"] = error;
                context.Request.Path = "/error/error";
                await _next(context);
            }
        }

        private static void ClearCacheHeaders(HttpResponse response)
        {
            response.Headers[HeaderNames.CacheControl] = "no-cache";
            response.Headers[HeaderNames.Pragma] = "no-cache";
            response.Headers[HeaderNames.Expires] = "-1";
            response.Headers.Remove(HeaderNames.ETag);
        }
    }
}
