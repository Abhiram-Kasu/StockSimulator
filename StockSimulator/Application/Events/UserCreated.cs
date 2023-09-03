using Coravel.Events.Interfaces;
using StockSimulator.Application.Models;

namespace StockSimulator.Application.Events
{
    public class UserCreated : IEvent
    {
        public User User { get; set; }

        public UserCreated(User user)
        {
            this.User = user;
        }
    }
}
