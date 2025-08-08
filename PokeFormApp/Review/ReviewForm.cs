using Newtonsoft.Json;
using PokemonReviewApp.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp.Review
{
    public partial class ReviewForm : Form
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7091/")
        };

        private bool columnsCreated = false;

        public ReviewForm()
        {
            InitializeComponent();
            this.Load += async (s, e) => await GetAllReviews();
        }

        // Review listesini getirir ve DataGridView'e doldurur
        private async Task GetAllReviews()
        {
            try
            {
                var response = await client.GetAsync("api/Review");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var reviews = JsonConvert.DeserializeObject<List<ReviewDto>>(json);

                    if (!columnsCreated)
                    {
                        dataGridView1.AutoGenerateColumns = false;
                        dataGridView1.Columns.Clear();

                        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID" });
                        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Title", HeaderText = "Title" });
                        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Text", HeaderText = "Text" });
                        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Rating", HeaderText = "Rating" });

                        columnsCreated = true;  // ÖNEMLİ: burada true olmalı
                    }

                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = reviews;
                }
                else
                {
                    MessageBox.Show("Review verileri alınamadı: " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        // Review silme işlemi
        private async Task DeleteReview(int id)
        {
            try
            {
                var response = await client.DeleteAsync($"api/Review/{id}");
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Review silindi.");
                    await GetAllReviews();
                }
                else
                {
                    MessageBox.Show("Silme işlemi başarısız: " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme sırasında hata: " + ex.Message);
            }
        }

        // Review detaylarını gösterir
        private async Task ShowReviewDetails(int id)
        {
            try
            {
                var response = await client.GetAsync($"api/Review/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var review = JsonConvert.DeserializeObject<ReviewDto>(json);

                    MessageBox.Show(
                        $"ID: {review.Id}\nTitle: {review.Title}\nText: {review.Text}\nRating: {review.Rating}",
                        "Review Detayları");
                }
                else
                {
                    MessageBox.Show("Detay alınamadı: " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Detay alınırken hata: " + ex.Message);
            }
        }

        // Ekle butonu
        private async void button1_Click(object sender, EventArgs e)
        {
            var addForm = new ReviewAddForm();
            var result = addForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                await GetAllReviews();
            }
        }

        // Sil butonu
        private async void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = (ReviewDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var confirm = MessageBox.Show($"Review #{selected.Id} silinsin mi?", "Sil", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    await DeleteReview(selected.Id);
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçin.");
            }
        }

        // Güncelle butonu
        private async void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = (ReviewDto)dataGridView1.SelectedRows[0].DataBoundItem;
                ReviewCreateDto reviewCreateDto = new ReviewCreateDto
                {
                    Id = selected.Id,
                    Rating = selected.Rating,
                    Text = selected.Text,
                    Title = selected.Title,
                };
                var updateForm = new ReviewUpdateForm(reviewCreateDto);
                var result = updateForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    await GetAllReviews();
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçin.");
            }
        }

        // Detay butonu
        private async void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = (ReviewDto)dataGridView1.SelectedRows[0].DataBoundItem;
                await ShowReviewDetails(selected.Id);
            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçin.");
            }
        }

        // Yenile butonu
        private async void button5_Click(object sender, EventArgs e)
        {
            await GetAllReviews();
        }

        // Geri dön butonu
        private void button6_Click(object sender, EventArgs e)
        {
            var mainForm = new Form1();
            mainForm.Show();
            this.Close();
        }
    }
}
