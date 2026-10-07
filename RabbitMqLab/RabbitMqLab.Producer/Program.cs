using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ConnectionFactory { HostName = "localhost" };

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

// Declare the queue; if it not exists, it will be created. If it already exists with the same
// parameters, nothing happens.
await channel.QueueDeclareAsync(
    queue: "hellos",
    durable: false,
    exclusive: false,
    autoDelete: false);

Console.WriteLine("Product ready. Writte a message and press Enter (or 'exit' to end):");

string? input;
while ((input = Console.ReadLine()) != "exit")
{
    byte[] body = Encoding.UTF8.GetBytes(input ?? string.Empty);

    await channel.BasicPublishAsync(
        exchange: string.Empty, // exchange "nameless"/default
        routingKey: "hellos",  // in deafult exchange, routingKey == queue name
        body: body);

    Console.WriteLine($"Sended: {input}");
}