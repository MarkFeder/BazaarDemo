using EverNext.Domain.Contracts.Model;
using EverNext.Domain.Contracts.Repositories;
using Microsoft.OData.Core;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using System.Web.OData.Extensions;
using System.Web.Http.Results;
using System.Net;
using System.Data.Entity;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using BazaarDemo.BackEnd.Domain.Contracts.MappableEntityRepository;

namespace BazaarDemo.BackEnd.Infrastructure.OData.Controllers
{
    #region Documentation
    /// <summary>
    /// ODataController which is intented to map models to entities and perform different operations
    /// </summary>
    /// <typeparam name="T">The Entity which derives from IBaseEntity</typeparam>
    /// <typeparam name="M">The Model associated with the Entity T which derives from IBaseEntity</typeparam>
    #endregion
    public class ODataMappableController<T,M> : ODataController 
        where T : class, IBaseEntity
        where M : class, IBaseEntity
    {
        #region Constructors

        public ODataMappableController(IMappableEntityRepository<T,M> mappableEntityRepository)
        {
            MappableEntityRepository = mappableEntityRepository;
        }

        public ODataMappableController() { }

        #endregion

        #region Properties
        // States our validation settings property
        internal static ODataValidationSettings validationSettings = new ODataValidationSettings();
        internal static ODataQuerySettings querySettings = new ODataQuerySettings();

        // States our mappable repository for our model class, which does not use specifications
        public IMappableEntityRepository<T,M> MappableEntityRepository { get; set; }

        #endregion

        #region Public Methods

        #region Documentation
        /// <summary>
        /// Get the model with the queryOptions applied
        /// </summary>
        /// <param name="key">the key of the model</param>
        /// <param name="queryOptions">ODataQueryOptions passed by parameter</param>
        /// <returns>Returns BadRequest if the action was not performed or 200(OK) and the model if the action was performed</returns> 
        #endregion
        [HttpGet]
        public virtual IHttpActionResult GetModel([FromODataUri] int key, ODataQueryOptions<M> queryOptions)
        {
            #region Validate Settings
            try
            {
                queryOptions.Validate(validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }
            #endregion

            ODataQueryOptions<T> newODataQueryOptions = buildEntityQueryOptions(queryOptions, queryOptions.IsExpandQuery());

            // Build the expression and find the model by its key
            var keyProperty = typeof(T).GetProperties().Where(prop => Attribute.GetCustomAttribute(prop, typeof(KeyAttribute)) != null).Single();

            if (keyProperty == null)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            M modelfound = MappableEntityRepository.FindOneModel(PropertyEquals<T,int>(keyProperty, key));
                
            if (queryOptions.SelectExpand != null)
            {
                Request.ODataProperties().SelectExpandClause = queryOptions.SelectExpand.SelectExpandClause;
            }

            if (modelfound != null)
            {
                return Ok<M>(modelfound);
            }
            else
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
        }

        #region Documentation
        /// <summary>
        /// Get the models with the queryOptions applied
        /// </summary>
        /// <param name="queryOptions">ODataQueryOptions passed by parameter</param>
        /// <returns>Returns BadRequest if the action was not performed or 200(OK) and the models if the action was performed</returns> 
        #endregion
        [HttpGet]
        public virtual IHttpActionResult GetModels(ODataQueryOptions<M> queryOptions)
        {
            #region Validate Settings
            try
            {
                queryOptions.Validate(validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }
            #endregion

            ODataQueryOptions<T> newODataQueryOptions = buildEntityQueryOptions(queryOptions, queryOptions.IsExpandQuery());

            var queriedObjects = (newODataQueryOptions.ApplyTo(MappableEntityRepository.Fetch()) as IQueryable<T>).AsEnumerable();

            IEnumerable<M> newResult = MappableEntityRepository.Mapper.Map<IEnumerable<T>, IEnumerable<M>>(queriedObjects);

            if (queryOptions.SelectExpand != null)
            {
                Request.ODataProperties().SelectExpandClause = queryOptions.SelectExpand.SelectExpandClause;
            }

            if (newResult.Any())
            {
                return Ok<IEnumerable<M>>(newResult);
            }
            else
            {
                return StatusCode(HttpStatusCode.NoContent);
            }

        }

        #region Documentation
        /// <summary>
        /// Post the model passed by "model" parameter
        /// </summary>
        /// <param name="model">The model to be persisted</param>
        /// <param name="queryOptions">ODataQueryOptions passed by parameter. It only allows $expand properties for the returned type</param>
        /// <returns>Returns BadRequest if the model passed by parameter was not valid or 200(OK) and the model saved if the action was performed</returns> 
        #endregion
        [HttpPost]
        public virtual IHttpActionResult PostModel(M model, ODataQueryOptions<M> queryOptions)
        {
            #region Validate settings and ModelState
            try
            {
                queryOptions.Validate(validationSettings);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }
            #endregion

            if (model != null)
            {
                M ModelSaved = MappableEntityRepository.SaveOrUpdateModel(model);

                if (queryOptions.SelectExpand != null)
                {
                    Request.ODataProperties().SelectExpandClause = queryOptions.SelectExpand.SelectExpandClause;
                }

                return ChecksModelAndReturnsActionResult(ModelSaved);
            }

            return StatusCode(HttpStatusCode.BadRequest);
        }

        #region Documentation
        /// <summary>
        /// Post the models passed by "models" parameter in ODataActionParameter
        /// </summary>
        /// <param name="parameters">ODataActionParameter which contains "models" as parameter</param>
        /// <param name="queryOptions">ODataQueryOptions passed by parameter. It only allows $expand properties for the returned type</param>
        /// <returns>Returns BadRequest if the models passed by parameter was not valid or 200(OK) and the models saved if the action was performed</returns> 
        #endregion
        [HttpPost]
        public virtual IHttpActionResult PostModels(ODataActionParameters parameters, ODataQueryOptions<M> queryOptions)
        {
            #region Validate settings and ModelState
            try
            {
                queryOptions.Validate(validationSettings);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }
            #endregion

            IEnumerable<M> Models = parameters["models"] as IEnumerable<M>;

            if (Models != null)
            {
                IEnumerable<M> ModelsSaved = MappableEntityRepository.SaveOrUpdateModels(Models);
                
                if (queryOptions.SelectExpand != null)
                {
                    Request.ODataProperties().SelectExpandClause = queryOptions.SelectExpand.SelectExpandClause;
                }

                return ChecksModelsAndReturnsActionResult(ModelsSaved);
            }

            return StatusCode(HttpStatusCode.BadRequest);
        }

        #region Documentation
        /// <summary>
        /// Put the model passed by "model" parameter in ODataActionParameter
        /// </summary>
        /// <param name="parameters">ODataActionParameter which contains "model" as parameter</param>
        /// <param name="queryOptions">ODataQueryOptions passed by parameter. It only allows $expand properties for the returned type</param>
        /// <returns>Returns BadRequest if the model passed by parameter was not valid or 200(OK) and the model updated if the action was performed</returns> 
        #endregion
        [HttpPut]
        public virtual IHttpActionResult PutModel([FromODataUri] int key, M model, ODataQueryOptions<M> queryOptions)
        {
            #region Validate settings and ModelState

            try
            {
                queryOptions.Validate(validationSettings);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }
            #endregion

            // Builds up lambda expression to compare the key of the model
            var lambda = PropertyEquals<M, int>(typeof(M).GetProperties().Where(prop => Attribute.GetCustomAttribute(prop, typeof(KeyAttribute)) != null).Single(), key);
            Func<M,bool> func = lambda.Compile();

            if (model != null && func(model))
            {
                M ModelUpdated = MappableEntityRepository.SaveOrUpdateModel(model);

                if (queryOptions.SelectExpand != null)
                {
                    Request.ODataProperties().SelectExpandClause = queryOptions.SelectExpand.SelectExpandClause;
                }

                return ChecksModelAndReturnsActionResult(ModelUpdated);
            }

            return StatusCode(HttpStatusCode.BadRequest);
        }

        #region Documentation
        /// <summary>
        /// Put the models passed by "models" parameter in ODataActionParameter
        /// </summary>
        /// <param name="parameters">ODataActionParameter which contains "models" as parameter</param>
        /// <param name="queryOptions">ODataQueryOptions passed by parameter. It only allows $expand properties for the returned type</param>
        /// <returns>Returns BadRequest if the models passed by parameter were not valid or 200(OK) and the models updated if the action was performed</returns> 
        #endregion
        [HttpPut]
        public virtual IHttpActionResult PutModels(ODataActionParameters parameters, ODataQueryOptions<M> queryOptions)
        {
            #region Validate settings and ModelState
            try
            {
                queryOptions.Validate(validationSettings);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }
            #endregion


            IEnumerable<M> Models = parameters["models"] as IEnumerable<M>;

            if (Models != null)
            {
                IEnumerable<M> ModelsUpdated = MappableEntityRepository.SaveOrUpdateModels(Models);

                if (queryOptions.SelectExpand != null)
                {
                    Request.ODataProperties().SelectExpandClause = queryOptions.SelectExpand.SelectExpandClause;
                }

                return ChecksModelsAndReturnsActionResult(ModelsUpdated);
            }

            return StatusCode(HttpStatusCode.BadRequest);
        }

        #region Documentation
        /// <summary>
        /// Delete the model passed by "model" parameter in ODataActionParameters
        /// </summary>
        /// <param name="parameters">ODataActionParameter which contains "model" as parameter</param>
        /// <returns>Returns BadRequest if the model passed by parameter was not valid or 200(OK) if the action was performed</returns> 
        #endregion
        [HttpDelete]
        public virtual IHttpActionResult DeleteModel([FromODataUri] int key)
        {
            #region Validate Model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            #endregion

            // Build the expression and find the model by its key
            var keyProperty = typeof(T).GetProperties().Where(prop => Attribute.GetCustomAttribute(prop, typeof(KeyAttribute)) != null).Single();

            if (keyProperty == null)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            M model = MappableEntityRepository.FindOneModel(PropertyEquals<T, int>(keyProperty, key));

            if (model != null)
            {
                MappableEntityRepository.DeleteModel(model);

                return StatusCode(HttpStatusCode.OK);
            }

            return StatusCode(HttpStatusCode.BadRequest);
        }

        #region Documentation
        /// <summary>
        /// Delete the models passed by "models" parameter in ODataActionParameters
        /// </summary>
        /// <param name="parameters">ODataActionParameter which contains "models" as parameter</param>
        /// <returns>Returns BadRequest if the models passed by parameter was not valid or 200(OK) if the action was performed</returns> 
        #endregion
        [HttpPost]
        public virtual IHttpActionResult DeleteModels(ODataActionParameters parameters)
        {
            #region Validate Model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            #endregion

            IEnumerable<M> Models = parameters["models"] as IEnumerable<M>;

            if (Models != null)
            {
                MappableEntityRepository.DeleteModels(Models);

                return StatusCode(HttpStatusCode.OK);
            }

            return StatusCode(HttpStatusCode.BadRequest);
        }

        #region Documentation
        /// <summary>
        /// Delete the models passed by "models_d" and Create/Update the models passed by "models_p" in ODataActionParameters, all in one transaction
        /// </summary>
        /// <param name="parameters">ODataActionParameter which contains "models_d" and "models_p" as parameters</param>
        /// <param name="queryOptions">ODataQueryOptions passed by parameter. It only allows $expand properties for the returned type</param>
        /// <returns>Returns BadRequest if the models passed by parameter were not valid or 200(OK) and the models updated if the action was performed</returns> 
        #endregion
        [HttpPost]
        public virtual IHttpActionResult BatchModels(ODataActionParameters parameters, ODataQueryOptions<M> queryOptions)
        {
            #region Validate settings and ModelState
            try
            {
                queryOptions.Validate(validationSettings);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }
            #endregion

            IEnumerable<M> ModelsToDelete = parameters["models_d"] as IEnumerable<M>;
            IEnumerable<M> ModelsToPostOrPut = parameters["models_p"] as IEnumerable<M>;

            if (ModelsToDelete != null && ModelsToPostOrPut != null)
            {
                IEnumerable<M> ModelsSavedOrUpdated = MappableEntityRepository.BatchModels(ModelsToPostOrPut, ModelsToDelete);

                if (queryOptions.SelectExpand != null)
                {
                    Request.ODataProperties().SelectExpandClause = queryOptions.SelectExpand.SelectExpandClause;
                }

                return ChecksModelsAndReturnsActionResult(ModelsSavedOrUpdated);
            }

            return StatusCode(HttpStatusCode.BadRequest);
        }

        #endregion

        #region Private Methods

        #region Documentation
        /// <summary>
        /// Builds up the Expression object to represent the expression: m => m.keyProperty == keyValue
        /// </summary>
        /// <typeparam name="TItem">The type of the entity</typeparam>
        /// <typeparam name="TValue">The type of the value to be compared. It must be an int</typeparam>
        /// <param name="property">The entity's property to be compared</param>
        /// <param name="value">The value to be compared</param>
        /// <returns></returns> 
        #endregion
        private Expression<Func<TItem, bool>> PropertyEquals<TItem, TValue>(PropertyInfo property, TValue value)
            where TItem : class, IBaseEntity
        {
            var param = Expression.Parameter(typeof(TItem));
            var body = Expression.Equal(Expression.Property(param, property), Expression.Constant(value));

            return Expression.Lambda<Func<TItem, bool>>(body, param);
        }

        #region Documentation
        /// <summary>
        /// A helper method that checks if a model is null and returns the proper IHttpActionResult
        /// </summary>
        /// <param name="model">The model to be checked</param>
        /// <returns>Returns BadRequest if the model was null or 200(OK) if it wasn't</returns> 
        #endregion
        private IHttpActionResult ChecksModelAndReturnsActionResult(M model)
        {
            if (model == null)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            else
            {
                return Ok<M>(model);
            }
        }

        #region Documentation
        /// <summary>
        /// A helper method that checks if models are null and returns the proper IHttpActionResult
        /// </summary>
        /// <param name="models">The models to be checked</param>
        /// <returns>Returns BadRequest if the models were null or 200(OK) if it wasn't</returns> 
        #endregion
        private IHttpActionResult ChecksModelsAndReturnsActionResult(IEnumerable<M> models)
        {
            if (models == null || !models.Any())
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            else
            {
                return Ok<IEnumerable<M>>(models);
            }
        }

        #region Documentation
        /// <summary>
        /// Builds up the ODataQueryOptions which belongs to an entity
        /// </summary>
        /// <param name="modelQueryOptions">The ODataQueryOptions which belongs to a model</param>
        /// <param name="isExpandQuery">A bool that represents whether the ODataQueryOptions passed by parameter supports $expand option </param>
        /// <returns>Returns the ODataQueryOptions which belongs to the entity</returns> 
        #endregion
        private ODataQueryOptions<T> buildEntityQueryOptions(ODataQueryOptions<M> modelQueryOptions,bool isExpandQuery)
        {
            // New ODataModelBuilder for entity type
            ODataModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<T>(typeof(T).Name);

            // Create new ODataQueryContext
            ODataQueryContext newContext = new ODataQueryContext(modelBuilder.GetEdmModel(), typeof(T), Request.ODataProperties().Path);

            if (isExpandQuery)
            {
                // Add custom OData querystring to handle $expand clause
                System.Net.Http.HttpRequestMessage newRequest = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, modelQueryOptions.Request.TrimQueryStringsAndRebuildUri("$expand"));

                return new ODataQueryOptions<T>(newContext, newRequest);
            }
            else
            { 
                // Create new ODataQueryOptions and return it
                return new ODataQueryOptions<T>(newContext, Request);
            }
        }

        #endregion
    }
}