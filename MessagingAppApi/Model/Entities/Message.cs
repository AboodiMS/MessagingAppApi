using MessagingAppApi.Shared.Entities;

namespace MessagingAppApi.Model.Entities
{
    public class Message:BaseEntity
    {
        public string Text { get; set; } = string.Empty;
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
    }
}
