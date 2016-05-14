using System.Collections.Generic;
using System.ServiceModel;
using EverNext.Domain.Contracts.Model;
using EverNext.Domain.Contracts.Services;
using System.Linq.Expressions;
using System;
using EverNext.Domain.Contracts.Repositories;

namespace BazaarDemo.BackEnd.Domain.Contracts.MappableEntityRepository
{
    [ServiceContract]
    public interface IMappableEntityRepository<T,M> : IEntityRepository<T>
        where T : class, IBaseEntity
        where M : class, IBaseEntity
    {
        IObjectMapper Mapper { get; set; }

        [OperationContract]
        IEnumerable<M> BatchModels(IEnumerable<M> saveorupdate, IEnumerable<M> delete);

        [OperationContract]
        IEnumerable<M> SaveOrUpdateModels(IEnumerable<M> modelList);

        [OperationContract]
        M SaveOrUpdateModel(M model);

        [OperationContract]
        void DeleteModel(M model);

        [OperationContract]
        void DeleteModels(IEnumerable<M> modelList);

        [OperationContract]
        M FindOneModel(Expression<Func<T, bool>> expression);

        [OperationContract]
        IEnumerable<M> FindAllModels(out int totalRows, int take = 0, int skip = 0, IEnumerable<EverNext.Domain.Model.Common.ServerSortDescriptor> sorts = null);
    }
}