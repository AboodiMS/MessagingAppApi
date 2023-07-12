using System.ComponentModel.DataAnnotations;

namespace MessagingAppApi.Dtos.RoomMemberships
{
    public class InvitationRoomMembershipDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid RoomId { get; set; }
    }
}
