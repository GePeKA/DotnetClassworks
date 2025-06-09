using ClickHouse;
using ClickHouseApi.GenericRepository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IExpressionSqlTranslator, ExpressionToSqlVisitor>();
builder.Services.AddScoped<IClickHouseClient>(provider => 
    new ClickHouseClient(builder.Configuration.GetConnectionString("ClickHouse")!));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var clickHouseClient = scope.ServiceProvider.GetRequiredService<IClickHouseClient>();

    const string createDbSql = @"
        CREATE DATABASE IF NOT EXISTS dotnet
        ENGINE = Atomic";

    await clickHouseClient.ExecuteAsync(createDbSql);
    Console.WriteLine("Database 'dotnet' initialized (if not exists)");
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
