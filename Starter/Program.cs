using RegexSchemaLib.Classes;
using RegexSchemaLib.Models;
using RegexSchemaLib.Structs;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

string searchText = "§ 433 BGB XXXXXXXXXXSOMETEXTXXXXXXXXXXX\n\r§ 111 ZPO XXXXXXXXXXSOMETEXTXXXXXXXXXXX\n\r§ 333 ZPO XXXXXXXXXXSOMETEXTXXXXXXXXXXX";
string regexPattern = "§ [NZ] [GK]";

SchemaModel schema = new()
{
    SearchText = searchText,
    RegexPattern = regexPattern
};

PlaceholderModel placeholderNZ = new()
{
    Name = "[NZ]",
    RegexGroupName = "NZ",
    ReplaceValue = "\\d+"
};

PlaceholderModel placeholderGK = new()
{
    Name = "[GK]",
    RegexGroupName = "GK",
    ReplaceValue = "(?:BGB|ZPO)"
};

schema.Placeholders.Add(placeholderNZ);
schema.Placeholders.Add(placeholderGK);

(string? result, ErrorModel? error) = new RegexSchema(schema).CreateRegexPattern();
string errorText = "";

if (error is not null)
{
    errorText = $"\nError:\n" +
    $"\tTyp: {error.Value.Type}\n" +
    $"\tMessage: {error.Value.Message}";
}

Console.WriteLine($"Result:\n" +
    $"Pattern: {result}" +
    $"{(error == null ? "" : errorText)}" +
    $"\n\n");

Console.WriteLine("Press any key to exit");
Console.ReadKey();

