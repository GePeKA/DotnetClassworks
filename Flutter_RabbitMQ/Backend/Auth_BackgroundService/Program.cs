using Auth_BackgroundService.BackgroundServices;
using Auth_BackgroundService.Consumers;
using Auth_BackgroundService.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddHostedService<MessageSenderService>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<LogoutConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("amqp://localhost:5672", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("logout_exchange", e =>
        {
            e.ConfigureConsumer<LogoutConsumer>(context);
        });
    });
});

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
