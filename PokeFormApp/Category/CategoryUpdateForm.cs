using Newtonsoft.Json;
using PokemonReviewApp.Dto;  // DTO namespace
using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class CategoryUpdateForm : Form
    {
        private int categoryId;

        public CategoryUpdateForm(int id, string name)
        {
            InitializeComponent();
            categoryId = id;
            textBox1.Text = name;  // Kategori ismi inputu
        }

        private async void button1_Click(object sender, EventArgs e) // Güncelleme butonu
        {
            string newName = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(newName))
            {
                MessageBox.Show("Kategori adı boş olamaz.");
                return;
            }

            var updatedCategory = new CategoryDto
            {
                Id = categoryId,
                Name = newName
            };

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7091/");
                string json = JsonConvert.SerializeObject(updatedCategory);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"api/Category/{categoryId}", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Kategori güncellendi.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Güncelleme başarısız!");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) // Kapatma butonu
        {
            this.Close();
        }
    }
}
