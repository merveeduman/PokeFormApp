using PokeFormApp.Autofac;
using PokeFormApp.Services;  // IHttpRequest servisi için
using PokemonReviewApp.Dto;
using System;
using System.Windows.Forms;

namespace PokeFormApp.Reviewer
{
    public partial class ReviewerAddForm : Form
    {
        private readonly IHttpRequest _httpRequest;

        public string FirstName => textBox1.Text.Trim();
        public string LastName => textBox2.Text.Trim();

        public ReviewerAddForm()
        {
            InitializeComponent();
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();

        }

        private async void button1_Click(object sender, EventArgs e) // Ekle butonu
        {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }

            var newReviewer = new ReviewerDto
            {
                FirstName = FirstName,
                LastName = LastName
            };

            try
            {
                bool success = await _httpRequest.PostAsync("api/Reviewer", newReviewer);

                if (success)
                {
                    MessageBox.Show("Reviewer başarıyla eklendi!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ekleme başarısız oldu.");
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
