using EverNext.Domain.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BazaarDemo.BackEnd.Domain.Contracts.DomainServices
{
    [ServiceContract]
    public interface IBazaarService : IDeserveAService
    {
        [OperationContract]
        int TotalProductsInBazaar();
    }
}
