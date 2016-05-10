using EverNext.Domain.Contracts.Model;
using EverNext.Domain.Model;
using EverNext.Domain.Model.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BazaarDemo.BackEnd.Domain.Entities
{
    [Table("ProductFamily")]
    public class ProductFamily : BaseEntity, IAggregateRoot
    {
        public ProductFamily()
        {
            this.Products = new List<Product>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("ProductFamilyId", Order = 1)]
        public int ProductFamilyId { get; set; }

        [Column("ProductFamilyName")]
        [Required]
        public string ProductFamilyName { get; set; }

        [Column("ProductFamilyDescription")]
        public string ProductFamilyDescription { get; set; }

        [NavigationProperty]
        public virtual ICollection<Product> Products { get; set; }
    }
}
