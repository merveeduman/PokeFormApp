using PokeFormApp.Autofac;
using PokeFormApp.Services;  // IHttpRequest için
using PokemonReviewApp.Dto;
using System;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class FoodAddForm : Form
    {
        private readonly IHttpRequest _httpRequest;

        public FoodAddForm()
        {
            InitializeComponent();
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();

            this.Load += async (s, e) => await LoadFoodTypes();
        }

        private async Task LoadFoodTypes()
        {
            try
            {
                var foodTypes = await _httpRequest.GetAllAsync<List<FoodTypeDto>>("api/FoodType");

                comboBox1.DataSource = foodTypes;
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "Id";
                comboBox1.SelectedIndex = -1; // varsayılan boş
            }
            catch (Exception ex)
            {
                MessageBox.Show("FoodType'lar yüklenemedi: " + ex.Message);
            }
        }

        private async void button2_Click(object sender, EventArgs e) // Ekle butonu
        {
            string foodName = textBox1.Text.Trim();
            string priceText = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(foodName) || string.IsNullOrEmpty(priceText))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }

            if (!decimal.TryParse(priceText, out decimal price))
            {
                MessageBox.Show("Geçerli bir fiyat girin.");
                return;
            }

            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Lütfen bir yemek tipi seçin.");
                return;
            }

            int foodTypeId = (int)comboBox1.SelectedValue;
            if (foodTypeId == 0)
            {
                MessageBox.Show("Lütfen bir yemek tipi seçin.");
                return;
            }


            var newFood = new FoodDto
            {
                Name = foodName,
                Price = price,
                FoodTypeId = foodTypeId
            };

            try
            {
                bool success = await _httpRequest.PostAsync("api/Food", newFood);

                if (success)
                {
                    MessageBox.Show("Food başarıyla eklendi!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Food ekleme başarısız oldu.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e) // Kapat butonu
        {
            this.Close();
        }
    }

}
