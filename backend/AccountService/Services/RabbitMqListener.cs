using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using AccountService.Dtos;

namespace AccountService.Services
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
            _channel.QueueDeclare(queue: "account_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();

                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    string response = await HandleMessageAsync(ea, message, accountService);
                    HandleMessage(ea, response);
                }
            };

            _channel.BasicConsume(queue: "account_queue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }       

        private async Task<string> HandleMessageAsync(BasicDeliverEventArgs ea, string message, IAccountService accountService)
        {
            var response = string.Empty;

            if (message == "get_account") {
                var accountData = JsonConvert.DeserializeObject<dynamic>(message);
                var accountDto = JsonConvert.DeserializeObject<AccountDto>(accountData.Data.ToString());
                int getBalance = await accountService.GetByIdAsync(accountDto.Id);
                response = JsonConvert.SerializeObject(getBalance);
                Console.WriteLine("Get account");
                return response;
            }

            if (message.Contains("create_account"))
            {
                var accountData = JsonConvert.DeserializeObject<dynamic>(message);
                var accountDto = JsonConvert.DeserializeObject<CreateAccountDto>(accountData.Data.ToString());
                int createdAccountId = await accountService.AddAsync(accountDto);
                response = JsonConvert.SerializeObject(createdAccountId);
                Console.WriteLine("Created new account");
                return response;
            }

            if (message == "get_balance") {
                var accountData = JsonConvert.DeserializeObject<dynamic>(message);
                var accountDto = JsonConvert.DeserializeObject<AccountDto>(accountData.Data.ToString());
                int getBalance = await accountService.GetBalanceAsync(accountDto.Id);
                response = JsonConvert.SerializeObject(getBalance);
                Console.WriteLine("Get balance to account");
                return response;
            }

            if (message.Contains("delete_account")) {
                var accountData = JsonConvert.DeserializeObject<dynamic>(message);
                var accountDto = JsonConvert.DeserializeObject<DeleteAccountDto>(accountData.Data.ToString());
                bool getResult = await accountService.DeleteAsync(accountDto.ClientId);

                if (getResult) {
                    response = JsonConvert.SerializeObject(getResult);
                    Console.WriteLine("Delete account");
                    return response;
                } else {
                    Console.WriteLine("You cannot remove an account that has moved or does not exist");
                    return "You cannot remove an account that has moved or does not exist";
                }
            }

            if (message.Contains("deactivate_account")) {
                var accountData = JsonConvert.DeserializeObject<dynamic>(message);
                var accountDto = JsonConvert.DeserializeObject<DeleteAccountDto>(accountData.Data.ToString());
                bool getResult = await accountService.DeactivateAccountAsync(accountDto.ClientId);

                if (getResult) {
                    response = JsonConvert.SerializeObject(getResult);
                    Console.WriteLine("Deactivate account");
                    return response;
                }

                Console.WriteLine("It cannot be inactivated as it has no transactions");
                return "It cannot be inactivated as it has no transactions";
            }

            if (message.Contains("debit_credit")) {
                var accountData = JsonConvert.DeserializeObject<dynamic>(message);
                var accountDto = JsonConvert.DeserializeObject<DebitCreditAccountDto>(accountData.Data.ToString());
                bool getResult = await accountService.DebitCredit(accountDto);

                if (getResult) {
                    response = JsonConvert.SerializeObject(getResult);
                    Console.WriteLine("Amount to account updated");
                    return response;
                }

                Console.WriteLine("Amount not updated");
                return "Amount not updated";
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
