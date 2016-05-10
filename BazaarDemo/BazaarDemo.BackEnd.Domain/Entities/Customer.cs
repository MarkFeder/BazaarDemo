using EverNext.Domain.Contracts.Model;
using EverNext.Domain.Model;
using EverNext.Domain.Model.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BazaarDemo.BackEnd.Domain.Entities
{
    [Table("Customers")]
    public class Customer : BaseEntity, IAggregateRoot
    {
        public Customer()
    	{
    		this.Orders = new List<Order>();
    	}

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("CustomerId", Order = 1)]
        public int CustomerId { get; set; }

        [Column("CustomerName")]
        [Required]
        public string CustomerName { get; set; }

        [Column("CustomerAddress")]
        public string CustomerAddress { get; set; }

        [Column("CustomerPostCode")]
        public string CustomerPostCode { get; set; }
       
        [NavigationProperty]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
