using PokemonReviewApp.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace PokeFormApp
{
    public partial class PokemonAddForm : Form
    {
        public PokemonAddForm()
        {
            InitializeComponent();

            button1.Click += button1_Click; 
            button2.Click += button2_Click; 
            this.Load += PokemonAddForm_Load; 
        }

        private async void PokemonAddForm_Load(object sender, EventArgs e)
        {
            await LoadOwnersAsync();
            await LoadCategoriesAsync();
        }

        private async Task LoadOwnersAsync()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7091/api/Owner");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var owners = JsonConvert.DeserializeObject<List<OwnerDto>>(json);

                var ownerList = owners.Select(o => new
                {
                    Id = o.Id,
                    FullName = o.FirstName + " " + o.LastName
                }).ToList();

                comboBox1.DataSource = ownerList;
                comboBox1.DisplayMember = "FullName";
                comboBox1.ValueMember = "Id";
                comboBox1.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Owner listesi yüklenemedi.");
            }
        }



        private async Task LoadCategoriesAsync()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7091/api/Category");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<CategoryDto>>(json);

                comboBox2.DataSource = categories;
                comboBox2.DisplayMember = "Name" +
                    "";    
                comboBox2.ValueMember = "Id";
                comboBox2.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Category listesi yüklenemedi.");
            }
        }



        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtName.Text.Trim();
                string birthDateStr = textBox2.Text.Trim();

                if (!DateTime.TryParse(birthDateStr, out DateTime birthDate))
                {
                    MessageBox.Show("Geçersiz tarih. Format: yyyy-MM-dd");
                    return;
                }

                if (comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Lütfen bir Owner seçin.");
                    return;
                }

                if (comboBox2.SelectedIndex == -1)
                {
                    MessageBox.Show("Lütfen bir Category seçin.");
                    return;
                }

                int ownerId = (int)comboBox1.SelectedValue;
                int categoryId = (int)comboBox2.SelectedValue;

                var newPokemon = new PokemonDto
                {
                    Name = name,
                    BirthDate = birthDate
                };

                var json = JsonConvert.SerializeObject(newPokemon);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                var url = $"https://localhost:7091/api/Pokemon?ownerId={ownerId}&catId={categoryId}";

                using var client = new HttpClient();
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Pokemon başarıyla eklendi!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Hata: {response.StatusCode}\n{error}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
