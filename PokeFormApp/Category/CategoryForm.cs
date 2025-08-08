using Newtonsoft.Json;
using PokemonReviewApp.Dto;  // DTO namespace'i
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class CategoryForm : Form
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7091/")
        };

        private bool columnsCreated = false;

        public CategoryForm()
        {
            InitializeComponent();
            this.Load += async (s, e) => await GetAllCategories();
        }

        private async Task GetAllCategories()
        {
            try
            {
                var response = await client.GetAsync("api/Category");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var categories = JsonConvert.DeserializeObject<List<CategoryDto>>(json);

                    if (!columnsCreated)
                    {
                        dataGridView1.AutoGenerateColumns = false;
                        dataGridView1.Columns.Clear();

                        var colId = new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Width = 50 };
                        var colName = new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Name", Width = 150 };

                        dataGridView1.Columns.Add(colId);
                        dataGridView1.Columns.Add(colName);

                        columnsCreated = true;
                    }

                    dataGridView1.DataSource = categories;
                }
                else
                {
                    MessageBox.Show("Kategoriler yüklenemedi. Kod: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private async Task DeleteCategory(int id)
        {
            var response = await client.DeleteAsync($"api/Category/{id}");
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Kategori silindi.");
                await GetAllCategories();
            }
            else
            {
                MessageBox.Show("Silme işlemi başarısız oldu.");
            }
        }

        // button1: Ekleme
        private async void button1_Click(object sender, EventArgs e)
        {
            var addForm = new CategoryAddForm();
            var result = addForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                await GetAllCategories();
            }
        }

        // button2: Silme
        private async void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = (CategoryDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var confirm = MessageBox.Show($"{selected.Name} kategorisini silmek istiyor musunuz?", "Sil", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    var response = await client.DeleteAsync($"api/Category/{selected.Id}");
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Kategori silindi!");
                        await GetAllCategories();
                    }
                    else
                    {
                        MessageBox.Show("Silme başarısız!");
                    }
                }
            }
        }

        // button3: Güncelleme
        private async void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = (CategoryDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var updateForm = new CategoryUpdateForm(selected.Id, selected.Name);
                var result = updateForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    await GetAllCategories();
                }
            }
        }

        // button4: Detay gösterme
        private async void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = (CategoryDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var response = await client.GetAsync($"api/Category/{selected.Id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var category = JsonConvert.DeserializeObject<CategoryDto>(json);
                    MessageBox.Show($"ID: {category.Id}\nName: {category.Name}", "Kategori Detayı");
                }
            }
        }

        // button5: Yenile
        private async void button5_Click(object sender, EventArgs e)
        {
            await GetAllCategories();
        }

        // button6: Geri Dön
        private void button6_Click(object sender, EventArgs e)
        {
            var mainForm = new Form1();
            mainForm.Show();
            this.Close();
        }

        
    }
}
