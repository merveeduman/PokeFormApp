using PokeFormApp.Autofac;
using PokeFormApp.Dto;
using PokeFormApp.Services;
using PokemonReviewApp.Dto;
using System;
using System.Windows.Forms;

namespace PokeFormApp.Review
{
    public partial class ReviewUpdateForm : Form
    {
        private readonly IHttpRequest _httpRequest;
        private ReviewUpdateDto currentReview;

        public ReviewDto UpdatedReview { get; private set; }

        public ReviewUpdateForm(ReviewUpdateDto reviewDto)
        {
            InitializeComponent();

            currentReview = reviewDto;
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();

            textBox1.Text = currentReview.Title;
            textBox2.Text = currentReview.Text;
            textBox3.Text = currentReview.Rating.ToString();
        }

        private async void button1_Click(object sender, EventArgs e) // Güncelle butonu
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

            var updatedReviewDto = new ReviewUpdateDto
            {
                Id = currentReview.Id,
                Title = title,
                Text = text,
                Rating = rating,
                ReviewerId = currentReview.ReviewerId,
                PokemonId = currentReview.PokemonId
            };

            try
            {
                bool success = await _httpRequest.PutAsync($"api/Review/{updatedReviewDto.Id}", updatedReviewDto);

                if (success)
                {
                    MessageBox.Show("Review başarıyla güncellendi.");
                    UpdatedReview = new ReviewDto
                    {
                        Id = updatedReviewDto.Id,
                        Title = updatedReviewDto.Title,
                        Text = updatedReviewDto.Text,
                        Rating = updatedReviewDto.Rating
                    };
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Güncelleme başarısız.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e) // Kapat butonu
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
