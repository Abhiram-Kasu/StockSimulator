using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;
using Spark.Library.Environment;
using StockSimulator.Application.Database;
using StockSimulator.Application.Models;

namespace StockSimulator.Application.Tasks
{
    public class GetBareStocksTask : IInvocable
    {
        private readonly IDbContextFactory<DatabaseContext> _factory;
        private readonly HttpClient _httpClient;
        private readonly ILogger<GetBareStocksTask> _logger;

        public GetBareStocksTask(IDbContextFactory<DatabaseContext> factory, HttpClient httpClient, ILogger<GetBareStocksTask> logger)
        {
            _factory = factory;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task Invoke()
        {
            var apiKey = Env.Get("ALPHA_VANTAGE_API_KEY") ?? throw new ArgumentNullException("apiKey", "ALPHA_VANTAGE_API_KEY' not present in .env file");
            const string baseUrl = "https://www.alphavantage.co/query";
            const string function = "LISTING_STATUS";
            const string exchange = "NASDAQ";

            var requestUri = $"{baseUrl}?function={function}&apikey={apiKey}&exchange={exchange}";

            var res = await _httpClient.GetStringAsync(requestUri);
            _logger.LogInformation("HTTP request completed successfully");

            var lines = res.Split("\r\n");
            _logger.LogInformation($"Received {lines.Length} lines of data");

            var listings = lines[1..].Select(x =>
            {
                if (string.IsNullOrEmpty(x))
                {
                    _logger.LogWarning("Skipping empty line");
                    return null;
                }

                var arr = x.Split(",");
                if (arr.Length <= 3)
                {
                    _logger.LogWarning("Skipping line with insufficient data: {Line}", x);
                    return null;
                }

                var stock = new BareStock { Symbol = arr[0], Name = arr[1], Exchange = arr[2] };
                _logger.LogInformation("Parsed stock: Symbol={Symbol}, Name={Name}, Exchange={Exchange}", stock.Symbol, stock.Name, stock.Exchange);
                return stock;
            }).Where(x => x != null).Where(x => x!.Exchange == "NASDAQ");

            _logger.LogInformation("Filtered and prepared listings");

            await using var context = await _factory.CreateDbContextAsync();
            _logger.LogInformation("Created DbContext");

            await context.BareStocks.AddRangeAsync(listings!);
            _logger.LogInformation("Added listings to DbContext");

            await context.SaveChangesAsync();
            _logger.LogInformation("Saved changes to the database");

        }
    }
}