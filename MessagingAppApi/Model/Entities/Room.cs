using MessagingAppApi.Shared.Entities;

namespace MessagingAppApi.Model.Entities
{
    public class Room:BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public List<RoomMembership> RoomMemberships { get; set; }
        public List<Message> Messages { get; set; }
    }
}
