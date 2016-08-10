using System;

namespace RestaurantApp.Voting.Business
{
    /// <summary>
    /// Representa uma interface de um provedor de Data e Hora
    /// </summary>
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}