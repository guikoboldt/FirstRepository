using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net.Http;
using Previsao.Temp.Models;

namespace Previsao.Temp.Lab.Services
{
    public class Cptec
    {
        public const string cidadeUrl = "http://servicos.cptec.inpe.br/XML/listaCidades?city={0}";
        public const string previsaoUrl = "http://servicos.cptec.inpe.br/XML/cidade/{0}/previsao.xml";

        async public Task<IEnumerable<Cidade>> GetCidades(string cidade)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(cidadeUrl, Uri.EscapeUriString(cidade));
                var xml = await client.GetStringAsync(url);
                if (string.IsNullOrWhiteSpace(xml))
                {
                    return new List<Cidade>();
                }
                var doc = XDocument.Parse(xml);

                var cidades = doc.Root.Descendants("cidade")
                    .Select(c => new Cidade()
                    {
                        Id = Convert.ToInt32(c.Element("id").Value),
                        Nome = c.Element("nome").Value,
                        Uf = c.Element("uf").Value
                    });

                return cidades;
            }
        }

        async public Task<IEnumerable<Models.Previsoes>> GetWheter(int IdCity)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(previsaoUrl, IdCity);
                var xml = await client.GetStringAsync(url);
                if (string.IsNullOrWhiteSpace(xml))
                {
                    return new List<Models.Previsoes>();
                }
                var doc = XDocument.Parse(xml);

                var previsoes = doc.Root.Descendants("previsao")
                    .Select(c => new Models.Previsoes()
                    {
                        Dia = c.Element("dia").Value,
                        Tempo = c.Element("tempo").Value,
                        Maxima = Convert.ToInt32(c.Element("maxima").Value),
                        Minima = Convert.ToInt32(c.Element("minima").Value)
                    });

                return previsoes;
            }
        }
    }
}
