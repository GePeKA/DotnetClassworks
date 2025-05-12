using Auth_BackgroundService.Messages;
using Auth_BackgroundService.Services;
using MassTransit;

namespace Auth_BackgroundService.Consumers
{
    public class LogoutConsumer(IAuthService authService) : IConsumer<LogoutMessage>
    {
        public Task Consume(ConsumeContext<LogoutMessage> context)
        {
            var username = context.Message.Username;
            authService.Logout(username);
            Console.WriteLine($"[RabbitMQ] User '{username}' logged out");
            return Task.CompletedTask;
        }
    }
}
