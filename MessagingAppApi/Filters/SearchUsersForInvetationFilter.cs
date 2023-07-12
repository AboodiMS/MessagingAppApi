using System.ComponentModel.DataAnnotations;

namespace MessagingAppApi.Filters
{
    public class SearchUsersForInvetationFilter
    {
        public string? Username { get; set; }
        [Required]
        public Guid RoomId { get; set; }
    }
}
