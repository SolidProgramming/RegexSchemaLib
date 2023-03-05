using System.Reflection;
using System.Text;

namespace RegexSchemaLib.Classes
{
    internal static class Helper
    {
        internal static List<string>? GetPropertyNames<T>(this T @class) where T : class
        {
            PropertyInfo[] propertyInfos = @class.GetType().GetProperties(BindingFlags.Public);
            List<string>? propertyNames = new();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                propertyNames.Add(propertyInfo.Name);
            }

            if (propertyNames.Count == 0)
                return null;

            return propertyNames;
        }
    }
}
