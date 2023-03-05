using RegexSchemaLib.Classes;
using RegexSchemaLib.Models;
using RegexSchemaLib.Structs;
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

(string? result, ErrorModel? error) = new RegexSchema(schema).SchemaReplace();
Console.WriteLine(result);



