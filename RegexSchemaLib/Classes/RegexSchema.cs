using RegexSchemaLib.Models;
using RegexSchemaLib.Structs;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace RegexSchemaLib.Classes
{
    public class RegexSchema
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schema"></param>
        /// <returns>(string? ResultString, ErrorModel?)</returns>
        public (string?, ErrorModel?) SchemaReplace(SchemaModel schema)
        {
            (bool success, ErrorModel? error) = VerifySchema(schema);

            if (!success)
                return (null, error);

            string? result = null;

            foreach (PlaceholderModel placeholder in schema.Placeholders)
            {
                //Laut meiner Recherche ist Regex Replace schneller als string.Replace und StringBuilder.Replace
                //ToDo: Parameter RegexOptions.Compiled nutzen?
                result = new Regex(schema.RegexPattern).Replace(placeholder.PlaceholderName, placeholder.ReplaceValue);
            }

            if (string.IsNullOrEmpty(result))
            {
                return (null, new ErrorModel()
                {
                    Message = "Replace resulted in an empty string!",
                    Type = Enums.ErrorType.Library
                });
            }

            return (result, null);
        }

        #region ====== Private Methods ======

        protected (bool success, ErrorModel?) VerifySchema(SchemaModel schema)
        {
            foreach (PlaceholderModel placeholder in schema.Placeholders)
            {
                if (string.IsNullOrEmpty(schema.SearchText) ||
                    string.IsNullOrEmpty(schema.RegexPattern) ||
                    string.IsNullOrEmpty(placeholder.RegexGroupName) ||
                    string.IsNullOrEmpty(placeholder.PlaceholderName) ||
                    string.IsNullOrEmpty(placeholder.ReplaceValue))
                {

                    ErrorModel error = new()
                    {
                        Type = Enums.ErrorType.Runtime,
                        Message = $"One of the following necessary values are null or empty:\n" +
                        $"{GetPropertyNamesString(schema, placeholder)}"
                    };

                    return (false, error);
                }


                if (!schema.RegexPattern.Contains(placeholder.PlaceholderName))
                {
                    ErrorModel error = new()
                    {
                        Type = Enums.ErrorType.Runtime,
                        Message = $"One of the following necessary values are null or empty:\n" +
                        $"{GetPropertyNamesString(schema, placeholder)}"
                    };

                    return (false, error);
                }

            }

            return (true, null);
        }

        protected string GetPropertyNamesString<T1, T2>(T1? t1 = default, T2? t2 = default)
            where T1 : SchemaModel
            where T2 : PlaceholderModel
        {

            if (t1 is default(T1) && t2 is default(T2))
                throw new ArgumentNullException($"T1: {t1}\nT2: {t2}");

            List<string>? propertyNames = null;

            if (t1 is not default(T1))
            {
                propertyNames = new();

                List<string>? propertyNamesT1 = Helper.GetPropertyNames(t1);

                if (propertyNamesT1 is not null && propertyNamesT1.Count > 0)
                {
                    propertyNames.AddRange(propertyNamesT1);
                }
            }

            if (t2 is not default(T2))
            {
                propertyNames ??= new();

                List<string>? propertyNamesT2 = Helper.GetPropertyNames(t2);

                if (propertyNamesT2 is not null && propertyNamesT2.Count > 0)
                {
                    propertyNames.AddRange(propertyNamesT2);
                }
            }

            if (propertyNames is null || propertyNames.Count == 0)
                throw new AggregateException("Could not determin any property Names!");

            return propertyNames.Concat("\n");
        }

        #endregion
    }
}