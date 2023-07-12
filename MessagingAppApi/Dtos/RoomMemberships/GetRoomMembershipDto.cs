namespace MessagingAppApi.Dtos.RoomMemberships
{
    public class GetRoomMembershipDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
    }
}
