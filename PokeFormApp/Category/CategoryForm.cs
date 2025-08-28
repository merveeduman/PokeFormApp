using PokeFormApp.Autofac;
using PokeFormApp.Services;
using PokemonReviewApp.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class CategoryForm : Form
    {
        private readonly IHttpRequest _httpRequest;
        private readonly string url = "api/Category";
        private bool columnsCreated = false;

        public CategoryForm()
        {
            InitializeComponent();

            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();

            this.Load += async (s, e) => await GetAllCategories();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = true;  // çoklu seçim aktif

        }

        private async Task GetAllCategories()
        {
            try
            {
                var categories = await _httpRequest.GetAllAsync<List<CategoryDto>>(url);

                if (!columnsCreated)
                {
                    dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.Columns.Clear();

                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Id",
                        HeaderText = "ID",
                        Width = 50,
                        ReadOnly = true
                    });

                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Name",
                        HeaderText = "Name",
                        Width = 150,
                        ReadOnly = true
                    });

                    columnsCreated = true;
                }

                dataGridView1.DataSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private async Task BulkDeleteCategories(List<int> ids)
        {
            var request = new BulkDeleteRequestDto
            {
                Ids = ids
            };

            var result = await _httpRequest.PostAsync<BulkDeleteResultDto>($"{url}/bulk-delete", request);

            if (result != null)
            {
                MessageBox.Show($"Silinenler: {string.Join(", ", result.DeletedIds)}\n" +
                                $"Bulunamayanlar: {string.Join(", ", result.NotFoundIds)}");
                await GetAllCategories();
            }
            else
            {
                MessageBox.Show("Toplu silme başarısız.");
            }
        }


        private async Task UpdateCategory(CategoryDto updated)
        {
            var success = await _httpRequest.PutAsync($"{url}/{updated.Id}", updated);
            if (success)
            {
                MessageBox.Show("Güncellendi!");
            }
      
        }

        private async void button1_Click(object sender, EventArgs e) // EKLE
        {
            var addForm = new CategoryAddForm();
            if (addForm.ShowDialog() == DialogResult.OK)
                await GetAllCategories();
        }

        private async void button2_Click(object sender, EventArgs e) // TOPLU SİL
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var ids = new List<int>();

                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    if (row.DataBoundItem is CategoryDto category)
                    {
                        ids.Add(category.Id);
                    }
                }

                var confirm = MessageBox.Show($"{ids.Count} kategori silinecek. Emin misiniz?",
                                               "Toplu Silme",
                                               MessageBoxButtons.YesNo);

                if (confirm == DialogResult.Yes)
                {
                    await BulkDeleteCategories(ids);
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için kategori(ler) seçin.");
            }
        }


        private async void button3_Click(object sender, EventArgs e) // GÜNCELLE
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = dataGridView1.SelectedRows[0].DataBoundItem as CategoryDto;
                if (selected == null)
                {
                    MessageBox.Show("Seçilen kategori bilgisi alınamadı.");
                    return;
                }

                var category = await _httpRequest.GetByIdAsync<CategoryDto>(url, selected.Id);
                var updateForm = new CategoryUpdateForm(category.Id, category.Name);

                if (updateForm.ShowDialog() == DialogResult.OK)
                {
                    await UpdateCategory(new CategoryDto
                    {
                        Id = category.Id,
                        Name = updateForm.UpdatedCategoryName
                    });

                    await GetAllCategories();
                }
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek için bir kategori seçin.");
            }
        }

        private async void button4_Click(object sender, EventArgs e) // DETAY
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = dataGridView1.SelectedRows[0].DataBoundItem as CategoryDto;
                if (selected == null)
                {
                    MessageBox.Show("Seçilen kategori bilgisi alınamadı.");
                    return;
                }

                var category = await _httpRequest.GetByIdAsync<CategoryDto>(url, selected.Id);
                MessageBox.Show($"ID: {category.Id}\nName: {category.Name}", "Kategori Detayı");
            }
            else
            {
                MessageBox.Show("Lütfen detay görmek için bir kategori seçin.");
            }
        }

        private async void button5_Click(object sender, EventArgs e) // YENİLE
        {
            await GetAllCategories();
        }

        private void button6_Click(object sender, EventArgs e) // GERİ DÖN
        {
            this.Close();
        }
    }
}
