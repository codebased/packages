using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ap.EntityFramework
{
    public interface IGenericRepository<TEntity> : IGenericRepository where TEntity : class
    {
        /// <summary>
        /// Delete a specific entity from DB set.
        /// </summary>
        /// <param name="id">Entity Identity</param>
        /// <returns>On success return true</returns>
        bool Delete(int id);

        /// <summary>
        /// Delete an entity from DB set.
        /// </summary>
        /// <param name="entity">Database entity</param>
        /// <returns>On success return true</returns>
        bool Delete(TEntity entity);

        /// <summary>
        /// Get records from entity database set.
        /// </summary>
        /// <param name="filter">filter LINQ expression query</param>
        /// <param name="orderBy">sorting order expression query</param>
        /// <param name="includeProperties">comma separated property</param>
        /// <param name="skip">pagination and skip number of records. This parameter is optional.</param>
        /// <param name="take">take x number of records. This parameter is optional</param>
        /// <returns>Query object that must be invoked by </returns>
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int skip = 0, int take = 0);

        TEntity Get(int id, string associatedProperties = null);

        IQueryable<TEntity> Get(IEnumerable<int> id, string associatedProperties = null);

        /// <summary>
        /// Get an entity by id.
        /// </summary>
        /// <param name="id">Record identity</param>
        /// <returns>object of entity type</returns>
        // TEntity Get(object id);

        /// <summary>
        /// Get count.
        /// </summary>
        /// <param name="filter">Filter expression</param>
        /// <returns>Count from Entity Database Set</returns>
        int GetCount(Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Insert entity into the database but don't save. It checks against 
        /// the entity schema and validate entity. On exception it throws DBEntityValidationException
        /// with the validation result.
        /// </summary>
        /// <param name="entity">Entity of Template Type</param>
        /// <returns>On insert it returns boolean value </returns>
        bool Insert(TEntity entity);

        /// <summary>
        /// Insert a range into DB set.
        /// </summary>
        /// <param name="entities">A Collection array of entities</param>
        /// <returns>On success return true.</returns>
        bool InsertRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Save context.
        /// </summary>
        /// <returns>On success return true</returns>
        bool Save();

        /// <summary>
        /// Save entity 
        /// </summary>
        /// <param name="exceptionMessages">exception messages </param>
        /// <returns>On success return true.</returns>
        bool Save(out string exceptionMessages);

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="id">original entity id.</param>
        /// <param name="updatedEntity">entity with new values.</param>
        /// <returns>On success return true.</returns>
        bool Update(int id, TEntity updatedEntity);

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="orignalEntity">Original Entity object.</param>
        /// <param name="updatedEntity">Updated Entity object.</param>
        /// <returns>On success return true.</returns>
        bool Update(TEntity orignalEntity, TEntity updatedEntity);

        /// <summary>
        /// Get a list of modified properties between original and updated entity.
        /// </summary>
        /// <param name="updatedEntity">The entity with new values</param>
        /// <returns>Collection of Dirty Property</returns>
        IEnumerable<DirtyProperty> GetModifiedProperties(TEntity updatedEntity);
    }
}
