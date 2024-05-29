using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using TransactionService.Dtos;

namespace TransactionService.Services
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
            _channel.QueueDeclare(queue: "transaction_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var transactionService = scope.ServiceProvider.GetRequiredService<ITransactionService>();

                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    string response = await HandleMessageAsync(ea, message, transactionService);
                    HandleMessage(ea, response);
                }
            };

            _channel.BasicConsume(queue: "transaction_queue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }       

        private async Task<string> HandleMessageAsync(BasicDeliverEventArgs ea, string message, ITransactionService transactionService)
        {
            var response = string.Empty;

            if (message.Contains("get_all_transactions")) {
                var transactionData = JsonConvert.DeserializeObject<dynamic>(message);
                var transactionDto = JsonConvert.DeserializeObject<GetTransactionDto>(transactionData.Data.ToString());
                var transactions = await transactionService.GetAllAsync(transactionDto);
                response = JsonConvert.SerializeObject(transactions);
                return response;
            }

            if (message.Contains("create_transaction"))
            {
                var transactionData = JsonConvert.DeserializeObject<dynamic>(message);
                var transactionDto = JsonConvert.DeserializeObject<CreateTransactionDto>(transactionData.Data.ToString());
                int createdTransactionId = await transactionService.AddAsync(transactionDto);
                var getTransaction = await transactionService.GetByIdAsync(createdTransactionId);
                response = JsonConvert.SerializeObject(getTransaction);
                Console.WriteLine("Created new transaction");
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
