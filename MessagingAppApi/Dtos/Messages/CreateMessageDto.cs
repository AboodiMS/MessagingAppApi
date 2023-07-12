using System.ComponentModel.DataAnnotations;

namespace MessagingAppApi.Dtos.Messages
{
    public class CreateMessageDto
    {
        [Required]
        public string Text { get; set; } = string.Empty;
        [Required]
        public Guid RoomId { get; set; }
    }
}
