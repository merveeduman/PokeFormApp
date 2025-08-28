using PokeFormApp.Autofac;
using PokeFormApp.Services;
using PokemonReviewApp.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class FoodTypeForm : Form
    {
        private readonly IHttpRequest _httpRequest;
        private readonly string url = "api/FoodType";
        private bool columnsCreated = false;

        public FoodTypeForm()
        {
            InitializeComponent();
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();
            this.Load += async (s, e) => await GetAllFoodTypes();
        }

        private async Task GetAllFoodTypes()
        {
            try
            {
                var foodTypes = await _httpRequest.GetAllAsync<List<FoodTypeDto>>(url);

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
                        DataPropertyName = "Name",
                        HeaderText = "Name",
                        Width = 150
                    });

                    columnsCreated = true;
                }

                dataGridView1.DataSource = foodTypes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private async Task DeleteFoodType(int id)
        {
            var success = await _httpRequest.DeleteAsync($"{url}/soft", id);
            if (success)
            {
                MessageBox.Show("FoodType silindi.");
                await GetAllFoodTypes();
            }
            else
            {
                MessageBox.Show("Silme işlemi başarısız.");
            }
        }

        private async Task UpdateFoodType(FoodTypeDto updated)
        {
            var success = await _httpRequest.PutAsync($"{url}/{updated.Id}", updated);
            if (success)
            {
                MessageBox.Show("FoodType güncellendi.");
            }
        }

        private async void button1_Click(object sender, EventArgs e) // EKLE
        {
            var addForm = new FoodTypeAddForm();
            if (addForm.ShowDialog() == DialogResult.OK)
                await GetAllFoodTypes();
        }

        private async void button2_Click(object sender, EventArgs e) // SİL
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = dataGridView1.SelectedRows[0].DataBoundItem as FoodTypeDto;
                if (selected == null)
                {
                    MessageBox.Show("Seçilen food type bilgisi alınamadı.");
                    return;
                }

                var confirm = MessageBox.Show($"{selected.Name} türünü silmek istiyor musunuz?", "Sil", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    await DeleteFoodType(selected.Id);
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir tür seçin.");
            }
        }

        private async void button3_Click(object sender, EventArgs e) // GÜNCELLE
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = (FoodTypeDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var foodType = await _httpRequest.GetByIdAsync<FoodTypeDto>(url, selected.Id);

                var updateForm = new FoodTypeUpdateForm(foodType.Id, foodType.Name);

                if (updateForm.ShowDialog() == DialogResult.OK)
                {
                    await UpdateFoodType(new FoodTypeDto
                    {
                        Id = foodType.Id,
                        Name = updateForm.UpdatedFoodTypeName
                    });

                    await GetAllFoodTypes();
                }
            }
        }

        private async void button4_Click(object sender, EventArgs e) // YENİLE
        {
            await GetAllFoodTypes();
        }

        private void button5_Click(object sender, EventArgs e) // GERİ DÖN
        {
            this.Close();
        }

        private async void button6_Click(object sender, EventArgs e) // DETAY
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = (FoodTypeDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var detay = await _httpRequest.GetByIdAsync<FoodTypeDto>(url, selected.Id);

                MessageBox.Show($"ID: {detay.Id}\nName: {detay.Name}", "Food Type Detayı");
            }
        }

        
    }
}
