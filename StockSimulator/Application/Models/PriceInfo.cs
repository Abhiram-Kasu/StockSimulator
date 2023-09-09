namespace StockSimulator.Application.Models
{
    public class PriceInfo
    {
        public static readonly PriceInfo EmptyPriceInfo = new("", 0);
        

        public PriceInfo(string Ticker, decimal Price, bool IsUp = false)
        {
            this.Ticker = Ticker;
            this.Price = Price;
            this.IsUp = IsUp;
            
        }

        public PriceInfo()
        {
            Ticker = "";
            Price = 0;
            IsUp = false;
        }

        public string Ticker { get; init; }
        public decimal Price { get; set; }
        public bool IsUp { get; set; }

        public void Deconstruct(out string Ticker, out decimal Price, out bool IsUp)
        {
            Ticker = this.Ticker;
            Price = this.Price;
            IsUp = this.IsUp;
        }

        public static bool operator ==(PriceInfo p1, PriceInfo p2) => (p1.Ticker, p1.Price, p1.IsUp) == (p2.Ticker, p2.Price, p2.IsUp);

        public static bool operator !=(PriceInfo p1, PriceInfo p2) => !(p1 == p2);
        public bool Equals(PriceInfo other)
        {
            return Ticker == other.Ticker && Price == other.Price && IsUp == other.IsUp;
        }

        public override bool Equals(object? obj)
        {
            return obj is PriceInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Ticker, Price);
        }
    }
}