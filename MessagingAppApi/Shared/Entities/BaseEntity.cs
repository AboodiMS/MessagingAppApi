using System.ComponentModel.DataAnnotations;

namespace MessagingAppApi.Shared.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTimeOffset? UpdateAt { get; set;}
        public Guid? UpdatedBy { get; set; }
        public bool Deleted { get; set; } = false;
        public DateTimeOffset? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }

        public void SetCreateInfo(Guid userid)
        {
            CreatedBy = userid;
            CreatedAt = DateTimeOffset.UtcNow;
            Deleted = false;
        }
        public void SetUpdateInfo(Guid userid)
        {
            UpdatedBy = userid;
            UpdateAt = DateTimeOffset.UtcNow;
        }
        public void SetDeleteInfo(Guid userid)
        {
            DeletedBy = userid;
            DeletedAt = DateTimeOffset.UtcNow;
            Deleted = true;
        }

    }
}
