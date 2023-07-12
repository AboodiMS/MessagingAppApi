namespace MessagingAppApi.Dtos.Rooms
{
    public class GetRoomDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
