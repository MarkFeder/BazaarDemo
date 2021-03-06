
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;

using BazaarDemo.BackEnd.Domain.Entities;
using BazaarDemo.BackEnd.Domain.Contracts.Specifications;
using System.Diagnostics.CodeAnalysis;
using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using EverNext.Domain.Model;
using System.Collections.Generic;


namespace BazaarDemo.BackEnd.Domain.Contracts.Specifications
{

[Serializable]
[DataContract(Name = "OrderSpecification", Namespace = "http://schemas.datacontract.org/2004/07/EverNext.Domain.Model.Specifications")]
[ExcludeFromCodeCoverage]
[GeneratedCode("SpecificationImplementationCodeGen","1.0")]
public partial class OrderSpecification : Specification<BazaarDemo.BackEnd.Domain.Entities.Order>
{
		[DataMember]
		public bool? Negate
		{
			get;
			set;
		}

		[DataMember]
		public OrderSpecification And
		{
			get;
			set;
		}

		[DataMember]
		public OrderSpecification Or
		{
			get;
			set;
		}

	[DataMember]
	public Nullable<int> OrderId
	{
		get; 
		set;
	}

	[DataMember]
	public ICollection<Nullable<int>> OrderIdList
	{
		get;
		set;
	}

	[DataMember]
	public Nullable<bool> RecurringOrder
	{
		get; 
		set;
	}

	[DataMember]
	public ICollection<Nullable<bool>> RecurringOrderList
	{
		get;
		set;
	}


    #region Navigation Properties
    

	[DataMember]
	public CustomerSpecification Customers
	{
		get;
		set;
	}


	[DataMember]
	public ProductSpecification Products
	{
		get;
		set;
	}

        #endregion

    

	/// <summary>
	/// Default constructor (needed for serialization)
	/// Initializes a new instance of the <see cref="OrderSpecification"/> class.
	/// </summary>
	public OrderSpecification()
	{

	}

	

	/// <summary>
	/// Initializes a new instance of the <see cref="OrderSpecification"/> class.
	/// </summary>
	/// <param name="initializeNavigationProperties">if set to <c>true</c> initialize navigation properties.</param>
	public OrderSpecification(bool initializeNavigationProperties)
	{
		if(!initializeNavigationProperties)
			return;


		this.Customers = new CustomerSpecification();

		this.Products = new ProductSpecification();

	}


    #region ISpecification Members
    

	public override Expression<Func<BazaarDemo.BackEnd.Domain.Entities.Order, bool>> GetExpression()
	{
		Expression<Func<BazaarDemo.BackEnd.Domain.Entities.Order, bool>> expression = x => true;


		if(OrderIdList!=null&&OrderIdList.Count > 0)
			expression = expression.And(x => OrderIdList.Contains(x.OrderId));



		if(OrderId.HasValue)
			expression = expression.And(x => x.OrderId == OrderId.Value);


		if(RecurringOrderList!=null&&RecurringOrderList.Count > 0)
			expression = expression.And(x => RecurringOrderList.Contains(x.RecurringOrder));



		if(RecurringOrder.HasValue)
			expression = expression.And(x => x.RecurringOrder == RecurringOrder.Value);
		
		//
		// Navigation properties
		//


		if(this.Customers != null)
		{

			var subExpression = Customers.GetExpression();
			expression = expression.And(x => x.Customers.AsQueryable().Any(subExpression));

		}


		if(this.Products != null)
		{

			var subExpression = Products.GetExpression();
			expression = expression.And(x => x.Products.AsQueryable().Any(subExpression));

		}
	
		if (Negate != null && Negate.Value)
		{
			expression = Expression.Lambda<Func<BazaarDemo.BackEnd.Domain.Entities.Order,bool>>(Expression.Not(expression.Body), expression.Parameters);
		}
			
		if(And != null)
		{
			expression = expression.And(And.GetExpression());
		}

		if (Or != null)
		{
			expression = expression.Or(Or.GetExpression());
		}

		return expression;
	}


	public override bool IsSatisfiedBy(BazaarDemo.BackEnd.Domain.Entities.Order entity)
	{
		// convert single entity to a IQueryable object, 
		// in order to be able to use lambda expressions
		IQueryable<BazaarDemo.BackEnd.Domain.Entities.Order> entities = (new[] { entity }).AsQueryable();
		
		return entities.Any(this.GetExpression());
	}
	
        #endregion

    
}

}
