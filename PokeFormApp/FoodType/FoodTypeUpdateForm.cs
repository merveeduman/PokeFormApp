using PokeFormApp.Autofac;
using PokeFormApp.Services; // IHttpRequest için
using PokemonReviewApp.Dto;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PokeFormApp
{
    public partial class FoodTypeUpdateForm : Form
    {
        private readonly int foodTypeId;
        private readonly IHttpRequest _httpRequest;

        public string UpdatedFoodTypeName => textBoxName.Text.Trim();

        public FoodTypeUpdateForm(int id, string name)
        {
            InitializeComponent();
            foodTypeId = id;
            textBoxName.Text = name;
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();
        }

        private async void button1_Click(object sender, EventArgs e) // Güncelle
        {
            if (string.IsNullOrEmpty(UpdatedFoodTypeName))
            {
                MessageBox.Show("Food type adı boş olamaz.");
                return;
            }

            var updatedFoodType = new FoodTypeDto
            {
                Id = foodTypeId,
                Name = UpdatedFoodTypeName
            };

            try
            {
                bool success = await _httpRequest.PutAsync($"api/FoodType/{foodTypeId}", updatedFoodType);

                if (success)
                {
                    MessageBox.Show("FoodType güncellendi.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Güncelleme başarısız!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e) // İptal
        {
            this.Close();
        }
    }
}
