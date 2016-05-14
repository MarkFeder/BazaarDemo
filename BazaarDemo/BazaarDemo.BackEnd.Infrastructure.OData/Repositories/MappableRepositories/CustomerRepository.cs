﻿using BazaarDemo.BackEnd.Domain.Entities;
using BazaarDemo.BackEnd.Domain.Models;
using BazaarDemo.BackEnd.Infrastructure.OData.Repositories;
using EverNext.Domain.Contracts.Services;

namespace EverNext.Infrastructure.WebApi.OData.Tests.CodeFirst.Repositories.MappableRepositories
{
    public class CustomerRepository : MappableEntityRepository<Customer, CustomerModel>
    {
        public CustomerRepository(System.Data.Entity.DbContext entities, IObjectMapper mapper) : base(entities, mapper)
        {

        }
    }
}
