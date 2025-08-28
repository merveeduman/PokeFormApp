using PokeFormApp.Autofac;
using PokeFormApp.Services; // IHttpRequest için
using PokemonReviewApp.Dto;
using System;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class CountryUpdateForm : Form
    {
        private int countryId;
        private readonly IHttpRequest _httpRequest;

        public string UpdatedCountryName => textBox1.Text.Trim();

        public CountryUpdateForm(int id, string name)
        {
            InitializeComponent();
            countryId = id;
            textBox1.Text = name;
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                MessageBox.Show("Ülke adı boş olamaz.");
                return;
            }

            var updatedCountry = new CountryDto
            {
                Id = countryId,
                Name = UpdatedCountryName
            };

            try
            {
                bool success = await _httpRequest.PutAsync($"api/Country/{countryId}", updatedCountry);

                if (success)
                {
                    MessageBox.Show("Ülke güncellendi.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }

}
