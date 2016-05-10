using EverNext.Domain.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EverNext.Domain.Model.Attributes;
using EverNext.Domain.Contracts.Model;

namespace BazaarDemo.BackEnd.Domain.Entities
{
    [Table("Products")]
    public class Product : BaseEntity, IAggregateRoot
    {
        public Product()
    	{
    		this.Orders = new List<Order>();
    	}

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("ProductId", Order = 1)]
        public int ProductId { get; set; }

        [Column("ProductName")]
        [Required]
        public string ProductName { get; set; }

        [Column("ProductDescription")]
        public string ProductDescription { get; set; }

        [Column("ProductCost")]
        public decimal? ProductCost { get; set; }

        [ForeignKey("ProductFamily"), Column("ProductFamilyId")]
        public int? ProductFamilyId { get; set; }
       
        [NavigationProperty]
        public virtual ICollection<Order> Orders { get; set; }

        [NavigationProperty]
        public virtual ProductFamily ProductFamily { get; set; }
    }
}
