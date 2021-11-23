
namespace P.ErrorManagement
{
    public interface IError
    {
        ProtocolStatus Status { get; set; }
        System.Exception Exception { get; set; }
    }
}
