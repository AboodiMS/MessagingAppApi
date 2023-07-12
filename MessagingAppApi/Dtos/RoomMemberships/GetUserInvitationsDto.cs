namespace MessagingAppApi.Dtos.RoomMemberships
{
    public class GetUserInvitationsDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string RoomName { get; set; }
    }
}
