namespace MessagingAppApi.Dtos.Accounts
{
    public class GetAccountDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
