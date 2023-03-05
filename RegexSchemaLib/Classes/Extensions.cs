using RegexSchemaLib.Models;
using RegexSchemaLib.Structs;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace RegexSchemaLib.Classes
{
    public static class Extensions
    {
        #region ====== Public Methods ======

        #endregion

        #region ====== Internal Methods ======

        internal static string Concat(this List<string> patterns, string separator)
        {
            if (patterns is null || patterns.Count == 0 || string.IsNullOrEmpty(separator))
                throw new ArgumentNullException($"patterns: {patterns}\nseparator: {separator}");

            StringBuilder sb = new();

            foreach (string pattern in patterns)
            {
                sb.Append(pattern);
                sb.Append(separator);
            }

            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        #endregion

        #region ====== Private Methods ======



        #endregion
    }
}
