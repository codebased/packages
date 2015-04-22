using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ap.EntityFramework
{
    public interface IDbContext
    {
        /// <summary>
        /// Defines the fake DbSet.
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <returns>It returns the DBSet object of type entity</returns>
        DbSet<T> Set<T>() where T : class;

        /// <summary>
        /// Defines the Entry of type T.
        /// </summary>
        /// <typeparam name="T">Entity type T</typeparam>
        /// <param name="entity">Entity Object</param>
        /// <returns>Database Entity Entry</returns>
        DbEntityEntry<T> Entry<T>(T entity) where T : class;

        /// <summary>
        /// Save changes into the database.
        /// </summary>
        /// <returns>Number of records affected by.</returns>
        int SaveChanges();

        /// <summary>
        /// Dispose object.
        /// </summary>
        void Dispose();
    }
}
