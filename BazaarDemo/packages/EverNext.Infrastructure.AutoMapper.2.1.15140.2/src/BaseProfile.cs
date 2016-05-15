using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AM = AutoMapper;
using EverNext.Domain.Contracts.Model;

namespace EverNext.Infrastructure.AutoMapper
{
    /// <summary>
    /// eNET's default AutoMapper profile
    /// </summary>
    /// <typeparam name="TModel">Model type</typeparam>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="K">Primary key type</typeparam>
    public class BaseProfile<TModel, TEntity, K> : MapperProfile<TModel, TEntity, K>
        where TModel : class, IBaseEntityModel<K>
        where TEntity : class, IBaseEntity<K>
    {
        protected override void Configure()
        {
            //base.Configure();
            MapEntityToModel();
            MapModelToEntity();
        }

        /// <summary>
        /// Default configuration for mapping from entity to model
        /// </summary>
        /// <returns></returns>
        protected virtual AM.IMappingExpression<TEntity, TModel> MapEntityToModel()
        {
            return AM.Mapper.CreateMap<TEntity, TModel>();
        }

        /// <summary>
        /// Default configuration for mapping from model to entity
        /// </summary>
        /// <returns></returns>
        /// <remarks>Add the repository support for loading the models using the repository</remarks>
        protected virtual AM.IMappingExpression<TModel, TEntity> MapModelToEntity()
        {
            return AM.Mapper.CreateMap<TModel, TEntity>()
                                 .ConstructUsing(x =>
                                 {
                                     var entity = Repository.FindOne(((TModel)x.SourceValue).Id) ?? (TEntity)Activator.CreateInstance(x.DestinationType);
                                     return entity;
                                 });
        }
    }
}
