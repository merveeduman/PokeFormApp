using PokeFormApp.Autofac;
using PokeFormApp.Dto;
using PokeFormApp.Services;
using PokemonReviewApp.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp.Review
{
    public partial class ReviewForm : Form
    {
        private readonly IHttpRequest _httpRequest;
        private readonly string url = "api/Review";
        private bool columnsCreated = false;

        public ReviewForm()
        {
            InitializeComponent();

            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();

            this.Load += async (s, e) => await GetAllReviews();
        }

        private async Task GetAllReviews()
        {
            try
            {
                var reviews = await _httpRequest.GetAllAsync<List<ReviewDto>>(url);

                if (!columnsCreated)
                {
                    dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.Columns.Clear();

                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Width = 50 });
                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Title", HeaderText = "Title", Width = 150 });
                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Text", HeaderText = "Text", Width = 300 });
                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Rating", HeaderText = "Rating", Width = 50 });

                    columnsCreated = true;
                }

                dataGridView1.DataSource = reviews;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private async Task DeleteReview(int id)
        {
            var success = await _httpRequest.DeleteAsync($"{url}/soft", id);
            if (success)
            {
                MessageBox.Show("Review silindi.");
                await GetAllReviews();
            }
            else
            {
                MessageBox.Show("Silme işlemi başarısız.");
            }
        }

        private async Task UpdateReview(ReviewDto updated)
        {
            var success = await _httpRequest.PutAsync($"{url}/{updated.Id}", updated);
            if (success)
            {
                MessageBox.Show("Review güncellendi!");
                await GetAllReviews();
            }
            else
            {
                MessageBox.Show("Güncelleme başarısız.");
            }
        }

        private async void button1_Click(object sender, EventArgs e) // Ekle
        {
            var addForm = new ReviewAddForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                await GetAllReviews();
            }
        }

        private async void button2_Click(object sender, EventArgs e) // Sil
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = dataGridView1.SelectedRows[0].DataBoundItem as ReviewDto;
                if (selected == null)
                {
                    MessageBox.Show("Seçilen review bilgisi alınamadı.");
                    return;
                }

                var confirm = MessageBox.Show($"Review #{selected.Id} silinsin mi?", "Sil", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    await DeleteReview(selected.Id);
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir satır seçin.");
            }
        }

        private async void button3_Click(object sender, EventArgs e) // Güncelle
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = dataGridView1.SelectedRows[0].DataBoundItem as ReviewDto;
                if (selected == null)
                {
                    MessageBox.Show("Seçilen review bilgisi alınamadı.");
                    return;
                }

                // Burada ReviewUpdateDto'ya dönüşüm yapıyoruz
                var review = await _httpRequest.GetByIdAsync<ReviewUpdateDto>(url, selected.Id);

                var updateForm = new ReviewUpdateForm(review);

                if (updateForm.ShowDialog() == DialogResult.OK && updateForm.UpdatedReview != null)
                {
                    await UpdateReview(updateForm.UpdatedReview);
                }
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek için bir satır seçin.");
            }
        }


        private async void button4_Click(object sender, EventArgs e) // Detay
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = dataGridView1.SelectedRows[0].DataBoundItem as ReviewDto;
                if (selected == null)
                {
                    MessageBox.Show("Seçilen review bilgisi alınamadı.");
                    return;
                }

                var review = await _httpRequest.GetByIdAsync<ReviewDto>(url, selected.Id);
                if (review != null)
                {
                    MessageBox.Show(
                        $"ID: {review.Id}\nTitle: {review.Title}\nText: {review.Text}\nRating: {review.Rating}",
                        "Review Detayı");
                }
            }
            else
            {
                MessageBox.Show("Lütfen detay için bir satır seçin.");
            }
        }

        private async void button5_Click(object sender, EventArgs e) // Yenile
        {
            await GetAllReviews();
        }

        private void button6_Click(object sender, EventArgs e) // Geri dön
        {
            this.Close();
        }
    }
}
