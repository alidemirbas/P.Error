using System;
using System.Collections.Generic;
using System.Text;

namespace P.ErrorManagement
{
    public interface IExceptionInitializer
    {
        void Init(string source);
    }
}
