using PokeFormApp.Autofac;
using PokeFormApp.Services; // IHttpRequest için
using PokemonReviewApp.Dto;
using System;
using System.Windows.Forms;

namespace PokeFormApp.Reviewer
{
    public partial class ReviewerUpdateForm : Form
    {
        private int reviewerId;
        private readonly IHttpRequest _httpRequest;

        public string UpdatedFirstName => textBox1.Text.Trim();
        public string UpdatedLastName => textBox2.Text.Trim();

        public ReviewerUpdateForm(int id, string firstName, string lastName)
        {
            InitializeComponent();
            reviewerId = id;
            textBox1.Text = firstName;
            textBox2.Text = lastName;
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();


            button1.Click += button1_Click;
            button2.Click += (s, e) => this.Close();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UpdatedFirstName) || string.IsNullOrEmpty(UpdatedLastName))
            {
                MessageBox.Show("İsim ve soyisim boş olamaz.");
                return;
            }

            var updatedReviewer = new ReviewerDto
            {
                Id = reviewerId,
                FirstName = UpdatedFirstName,
                LastName = UpdatedLastName
            };

            try
            {
                bool success = await _httpRequest.PutAsync($"api/Reviewer/{reviewerId}", updatedReviewer);
                if (success)
                {
                    
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Güncelleme başarısız.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }
}
