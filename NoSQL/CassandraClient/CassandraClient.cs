using Cassandra;

namespace CassandraClient;

public class CassandraClient(string connectionString, string keyspace) : ICassandraClient
{
    private readonly Cluster _cluster = Cluster.Builder()
                                        .WithConnectionString(connectionString)
                                        .Build();
    private ISession? _session;
    private readonly string _keyspace = keyspace;

    public async Task ExecuteAsync(string cql)
    {
        var session = await GetSessionAsync();
        await session.ExecuteAsync(new SimpleStatement(cql));
    }

    public async Task<IEnumerable<Dictionary<string, object>>> QueryAsync(string cql)
    {
        var session = await GetSessionAsync();
        var result = await session.ExecuteAsync(new SimpleStatement(cql));

        var columnNames = result.Columns.Select(c => c.Name).ToArray();
        var rows = new List<Dictionary<string, object>>();

        foreach (var row in result)
        {
            var rowDict = new Dictionary<string, object>();
            foreach (var columnName in columnNames)
            {
                var value = row.GetValue<object>(columnName);
                rowDict[columnName] = value;
            }
            rows.Add(rowDict);
        }

        return rows;
    }

    public async Task InsertAsync(string tableName, Dictionary<string, object> row)
    {
        var session = await GetSessionAsync();
        var columns = string.Join(", ", row.Keys.Select(ToSnakeCase));
        var values = string.Join(", ", row.Keys.Select(k => "?"));

        var statement = await session.PrepareAsync($"INSERT INTO {tableName} ({columns}) VALUES ({values})");
        await session.ExecuteAsync(statement.Bind([.. row.Values]));
    }

    public async Task DeleteAsync(string tableName, string whereClause)
    {
        var session = await GetSessionAsync();

        var cql = string.IsNullOrWhiteSpace(whereClause)
            ? $"TRUNCATE {_keyspace}.{tableName}"
            : $"DELETE FROM {tableName} WHERE {whereClause}";

        await session.ExecuteAsync(new SimpleStatement(cql));
    }

    public async Task BulkInsertAsync(string tableName, IEnumerable<Dictionary<string, object>> rows)
    {
        var session = await GetSessionAsync();
        var firstRow = rows.FirstOrDefault();

        if (firstRow == null) return;

        var columns = firstRow.Keys.Select(ToSnakeCase).ToArray();
        var prepared = await session.PrepareAsync(
            $"INSERT INTO {tableName} ({string.Join(", ", columns)}) VALUES ({string.Join(", ", columns.Select(c => "?"))})");

        foreach (var row in rows)
        {
            // Привязка значений в порядке колонок
            var values = columns.Select(col =>
            {
                // Получаем исходное имя колонки из snake_case
                var originalKey = firstRow.Keys.FirstOrDefault(k => ToSnakeCase(k) == col);
                if (originalKey == null || !row.TryGetValue(originalKey, out var val))
                    return null!;
                return val ?? null!;
            }).ToArray();

            await session.ExecuteAsync(prepared.Bind(values));
        }
    }

    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            var ch = input[i];
            if (char.IsUpper(ch))
            {
                if (i > 0) sb.Append('_');
                sb.Append(char.ToLowerInvariant(ch));
            }
            else
            {
                sb.Append(ch);
            }
        }
        return sb.ToString();
    }

    private async Task<ISession> GetSessionAsync()
    {
        _session ??= await _cluster.ConnectAsync(_keyspace);
        return _session;
    }
}
