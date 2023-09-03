using Spark.Library.Database;

namespace StockSimulator.Application.Models
{
    public class Stock : BaseModel
    {

        
        public int UserId { get; set; }
        
        public string Ticker { get; set; }
        public int Amount { get; set; }
        public decimal PriceBoughtAt { get; set; }
    }
}