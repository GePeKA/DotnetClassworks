using Auth_BackgroundService.Messages;
using MassTransit;

namespace Auth_BackgroundService.BackgroundServices
{
    public class MessageSenderService(IBus bus) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var count = 1;
            while (!stoppingToken.IsCancellationRequested)
            {
                await bus.Publish(new SimpleMessage() { Text = $"Сообщение {count}. Отправлено в {DateTime.Now}" }, stoppingToken);
                count++;
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
