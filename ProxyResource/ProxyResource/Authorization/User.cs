using System.ComponentModel.DataAnnotations;

namespace ProxyResource.Authorization
{
    public class User
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        public User(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public User()
        {
        }
    }
}
