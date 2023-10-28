using RegexSchemaLib.Classes;
using RegexSchemaLib.Enums;
using RegexSchemaLib.Models;
using RegexSchemaLib.Structs;

#region schema 1
SchemaModel schema = new()
{
    Pattern = "§ [NZ] [GK]"
};

PlaceholderModel placeholderNZ = new(useDefaultGroupOptions: true)
{
    Name = "NZ",
    ReplaceValue = "\\d+"
};

PlaceholderModel placeholderGK = new(useDefaultGroupOptions: true)
{
    Name = "GK",
    ReplaceValue = "(?:BGB|ZPO)",
};

schema.Placeholders.Add(placeholderNZ);
schema.Placeholders.Add(placeholderGK);
#endregion

#region schema 2
SchemaModel schema2 = new()
{
    Pattern = "Art. [NZ] [FW] [GK]"
};

placeholderNZ = new(useDefaultGroupOptions: true)
{
    Name = "NZ",
    ReplaceValue = "\\d+[a-z]{1,2}"
};

PlaceholderModel placeholderFW = new(useDefaultGroupOptions: true)
{
    Name = "FW",
    ReplaceValue = "(IV.) lit. [a-z]{1,2}) Nr. \\d+",
};

placeholderGK = new(useDefaultGroupOptions: true)
{
    Name = "GK",
    ReplaceValue = "(?:BGB|ZPO)"
};

schema2.Placeholders.Add(placeholderNZ);
schema2.Placeholders.Add(placeholderFW);
schema2.Placeholders.Add(placeholderGK);
#endregion

#region schema 3
SchemaModel schema3 = new()
{
    Pattern = "THIS SCHEMA SHOULD THROW ERROR(Missing SearchText parameter)"
};
#endregion

List<SchemaModel> schemas = new()
{
    schema,
    schema2,
    schema3
};

int i = 0;

foreach (SchemaModel _schema in schemas)
{
    Console.WriteLine($"Schema #{i}:\n");

    foreach (PlaceholderModel placeholder in _schema.Placeholders)
    {
        Console.WriteLine(
            $"Create Named Group({placeholder.Name}): {(placeholder.GroupOptions.GroupOptions.HasFlag(RegexGroupOption.Named) ? "Yes" : "No")}");
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

    i++;
}

Console.WriteLine("\nPress any key to exit");
Console.ReadKey();
