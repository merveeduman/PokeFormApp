using Newtonsoft.Json;
using PokeFormApp.Autofac;
using PokeFormApp.Services;  // HttpRequest servisi için
using PokemonReviewApp.Dto;
using System;
using System.Text;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class CountryAddForm : Form
    {
        private readonly IHttpRequest _httpRequest;

        public CountryAddForm()
        {
            InitializeComponent();
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();
        }

        private async void button1_Click(object sender, EventArgs e) // Ekle butonu
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
                bool success = await _httpRequest.PostAsync("api/Country", newCountry);

                if (success)
                {
                    MessageBox.Show("Ülke başarıyla eklendi!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ekleme başarısız oldu.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e) // Kapat butonu
        {
            this.Close();
        }
    }
}
