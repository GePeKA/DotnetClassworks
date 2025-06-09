using Nest;
using ElasticSearch.Entities;

namespace ElasticSearch;

public class ElasticService(IElasticClient client)
{
    private const string IndexName = "words";

    public async Task<string> AddTextAsync(string text)
    {
        var textId = Guid.NewGuid().ToString();
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select((word, index) => new Word
                        {
                            TextId = textId,
                            Value = word.ToLower(),
                            Position = index
                        })
                        .ToList();

        var bulkResponse = await client.BulkAsync(b => b
            .Index(IndexName)
            .IndexMany(words));

        if (bulkResponse.Errors)
            throw new Exception("Bulk insert failed.");

        return textId;
    }

    public async Task<IEnumerable<Word>> GetWordsByTextIdAsync(string textId)
    {
        var searchResponse = await client.SearchAsync<Word>(s => s
            .Index(IndexName)
            .Query(q => q.Term("textId.keyword", textId))
            .Size(1000));

        return searchResponse.Documents;
    }

    public async Task<Word> AddWordToTextAsync(string textId, string wordValue, int position)
    {
        var word = new Word
        {
            TextId = textId,
            Value = wordValue.ToLower(),
            Position = position
        };

        var response = await client.IndexDocumentAsync(word);
        if (!response.IsValid)
            throw new Exception("Failed to add word.");

        return word;
    }

    public async Task<IEnumerable<(string Match, Word Word)>> SearchByWordOrSyllableAsync(string pattern)
    {
        var response = await client.SearchAsync<Word>(s => s
            .Index(IndexName)
            .Query(q => q.MatchPhrase(m => m
                .Field(f => f.Value)
                .Query(pattern.ToLower())))
            .Highlight(h => h
                .Fields(f => f
                    .Field(w => w.Value)
                    .PreTags("<b>")
                    .PostTags("</b>")))
            .Size(1000));

        return response.Hits.Select(hit => (
            hit.Highlight.TryGetValue("value", out var highlights)
                ? highlights.FirstOrDefault() ?? hit.Source.Value
                : hit.Source.Value,
            hit.Source));
    }

    public async Task UpdateWordInTextAsync(string textId, int position, string newValue)
    {
        await client.UpdateByQueryAsync<Word>(u => u
            .Query(q => q
                .Bool(b => b
                    .Must(
                        m => m.Term("textId.keyword", textId),
                        m => m.Term("position", position)
                    )
                )
            )
            .Script(s => s
                .Source("ctx._source.value = params.newValue")
                .Params(p => p.Add("newValue", newValue))
            )
        );
    }

    public async Task DeleteWordFromTextAsync(string textId, int position)
    {
        var word = await FindWordByTextIdAndPositionAsync(textId, position);
        if (word is null)
            throw new Exception("Not found");

        var response = await client.DeleteAsync<Word>(word.Id, d => d.Index(IndexName));
        if (!response.IsValid)
            throw new Exception("Delete failed.");
    }

    private async Task<Word?> FindWordByTextIdAndPositionAsync(string textId, int position)
    {
        var response = await client.SearchAsync<Word>(s => s
            .Index(IndexName)
            .Query(q => q.Bool(b => b
                .Must(
                    m => m.Term("textId.keyword", textId),
                    m => m.Term("position", position)
                )))
            .Size(1));

        return response.Documents.FirstOrDefault();
    }
}
