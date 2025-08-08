using Newtonsoft.Json;
using PokemonReviewApp.Dto;
using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class CountryUpdateForm : Form
    {
        private int countryId;
        public string UpdatedCountryName => textBox1.Text.Trim();

        public CountryUpdateForm(int id, string name)
        {
            InitializeComponent();
            countryId = id;
            textBox1.Text = name;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string newName = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(newName))
            {
                MessageBox.Show("Ülke adı boş olamaz.");
                return;
            }

            var updatedCountry = new CountryDto
            {
                Id = countryId,
                Name = newName
            };

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7091/");
                var json = JsonConvert.SerializeObject(updatedCountry);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"api/Country/{countryId}", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Ülke güncellendi.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Güncelleme başarısız!");
                }
            }
        }
    }
}
