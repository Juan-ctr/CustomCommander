
namespace CustomCommander.Models
{
    public class Exceptions
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public Exceptions(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
