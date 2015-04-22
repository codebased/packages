using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ap.EntityFramework
{
    /// <summary>
    /// Defines the Identity that stores generic repository methods.
    /// </summary>
    public interface IGenericRepository : IDisposable
    {
        IDbContext DatabaseContext { get; }
    }
}
