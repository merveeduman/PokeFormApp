using Newtonsoft.Json;
using PokemonReviewApp.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PokeFormApp.Review
{
    public partial class ReviewAddForm : Form
    {
        private readonly HttpClient client = new HttpClient { BaseAddress = new Uri("https://localhost:7091/") };

        public ReviewAddForm()
        {
            InitializeComponent();

            this.Load += async (s, e) =>
            {
                await LoadReviewers();
                await LoadPokemons();
            };
        }

        private async Task LoadReviewers()
        {
            var response = await client.GetAsync("api/Reviewer");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var reviewers = JsonConvert.DeserializeObject<List<ReviewerDto>>(json);

                comboBox1.DataSource = reviewers;
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "Id";
                

                var reviewerList = reviewers.Select(o => new
                {
                    Id = o.Id,
                    FullName = o.FirstName + " " + o.LastName
                }).ToList();

                comboBox1.DataSource = reviewerList;
                comboBox1.DisplayMember = "FullName";
                comboBox1.ValueMember = "Id";
                comboBox1.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Reviewer verileri alınamadı.");
            }
        }

        private async Task LoadPokemons()
        {
            var response = await client.GetAsync("api/Pokemon");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var pokemons = JsonConvert.DeserializeObject<List<PokemonDto>>(json);

                comboBox2.DataSource = pokemons;
                comboBox2.DisplayMember = "Name";
                comboBox2.ValueMember = "Id";
                comboBox2.SelectedIndex = -1;

            }
            else
            {
                MessageBox.Show("Pokemon verileri alınamadı.");
            }
        }

        private async void button1_Click(object sender, EventArgs e) // EKLE
        {
            var createDto = new ReviewCreateDto
            {
                Title = textBox3.Text,
                Text = textBox4.Text,
                Rating = int.TryParse(textBox5.Text, out int rating) ? rating : 0

            };

            int reviewerId = Convert.ToInt32(comboBox1.SelectedValue);
            int pokemonId = Convert.ToInt32(comboBox2.SelectedValue);

            string json = JsonConvert.SerializeObject(createDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"api/Review?reviewerId={reviewerId}&pokeId={pokemonId}", content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Review başarıyla eklendi.");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show($"Ekleme başarısız: {response.ReasonPhrase}");
            }
        }

        private void button2_Click(object sender, EventArgs e) // KAPAT
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
