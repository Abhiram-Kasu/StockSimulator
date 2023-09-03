using Spark.Library.Database;

namespace StockSimulator.Application.Models;

public class BareStock : BaseModel
{
    public string Symbol { get; init; }
    public string Name { get; init; }
    public string Exchange { get; init; }
}