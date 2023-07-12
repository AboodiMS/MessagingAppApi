namespace MessagingAppApi.Shared.Filters
{
    public class BaseFilter
    {
        public DateTimeOffset? FromCreatedDate { get; set; }
        public DateTimeOffset? ToCreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public int? Skip { get; set; } = 0;
        public int? Take { get; set; } = 10;
}
}
