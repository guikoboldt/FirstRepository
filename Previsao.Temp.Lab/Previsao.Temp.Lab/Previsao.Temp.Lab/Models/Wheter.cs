using System.Collections.Generic;

namespace Previsao.Temp.Models
{
    public class Cidade
    {
        public int Id { get; set; } = 0;
        public string Uf { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;

        public override string ToString()
        {
            return string.Format("{0} - {1} - {2} ",Nome, Uf, Id);
        }
    }

    public class Previsoes
    {
        public string Dia { get; set; } = string.Empty;
        public string Tempo { get; set; } = string.Empty;
        public int Minima { get; set; } = 0;
        public int Maxima { get; set; } = 0;

        public override string ToString()
        {
            return string.Format("Dia: {0} - Tempo: {1} \r\n Minima: {2}ºC    Maxima: {3}ºC  \r\n", this.Dia, 
                this.Tempo, this.Minima, this.Maxima);
        }
    }

    public class TempoRoot
    {
        public string Nome { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public string atualizacao { get; set; } = string.Empty;
        public IEnumerable<Previsoes> Previsoes { get; set; } = new List<Previsoes>();
    }
}