using RegexSchemaLib.Classes;
using RegexSchemaLib.Models;
using System.Text.RegularExpressions;

string searchText = "§ 433 BGB XXXXXXXXXXSOMETEXTXXXXXXXXXXX\n\r§ 111 ZPO XXXXXXXXXXSOMETEXTXXXXXXXXXXX\n\r§ 333 ZPO XXXXXXXXXXSOMETEXTXXXXXXXXXXX";
string regexPattern = "§ [NZ] [GK]";

SchemaModel schema = new()
{
    RegexPattern = regexPattern
};

PlaceholderModel placeholderNZ = new()
{
    PlaceholderName = "[NZ]",
    RegexGroupName = "NZ",
    ReplaceValue = "\\d+"
};

PlaceholderModel placeholderGK = new()
{
    PlaceholderName = "[GK]",
    RegexGroupName = "GK",
    ReplaceValue = "(?:BGB|ZPO)"
};

schema.Placeholders.Add(placeholderNZ);
schema.Placeholders.Add(placeholderGK);



