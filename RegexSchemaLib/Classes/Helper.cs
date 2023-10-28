using System.Reflection;
using System.Text;

namespace RegexSchemaLib.Classes
{
    public static class Helper
    {
        /// <summary>
        /// Methode zum auslesen der enthaltenen Eigenschaften einer Klasse mittels Reflection
        /// </summary>
        /// <typeparam name="T">(T) Typ</typeparam>
        /// <param name="class">(class) Klasse</param>
        /// <returns>(List<string>) Liste der enthaltenen Eigenschaften der Klasse</returns>
        public static List<string>? GetPropertyNames<T>() where T : class
        {
            PropertyInfo[] propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
            List<string> propertyNames = new();

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
