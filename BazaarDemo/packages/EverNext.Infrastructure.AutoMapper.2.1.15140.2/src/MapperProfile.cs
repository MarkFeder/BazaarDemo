using System;
using EverNext.Domain.Contracts.Model;
using EverNext.Domain.Contracts.Repositories;
using AM = AutoMapper;

namespace EverNext.Infrastructure.AutoMapper
{
    public class MapperProfile<TModel, TEntity, K> : AM.Profile, IMapperProfile<TEntity, K>
        where TModel : class, IBaseEntityModel<K>
        where TEntity : class, IBaseEntity<K>
    {

        public IRepository<TEntity, K> Repository { get; set; }

        protected override void Configure()
        {
            base.Configure();
            AM.Mapper.CreateMap<TEntity, TModel>();
            AM.Mapper.CreateMap<TModel, TEntity>()
               .ConstructUsing(x =>
               {
                   var entity = Repository.FindOne(((TModel)x.SourceValue).Id) ?? (TEntity)Activator.CreateInstance(x.DestinationType);
                   return entity;
               });
        }

    }

    public interface IMapperProfile<T, K>
        where T : class,IBaseEntity<K>
    {
        IRepository<T, K> Repository { get; set; }
    }
}
