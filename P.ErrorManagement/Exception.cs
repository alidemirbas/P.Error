
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace P.ErrorManagement
{
    [DataContract]//note bunun sayesinde sadece [DataMember] olanlar serilize edilir
    //[JsonObject(MemberSerialization.OptIn)]
    public class Exception : System.Exception//abstract olmak zorunda Init() icin
    {
        private const string D_CRLF = "\r\n\r\n";

        //note [Newtonsoft.Json.JsonConstructor]//baska ctr olursa lazim
        public Exception(string name, int code, string message)
        //:base(message)
        {
            _name = name;
            _code = code;
            _message = message;
        }

        protected string _name;
        [DataMember]
        public virtual string Name => _name;

        protected int _code;
        [DataMember]
        public int Code => _code;

        protected string _message;
        [DataMember]
        public override string Message { get { return _message; } }//attn override

        public static IReadOnlyCollection<Exception> Collection { get; set; }

        public static System.Exception NotInitializedException => new System.Exception("P.ErrorManagement.Exception is not initialized.");

        //rule P.Portal'da ApiClient cagrilip ApiClient'in error'una dusuldugu zaman P.Portalda o apinin error'unu client'a gondermek sacma olur
        //mesela api'den 400 geldi bunu p.portal uzerinden client'a yansittiginda p.portal'in 400'uymus gibi gozukecek halbuki api'de 400 olursa bu portal'dan client'a 500 olarak yansitilmali
        //ama yine de asil hatanin api'den geldigi de bilinmeli
        //bu yuzden bu prop var
        //P.Portal'da api error'a dusuldugunde portal'in kendisi yeni bir exp firlatmali ve innererror'a da api error'unu atamali. sonrasinda zaten portal exp'de 500 error'u ile sarmalanip client'a gonderilir
        [DataMember]
        public Error InnerError { get; set; }

        public static Exception Get(string exceptionName)
        {
            if (Collection == null || Collection.Count == 0)
                throw NotInitializedException;

            return Collection.Single(x => x.Name == exceptionName).Clone();//attn clone'daki nota bak
        }

        public void MessageFormat(params string[] values)
        {
            _message = string.Format(_message, values);
        }

        public void AddMessages(params string[] values)
        {
            _message = $"{_message}{D_CRLF}{string.Join(D_CRLF, values)}";
        }

        protected Exception Clone()//attn olmazsa ExceptionFactory.Collection'da mesaj degisir with calling MessageFormat method
        {
            var clone = this.MemberwiseClone() as Exception;//shallow diiler
            //clone._message = string.Copy(this._message);//gerek yok string de value type olmasa da ozel old icin kopyalaniyor

            return clone;
        }
    }
}

