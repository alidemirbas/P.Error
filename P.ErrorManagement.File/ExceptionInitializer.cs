using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace P.ErrorManagement.File
{
    public class ExceptionInitializer : IExceptionInitializer
    {
        public void Init(string source)
        {
            string json = System.IO.File.ReadAllText(source);

            Exception.Collection = JsonConvert.DeserializeObject<List<Exception>>(json);
        }
    }
}
