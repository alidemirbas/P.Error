using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace P.ErrorManagement.Mvc
{
    //imp eden project'in ErrorController:ErrorController yapmasi zorunlu olsun
    //cunku bu attribute'leri orada koymak daha mantikli (mvc/api)
    //[Route("[controller]/[action]")]
    //[ApiController]
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public virtual ActionResult Error()//Error error)
        {
            //note this.ViewData.Model;//goruldugu gibi sayfaya model illa View(model); olarak verilmesi gerekmiyor.

            var api = Attribute.IsDefined(this.GetType(), typeof(ApiControllerAttribute));
            //todo ErrorHandlingMiddleware'dan anca boyle oldu (action prm olarak gecemedim)
            var err = (Error)HttpContext.Items["error"];
            //404 gibilerde request body null'dir. err.exception yoktur zaten bir exp degil err dir

            if (api || Request.IsAjaxRequest())
            {
                //string json = "{}";//= string.Empty;
                //if (err.Exception != null)
                //{
                    //json = JsonConvert.SerializeObject(error.Exception, new JsonSerializerSettings
                    //{
                    //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    //});
                    //json = JsonConvert.SerializeObject(error.Exception);
                ///}

                //return Content(json, "application/json");
                //return Json(error.Exception);
                // circular reference loop exception oluyor
                //du ama artik [DataContract attr ile sadece lazim olan prop'lar serilize ediliyor]


                return StatusCode(err.Status.Code, err.Exception);//response content tpye url encoded de olabilir, object result halledr

            }

            return View(err);
        }
    }

    public static class HttpRequestBaseExtension
    {
        /// <summary>
        /// Determines whether the specified HTTP request is an AJAX request.
        /// </summary>
        /// 
        /// <returns>
        /// true if the specified HTTP request is an AJAX request; otherwise, false.
        /// </returns>
        /// <param name="request">The HTTP request.</param><exception cref="T:System.ArgumentNullException">The <paramref name="request"/> parameter is null (Nothing in Visual Basic).</exception>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
    }


}
