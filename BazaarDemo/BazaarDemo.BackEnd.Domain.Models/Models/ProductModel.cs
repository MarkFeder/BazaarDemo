using EverNext.Domain.Contracts.Model;
using EverNext.Domain.Model;
using EverNext.Domain.Model.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BazaarDemo.BackEnd.Domain.Models
{
    [DataContract(Name = "ProductModel")]
    [KnownType(typeof(OrderModel))]
    [KnownType(typeof(ProductFamilyModel))]
    public class ProductModel : BaseEntity
    {
        public ProductModel()
    	{
    		this.Orders = new Collection<OrderModel>();
    	}

        [Key]
        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public string ProductDescription { get; set; }

        [DataMember]
        public decimal? ProductCost { get; set; }

        [ForeignKey("ProductFamily"), Column("ProductFamilyId")]
        [DataMember]
        public int? ProductFamilyId { get; set; }

        [NavigationProperty]
        [DataMember]
        public virtual ICollection<OrderModel> Orders { get; set; }

        [NavigationProperty]
        [DataMember]
        public virtual ProductFamilyModel ProductFamily { get; set; }
    }
}
