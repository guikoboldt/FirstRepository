using System;

namespace Infrastructure.Common
{
    /// <summary>
    /// Representam Métodos de Extension para facilitar e modularizar operações comuns sobre <see cref="DateTime"/>
    /// </summary>
    public static class DatetimeExtensions
    {
        /// <summary>
        /// Retorna um <see cref="Datetime"/> que representa o primeiro dia da semana representado pelo <paramref name="datetime"/>
        /// </summary>
        /// <param name="dateTime">O <see cref="Datetime"/> a ser utilizado como referencia.</param>
        /// <returns>Um <see cref="Datetime"/> representando o primeiro dia da semana</returns>
        public static DateTime GetStartOfWeekNow(this DateTime datetime)
        {
            var date = datetime.Date;
            return date.AddDays(-(int)date.DayOfWeek);
        }
    }
}
