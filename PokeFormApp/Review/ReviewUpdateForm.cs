using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using PokemonReviewApp.Dto;  // DTO namespace'ini ekle

namespace PokeFormApp.Review
{
    public partial class ReviewUpdateForm : Form
    {
        private ReviewCreateDto currentReview;

        public ReviewCreateDto UpdatedReview { get; private set; }

        public ReviewUpdateForm(ReviewCreateDto reviewDto)
        {
            InitializeComponent();

            currentReview = reviewDto;

            // Textbox'ları doldur
            textBox1.Text = currentReview.Title;
            textBox2.Text = currentReview.Text;
            textBox3.Text = currentReview.Rating.ToString();

            // Buton eventlerini bağla
            button1.Click += async (s, e) => await UpdateReviewAsync();
            button2.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
        }

        private async Task UpdateReviewAsync()
        {
            string title = textBox1.Text.Trim();
            string text = textBox2.Text.Trim();

            if (!int.TryParse(textBox3.Text.Trim(), out int rating))
            {
                MessageBox.Show("Rating sayı olmalı!");
                return;
            }

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Title boş olamaz!");
                return;
            }

            // Güncellenmiş DTO oluşturuyoruz
            var updatedReviewDto = new ReviewDto
            {
                Id = currentReview.Id,
                Title = title,
                Text = text,
                Rating = rating,

            };

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7091/");

                    string json = JsonConvert.SerializeObject(updatedReviewDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync($"api/Review/{updatedReviewDto.Id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Review başarıyla güncellendi.");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Güncelleme başarısız: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

       
    }
}
