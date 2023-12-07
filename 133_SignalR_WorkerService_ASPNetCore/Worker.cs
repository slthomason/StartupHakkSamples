using Microsoft.AspNetCore.SignalR;

namespace WebApplication18
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHubContext<StockHub> _stockHub;
        private const string stockName = "Basic Stock Name";
        private const decimal stockPrice = 100;

        public Worker(ILogger<Worker> logger, IHubContext<StockHub> stockHub)
        {
            _logger = logger;
            _stockHub = stockHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                //do some stock works

                Random rnd = new Random();
                decimal stockRaise = rnd.Next(1, 100);

                await _stockHub.Clients.All.SendAsync("ReceiveStockPrice", stockName, stockPrice + stockRaise);
                await Task.Delay(4000, stoppingToken);
            }
        }
    }
}
