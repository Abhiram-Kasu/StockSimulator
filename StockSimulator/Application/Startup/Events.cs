using Coravel;
using Coravel.Events.Interfaces;
using StockSimulator.Application.Events;
using StockSimulator.Application.Events.Listeners;

namespace StockSimulator.Application.Startup
{
    public static class Events
    {
        public static IServiceProvider RegisterEvents(this IServiceProvider services)
        {
            IEventRegistration registration = services.ConfigureEvents();

            // add events and listeners here
            registration
                .Register<UserCreated>()
                .Subscribe<EmailNewUser>();

            return services;
        }
    }
}
