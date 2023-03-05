using RegexSchemaLib.Models;
using RegexSchemaLib.Structs;
using RegexSchemaLib.Enums;
using RegexSchemaLib.Classes;
using System.Text.RegularExpressions;
//using RegexSchemaLib.Interfaces

namespace RegexSchemaLib.Classes
{
    public class RegexSchema
    {
        #region ====== Boilerplate ======

        private readonly SchemaModel Schema;
        private bool SchemaValidated;

        public RegexSchema(SchemaModel schema, bool verifySchemaOnInit = true, bool throwOnVerifyOnInitError = false)
        {
            if (verifySchemaOnInit)
            {
                (bool success, _) = ValidateSchema(schema);

                if (!success && throwOnVerifyOnInitError)
                    throw new AggregateException("Schema is invalid!");

                SchemaValidated = success;
            }

            Schema = schema;
        }

        #endregion

        #region ====== Public Methods ======

        public (string?, ErrorModel?) CreateRegexPattern()
        {
            if (!SchemaValidated)
            {
                (bool success, ErrorModel? error) = ValidateSchema(Schema);
                if (!success)
                    return (null, error);
            }

            string? result = Schema.Pattern;

            foreach (PlaceholderModel placeholder in Schema.Placeholders)
            {
                if (placeholder.ReplaceWithNamedGroup)
                {
                    placeholder.ReplaceValue = $"(?'{placeholder.Name}'{placeholder.ReplaceValue})";
                }

                string placeholderPattern = $"[{placeholder.Name}]";
                result = result.Replace(placeholderPattern, placeholder.ReplaceValue);
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



        #endregion

        #region ====== Private Methods ======

        protected (bool Success, ErrorModel?) ValidateSchema(SchemaModel schema)
        {
            ErrorModel error = new()
            {
                Type = Enums.ErrorType.Runtime,
                Message = $"One of the following necessary values/parameter are null or empty:\n" +
                        $"{GetPropertyNamesString<SchemaModel, PlaceholderModel>(schema, null)}"
            };

            if (string.IsNullOrEmpty(schema.SearchText))
                return (false, error);

            foreach (PlaceholderModel placeholder in schema.Placeholders)
            {
                if (string.IsNullOrEmpty(schema.Pattern) ||
                    string.IsNullOrEmpty(placeholder.Name) ||
                    string.IsNullOrEmpty(placeholder.ReplaceValue))
                {
                    return (false, error);
                }


                if (!schema.Pattern.Contains(placeholder.Name))
                {
                     error = new()
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