
namespace Ap.Common.Dumper
{
    using System.IO;

    public class TextFileWriter : IWriter
    {
        private readonly TextWriter _writer;

        public TextFileWriter(string filePath)
        {
            _writer = File.CreateText(filePath);
        }

        public void Write(string message)
        {
            _writer.Write(message);
        }

        public void WriteLine()
        {
            _writer.WriteLine();
        }

        public void WriteLine(string message)
        {
            _writer.WriteLine(message);
        }
    }
}
