using AppTempoAgora.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace AppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            try
            {
                Tempo? t = null;
                string chave = "369f74b2d1d24b67fc1b27469df4e10d";
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&appid={chave}&lang=pt";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage resp = await client.GetAsync(url);

                    if (resp.IsSuccessStatusCode)
                    {
                        string json = await resp.Content.ReadAsStringAsync();
                        var rascunho = JObject.Parse(json);

                        DateTime time = new();
                        DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                        DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                        t = new()
                        {
                            lat = (double)rascunho["coord"]["lat"],
                            lon = (double)rascunho["coord"]["lon"],
                            description = (string)rascunho["weather"][0]["description"],
                            main = (string)rascunho["weather"][0]["main"],
                            temp_min = (double)rascunho["main"]["temp_min"],
                            temp_max = (double)rascunho["main"]["temp_max"],
                            speed = (double)rascunho["wind"]["speed"],
                            visibility = (int)rascunho["visibility"],
                            sunrise = sunrise.ToString("HH:mm"),
                            sunset = sunset.ToString("HH:mm"),
                        };
                    }
                    else if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new Exception("Cidade não encontrada. Verifique o nome e tente novamente.");
                    }
                    else
                    {
                        throw new Exception($"Erro na API: {resp.StatusCode}");
                    }
                }
                return t;
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("No such host is known"))
            {
                throw new Exception("Sem conexão com a internet. Verifique sua rede.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter previsão: {ex.Message}");
            }
        }
    }
}