namespace RegexSchemaLib.Enums
{
    /// <summary>
    /// Enumerationstyp für Fehler Typen
    /// </summary>
    public enum ErrorType
    {
        Runtime, //Laufzeit
        Library, //Bibliothek
        Data //Daten(wenn z.b der User eine unmögliche Value eingetragen hat)
    }
}
