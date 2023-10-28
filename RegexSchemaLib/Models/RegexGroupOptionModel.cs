
using RegexSchemaLib.Enums;

namespace RegexSchemaLib.Models
{
    /// <summary>
    /// Objekt/Model zum speichern von Regex Gruppen Optionen
    /// </summary>
    public class RegexGroupOptionModel
    {
        /// <summary>
        /// Konstruktor des Objekt/Model
        /// </summary>
        public RegexGroupOptionModel()
        {
            //Initialisiert mit den Default Werten
        }

        //ToDo: Validierung einbauen
        /// <summary>
        /// Konstruktor des Objekt/Model
        /// </summary>
        /// <param name="groupOptions">(enum) Regex Gruppen Optionen</param>
        /// <param name="groupNameOptions">(enum) Regex Gruppen Namen Optionen</param>
        public RegexGroupOptionModel(RegexGroupOption groupOptions, RegexGroupNameOption groupNameOptions) //bool validate = false
        {
            GroupOptions = groupOptions;
            GroupNameOptions = groupNameOptions;
        }

        //ToDo: Validierung einbauen
        /// <summary>
        /// Konstruktor des Objekt/Model
        /// </summary>
        /// <param name="groupOptions">(enum) Regex Grppen Optionen</param>
        public RegexGroupOptionModel(RegexGroupOption groupOptions) //bool validate = false
        {
            GroupOptions = groupOptions;

        }

        /// <summary>
        /// Speichert die Regex Gruppen Optionen
        /// </summary>
        public RegexGroupOption GroupOptions { get; set; } = RegexGroupOption.Named;

        /// <summary>
        /// Speichert die Regex Gruppen Namen Optionen
        /// </summary>
        public RegexGroupNameOption GroupNameOptions { get; set; } = RegexGroupNameOption.InheritName;
    }
}
