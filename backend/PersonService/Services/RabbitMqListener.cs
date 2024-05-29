using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using PersonService.Dtos;

namespace PersonService.Services
{
    public class RabbitMqListener : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMqListener(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"],
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"],
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "person_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var personService = scope.ServiceProvider.GetRequiredService<IPersonService>();

                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    string response = await HandleMessageAsync(ea, message, personService);
                    HandleMessage(ea, response);
                }
            };

            _channel.BasicConsume(queue: "person_queue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }       

        private async Task<string> HandleMessageAsync(BasicDeliverEventArgs ea, string message, IPersonService personService)
        {
            var response = string.Empty;

            if (message.Contains("get_all_clients")) {
                var clientData = JsonConvert.DeserializeObject<dynamic>(message);
                var clientDto = JsonConvert.DeserializeObject<CreatePersonDto>(clientData.Data.ToString());
                var clients = await personService.GetAllClientsAsync(clientDto.Role);
                response = JsonConvert.SerializeObject(clients);
                Console.WriteLine($"Total: {response}");
                Console.WriteLine($"Get all {clientDto.Role}");
                return response;
            }

            if (message.Contains("create_client")) {
                var clientData = JsonConvert.DeserializeObject<dynamic>(message);
                var clientDto = JsonConvert.DeserializeObject<CreatePersonDto>(clientData.Data.ToString());
                int createdClientId = await personService.AddAsync(clientDto);
                response = JsonConvert.SerializeObject(createdClientId);
                Console.WriteLine("Created new person");
                return response;
            }

            return "Invalid message";
        }

        private void HandleMessage(BasicDeliverEventArgs ea, string response)
        {
            var replyProps = _channel.CreateBasicProperties();
            replyProps.CorrelationId = ea.BasicProperties.CorrelationId;

            var responseBytes = Encoding.UTF8.GetBytes(response);
            _channel.BasicPublish(exchange: "", routingKey: ea.BasicProperties.ReplyTo, basicProperties: replyProps, body: responseBytes);
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
