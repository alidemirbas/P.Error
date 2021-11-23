
namespace P.ErrorManagement
{
    public class Error : IError
    {
        public Error()
        {

        }

        public Error(ProtocolStatus status,System.Exception exception)
        {
            Status = status;
            Exception = exception;
        }

        public Error(int protocolStatusCode, System.Exception exception)
            :this(ProtocolStatus.Instance[protocolStatusCode],exception)
        {

        }

        public Error(int protocolStatusCode)
            :this(ProtocolStatus.Instance[protocolStatusCode],null)
        {

        }


        public ProtocolStatus Status { get; set; }
        public System.Exception Exception { get; set; }
    }
}
