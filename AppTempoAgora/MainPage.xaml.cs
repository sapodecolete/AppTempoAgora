using AppTempoAgora.Models;
using AppTempoAgora.Services;

namespace AppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        // Converter unidades
                        double velocidadeKmH = (double)(t.speed * 3.6); // m/s para km/h
                        double visibilidadeKm = (double)(t.visibility / 1000.0); // metros para km

                        string dados_previsao = $"Condição: {t.main}\n" +
                                              $"Descrição: {t.description}\n" +
                                              $"Temperatura Mín: {t.temp_min}°C\n" +
                                              $"Temperatura Máx: {t.temp_max}°C\n" +
                                              $"Velocidade do Vento: {velocidadeKmH:F1} km/h\n" +
                                              $"Visibilidade: {visibilidadeKm:F1} km\n" +
                                              $"Nascer do Sol: {t.sunrise}\n" +
                                              $"Pôr do Sol: {t.sunset}\n" +
                                              $"Latitude: {t.lat}\n" +
                                              $"Longitude: {t.lon}";

                        lbl_res.Text = dados_previsao;
                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de Previsão";
                    }
                }
                else
                {
                    await DisplayAlert("Atenção", "Por favor, preencha o nome da cidade.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
                lbl_res.Text = string.Empty;
            }
        }
    }
}