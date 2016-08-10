using System;

namespace RestaurantApp.Voting.Business
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Today { get { return DateTime.Today; } }
        public DateTime Now { get { return DateTime.Now; } }
    }
}