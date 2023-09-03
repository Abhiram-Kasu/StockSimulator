namespace StockSimulator.Application.Models
{
    public class PriceInfo
    {
        public static readonly PriceInfo EmptyPriceInfo = new("", 0);

        public PriceInfo(string Ticker, decimal Price)
        {
            this.Ticker = Ticker;
            this.Price = Price;
        }

        public PriceInfo()
        {
            Ticker = "";
            Price = 0;
        }

        public string Ticker { get; init; }
        public decimal Price { get; set; }

        public void Deconstruct(out string Ticker, out decimal Price)
        {
            Ticker = this.Ticker;
            Price = this.Price;
        }

        public static bool operator ==(PriceInfo p1, PriceInfo p2) => (p1.Ticker, p1.Price) == (p2.Ticker, p2.Price);

        public static bool operator !=(PriceInfo p1, PriceInfo p2) => !(p1 == p2);
    }
}