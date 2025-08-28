using PokeFormApp.Autofac;
using PokeFormApp.Services;
using PokemonReviewApp.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class FoodForm : Form
    {
        private readonly IHttpRequest _httpRequest;
        private readonly string url = "api/Food";
        private bool columnsCreated = false;

        public FoodForm()
        {
            InitializeComponent();

            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();

            this.Load += async (s, e) => await GetAllFoods();
            dataGridView1.KeyDown += DataGridView1_KeyDown;

            dataGridView1.MultiSelect = true; // Çoklu seçim aktif olsun
        }
        private async void DataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true; // Enter'ın başka işlem yapmasını engelle

                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Lütfen silmek için bir veya daha fazla öğe seçin.");
                    return;
                }

                var selectedFoods = new List<FoodDto>();
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    if (row.DataBoundItem is FoodDto food)
                    {
                        selectedFoods.Add(food);
                    }
                }

                if (selectedFoods.Count == 0)
                {
                    MessageBox.Show("Seçilen öğeler alınamadı.");
                    return;
                }

                var confirm = MessageBox.Show($"{selectedFoods.Count} adet yi silmek istiyor musunuz?", "Sil", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    int successCount = 0;
                    foreach (var food in selectedFoods)
                    {
                        bool success = await _httpRequest.DeleteAsync($"api/Food/soft", food.Id);
                        if (success) successCount++;
                    }

                    MessageBox.Show($"{successCount} adet food silindi.");
                    await GetAllFoods();
                }
            }
        }

        private async Task GetAllFoods()
        {
            try
            {
                var foods = await _httpRequest.GetAllAsync<List<FoodDto>>(url);

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

                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Price",
                        HeaderText = "Price",
                        Width = 80
                    });

                    columnsCreated = true;
                }

                dataGridView1.DataSource = foods;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        

        private async Task UpdateFood(FoodDto updated)
        {
            var success = await _httpRequest.PutAsync($"{url}/{updated.Id}", updated);
            if (success)
            {
                MessageBox.Show("Güncellendi!");
            }
        }

        private async void button1_Click(object sender, EventArgs e) // EKLE
        {
            var addForm = new FoodAddForm();
            if (addForm.ShowDialog() == DialogResult.OK)
                await GetAllFoods();
        }

        private async void button2_Click(object sender, EventArgs e) // SİL
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silmek için bir veya daha fazla öğe seçin.");
                return;
            }

            // Çoklu seçilen satırları listeye al
            var selectedFoods = new List<FoodDto>();
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (row.DataBoundItem is FoodDto food)
                {
                    selectedFoods.Add(food);
                }
            }

            if (selectedFoods.Count == 0)
            {
                MessageBox.Show("Seçilen öğeler alınamadı.");
                return;
            }

            // Tüm seçilen öğeler için genel bir onay mesajı göster
            var confirm = MessageBox.Show($"{selectedFoods.Count} adet yi silmek istiyor musunuz?", "Sil", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                int successCount = 0;

                // Tüm seçilen yiyecekleri sırayla sil
                foreach (var food in selectedFoods)
                {
                    bool success = await _httpRequest.DeleteAsync($"api/Food/soft", food.Id);
                    if (success)
                        successCount++;
                }

                MessageBox.Show($"{successCount} adet food silindi.");
                await GetAllFoods();
            }
        }


        private async void button3_Click(object sender, EventArgs e) // GÜNCELLE
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = dataGridView1.SelectedRows[0].DataBoundItem as FoodDto;
                if (selected == null)
                {
                    MessageBox.Show("Seçilen food bilgisi alınamadı.");
                    return;
                }

                var food = await _httpRequest.GetByIdAsync<FoodDto>(url, selected.Id);
                var updateForm = new FoodUpdateForm(food.Id, food.Name, food.FoodTypeId, food.Price);

                if (updateForm.ShowDialog() == DialogResult.OK)
                {
                    await UpdateFood(new FoodDto
                    {
                        Id = food.Id,
                        Name = updateForm.UpdatedFoodName,
                        Price = updateForm.UpdatedFoodPrice,
                        FoodTypeId = updateForm.UpdatedFoodTypeId
                    });

                    await GetAllFoods();
                }
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek için bir food seçin.");
            }
        }

        private async void button4_Click(object sender, EventArgs e) // DETAY
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = dataGridView1.SelectedRows[0].DataBoundItem as FoodDto;
                if (selected == null)
                {
                    MessageBox.Show("Seçilen food bilgisi alınamadı.");
                    return;
                }

                var food = await _httpRequest.GetByIdAsync<FoodDto>(url, selected.Id);
                MessageBox.Show($"ID: {food.Id}\nName: {food.Name}\nPrice: {food.Price}", "Food Detayı");
            }
            else
            {
                MessageBox.Show("Lütfen detay görmek için bir food seçin.");
            }
        }

        private async void button5_Click(object sender, EventArgs e) // YENİLE
        {
            await GetAllFoods();
        }

        private void button6_Click(object sender, EventArgs e) // GERİ DÖN
        {
            this.Close();
        }
    }
}
