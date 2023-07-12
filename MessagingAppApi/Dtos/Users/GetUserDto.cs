namespace MessagingAppApi.Dtos.Users
{
    public class GetUserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
