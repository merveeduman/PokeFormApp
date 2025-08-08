using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using PokemonReviewApp.Dto;

namespace PokeFormApp.Reviewer
{
    public partial class ReviewerAddForm : Form
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7091/")
        };

        public ReviewerAddForm()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string firstName = textBox1.Text.Trim();
            string lastName = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }

            var newReviewer = new ReviewerDto
            {
                FirstName = firstName,
                LastName = lastName
            };

            var json = JsonConvert.SerializeObject(newReviewer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Reviewer", content);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Reviewer eklendi!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Ekleme başarısız.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
