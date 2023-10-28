using RegexSchemaLib.Enums;
using System.Collections.Generic;

namespace RegexSchemaLib.Models
{
    /// <summary>
    /// Objekt/Model zum speichern von Daten für Platzhalter
    /// </summary>
    public class PlaceholderModel
    {
        /// <summary>
        /// Konstruktor des Objekt/Model
        /// </summary>
        public PlaceholderModel()
        {
            //Initialisiert GroupOptions nicht
        }

        /// <summary>
        /// Konstruktor des Objekt/Model
        /// </summary>
        /// <param name="useDefaultGroupOptions">(bool) Standardwerte für Gruppenoptionen nutzen</param>
        public PlaceholderModel(bool useDefaultGroupOptions)
        {
            if (useDefaultGroupOptions)
            {
                //Initialisiert mit den Default Werten
                GroupOptions = new RegexGroupOptionModel();
            }
        }

        /// <summary>
        /// Konstruktor des Objekt/Model
        /// </summary>
        /// <param name="groupOptions">(enum) Regex Gruppen Optionen</param>
        /// <param name="groupNameOptions">(enum) Regex Gruppen Namen Optionen</param>
        public PlaceholderModel(RegexGroupOption groupOptions, RegexGroupNameOption groupNameOptions)
        {
            //Initialisiert mit den Werten aus den Parametern
            GroupOptions = new RegexGroupOptionModel(groupOptions, groupNameOptions);
        }

        /// <summary>
        /// Speichert den Namen
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Speichert den Wert mit dem der Platzhalter ersetzt wird
        /// </summary>
        public string? ReplaceValue { get; set; }

        /// <summary>
        /// Speichert die Gruppen Optionen
        /// </summary>
        public RegexGroupOptionModel? GroupOptions { get; set; }
    }
}
