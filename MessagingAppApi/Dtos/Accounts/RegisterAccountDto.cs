using System.ComponentModel.DataAnnotations;

namespace MessagingAppApi.Dtos.Accounts
{
    public class RegisterAccountDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
