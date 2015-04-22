namespace Ap.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Defines the class with generic database operations. 
    /// It implements <seealso>
    ///         <cref>IGenericRepository</cref>
    ///     </seealso>
    ///     interface.
    /// </summary>
    /// <typeparam name="TEntity">Entity of type class</typeparam>
    public abstract class GenericRepository<TEntity> : GenericRepository, IGenericRepository<TEntity> where TEntity : IdentityBase
    {
        /// <summary>
        /// Define database set.
        /// </summary>
        private readonly IDbSet<TEntity> _databaseSet;

        /// <summary>
        /// Defines a flag if the object is already disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the GenericRepository class.
        /// </summary>
        /// <param name="dataContext">Database context</param>
        protected GenericRepository(IDbContext dataContext)
            : base(dataContext)
        {
            _databaseSet = DbContext.Set<TEntity>();
        }

        /// <summary>
        /// Get records from entity database set.
        /// </summary>
        /// <param name="filter">filter LINQ expression query</param>
        /// <param name="orderBy">sorting order expression query</param>
        /// <param name="includeProperties">comma separated property</param>
        /// <param name="skip">pagination and skip number of records. This parameter is optional.</param>
        /// <param name="take">take x number of records. This parameter is optional</param>
        /// <returns>Query object that must be invoked by </returns>
        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int skip = 0, int take = 0)
        {
            IQueryable<TEntity> query = _databaseSet;

            if (query != null)
            {
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (includeProperties == null)
                {
                    includeProperties = string.Empty;
                }

                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, property) => current.Include(property));

                if (orderBy != null)
                {
                    query = orderBy(query);

                    if (take != 0)
                    {
                        query = query.Skip(skip).Take(take);
                    }
                    else if (skip > 0)
                    {
                        query = query.Skip(skip);
                    }
                }
            }

            return query;
        }

        /// <summary>
        /// Get an entity by id.
        /// </summary>
        /// <param name="id">Record identity</param>
        /// <param name="associatedProperties">Get associated properties with an object</param>
        /// <returns>object of entity type</returns>
        public virtual TEntity Get(int id, string associatedProperties = null)
        {
            return Get(filter: e => e.Id.Equals(id), includeProperties: associatedProperties).FirstOrDefault();
        }

        public virtual IQueryable<TEntity> Get(IEnumerable<int> id, string associatedProperties = null)
        {
            return Get(filter: entity => id.Contains(entity.Id), includeProperties: associatedProperties);
        }

        /// <summary>
        /// Get count.
        /// </summary>
        /// <param name="filter">Filter expression</param>
        /// <returns>Count from Entity Database Set</returns>
        public virtual int GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _databaseSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Count();
        }

        /// <summary>
        /// Insert entity into the database but don't save. It checks against 
        /// the entity schema and validate entity. On exception it throws DBEntityValidationException
        /// with the validation result.
        /// </summary>
        /// <param name="entity">Entity of Template Type</param>
        /// <returns>On insert it returns boolean value </returns>
        public virtual bool Insert(TEntity entity)
        {
            var validationResult = DbContext.Entry(entity).GetValidationResult();

            if (validationResult.IsValid)
            {
                _databaseSet.Add(entity);
                return true;
            }

            var errors = new List<DbEntityValidationResult> { validationResult };
            throw new DbEntityValidationException(string.Empty, errors);
        }

        /// <summary>
        /// Insert a range into DB set.
        /// </summary>
        /// <param name="entities">A Collection array of entities</param>
        /// <returns>On success return true.</returns>
        public virtual bool InsertRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }

            return true;
        }

        /// <summary>
        /// Delete a specific entity from DB set.
        /// </summary>
        /// <param name="id">Entity Identity</param>
        /// <returns>On success return true</returns>
        public virtual bool Delete(int id)
        {
            TEntity entity = _databaseSet.Find(id);
            return Delete(entity);
        }

        /// <summary>
        /// Delete an entity from DB set.
        /// </summary>
        /// <param name="entity">Database entity</param>
        /// <returns>On success return true</returns>
        public bool Delete(TEntity entity)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                _databaseSet.Attach(entity);
            }

            _databaseSet.Remove(entity);

            return true;
        }

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="id">original entity id.</param>
        /// <param name="updatedEntity">entity with new values.</param>
        /// <returns>On success return true.</returns>
        public virtual bool Update(int id, TEntity updatedEntity)
        {
            var orgEntity = _databaseSet.Find(id);
            return Update(orgEntity, updatedEntity);
        }

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="orignalEntity">Original Entity object.</param>
        /// <param name="updatedEntity">Updated Entity object.</param>
        /// <returns>On success return true.</returns>
        public virtual bool Update(TEntity orignalEntity, TEntity updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ArgumentException("Cannot add a null entity.");
            }

            if (orignalEntity != null)
            {
                var validationResult = DbContext.Entry(updatedEntity).GetValidationResult();

                if (validationResult.IsValid)
                {
                    var attachedEntity = DbContext.Entry(orignalEntity);
                    attachedEntity.CurrentValues.SetValues(updatedEntity);

                    var excludeProperties = new List<string>
                    {
                        "UniqueIdentifier",
                        "AuthorID",
                        "CreatedBy",
                        "DateCreated"
                    };

                    // Some of these fields you cannot just modify at all.
                    excludeProperties.AddRange(ExcludeUpdateProperties());

                    foreach (var name in excludeProperties)
                    {
                        var property = attachedEntity.Property(name);
                        if (property != null)
                        {
                            attachedEntity.Property(name).IsModified = false;
                        }
                    }

                    attachedEntity.Property("DateModified").IsModified = true;
                }
                else
                {
                    var errors = new List<DbEntityValidationResult>();
                    errors.Add(validationResult);
                    throw new DbEntityValidationException(string.Empty, errors);
                }
            }
            else
            {
                DbContext.Entry(updatedEntity).State = EntityState.Modified;
            }

            return true;
        }

        /// <summary>
        /// Save context.
        /// </summary>
        /// <returns>On success return true</returns>
        public virtual bool Save()
        {
            string exceptionMessages;
            return Save(out exceptionMessages);
        }

        /// <summary>
        /// Save entity 
        /// </summary>
        /// <param name="exceptionMessages">exception messages </param>
        /// <returns>On success return true.</returns>
        public virtual bool Save(out string exceptionMessages)
        {
            exceptionMessages = string.Empty;

            try
            {
                // SaveChanges call GetValidationErrors() internally that eventually 
                // throws an exception of DbEntityValidationException.
                return DbContext.SaveChanges() > 0;
            }
            catch (DbEntityValidationException e)
            {
                // If you have gone down to that level then 
                // better you check DataAnotation set for the Model.
                foreach (var eve in e.EntityValidationErrors)
                {
                    exceptionMessages +=
                        string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    exceptionMessages = eve.ValidationErrors.Aggregate(exceptionMessages,
                        (current, ve) =>
                            current +
                            string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                }

                throw;
            }
        }

        /// <summary>
        /// Get a list of modified properties between original and updated entity. It will
        /// also exclude any property where the old value is equal to original value.
        /// </summary>
        /// <param name="updatedEntity">The entity with new values</param>
        /// <returns>Collection of DirtyProperty</returns>
        public IEnumerable<DirtyProperty> GetModifiedProperties(TEntity updatedEntity)
        {
            if (updatedEntity == null)
            {
                return null;
            }

            var attachedEntity = DbContext.Entry(updatedEntity);

            IEnumerable<DirtyProperty> items = attachedEntity.CurrentValues.PropertyNames
                .Where(p => attachedEntity.Property(p).IsModified)
                .Select(p => new DirtyProperty { NewValue = attachedEntity.Property(p).CurrentValue.ToString(), OldValue = attachedEntity.Property(p).OriginalValue.ToString(), Name = p }).ToList();

            return from item in items
                   where !item.NewValue.Equals(item.OldValue)
                   select item;
        }

        /// <summary>
        /// This method must be override by callee when you want Entity Framework to exclude some properties from its update operation.
        /// </summary>
        /// <returns>A Collection of string type that represent property names matches with the column/ model field/ property name.</returns>
        public virtual IEnumerable<string> ExcludeUpdateProperties()
        {
            var excludingItems = new List<string>();
            return excludingItems;
        }

        public new void Dispose()
        {
            Dispose(true);
            base.Dispose();
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // dispose any managed object here...
                if (DbContext != null)
                {
                    DbContext.Dispose();
                    DbContext = null;
                }
            }

            // free any unmanaged object here...
            _disposed = true;
        }

        ~GenericRepository()
        {
            Dispose(false);
        }
    }
}
