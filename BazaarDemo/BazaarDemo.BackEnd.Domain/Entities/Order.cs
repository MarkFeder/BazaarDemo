using EverNext.Domain.Contracts.Model;
using EverNext.Domain.Model;
using EverNext.Domain.Model.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BazaarDemo.BackEnd.Domain.Entities
{
    [Table("Orders")]
    public class Order : BaseEntity
    {
        public Order()
    	{
    		this.Products = new List<Product>();
            this.Customers = new List<Customer>();
    	}

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("OrderId", Order = 1)]
        public int OrderId { get; set; }

        [Column("RecurringOrder")]
        public bool RecurringOrder { get; set; }
        
        [NavigationProperty]
        public virtual ICollection<Customer> Customers { get; set; }

        [NavigationProperty]
        public virtual ICollection<Product> Products { get; set; }
    }
}
