namespace CassandraClient;

public interface ICassandraClient
{
    Task ExecuteAsync(string cql);
    Task<IEnumerable<Dictionary<string, object>>> QueryAsync(string cql);
    Task InsertAsync(string tableName, Dictionary<string, object> row);
    Task DeleteAsync(string tableName, string whereClause);
    Task BulkInsertAsync(string tableName, IEnumerable<Dictionary<string, object>> rows);
}