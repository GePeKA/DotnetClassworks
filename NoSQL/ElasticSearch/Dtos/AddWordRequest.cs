namespace ElasticSearch.Dtos
{
    public class AddWordRequest
    {
        public string Value { get; set; } = null!;
        public int Position { get; set; }
    }
}
