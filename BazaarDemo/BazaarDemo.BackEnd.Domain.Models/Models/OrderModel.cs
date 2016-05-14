using EverNext.Domain.Contracts.Model;
using EverNext.Domain.Model;
using EverNext.Domain.Model.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BazaarDemo.BackEnd.Domain.Models
{
    [DataContract(Name = "OrderModel")]
    [KnownType(typeof(CustomerModel))]
    [KnownType(typeof(ProductModel))]
    public class OrderModel : BaseEntity
    {
        public OrderModel()
        {
            this.Products = new List<ProductModel>();
            this.Customers = new List<CustomerModel>();
        }

        [Key]
        [DataMember]
        public int OrderId { get; set; }

        [NavigationProperty]
        [DataMember]
        public virtual ICollection<CustomerModel> Customers { get; set; }

        [NavigationProperty]
        [DataMember]
        public virtual ICollection<ProductModel> Products { get; set; }
    }
}
