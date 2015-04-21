using System;

namespace Ap.EntityFramework
{
    public class GenericRepository : IGenericRepository
    {
        /// <summary>
        /// Defines database context.
        /// </summary>
        private IDbContext _context;

        private bool _disposed;

        public GenericRepository(IDbContext dataContext)
        {
            _context = dataContext;
            _disposed = false;
        }

        protected internal IDbContext DbContext
        {
            get { return _context; }

            protected set { _context = value; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }

            _disposed = true;

        }

        public IDbContext DatabaseContext
        {
            get { return _context; }
        }
    }
}
