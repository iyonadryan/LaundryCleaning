namespace LaundryCleaning.Common.Models.Messages
{
    public class UserCreated
    {
        public string Username { get; set; }

        public UserCreated(string username)
        {
            Username = username;
        }
    }
}
