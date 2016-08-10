using System;

namespace RestaurantApp.Voting.Common.Models.Exceptions
{
    /// <summary>
    /// Representa um erro devido ao violação da regra de
    /// negócio de votação.
    /// </summary>
    public class VotingException : Exception
    {
        public VotingException()
            : base("Votação não realizado com sucesso, por favor tente novamente.") { }
        public VotingException(string message, params object[] args)
            : base(string.Format(message, args)) { }
    }

    /// <summary>
    /// Representa um erro quando nao há restaurante a serem votado
    /// </summary>
    public class NoElegibleRestaurantException : VotingException
    {
        public NoElegibleRestaurantException()
            : base("Nenhum restaurante a ser votado: \n\nPossibilidades, não existe Restaurantes cadastrados ou\nTodos já foram mais votados está semana.") { }
    }

    /// <summary>
    /// Representa um erro devido ao violação da regra de
    /// negócio de repetição de restaurante.
    /// </summary>
    public class RestaurantRepetitionException : VotingException
    {
        public RestaurantRepetitionException()
            : base("Restaurante já foi contemplado.") { }
    }

    /// <summary>
    /// Representa um erro devido ao violação da regra de
    /// negócio de repetição de votação.
    /// </summary>
    public class VoteRepetitionException : VotingException
    {
        public VoteRepetitionException()
            : base("Usuário já tinha lançado seu voto.") { }
        public VoteRepetitionException(string candidate)
            : base("Usuário já tinha lançado seu voto no '{0}'.", candidate) { }
    }
    /// <summary>
    /// Representa uma tentativa de votar fora de hora definida
    /// </summary>
    public class PollClosedException : VotingException
    {
        public PollClosedException()
            : base("Votação já encerrada.") { }
    }


    /// <summary>
    /// Representa uma tentativa de autenticação sem êxito.
    /// </summary>
    public class AuthenticationException : VotingException
    {
        public AuthenticationException()
            : base("Nome de usuário ou senha errados. Por favor tente novamente.") { }
    }
}
