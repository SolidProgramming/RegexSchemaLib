using System.Collections.Generic;

namespace RegexSchemaLib.Models
{
    public class SchemaModel
    {
        public string? SearchText { get; set; }
        public string? RegexPattern { get; set; }
        public List<PlaceholderModel> Placeholders { get; set; } = new List<PlaceholderModel>();
    }
}
