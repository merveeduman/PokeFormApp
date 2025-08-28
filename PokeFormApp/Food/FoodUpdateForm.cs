using PokeFormApp.Autofac;
using PokeFormApp.Services;
using PokemonReviewApp.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class FoodUpdateForm : Form
    {
        private int foodId;
        private readonly IHttpRequest _httpRequest;

        public string UpdatedFoodName => textBox1.Text.Trim();
        public int UpdatedFoodTypeId
        {
            get
            {
                if (comboBox1.SelectedValue == null)
                    return 0;

                if (int.TryParse(comboBox1.SelectedValue.ToString(), out int val))
                    return val;
                return 0;
            }
        }

        public decimal UpdatedFoodPrice
        {
            get
            {
                decimal.TryParse(textBox2.Text.Trim(), out decimal price);
                return price;
            }
        }

        public FoodUpdateForm(int id, string name, int foodTypeId, decimal price)
        {
            InitializeComponent();

            foodId = id;
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();

            textBox1.Text = name;
            textBox2.Text = price.ToString();

            this.Load += async (s, e) => await LoadFoodTypes(foodTypeId);
        }

        private async Task LoadFoodTypes(int selectedId)
        {
            try
            {
                var foodTypes = await _httpRequest.GetAllAsync<List<FoodTypeDto>>("api/FoodType");

                if (foodTypes == null || foodTypes.Count == 0)
                {
                    MessageBox.Show("FoodType listesi boş döndü!");
                    return;
                }

                comboBox1.DataSource = foodTypes;
                comboBox1.DisplayMember = "Name"; // Buradaki "Name" property FoodTypeDto'da var mı kontrol et
                comboBox1.ValueMember = "Id";     // Buradaki "Id" property FoodTypeDto'da var mı kontrol et
                comboBox1.SelectedValue = selectedId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("FoodType'lar yüklenemedi: " + ex.Message);
            }
        }


        private async void button2_Click(object sender, EventArgs e) // Güncelle
        {
            if (string.IsNullOrWhiteSpace(UpdatedFoodName))
            {
                MessageBox.Show("Yemek adı boş olamaz.");
                return;
            }

            if (UpdatedFoodTypeId == 0)
            {
                MessageBox.Show("Lütfen bir yemek tipi seçin.");
                return;
            }

            decimal price;
            string rawInput = textBox2.Text.Trim().Replace(",", ".");

            if (!decimal.TryParse(rawInput, System.Globalization.NumberStyles.Any,
                                  System.Globalization.CultureInfo.InvariantCulture, out price) || price <= 0)
            {
                MessageBox.Show("Geçerli bir fiyat girin (örnek: 250.50).");
                return;
            }

            var updatedFood = new FoodDto
            {
                Id = foodId,
                Name = UpdatedFoodName,
                FoodTypeId = UpdatedFoodTypeId,
                Price = price
            };

            try
            {
                bool success = await _httpRequest.PutAsync($"api/Food/{foodId}", updatedFood);

                if (success)
                {
                    MessageBox.Show("Yemek başarıyla güncellendi.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Güncelleme başarısız oldu.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e) // Kapat
        {
            this.Close();
        }
    }
}
