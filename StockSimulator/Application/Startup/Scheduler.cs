using Coravel;
using StockSimulator.Application.Tasks;

namespace StockSimulator.Application.Startup
{
    public static class Scheduler
    {
        public static IServiceProvider RegisterScheduledJobs(this IServiceProvider services)
        {
            services.UseScheduler(scheduler =>
            {
                // example scheduled job
                scheduler
                    .Schedule<GetBareStocksTask>()
                    .DailyAtHour(1);
            });
            return services;
        }
    }
}
