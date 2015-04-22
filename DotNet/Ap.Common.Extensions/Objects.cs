using System.Collections.Specialized;

namespace Ap.Common.Extensions
{
    public static class Objects
    {
        public static NameValueCollection ToNameValueCollection(this object item)
        {
            if (!item.IsNotNull()) return null;

            var propNames = new NameValueCollection();

            foreach (var propertyInfo in item.GetType().GetProperties())
            {
                if (!propertyInfo.CanRead) continue;

                var propertyName = propertyInfo.Name;
                var propertyValue = propertyInfo.GetValue(item, null);
                propNames.Add(propertyName, (propertyValue ?? string.Empty).ToString());
            }

            return propNames;
        }
    }
}