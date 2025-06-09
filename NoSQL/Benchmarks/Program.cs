#region ПОДГОТОВКА КЛИЕНТОВ И ДАННЫХ

using ClickHouse;
using System.Diagnostics;

var getManyTests = (int idFrom, int idTo) =>
{
    var tests = new List<Dictionary<string, object>>();

    for (int i = idFrom; i < idTo; i++)
    {
        tests.Add(
            new()
            {
                { "id", i },
                { "value", $"ABC_{i}" }
            }
        );
    }

    return tests;
};

var cassandraClient = new CassandraClient.CassandraClient("Contact Points=127.0.0.1;Port=9042", "test_keyspace");
var tableName = "test";

var clickHouseClient = new ClickHouseClient("Host=localhost;Port=8123;Username=default;Password=default;Database=dotnet");
await clickHouseClient.ExecuteAsync($"CREATE TABLE IF NOT EXISTS {tableName} (id Int32, value String) ENGINE = MergeTree() ORDER BY tuple()");

var tests_2000 = getManyTests(1, 2000);

await clickHouseClient.ExecuteAsync($"TRUNCATE TABLE {tableName}");
await cassandraClient.DeleteAsync(tableName, "");

#endregion

#region ВСТАВКА (единожды)

var sw = Stopwatch.StartNew();
await cassandraClient.BulkInsertAsync(tableName, tests_2000);
sw.Stop();
Console.WriteLine($"Cassandra BulkInsert: {sw.Elapsed.TotalMilliseconds:F2} ms");

sw.Restart();
await clickHouseClient.BulkInsertAsync(tableName, tests_2000);
sw.Stop();
Console.WriteLine($"ClickHouse BulkInsert: {sw.Elapsed.TotalMilliseconds:F2} ms");

#endregion

#region БЕНЧМАРКИ (100 прогонов каждой операции)

async Task Benchmark(string label, Func<Task> action)
{
    const int iterations = 100;
    var total = 0.0;

    for (int i = 0; i < iterations; i++)
    {
        var watch = Stopwatch.StartNew();
        await action();
        watch.Stop();
        total += watch.Elapsed.TotalMilliseconds;
    }

    Console.WriteLine($"{label}: {(total / iterations):F2} ms (avg over {iterations} runs)");
}

var testRow = new Dictionary<string, object> { { "id", 999999 }, { "value", "TestValue" } };

Console.WriteLine("\n=== SELECT * ===");
await Benchmark("Cassandra SELECT *", () => cassandraClient.QueryAsync($"SELECT * FROM {tableName}"));
await Benchmark("ClickHouse SELECT *", () => clickHouseClient.QueryAsync($"SELECT * FROM {tableName}"));

Console.WriteLine("\n=== SELECT WHERE ID = 1000 ===");
await Benchmark("Cassandra SELECT WHERE id = 1000", () => cassandraClient.QueryAsync($"SELECT * FROM {tableName} WHERE id = 1000"));
await Benchmark("ClickHouse SELECT WHERE id = 1000", () => clickHouseClient.QueryAsync($"SELECT * FROM {tableName} WHERE id = 1000"));

Console.WriteLine("\n=== SELECT WHERE id BETWEEN 1000 AND 1010 ===");
await Benchmark("Cassandra SELECT WHERE id BETWEEN", () => cassandraClient.QueryAsync($"SELECT * FROM {tableName} WHERE id >= 1000 AND id <= 1010 ALLOW FILTERING"));
await Benchmark("ClickHouse SELECT WHERE id BETWEEN", () => clickHouseClient.QueryAsync($"SELECT * FROM {tableName} WHERE id BETWEEN 1000 AND 1010"));

Console.WriteLine("\n");
await Benchmark("Cassandra SELECT WHERE value = 100", () => cassandraClient.QueryAsync($"SELECT * FROM {tableName} WHERE value='100'"));
await Benchmark("Clickhouse SELECT WHERE value = 100", () => clickHouseClient.QueryAsync($"SELECT * FROM {tableName} WHERE value='100'"));

Console.WriteLine("\n=== INSERT ===");
// перед каждым вставляем и удаляем, чтобы избежать конфликта
async Task CassandraInsertTest()
{
    await cassandraClient.DeleteAsync(tableName, "(id = 999999)");
    await cassandraClient.InsertAsync(tableName, testRow);
}

async Task ClickHouseInsertTest()
{
    await clickHouseClient.DeleteAsync(tableName, "(id = 999999)");
    await clickHouseClient.InsertAsync(tableName, testRow);
}

await Benchmark("Cassandra Insert (id=999999)", CassandraInsertTest);
await Benchmark("ClickHouse Insert (id=999999)", ClickHouseInsertTest);

Console.WriteLine("\n=== UPDATE ===");
async Task CassandraUpdateTest()
{
    await cassandraClient.ExecuteAsync($"UPDATE {tableName} SET value = 'Updated' WHERE id = 1");
}

async Task ClickHouseUpdateTest()
{
    await clickHouseClient.ExecuteAsync($"ALTER TABLE {tableName} DELETE WHERE id = 1");
    await clickHouseClient.InsertAsync(tableName, new Dictionary<string, object> { { "id", 1 }, { "value", "Updated" } });
}

await Benchmark("Cassandra UPDATE (id=1)", CassandraUpdateTest);
await Benchmark("ClickHouse UPDATE (id=1, simulated)", ClickHouseUpdateTest);

Console.WriteLine("\n=== DELETE ===");
// удаляем и переинсертим для следующей итерации
async Task CassandraDeleteTest()
{
    await cassandraClient.DeleteAsync(tableName, "id = 500");
    await cassandraClient.InsertAsync(tableName, new Dictionary<string, object> { { "id", 500 }, { "value", "Restored" } });
}

async Task ClickHouseDeleteTest()
{
    await clickHouseClient.ExecuteAsync($"ALTER TABLE {tableName} DELETE WHERE id = 500");
    await clickHouseClient.InsertAsync(tableName, new Dictionary<string, object> { { "id", 500 }, { "value", "Restored" } });
}

await Benchmark("Cassandra DELETE (id=500)", CassandraDeleteTest);
await Benchmark("ClickHouse DELETE (id=500)", ClickHouseDeleteTest);

#endregion