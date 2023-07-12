using MessagingAppApi.Shared.Entities;

namespace MessagingAppApi.Model.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<RoomMembership> RoomMemberships { get; set; }
    }
}
