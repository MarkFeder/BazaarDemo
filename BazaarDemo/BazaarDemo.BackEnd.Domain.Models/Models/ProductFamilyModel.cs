using EverNext.Domain.Contracts.Model;
using EverNext.Domain.Model;
using EverNext.Domain.Model.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BazaarDemo.BackEnd.Domain.Models
{
    [DataContract(Name = "ProductFamilyModel")]
    [KnownType(typeof(ProductModel))]
    public class ProductFamilyModel : BaseEntity, IModelAggregateRoot
    {
        public ProductFamilyModel()
        {
            this.Products = new List<ProductModel>();
        }

        [Key]
        [DataMember]
        public int ProductFamilyId { get; set; }

        [Required]
        [DataMember]
        public string ProductFamilyName { get; set; }

        [NavigationProperty]
        [DataMember]
        public virtual ICollection<ProductModel> Products { get; set; }
    }
}
