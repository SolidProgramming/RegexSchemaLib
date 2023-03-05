using RegexSchemaLib.Classes;
using RegexSchemaLib.Models;
using RegexSchemaLib.Structs;

#region schema 1
SchemaModel schema = new()
{
    SearchText = "SomeTextNotEmpty",
    Pattern = "§ [NZ] [GK]"
};

PlaceholderModel placeholderNZ = new()
{
    Name = "NZ",
    ReplaceValue = "\\d+"
};

PlaceholderModel placeholderGK = new()
{
    Name = "GK",
    ReplaceValue = "(?:BGB|ZPO)",
    ReplaceWithNamedGroup = true
};

schema.Placeholders.Add(placeholderNZ);
schema.Placeholders.Add(placeholderGK);
#endregion

#region schema 2
SchemaModel schema2 = new()
{
    SearchText = "SomeTextNotEmpty",
    Pattern = "Art. [NZ] [FW] [GK]"
};

placeholderNZ = new()
{
    Name = "NZ",
    ReplaceValue = "\\d+[a-z]{1,2}"
};

PlaceholderModel placeholderFW = new()
{
    Name = "FW",
    ReplaceValue = "(IV.) lit. [a-z]{1,2}) Nr. \\d+",
    ReplaceWithNamedGroup = true
};

placeholderGK = new()
{
    Name = "GK",
    ReplaceValue = "(?:BGB|ZPO)",
    ReplaceWithNamedGroup = true
};

schema2.Placeholders.Add(placeholderNZ);
schema2.Placeholders.Add(placeholderFW);
schema2.Placeholders.Add(placeholderGK);
#endregion

#region schema 3
SchemaModel schema3 = new()
{
    SearchText = "", //<== text is empty, library should throw error
    Pattern = "Art. [NZ] [FW] [GK]"
};

placeholderNZ = new()
{
    Name = "NZ",
    ReplaceValue = "\\d+[a-z]{1,2}"
};

placeholderFW = new()
{
    Name = "FW",
    ReplaceValue = "(IV.) lit. [a-z]{1,2}) Nr. \\d+",
    ReplaceWithNamedGroup = true
};

placeholderGK = new()
{
    Name = "GK",
    ReplaceValue = "(?:BGB|ZPO)",
    ReplaceWithNamedGroup = true
};

schema3.Placeholders.Add(placeholderNZ);
schema3.Placeholders.Add(placeholderFW);
schema3.Placeholders.Add(placeholderGK);
#endregion

List<SchemaModel> schemas = new List<SchemaModel>();

schemas.Add(schema);
schemas.Add(schema2);
schemas.Add(schema3);

foreach (SchemaModel _schema in schemas)
{
    foreach (PlaceholderModel placeholder in _schema.Placeholders)
    {
        Console.WriteLine(
            $"Create Named Group({placeholder.Name}): {(placeholder.ReplaceWithNamedGroup ? "Yes" : "No")}");
    }

    (string? result, ErrorModel? error) = new RegexSchema(_schema).CreateRegexPattern();
    string errorText = "";

    if (error is not null)
    {
        errorText = $"\nError:\n" +
        $"\tTyp: {error.Value.Type}\n" +
        $"\tMessage: {error.Value.Message}";
    }

    Console.WriteLine(
        "Pattern:\n" +
        $"\tresult: {result}\n" +
        $"\tschema: {_schema.Pattern}\n" +
        $"{(error == null ? "" : errorText)}");
    Console.WriteLine("---------------------------------------------------------------------------------");
}

Console.WriteLine("\nPress any key to exit");
Console.ReadKey();
