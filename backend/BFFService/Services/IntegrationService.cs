using BFFService.Dtos;
using RabbitMQ.Client;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using TransactionService.Dtos;

namespace BFFService.Services
{
    public class IntegrationService : IIntegrationService
    {
        private readonly IDistributedCache _cache;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public IntegrationService(IDistributedCache cache)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "person_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: "account_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: "transaction_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _cache = cache;
        }

        public async Task<string> CreateClientAsync(CreateClientDto clientDto)
        {
            var message = JsonConvert.SerializeObject(new { Action = "create_client", Data = clientDto });
            
            var messageGet = JsonConvert.SerializeObject(new { Action = "get_all_clients", Data = new { Role = clientDto.Role } });
            return await CreateCacheAsync(
                "all_clients",
                async () => await SendMessageAsync("person_queue", message),
                "person_queue",
                messageGet
            );
        }

        public async Task<string> CreateAccountAsync(CreateAccountDto accountDto)
        {
            var message = JsonConvert.SerializeObject(new { Action = "create_account", Data = accountDto });
            var messageGet = JsonConvert.SerializeObject(new { Action = "get_all_clients", Data = new { Role = "client" } });

            await CreateCacheAsync(
                "account_clients",
                async () => await SendMessageAsync("account_queue", message),
                "account_queue",
                messageGet
            );

            return await CreateCacheAsync(
                "all_clients",
                async () => await SendMessageAsync("person_queue", messageGet),
                "person_queue",
                messageGet
            );
        }

        public async Task<string> CreateTransactionAsync(CreateTransactionDto transactionDto)
        {
            var message = JsonConvert.SerializeObject(new { Action = "create_transaction", Data = transactionDto });
            var messageGet = JsonConvert.SerializeObject(
                new {
                    Action = "get_all_transactions",
                    Data = new { 
                        From = transactionDto.TransactionDate,
                        To = transactionDto.TransactionDate,
                        AccountId = transactionDto.AccountId
                    }
                }
            );

            await CreateCacheAsync(
                "transactions",
                async () => await SendMessageAsync("transaction_queue", message),
                "transaction_queue",
                messageGet
            );

            var messageGet2 = JsonConvert.SerializeObject(new { Action = "debit_credit", Data = new { AccountId = transactionDto.AccountId, Amount = transactionDto.Amount, TransactionType = transactionDto.TransactionType } });
            await CreateCacheAsync(
                "account_debit_credit",
                async () => await SendMessageAsync("account_queue", messageGet2),
                "account_queue",
                messageGet2
            );
            var messageGet3 = JsonConvert.SerializeObject(new { Action = "get_all_clients", Data = new { Role = "client" } });
            return await CreateCacheAsync(
                "all_clients",
                async () => await SendMessageAsync("person_queue", messageGet3),
                "person_queue",
                messageGet3
            );
        }

        public async Task<string> GetAllClientsAsync(GetAllClientDto getAllClients)
        {
            var message = JsonConvert.SerializeObject(new { Action = "get_all_clients", Data = getAllClients });
            return await GetCacheAsync(
                "all_clients",
                async () => await SendMessageAsync("person_queue", message),
                "person_queue",
                message
            );
        }

        public async Task<string> GetAllAccountsAsync()
        {
            var message = JsonConvert.SerializeObject(new { Action = "get_account" });
            return await GetCacheAsync(
                "account",
                async () => await SendMessageAsync("account_queue", message),
                "account_queue",
                message
            );
        }

        public async Task<string> DeleteAccountAsync(int accountId)
        {
            var message = JsonConvert.SerializeObject(new { Action = "delete_account", Data = new { ClientId = accountId } });
            var messageGet = JsonConvert.SerializeObject(new { Action = "get_all_clients", Data = new { Role = "client" } });

            await CreateCacheAsync(
                "account_clients",
                async () => await SendMessageAsync("account_queue", message),
                "account_queue",
                messageGet
            );

            return await CreateCacheAsync(
                "all_clients",
                async () => await SendMessageAsync("person_queue", messageGet),
                "person_queue",
                messageGet
            );
        }

        public async Task<string> DeactivateAccountAsync(int accountId)
        {
            var message = JsonConvert.SerializeObject(new { Action = "deactivate_account", Data = new { ClientId = accountId } });
            var messageGet = JsonConvert.SerializeObject(new { Action = "get_all_clients", Data = new { Role = "client" } });

            await CreateCacheAsync(
                "account_clients",
                async () => await SendMessageAsync("account_queue", message),
                "account_queue",
                messageGet
            );

            return await CreateCacheAsync(
                "all_clients",
                async () => await SendMessageAsync("person_queue", messageGet),
                "person_queue",
                messageGet
            );
        }

        public async Task<string> GetClientByIdAsync(int clientId)
        {
            return await SendMessageAsync("person_queue", $"get_client_by_id:{clientId}");
        }

        public async Task<string> GetAccountByIdAsync(int accountId)
        {
            return await SendMessageAsync("account_queue", $"get_account_by_id:{accountId}");
        }

        public async Task<string> GetAllTransactionAsync(int AccountId, DateTime From, DateTime To)
        {
            var messageGet = JsonConvert.SerializeObject(
                new {
                    Action = "get_all_transactions",
                    Data = new {
                        AccountId = AccountId,
                        From = From,
                        To = To
                    }
                }
            );

            return await CreateCacheAsync(
                "transactions",
                async () => await SendMessageAsync("transaction_queue", messageGet),
                "transaction_queue",
                messageGet
            );
        }

        /**************************************************************
        * Method to get cache
        **/
        private async Task<string> GetCacheAsync(string cacheKey, Func<Task<string>> getData, string queue, string queueKey)
        {
            Console.WriteLine($"get Cache Luiz: {cacheKey} {getData} {queue} {queueKey}");

            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return cachedData;
            }

            var data = await getData();
            if (!string.IsNullOrEmpty(data))
            {
                var getAll = await SendMessageAsync(queue, queueKey);
                await _cache.SetStringAsync(cacheKey, getAll);
                return data;
            }

            return "No data available.";
        }

        /**************************************************************
        * Method to create cache
        **/
        private async Task<string> CreateCacheAsync(string cacheKey, Func<Task<string>> getData, string queue, string queueKey)
        {
            Console.WriteLine($"create Cache Luiz: {cacheKey} {getData} {queue} {queueKey}");

            var data = await getData();
            if (!string.IsNullOrEmpty(data))
            {
                var getAll = await SendMessageAsync(queue, queueKey);
                await _cache.SetStringAsync(cacheKey, getAll);
                return data;
            }

            return "No data available.";
        }

        /**************************************************************
        * Method to send message to queue
        **/
        private async Task<string> SendMessageAsync(string queue, string message)
        {
            var correlationId = Guid.NewGuid().ToString();
            var replyQueueName = _channel.QueueDeclare().QueueName;
            var consumer = new EventingBasicConsumer(_channel);
            var tcs = new TaskCompletionSource<string>();

            consumer.Received += (model, ea) =>
            {
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    var body = ea.Body.ToArray();
                    var response = Encoding.UTF8.GetString(body);
                    tcs.SetResult(response);
                }
            };

            _channel.BasicConsume(consumer: consumer, queue: replyQueueName, autoAck: true);

            var props = _channel.CreateBasicProperties();
            props.ReplyTo = replyQueueName;
            props.CorrelationId = correlationId;

            var messageBytes = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: props, body: messageBytes);

            return await tcs.Task;
        }
    }
}
