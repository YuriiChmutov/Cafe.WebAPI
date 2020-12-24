using Cafe.API.Models.Entities;
using Cafe.API.Models.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Cafe.API.Services;

namespace Cafe.API.ThirdLaba
{
    public class Consumer : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory scopeFactory;
        private int executionCount = 0;
        private Timer _timer;

        public Consumer(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(8));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var rand = new Random();
                
                var _dataRepository = scope.ServiceProvider.GetRequiredService<IDataRepository<Client>>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Consumer>>();

                var count = Interlocked.Increment(ref executionCount);
                
                var client = new Client
                {
                    Name = ClientParams.Names[GetRandom.returnRandom(ClientParams.Names.Count)],
                    SecondName = ClientParams.SecondNames[GetRandom.returnRandom(ClientParams.SecondNames.Count)],
                    Age = rand.Next(15, 80),
                    IsHungry = true,
                    TimeOfComing = DateTime.Now
                };

                _dataRepository.Add(client);
                logger.LogInformation($"[{client.Id}] The client {client.Name.ToUpper()} {client.SecondName.ToUpper()} with id {client.Id} came.");
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
