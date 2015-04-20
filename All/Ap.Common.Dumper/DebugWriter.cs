using System.Diagnostics;

namespace Ap.Common.Dumper
{
    public class DebugWriter : IWriter
    {
        public void Write(string message)
        {
            Debug.Write(message);
        }

        public void WriteLine(string message)
        {
            Debug.WriteLine(message);
        }
        
        public void WriteLine()
        {
            Debug.WriteLine(string.Empty);
        }
    }
}
