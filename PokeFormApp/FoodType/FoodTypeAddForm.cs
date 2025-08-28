using PokeFormApp.Autofac;
using PokeFormApp.Services;
using PokemonReviewApp.Dto;
using System;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class FoodTypeAddForm : Form
    {
        private readonly IHttpRequest _httpRequest;

        public FoodTypeAddForm()
        {
            InitializeComponent();
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();
        }

        private async void button1_Click(object sender, EventArgs e) // Ekle butonu
        {
            string name = textBoxName.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Lütfen bir food type ismi girin.");
                return;
            }

            var newFoodType = new FoodTypeDto { Name = name };

            try
            {
                bool success = await _httpRequest.PostAsync("api/FoodType", newFoodType);

                if (success)
                {
                    MessageBox.Show("FoodType başarıyla eklendi!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("FoodType ekleme başarısız oldu.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e) // Kapat butonu
        {
            this.Close();
        }
    }
}
