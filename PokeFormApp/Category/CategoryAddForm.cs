using Newtonsoft.Json;
using PokemonReviewApp.Dto; // DTO namespace'i
using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class CategoryAddForm : Form
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7091/")
        };

        public CategoryAddForm()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e) // Ekle butonu
        {
            string categoryName = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Lütfen bir kategori ismi girin.");
                return;
            }

            var newCategory = new CategoryDto
            {
                Name = categoryName
            };

            try
            {
                string json = JsonConvert.SerializeObject(newCategory);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/Category", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Kategori eklendi!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ekleme başarısız: " + response.ReasonPhrase);
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
