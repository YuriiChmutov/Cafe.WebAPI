using Cafe.API.Models.Entities;
using Cafe.API.Models.Repository;
using Cafe.API.Static;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.API.ThirdLaba
{
    public class Producer : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory scopeFactory;
        private int executionCount = 0;
        private Timer _timer;

        public Producer(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                Random random = new Random();
                var _clients = scope.ServiceProvider.GetRequiredService<IDataRepository<Client>>();
                var _sales = scope.ServiceProvider.GetRequiredService <IDataRepository<ClientProduct>>();
                var _dishes = scope.ServiceProvider.GetRequiredService<IDataRepository<Product>>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Producer>>();

                var count = Interlocked.Increment(ref executionCount);

                var firstHungryClient = _clients.GetLast();

                if(firstHungryClient == null)
                {
                    logger.LogInformation($"No hungry clients".ToUpper());
                }
                else
                {
                    var randomIdOfDish = random.Next(1, _dishes.Count());

                    logger.LogInformation($"[{firstHungryClient.Id}] {firstHungryClient.Name.ToUpper()}" +
                        $" {firstHungryClient.SecondName.ToUpper()} is hungry." +
                        $"You should give him something to eat or drink! Hurry up!");

                    var sale = new ClientProduct()
                    {
                        ClientId = firstHungryClient.Id,
                        ProductId = randomIdOfDish
                    };

                    _sales.Add(sale);

                    var servedClient = new Client
                    {
                        Name = firstHungryClient.Name,
                        SecondName = firstHungryClient.SecondName,
                        Age = firstHungryClient.Age,
                        TimeOfComing = firstHungryClient.TimeOfComing,
                        IsHungry = false
                    };

                    _clients.Update(firstHungryClient, servedClient);

                    logger.LogInformation($"[{firstHungryClient.Id}]{firstHungryClient.Name.ToUpper()}" +
                        $" {firstHungryClient.SecondName.ToUpper()}" +
                        $" got a dish {_dishes.Get(randomIdOfDish).Name}" +
                        $" which costs {_dishes.Get(randomIdOfDish).Price}. Bon appetit!");
                }
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            //_logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
