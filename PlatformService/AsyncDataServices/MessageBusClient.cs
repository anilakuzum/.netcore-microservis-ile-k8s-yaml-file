using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabitMqHost"],
                Port = int.Parse(_configuration["5672"])
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: " trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("Connected to MessageBus");
            }
            catch (Exception ex)
            {

                Console.WriteLine($" Could not connet to the Message Bus:  {ex.Message}");
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine(" RabbitMq ConnectionShutdown");
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger",
                                  routingKey: "",
                                  basicProperties: null,
                                  body: body);

            Console.WriteLine($"We have sent {message}");

        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus Dispose");
            if (_channel.IsOpen)
                _channel.Close();
            _connection.Close();

        }

        public void PublishNewPlatform(PlatformPublishDto platformPublishDto)
        {
            var msg = JsonSerializer.Serialize(platformPublishDto);

            if (_connection.IsOpen)
            {
                Console.WriteLine("RabbitMQ Connection Open, sending message...");
                SendMessage(msg);
            }
            else
            {
                Console.WriteLine("RabbitMQ Connection close, not message...");
            }
        }
    }
}
