using BazaarDemo.BackEnd.Domain.Entities;
using BazaarDemo.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.OData.Routing;

namespace BazaarDemo.BackEnd.Infrastructure.OData.Controllers
{
    [ODataRoutePrefix("Products")]
    public class ProductsController : ODataMappableController<Product, ProductModel>
    {
    }

    [ODataRoutePrefix("Customers")]
    public class CustomersController : ODataMappableController<Customer, CustomerModel>
    {
    }

    [ODataRoutePrefix("Orders")]
    public class OrdersController : ODataMappableController<Order, OrderModel>
    {
    }

    [ODataRoutePrefix("ProductFamilies")]
    public class ProductFamiliesController : ODataMappableController<ProductFamily, ProductFamilyModel>
    {
    }
}