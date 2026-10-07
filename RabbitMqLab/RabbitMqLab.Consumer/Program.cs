using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new ConnectionFactory { HostName = "localhost" };

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(
    queue: "hellos",
    durable: false,
    exclusive: false,
    autoDelete: false);

AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += async (sender, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received: {message}");

    await Task.CompletedTask;
};

await channel.BasicConsumeAsync(
    queue: "hellos",
    autoAck: true,
    consumer: consumer);

Console.WriteLine("Consumer listened. Press Enter to exit.");
Console.ReadLine();