using Microsoft.AspNetCore.SignalR;

namespace WebApplication18
{
    public class StockHub : Hub
    {
        public async Task SendStockPrice(string stockName, decimal price)
        {
            await Clients.All.SendAsync("ReceiveStockPrice", stockName, price);
        }
    }
}
