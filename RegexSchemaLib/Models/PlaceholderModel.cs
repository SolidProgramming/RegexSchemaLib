using System.Collections.Generic;

namespace RegexSchemaLib.Models
{
    public class PlaceholderModel
    {
        public string? Name { get; set; }
        public string? ReplaceValue { get; set; }
        public bool ReplaceWithNamedGroup { get; set; }
    }
}
