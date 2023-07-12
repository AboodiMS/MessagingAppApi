using MessagingAppApi.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MessagingAppApi.Filters
{
    public class MessagesFilter:BaseFilter
    {
        [Required]
        public Guid RoomId { get; set; }
    }
}
