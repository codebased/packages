using System.IO;
using System.Reflection;

namespace Ap.Common.Extensions
{
    public static class Assemblies
    {
        public static string GetResourceText(this Assembly source, string resourceName)
        {
            string result;
            using (var stream = source.GetManifestResourceStream(resourceName))
            {
                if (stream == null) return string.Empty;
                var reader = new StreamReader(stream);
                result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
