using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ap.EntityFramework
{
    public class IdentityBase
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the unique identifier of a row.
        /// </summary>
        public Guid UniqueIdentifier
        {
            get;
            set;
        }
    }
}
