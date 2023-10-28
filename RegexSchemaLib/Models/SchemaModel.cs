using System.Collections.Generic;

namespace RegexSchemaLib.Models
{
    public class SchemaModel
    {
        public string? Pattern { get; set; }
        public List<PlaceholderModel> Placeholders { get; set; } = new List<PlaceholderModel>();
    }
}
