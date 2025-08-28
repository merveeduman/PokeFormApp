using PokeFormApp.Autofac;
using PokeFormApp.Services;
using PokemonReviewApp.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp.Reviewer
{
    public partial class ReviewerForm : Form
    {
        private readonly IHttpRequest _httpRequest;
        private readonly string url = "api/Reviewer";
        private bool columnsCreated = false;

        public ReviewerForm()
        {
            InitializeComponent();
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();

            this.Load += async (s, e) => await GetAllReviewers();
        }

        private async Task GetAllReviewers()
        {
            try
            {
                var reviewers = await _httpRequest.GetAllAsync<List<ReviewerDto>>(url);

                if (!columnsCreated)
                {
                    dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.Columns.Clear();

                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Id",
                        HeaderText = "ID",
                        Width = 50
                    });

                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "FirstName",
                        HeaderText = "First Name",
                        Width = 150
                    });

                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "LastName",
                        HeaderText = "Last Name",
                        Width = 150
                    });

                    columnsCreated = true;
                }

                dataGridView1.DataSource = reviewers;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private async Task DeleteReviewer(int id)
        {
            var success = await _httpRequest.DeleteAsync($"{url}/soft", id);
            if (success)
            {
                MessageBox.Show("Reviewer silindi.");
                await GetAllReviewers();
            }
            else
            {
                MessageBox.Show("Silme işlemi başarısız.");
            }
        }

        private async Task UpdateReviewer(ReviewerDto updated)
        {
            var success = await _httpRequest.PutAsync($"{url}/{updated.Id}", updated);
            if (success)
            {
                MessageBox.Show("Reviewer güncellendi.");
            }
        }

        private async void button1_Click(object sender, EventArgs e) // EKLE
        {
            var addForm = new  ReviewerAddForm();
            if (addForm.ShowDialog() == DialogResult.OK)
                await GetAllReviewers();
        }

        private async void button2_Click(object sender, EventArgs e) // SİL
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = dataGridView1.SelectedRows[0].DataBoundItem as ReviewerDto;
                if (selected == null)
                {
                    MessageBox.Show("Reviewer bilgisi alınamadı.");
                    return;
                }

                var confirm = MessageBox.Show($"{selected.FirstName} {selected.LastName} silinsin mi?", "Sil", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                    await DeleteReviewer(selected.Id);
            }
            else
            {
                MessageBox.Show("Lütfen bir reviewer seçin.");
            }
        }

        private async void button3_Click(object sender, EventArgs e) // GÜNCELLE
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = (ReviewerDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var reviewer = await _httpRequest.GetByIdAsync<ReviewerDto>(url, selected.Id);

                var updateForm = new ReviewerUpdateForm(reviewer.Id, reviewer.FirstName, reviewer.LastName);

                if (updateForm.ShowDialog() == DialogResult.OK)
                {
                    await UpdateReviewer(new ReviewerDto
                    {
                        Id = reviewer.Id,
                        FirstName = updateForm.UpdatedFirstName,
                        LastName = updateForm.UpdatedLastName
                    });

                    await GetAllReviewers();
                }
            }
        }

        private async void button4_Click(object sender, EventArgs e) // DETAY
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = (ReviewerDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var detay = await _httpRequest.GetByIdAsync<ReviewerDto>(url, selected.Id);

                MessageBox.Show($"ID: {detay.Id}\nFirst Name: {detay.FirstName}\nLast Name: {detay.LastName}", "Reviewer Detay");
            }
        }

        private async void button5_Click(object sender, EventArgs e) // YENİLE
        {
            await GetAllReviewers();
        }

        private void button6_Click(object sender, EventArgs e) // GERİ DÖN
        {
            this.Close();
        }
    }
}
