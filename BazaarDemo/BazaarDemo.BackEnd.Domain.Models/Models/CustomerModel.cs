using EverNext.Domain.Model;
using EverNext.Domain.Model.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BazaarDemo.BackEnd.Domain.Models
{
    [DataContract(Name = "CustomerModel")]
    [KnownType(typeof(OrderModel))]
    public class CustomerModel : BaseEntity
    {
        public CustomerModel()
    	{
    		this.Orders = new List<OrderModel>();
    	}

        [DataMember]
        [Key]
        public int CustomerId { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [NavigationProperty]
        [DataMember]
        public virtual ICollection<OrderModel> Orders { get; set; }
    }
}
