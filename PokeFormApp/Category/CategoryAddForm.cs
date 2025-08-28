using PokeFormApp.Autofac;
using PokeFormApp.Services;  // IHttpRequest için
using PokemonReviewApp.Dto;
using System;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class CategoryAddForm : Form
    {
        private readonly IHttpRequest _httpRequest;

        public CategoryAddForm()
        {
            InitializeComponent();
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();
        }

        private async void button1_Click(object sender, EventArgs e) // Ekle butonu
        {
            string categoryName = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Lütfen bir kategori ismi girin.");
                return;
            }

            var newCategory = new CategoryDto { Name = categoryName };

            try
            {
                bool success = await _httpRequest.PostAsync("api/Category", newCategory);

                if (success)
                {
                    MessageBox.Show("Kategori başarıyla eklendi!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Kategori ekleme başarısız oldu.");
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
