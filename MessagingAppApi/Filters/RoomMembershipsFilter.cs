using MessagingAppApi.Shared.Filters;
using System.ComponentModel.DataAnnotations;

namespace MessagingAppApi.Filters
{
    public class RoomMembershipsFilter:BaseFilter
    {
        [Required]
        public Guid RoomId { get; set; }
        public bool? Accepted { get; set; }
    }
}
