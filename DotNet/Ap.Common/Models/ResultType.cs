using System;

namespace Ap.Common.Models
{
    [Flags]
    public enum ResultType
    {
        Success = 0,
        Warning = 1,
        Error = 2,
    }
}
