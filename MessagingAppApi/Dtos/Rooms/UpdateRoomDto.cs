using System.ComponentModel.DataAnnotations;

namespace MessagingAppApi.Dtos.Rooms
{
    public class UpdateRoomDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
