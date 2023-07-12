using System.ComponentModel.DataAnnotations;

namespace MessagingAppApi.Dtos.Rooms
{
    public class CreateRoomDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
