using Newtonsoft.Json;
using PokemonReviewApp.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp.Reviewer
{
    public partial class ReviewerForm : Form
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7091/")
        };

        private bool columnsCreated = false;

        public ReviewerForm()
        {
            InitializeComponent();
            this.Load += async (s, e) => await GetAllReviewers();
        }

        private async Task GetAllReviewers()
        {
            try
            {
                var response = await client.GetAsync("api/Reviewer");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var reviewers = JsonConvert.DeserializeObject<List<ReviewerDto>>(json);

                    if (!columnsCreated)
                    {
                        dataGridView1.AutoGenerateColumns = false;
                        dataGridView1.Columns.Clear();

                        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Width = 50 });
                        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FirstName", HeaderText = "First Name", Width = 150 });
                        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "LastName", HeaderText = "Last Name", Width = 150 });

                        columnsCreated = true;
                    }

                    dataGridView1.DataSource = reviewers;
                }
                else
                {
                    MessageBox.Show("Reviewer verileri alınamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private async Task DeleteReviewer(int id)
        {
            var response = await client.DeleteAsync($"api/Reviewer/{id}");
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Reviewer silindi.");
                await GetAllReviewers();
            }
            else
            {
                MessageBox.Show("Silme işlemi başarısız.");
            }
        }

        private async void button1_Click(object sender, EventArgs e) // Ekle
        {
            var addForm = new ReviewerAddForm();
            var result = addForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                await GetAllReviewers();
            }
        }

        private async void button2_Click(object sender, EventArgs e) // Sil
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = (ReviewerDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var confirm = MessageBox.Show($"{selected.FirstName} {selected.LastName} silinsin mi?", "Sil", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    await DeleteReviewer(selected.Id);
                }
            }
        }

        private async void button3_Click(object sender, EventArgs e) // Güncelle
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = (ReviewerDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var updateForm = new ReviewerUpdateForm(selected.Id, selected.FirstName, selected.LastName);
                var result = updateForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    await GetAllReviewers();
                }
            }
        }

        private async void button4_Click(object sender, EventArgs e) // Detay
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = (ReviewerDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var response = await client.GetAsync($"api/Reviewer/{selected.Id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var reviewer = JsonConvert.DeserializeObject<ReviewerDto>(json);
                    MessageBox.Show($"ID: {reviewer.Id}\nFirst Name: {reviewer.FirstName}\nLast Name: {reviewer.LastName}", "Reviewer Detay");
                }
            }
        }

        private async void button5_Click(object sender, EventArgs e) => await GetAllReviewers();

        private void button6_Click(object sender, EventArgs e)
        {
            var mainForm = new Form1();
            mainForm.Show();
            this.Close();
        }

        
    }
}
