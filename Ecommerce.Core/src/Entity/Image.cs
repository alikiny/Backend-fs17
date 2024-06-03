
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.src.Entity
{
    [Table("images")]
    public class Image : BaseEntity
    {
        public Guid? ProductId { get; set; }
        public string Url { get; set; }

        // Navigation property to represent the product associated with the image
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        // Add a parameterless constructor for Entity Framework
        public Image() { }

        // Constructor with parameters
        public Image(Guid? productId, string url)
        {
            ProductId = productId;
            Url = url;
        }
    }
}
