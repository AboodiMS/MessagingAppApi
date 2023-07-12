using System.ComponentModel.DataAnnotations;

namespace MessagingAppApi.Dtos.Messages
{
    public class UpdateMessageDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Text { get; set; } = string.Empty;
    }
}
