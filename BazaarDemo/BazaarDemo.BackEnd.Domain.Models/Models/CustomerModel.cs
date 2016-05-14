using EverNext.Domain.Contracts.Model;
using EverNext.Domain.Model;
using EverNext.Domain.Model.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BazaarDemo.BackEnd.Domain.Models
{
    [DataContract(Name = "CustomerModel")]
    [KnownType(typeof(OrderModel))]
    public class CustomerModel : BaseEntity, IModelAggregateRoot
    {
        public CustomerModel()
    	{
    		this.Orders = new List<OrderModel>();
    	}

        [Key]
        [DataMember]
        public int CustomerId { get; set; }

        [Required]
        [DataMember]
        public string CustomerName { get; set; }

        [NavigationProperty]
        [DataMember]
        public virtual ICollection<OrderModel> Orders { get; set; }
    }
}
