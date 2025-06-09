using Nest;

namespace ElasticSearch.Entities
{
    public class Word
    {
        [Keyword(Name = "id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Text(Name = "value", Analyzer = "russian")]
        public string Value { get; set; } = string.Empty;

        [Number(NumberType.Integer, Name = "position")]
        public int Position { get; set; }

        [Keyword(Name = "textId")]
        public string TextId { get; set; } = string.Empty;
    }
}
