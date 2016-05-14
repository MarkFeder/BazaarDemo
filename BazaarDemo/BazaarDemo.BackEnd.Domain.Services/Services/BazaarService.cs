using BazaarDemo.BackEnd.Domain.Contracts.DomainServices;
using BazaarDemo.BackEnd.Domain.Contracts.UnitOfWork.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BazaarDemo.BackEnd.Domain.Models;
using BazaarDemo.BackEnd.Domain.Entities;

namespace BazaarDemo.BackEnd.Domain.Services.Services
{
    public class BazaarService : IBazaarService
    {
        public IUnitOfWork UnitOfWork { get; set; }

        // Implement domain service
        public int TotalProductsInBazaar()
        {
            return UnitOfWork.ProductRepository.Count();
        }
    }
}
