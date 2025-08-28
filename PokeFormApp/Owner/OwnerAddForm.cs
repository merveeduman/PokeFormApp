using PokeFormApp.Autofac;
using PokeFormApp.Services;
using PokemonReviewApp.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp.Owner
{
    public partial class OwnerAddForm : Form
    {
        private readonly IHttpRequest _httpRequest;
        private List<CountryDto> countries;

        public OwnerAddForm()
        {
            InitializeComponent();
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();

            // Butonların eventlerini manuel olarak bağla
            button1.Click += button1_Click; // Ekle butonu
            button2.Click += button2_Click; // Kapat butonu

            this.Load += async (s, e) => await LoadCountriesAsync(); // Form yüklenince ülkeleri yükle
        }

        private async Task LoadCountriesAsync()
        {
            try
            {
                countries = await _httpRequest.GetAllAsync<List<CountryDto>>("api/Country");

                comboBox1.DataSource = countries;
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "Id";
                comboBox1.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ülke verileri alınamadı: " + ex.Message);
            }
        }

        private async void button1_Click(object sender, EventArgs e) // Ekle butonu
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen bir ülke seçiniz!");
                return;
            }

            int countryId = (int)comboBox1.SelectedValue;
            string firstName = textBox2.Text.Trim();
            string lastName = textBox3.Text.Trim();
            string gym = textBox4.Text.Trim();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("FirstName ve LastName boş olamaz!");
                return;
            }

            var newOwner = new OwnerDto
            {
                FirstName = firstName,
                LastName = lastName,
                Gym = gym
            };

            try
            {
                bool success = await _httpRequest.PostAsync($"api/Owner?countryId={countryId}", newOwner);

                if (success)
                {
                    MessageBox.Show("Owner başarıyla eklendi!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Owner ekleme başarısız. (Sunucu başarılı cevap vermedi)");
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
