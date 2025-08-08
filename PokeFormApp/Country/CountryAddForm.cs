using Newtonsoft.Json;
using PokemonReviewApp.Dto;
using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class CountryAddForm : Form
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7091/")
        };

        public CountryAddForm()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string countryName = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(countryName))
            {
                MessageBox.Show("Lütfen bir ülke ismi girin.");
                return;
            }

            var newCountry = new CountryDto { Name = countryName };

            try
            {
                string json = JsonConvert.SerializeObject(newCountry);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/Country", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Eklendi!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ekleme başarısız: " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
