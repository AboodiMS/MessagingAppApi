namespace MessagingAppApi.Dtos.Messages
{
    public class GetMessageDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public Guid CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid RoomId { get; set; }
    }
}
