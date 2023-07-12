using MessagingAppApi.Shared.Entities;

namespace MessagingAppApi.Model.Entities
{
    public class RoomMembership:BaseEntity
    {
        public Guid UserId { get; set; } 
        public User User { get; set; }
        public Guid RoomId { get; set; }
        public Room Room { get; set; } 
        public bool Accepted { get; set; } = false;
    }
}
