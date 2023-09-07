using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spark.Library.Environment;
using StockSimulator.Application.Database;
using StockSimulator.Application.Errors;
using StockSimulator.Application.Models;

namespace StockSimulator.Application.Services
{

    public class StockService
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger<StockService> _logger;
        private readonly string _apiKey;
        private readonly IDbContextFactory<DatabaseContext> _factory;

        public StockService(HttpClient httpClient, ILogger<StockService> logger, IDbContextFactory<DatabaseContext> factory)
        {
            _apiKey = Env.Get("ALPHA_VANTAGE_API_KEY") ?? throw new NullReferenceException("'ALPHA_VANTAGE_API_KEY' not present in .env file");
            _httpClient = httpClient;
            _logger = logger;
            _factory = factory;
        }

        public async Task<List<PriceInfo>> BatchGetCurrentStockPrices(string[] tickers)
        {
            _logger.LogInformation("Fetching current prices for {TickerCount} tickers", tickers.Length);

            var results = await Task.WhenAll(tickers.Distinct().AsParallel().Select(async x => await GetTickerDataAsync(x).Then<OneOf<decimal,FetchPriceError>,PriceInfo>(res => res.Match(price => new PriceInfo(x, price), err => new PriceInfo()))));

            _logger.LogInformation("Received all {TickerCount} ticker prices", results.Length);

            return results.Where(x => x != PriceInfo.EmptyPriceInfo).ToList();
        }

        public async Task<OneOf<decimal, FetchPriceError>> GetTickerDataAsync(string ticker)
        {
            _logger.LogInformation("Fetching current price for {Ticker}", ticker);

            // Construct API URL with dates
            var requestUri = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={ticker}&apikey={_apiKey}";

            // Call API
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            // Read response
            var json = await response.Content.ReadAsStringAsync();
            var jObject = JsonConvert.DeserializeObject<JObject>(json);

            if (jObject?["Global Quote"]?["05. price"]?.Value<decimal>() is { } price)
            {
                _logger.LogInformation("Fetched current price {Price} for {Ticker}", price, ticker);
                return price;
            }

            _logger.LogWarning("Failed to fetch current price for {Ticker}", ticker);
            return new FetchPriceError();
        }

        public async Task<List<BareStock>> GetAllBareStocks()
        {
            await using var context = await _factory.CreateDbContextAsync();
            return await context.BareStocks.ToListAsync();
        }
        
       

    }

}