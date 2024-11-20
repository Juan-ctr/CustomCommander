
namespace CustomCommander.Models
{
    public class MyConnectionInfo
    {
        public string HostName { get; set; }
        public string Description { get; set; }

        public MyConnectionInfo(string hostname, string description)
        {
            HostName = hostname;
            Description = description;
        }
    }
}
