namespace ElasticSearch.Dtos
{
    public class UpdateWordRequest
    {
        public int Position { get; set; }
        public string NewValue { get; set; } = null;
    }
}
