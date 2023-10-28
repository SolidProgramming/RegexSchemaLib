namespace RegexSchemaLib.Enums
{
    /// <summary>
    /// Enumerationstyp mit Flag Attribut für Gruppenoptionen von regulären Ausdrücken
    /// </summary>
    [Flags]
    public enum RegexGroupOption
    {
        Named = 1, //Benannt
        Optional = 2, //Optional
        UnNamed = 4, //Unbenannt
    }
}
