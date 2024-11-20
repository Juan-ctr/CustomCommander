
namespace CustomCommander.Models
{
    public class MySession : MyConnectionInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CurrentDir { get; set; }

        public MySession(string hostname, string description, string username, string password, string currentdir)
            : base(hostname, description)
        {
            UserName = username;
            Password = password;
            CurrentDir = currentdir;
        }
    }
}
