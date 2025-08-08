using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using PokemonReviewApp.Dto;

namespace PokeFormApp.Reviewer
{
    public partial class ReviewerUpdateForm : Form
    {
        private int reviewerId;
        public ReviewerDto UpdatedReviewer { get; private set; }

        public ReviewerUpdateForm(int id, string firstName, string lastName)
        {
            InitializeComponent();

            reviewerId = id;

            textBox1.Text = firstName;
            textBox2.Text = lastName;

            button1.Click += button1_Click;
            button2.Click += (s, e) => this.Close();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string firstName = textBox1.Text.Trim();
            string lastName = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("İsim ve soyisim boş olamaz.");
                return;
            }

            var updatedReviewer = new ReviewerDto
            {
                Id = reviewerId,
                FirstName = firstName,
                LastName = lastName
            };

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7091/");
                var json = JsonSerializer.Serialize(updatedReviewer);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"api/Reviewer/{reviewerId}", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Reviewer güncellendi.");
                    UpdatedReviewer = updatedReviewer;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Güncelleme başarısız! StatusCode: {response.StatusCode}\nDetay: {error}");
                }
            }
        }
    }
}
