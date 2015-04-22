using System;

namespace Ap.EntityFramework
{
    public abstract class EntityBase : IdentityBase
    {
        /// <summary>
        /// Initializes a new instance of the EntityBase class.
        /// </summary>
        public EntityBase()
        {
            DateModified = DateTime.UtcNow;
            DateCreated = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets or sets the date when the entity was created.
        /// </summary>
        public DateTime? DateCreated
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date when the entity was updated.
        /// </summary>
        public DateTime? DateModified
        {
            get;
            set;
        }
    }
}
