using System;
using RestaurantApp.Voting.Business;

namespace RestaurantApp.Voting.IntegrationTests.Helpers.Mocks
{
    /// <summary>
    /// Provedor de tempo de teste que permie definir o tempo atual.
    /// </summary>
    public class TestDateTimeProvider : IDateTimeProvider
    {
        DateTime now;
        public DateTime Now 
        { 
            get { return now; } 
            set {
                if (value == now)
                    return;

                Console.WriteLine();
                Console.WriteLine("DateTime: {{ Now = '{0}', Was = '{1}' }}", value, now);
                Console.WriteLine();
                now = value;
            } 
        }
    }
}