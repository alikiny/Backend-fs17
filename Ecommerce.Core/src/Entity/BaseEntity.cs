using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.src.Entity
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } //changed from "protected set" to "set": we need to be able to set Id for adding seed data (need to set id before hand)

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public BaseEntity()
        {
            CreatedAt = DateTimeOffset.UtcNow; // Ensure this is in UTC
        }

        public void SetUpdatedAt()
        {
            UpdatedAt = DateTimeOffset.UtcNow; // Ensure this is in UTC
        }
    }
}
