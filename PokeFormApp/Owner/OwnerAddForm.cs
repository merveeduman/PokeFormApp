using Newtonsoft.Json;
using PokemonReviewApp.Dto; 
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp.Owner
{
    public partial class OwnerAddForm : Form
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7091/")
        };

        public OwnerAddForm()
        {
            InitializeComponent();

            button1.Click += async (s, e) => await AddOwnerAsync();
            button2.Click += (s, e) => this.Close();

            this.Load += OwnerAddForm_Load; // Form yüklendiğinde ülkeleri yükle
        }

        private async void OwnerAddForm_Load(object sender, EventArgs e)
        {
            await LoadCountriesAsync();
        }

        private async Task LoadCountriesAsync()
        {
            try
            {
                var response = await client.GetAsync("api/Country");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var countries = JsonConvert.DeserializeObject<List<CountryDto>>(json);

                    comboBox1.DataSource = countries;
                    comboBox1.DisplayMember = "Name";  // Görünen değer
                    comboBox1.ValueMember = "Id";      // Seçilen değer
                    comboBox1.SelectedIndex = -1;      // Başlangıçta seçili yok

               

                }
                else
                {
                    MessageBox.Show("Country listesi yüklenemedi.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private async Task AddOwnerAsync()
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen bir Country seçiniz!");
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

            var ownerDto = new OwnerDto
            {
                FirstName = firstName,
                LastName = lastName,
                Gym = gym
            };

            try
            {
                var json = JsonConvert.SerializeObject(ownerDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"api/Owner?countryId={countryId}", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Owner başarıyla eklendi!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Ekleme başarısız: {response.StatusCode}\nDetay: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }
}
