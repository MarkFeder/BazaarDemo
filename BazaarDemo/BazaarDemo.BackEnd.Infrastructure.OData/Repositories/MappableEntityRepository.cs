
using EverNext.Domain.Contracts.Model;
using EverNext.Domain.Contracts.Repositories;
using EverNext.Domain.Contracts.Services;
using EverNext.Domain.Model.Exceptions;
using EverNext.Infrastructure.EntityFramework.DbContext;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq.Expressions;
using System.Reflection;
using BazaarDemo.BackEnd.Domain.Contracts.MappableEntityRepository;

namespace BazaarDemo.BackEnd.Infrastructure.OData.Repositories
{
    public class MappableEntityRepository<T,M> : LinqRepository<T>, IMappableEntityRepository<T,M>
        where T : class, IBaseEntity
        where M : class, IBaseEntity
    {
        #region Constructors

        public MappableEntityRepository() { }
        
        public MappableEntityRepository(System.Data.Entity.DbContext entities, IObjectMapper mapper)
            : base(entities)
        {
            Mapper = mapper;
        }

        #endregion

        #region Properties

        public virtual IObjectMapper Mapper { get; set; }

        #endregion

        #region Methods

        public virtual IEnumerable<M> BatchModels(IEnumerable<M> saveorupdate, IEnumerable<M> delete)
        {
            DeleteModels(delete);
            return SaveOrUpdateModels(saveorupdate);
        }

        public virtual void DeleteModel(M model)
        {
            DeleteModelInternal(model);
            InternalContext.SaveChanges();
        }

        private void DeleteModelInternal(M model)
        {
            var lambda = LambdaBuilder(model);
            T entityToDelete = Fetch().Where(lambda).Single();
            var entity = Mapper.Map<M, T>(model, entityToDelete);
            Delete(entity);
        }

        public virtual void DeleteModels(IEnumerable<M> modelList)
        {
            modelList.ForEach(c => DeleteModelInternal(c));
            InternalContext.SaveChanges();
        }

        public virtual M SaveOrUpdateModel(M model)
        {
            M returnedModel = SaveOrUpdateModelInternal(model);

            return returnedModel;
        }

        public virtual IEnumerable<M> SaveOrUpdateModels(IEnumerable<M> modelList)
        {
            IEnumerable<M> returnList = new List<M>();
            returnList = SaveOrUpdateModelsInternal(modelList);

            return returnList;
        }

        private IEnumerable<M> SaveOrUpdateModelsInternal(IEnumerable<M> models)
        {
            List<Tuple<T, M>> listEntityModelTuples = new List<Tuple<T, M>>();

            foreach (M model in models)
            {
                T entity = FindOne(LambdaBuilder(model));
                if (entity == null)
                {
                    entity = Activator.CreateInstance<T>();
                }
                else
                {
                    if (InternalContext.Entry<T>(entity).State == System.Data.Entity.EntityState.Deleted)
                        InternalContext.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
                }

                entity = Mapper.Map<M, T>(model, entity);
                entity = (T)DeepMapping(model, entity);

                SaveOrUpdate(entity);
                listEntityModelTuples.Add(new Tuple<T, M>(entity, model));
            }

            InternalContext.SaveChanges();

            List<M> returnList = new List<M>();

            listEntityModelTuples.ForEach(c => returnList.Add(Mapper.Map<T, M>(c.Item1, c.Item2)));

            return returnList;
        }

        private M SaveOrUpdateModelInternal(M model)
        {
            try
            {
                T entity = FindOne(LambdaBuilder(model));
                if (entity == null)
                {
                    entity = Activator.CreateInstance<T>();
                }
                else
                {
                    if (InternalContext.Entry<T>(entity).State == System.Data.Entity.EntityState.Deleted)
                        InternalContext.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
                }

                entity = Mapper.Map<M, T>(model, entity);
                entity = (T)DeepMapping(model, entity);

                SaveOrUpdate(entity);

                InternalContext.SaveChanges();

                return Mapper.Map<T, M>(entity);
            }
            catch (DbEntityValidationException ex)
            {
                List<string> errorMessages = new List<string>();
                foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
                {
                    string entityName = validationResult.Entry.Entity.GetType().Name;
                    foreach (DbValidationError error in validationResult.ValidationErrors)
                    {
                        errorMessages.Add(entityName + "." + error.PropertyName + ": " + error.ErrorMessage);
                    }
                }
                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                throw new EverNextInfrastructureException(new DbEntityValidationException(), new object[]{exceptionMessage});
            }
        }

        public virtual M FindOneModel(Expression<Func<T, bool>> expression)
        {
            return Mapper.Map<T, M>(FindOne(expression));
        }

        public virtual IEnumerable<M> FindAllModels(out int totalRows, int take = 0, int skip = 0, IEnumerable<EverNext.Domain.Model.Common.ServerSortDescriptor> sorts = null)
        {
            return Mapper.Map<T, M>(FindAll(out totalRows, take, skip, sorts));
        }

        #endregion

        #region PrivateMethods
        private IBaseEntity DeepMapping(IBaseEntity model, IBaseEntity entity, ICollection<IBaseEntity> alreadyMappedModels = null)
        {
            if (alreadyMappedModels == null)
                alreadyMappedModels = new List<IBaseEntity>();
            if (!alreadyMappedModels.Contains(model))
            {
                alreadyMappedModels.Add(model);

                var navPropertiesPropInfoCollection = model.GetTypeUnproxied()
                                                           .GetProperties()
                                                           .Where(p => Attribute.IsDefined(p, typeof(EverNext.Domain.Model.Attributes.NavigationPropertyAttribute), true));
                foreach (PropertyInfo pi in navPropertiesPropInfoCollection)
                {
                    dynamic modelCollection = pi.GetValue(model, null);
                    PropertyInfo entityPi = entity.GetTypeUnproxied().GetProperties().Where(p => p.Name == pi.Name).FirstOrDefault();
                    dynamic entityCollection = entityPi.GetValue(entity, null);
                    if (modelCollection == null)
                    {
                        entityPi.SetValue(entity, null, null);
                    }
                    else
                    {
                        if (entityCollection == null)
                        {
                            entityCollection = Activator.CreateInstance(entityPi.PropertyType);
                        }
                        if (typeof(System.Collections.IEnumerable).IsAssignableFrom(entityCollection.GetType()))
                        {
                            entityCollection.Clear();

                            foreach (dynamic mappableModel in modelCollection)
                            {
                                List<object> queryParams = new List<object>();
                                foreach (PropertyInfo lambdaPi in mappableModel.GetTypeSpecificKeyProperties())
                                {
                                    PropertyInfo keyedObjectPi = mappableModel.GetType().GetProperty(lambdaPi.Name);
                                    queryParams.Add(keyedObjectPi.GetValue(mappableModel, null));
                                }

                                dynamic entity2Map = InternalContext.Set(entityCollection.GetType().GetGenericArguments()[0]).Find(queryParams.ToArray());
                                if (entity2Map == null)
                                {
                                    entity2Map = Activator.CreateInstance(entityCollection.GetType().GetGenericArguments()[0]);
                                }
                                if (InternalContext.Entry(entity2Map).State == System.Data.Entity.EntityState.Deleted)
                                    InternalContext.Entry(entity2Map).State = System.Data.Entity.EntityState.Modified;

                                entity2Map = Mapper.Map(mappableModel, entity2Map, mappableModel.GetTypeUnproxied(), entity2Map.GetTypeUnproxied());
                                entityCollection.Add(entity2Map);

                                entity2Map = DeepMapping(mappableModel, entity2Map, alreadyMappedModels);
                            }
                        }
                        else
                        {
                            List<object> queryParams = new List<object>();
                            foreach (PropertyInfo lambdaPi in modelCollection.GetTypeSpecificKeyProperties())
                            {
                                PropertyInfo keyedObjectPi = modelCollection.GetType().GetProperty(lambdaPi.Name);
                                queryParams.Add(keyedObjectPi.GetValue(modelCollection, null));
                            }

                            dynamic entity2Map = InternalContext.Set(entityCollection.GetTypeUnproxied()).Find(queryParams.ToArray());
                            if (entity2Map == null)
                            {
                                entity2Map = Activator.CreateInstance(entityCollection.GetTypeUnproxied());
                            }
                            if (InternalContext.Entry(entity2Map).State == System.Data.Entity.EntityState.Deleted)
                                InternalContext.Entry(entity2Map).State = System.Data.Entity.EntityState.Modified;

                            entity2Map = Mapper.Map(modelCollection, entity2Map, modelCollection.GetTypeUnproxied(), entity2Map.GetTypeUnproxied());
                            entityPi.SetValue(entity, entity2Map, null);
                            entity2Map = DeepMapping(modelCollection, entity2Map, alreadyMappedModels);
                        }
                    }
                }
            }
            return entity;
        }

        #endregion
    }
}