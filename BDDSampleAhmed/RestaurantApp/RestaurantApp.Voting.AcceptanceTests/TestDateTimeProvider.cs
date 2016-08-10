using System;
using RestaurantApp.Voting.Business;

namespace RestaurantApp.Voting.AcceptanceTests
{
    /// <summary>
    /// Provedor de tempo de teste que permie definir o tempo atual.
    /// </summary>
    public class TestDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now { get; set; }
    }
}