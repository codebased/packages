namespace Ap.Common.Dumper
{
    public interface IWriter
    {
        void Write(string message);
        void WriteLine();
        void WriteLine(string message);
    }
}
