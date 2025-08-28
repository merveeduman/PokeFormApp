using PokeFormApp.Autofac;
using PokeFormApp.Services; // IHttpRequest için
using PokemonReviewApp.Dto;
using System;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class CategoryUpdateForm : Form
    {
        private int categoryId;
        private readonly IHttpRequest _httpRequest;

        public string UpdatedCategoryName => textBox1.Text.Trim();

        public CategoryUpdateForm(int id, string name)
        {
            InitializeComponent();
            categoryId = id;
            textBox1.Text = name;
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string newName = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(newName))
            {
                MessageBox.Show("Kategori adı boş olamaz.");
                return;
            }

            var updatedCategory = new CategoryDto { Id = categoryId, Name = newName };

            try
            {
                bool success = await _httpRequest.PutAsync("api/Category/" + categoryId, updatedCategory);

                if (success)
                {
                    MessageBox.Show("Kategori güncellendi.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
             
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
