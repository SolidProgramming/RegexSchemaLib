using RegexSchemaLib.Models;
using RegexSchemaLib.Structs;
using RegexSchemaLib.Enums;
using RegexSchemaLib.Classes;
using System.Text.RegularExpressions;
//using RegexSchemaLib.Interfaces

namespace RegexSchemaLib.Classes
{
    /// <summary>
    /// Klasse die Funktionen für Regex Schema Modelle und Platzhalter bereitsteltl
    /// </summary>
    public class RegexSchema
    {
        #region ====== Boilerplate ======

        /// <summary>
        /// Speichert das Schema
        /// </summary>
        private readonly SchemaModel Schema;

        /// <summary>
        /// Speichert ob das Schema validiert wurde
        /// </summary>
        private bool SchemaValidated;

        /// <summary>
        /// Konstruktor der RegexSchema Klasse
        /// </summary>
        /// <param name="schema">(SchemaModel) Schema</param>
        /// <param name="verifySchemaOnInit">(opt. bool) Schema beim initialisieren validieren</param>
        /// <param name="throwOnValidationOnInitError">(opt. bool) Exception auslösen wenn das Schema unzulässig ist</param>
        /// <exception cref="AggregateException"></exception>
        public RegexSchema(SchemaModel schema, bool verifySchemaOnInit = true, bool throwOnValidationOnInitError = false)
        {
            if (verifySchemaOnInit)
            {
                (bool success, _) = ValidateSchema(schema);

                if (!success && throwOnValidationOnInitError)
                    throw new AggregateException("Schema is invalid!");

                SchemaValidated = success;
            }

            Schema = schema;
        }

        #endregion

        #region ====== Public Methods ======

        /// <summary>
        /// Methode zum erstellen des Regex Pattern über das zuvor im Konstruktor angegebene Schema
        /// </summary>
        /// <returns>(string RegexPattern, ErrorModel Error) Regex Pattern und Fehler falls angegeben</returns>
        public (string? regexPattern, ErrorModel? error) CreateRegexPattern()
        {
            if (!SchemaValidated)
            {
                (bool success, ErrorModel? error) = ValidateSchema(Schema);
                if (!success)
                    return (null, error);
            }

            string result = Schema.Pattern;

            if (string.IsNullOrEmpty(result))
                return (null, new ErrorModel() { Type = ErrorType.Data, Message = "Schema Pattern null or empty!" });

            foreach (PlaceholderModel placeholder in Schema.Placeholders)
            {
                if (placeholder.GroupOptions != null)
                {
                    string groupName = "";

                    if (placeholder.GroupOptions.GroupOptions.HasFlag(RegexGroupOption.Named))
                    {
                        groupName = GetGroupName(placeholder.GroupOptions.GroupNameOptions, placeholder);

                        if(string.IsNullOrEmpty(groupName))
                            return (null, new ErrorModel() { Type = ErrorType.Data, Message = $"Could't parse Group Name of {placeholder.Name}" });

                        placeholder.ReplaceValue = $"(?'{groupName}'{placeholder.ReplaceValue})";
                    }
                    else
                    {
                        placeholder.ReplaceValue = $"({placeholder.ReplaceValue})";
                    }

                    if (placeholder.GroupOptions.GroupOptions.HasFlag(RegexGroupOption.Optional))
                    {
                        placeholder.ReplaceValue += "?";
                    }

                    if (placeholder.GroupOptions.GroupNameOptions == RegexGroupNameOption.SmartInheritName)
                    {
                        placeholder.Name = Regex.Replace(placeholder.Name, "[\\[\\(\\{\\]\\)\\}]", "");
                    }
                }

                string placeholderPattern = $"[{placeholder.Name}]";
                result = result.Replace(placeholderPattern, placeholder.ReplaceValue);
            }

            if (string.IsNullOrEmpty(result))
            {
                return (null, new ErrorModel()
                {
                    Message = "Replace resulted in an empty string!",
                    Type = ErrorType.Library
                });
            }

            return (result, default);
        }

        #endregion

        #region ====== Private Methods ======

        /// <summary>
        /// Gibt den Regex Gruppennamen anhand der RegexGroupNameOptions zurück
        /// </summary>
        /// <param name="nameOptions">(enum) RegexGroupNameOptions</param>
        /// <param name="placeholder">(PlaceholderModel) Platzhalter</param>
        /// <returns>(string) Regex Gruppennamen</returns>
        protected string? GetGroupName(RegexGroupNameOption nameOptions, PlaceholderModel placeholder)
        {
            string? groupName = "";

            switch (nameOptions)
            {
                case RegexGroupNameOption.SmartInheritName:
                    groupName = Regex.Replace(placeholder.Name, "[\\[\\(\\{\\]\\)\\}]", "");
                    break;
                case RegexGroupNameOption.InheritName:
                    groupName = placeholder.Name;
                    break;
                default:
                    break;
            }

            return groupName;
        }

        /// <summary>
        /// Methode zum validieren des angegebenen Schemas
        /// </summary>
        /// <param name="schema">(SchemaModel) Schema</param>
        /// <returns>(bool Success, ErrorModel Error)</returns>
        protected (bool success, ErrorModel? error) ValidateSchema(SchemaModel schema)
        {
            ErrorModel error = new()
            {
                Type = ErrorType.Data,
                Message = $"One of the following necessary values/parameter are null or empty:\n" +
                        $"{GetPropertyNamesString<SchemaModel, PlaceholderModel>(schema, null)}"
            };

            if (schema.Placeholders.Count == 0)
                return (false, error);

            if(string.IsNullOrEmpty(schema.Pattern))
                return (false, new ErrorModel() { Type = ErrorType.Data, Message = "Schema Pattern is null or empty!" });

            foreach (PlaceholderModel placeholder in schema.Placeholders)
            {
                if (string.IsNullOrEmpty(placeholder.Name))
                    return (false, error);                

                if (!schema.Pattern.Contains(placeholder.Name))
                {
                    error = new ErrorModel()
                    {
                        Type = ErrorType.Data,
                        Message = $"One of the following necessary values are null or empty:\n" +
                       $"{GetPropertyNamesString(schema, placeholder)}"
                    };

                    return (false, error);
                }
            }

            return (true, null);
        }

        /// <summary>
        /// Methode zum ermitteln der Namen der Eigenschaftern der angegebenen Schema und Platzhalter Objekte enthält
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns>(string) Namen der Eigenschaften</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="AggregateException"></exception>
        protected string GetPropertyNamesString<T1, T2>(T1? t1 = default, T2? t2 = default)
            where T1 : SchemaModel
            where T2 : PlaceholderModel
        {

            if (t1 is default(T1) && t2 is default(T2))
                throw new ArgumentNullException($"T1: {t1}\nT2: {t2}");

            List<string>? propertyNames = null;

            if (t1 != default(T1))
            {
                propertyNames = new List<string>();

                List<string>? propertyNamesT1 = Helper.GetPropertyNames<T1>();

                if (propertyNamesT1 != null && propertyNamesT1.Count > 0)
                {
                    propertyNames.AddRange(propertyNamesT1);
                }
            }

            if (t2 != default(T2))
            {
                propertyNames ??= new List<string>();

                List<string>? propertyNamesT2 = Helper.GetPropertyNames<T2>();

                if (propertyNamesT2 != null && propertyNamesT2.Count > 0)
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