using ClickHouse;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ClickHouseApi.GenericRepository;

public class GenericRepository<T>(IClickHouseClient client, IExpressionSqlTranslator translator): IGenericRepository<T> where T : class, new()
{
    private readonly string _tableName = ToSnakeCase(typeof(T).Name);
    private readonly PropertyInfo[] _properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

    public async Task CreateTableAsync()
    {
        var columnsSql = new StringBuilder();

        foreach (var prop in _properties)
        {
            var colName = ToSnakeCase(prop.Name);
            var colType = MapClrTypeToClickHouse(prop.PropertyType);
            columnsSql.Append($"{colName} {colType}, ");
        }

        if (columnsSql.Length > 2)
            columnsSql.Length -= 2; // убрать последнюю запятую

        var sql = $"CREATE TABLE IF NOT EXISTS {_tableName} ({columnsSql}) ENGINE = MergeTree() ORDER BY tuple()";

        await client.ExecuteAsync(sql);
    }

    public async Task DeleteTableAsync()
    {
        var sql = $"DROP TABLE IF EXISTS {_tableName}";
        await client.ExecuteAsync(sql);
    }

    public async Task InsertAsync(T entity)
    {
        var row = new Dictionary<string, object>();

        foreach (var prop in _properties)
        {
            row[ToSnakeCase(prop.Name)] = prop.GetValue(entity) ?? DBNull.Value;
        }

        await client.InsertAsync(_tableName, row);
    }

    public async Task BulkInsertAsync(IEnumerable<T> entities)
    {
        var rows = new List<Dictionary<string, object>>();

        foreach (var entity in entities)
        {
            var row = new Dictionary<string, object>();
            foreach (var prop in _properties)
            {
                row[ToSnakeCase(prop.Name)] = prop.GetValue(entity) ?? DBNull.Value;
            }
            rows.Add(row);
        }

        await client.BulkInsertAsync(_tableName, rows);
    }

    /// <summary>
    /// Удаляет записи из таблицы по условию в виде SQL строки, например "id = 5"
    /// </summary>
    public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
    {
        var whereClause = translator.Translate(predicate);

        await client.DeleteAsync(_tableName, whereClause);
    }

    /// <summary>
    /// Возвращает список сущностей, удовлетворяющих условию predicate.
    /// Простейшая трансляция Expression в SQL WHERE.
    /// </summary>
    public async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate)
    {
        var whereClause = translator.Translate(predicate);
        var sql = $"SELECT * FROM {_tableName} WHERE {whereClause}";

        var rows = await client.QueryAsync(sql);

        var result = new List<T>();
        foreach (var row in rows)
        {
            var entity = new T();
            foreach (var prop in _properties)
            {
                var colName = ToSnakeCase(prop.Name);
                if (row.TryGetValue(colName, out var value) && value != DBNull.Value)
                {
                    if (prop.PropertyType == typeof(DateOnly) && value is DateTime dateTimeValue)
                    {
                        prop.SetValue(entity, DateOnly.FromDateTime(dateTimeValue));
                    }
                    else if (prop.PropertyType.IsEnum)
                    {
                        prop.SetValue(entity, Enum.ToObject(prop.PropertyType, value));
                    }
                    else
                    {
                        prop.SetValue(entity, Convert.ChangeType(value, prop.PropertyType));
                    }
                }
            }
            result.Add(entity);
        }
        return result;
    }

    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var sb = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            var ch = input[i];
            if (char.IsUpper(ch) && i > 0)
                sb.Append('_');
            sb.Append(char.ToLowerInvariant(ch));
        }
        return sb.ToString();
    }

    private static string MapClrTypeToClickHouse(Type type)
    {
        var t = Nullable.GetUnderlyingType(type) ?? type;

        return t switch
        {
            _ when t == typeof(int) => "Int32",
            _ when t == typeof(long) => "Int64",
            _ when t == typeof(short) => "Int16",
            _ when t == typeof(byte) => "UInt8",
            _ when t == typeof(bool) => "UInt8",
            _ when t == typeof(string) => "String",
            _ when t == typeof(DateTime) => "DateTime",
            _ when t == typeof(DateOnly) => "Date",
            _ when t == typeof(float) => "Float32",
            _ when t == typeof(double) => "Float64",
            _ when t == typeof(Guid) => "UUID",
            _ when t.IsEnum => "UInt8",
            _ => throw new NotSupportedException($"Тип {t.Name} не поддерживается для ClickHouse.")
        };
    }
}