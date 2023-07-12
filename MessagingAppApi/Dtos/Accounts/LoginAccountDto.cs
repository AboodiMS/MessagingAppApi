using System.ComponentModel.DataAnnotations;

namespace MessagingAppApi.Dtos.Accounts
{
    public class LoginAccountDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
