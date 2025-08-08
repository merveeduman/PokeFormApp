using Newtonsoft.Json;
using PokemonReviewApp.Dto;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp.Owner
{
    public partial class OwnerUpdateForm : Form
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7091/")
        };

        private int ownerId;

        public OwnerDto UpdatedOwner { get; private set; }

        public OwnerUpdateForm(int id, string firstName, string lastName, string gym)
        {
            InitializeComponent();

            ownerId = id;

            textBox1.Text = id.ToString();
            textBox1.ReadOnly = true;
            textBox2.Text = firstName;
            textBox3.Text = lastName;
            textBox4.Text = gym;

            button1.Click += async (s, e) => await UpdateOwnerAsync();
            button2.Click += (s, e) => this.Close();
        }

        private async Task UpdateOwnerAsync()
        {
            string firstName = textBox2.Text.Trim();
            string lastName = textBox3.Text.Trim();
            string gym = textBox4.Text.Trim();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("FirstName ve LastName boş olamaz!");
                return;
            }

            var updatedOwner = new OwnerDto
            {
                Id = ownerId,
                FirstName = firstName,
                LastName = lastName,
                Gym = gym
            };

            try
            {
                var json = JsonConvert.SerializeObject(updatedOwner);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"api/Owner/{ownerId}", content);

                if (response.IsSuccessStatusCode)
                {
                    UpdatedOwner = updatedOwner;
                    MessageBox.Show("Owner başarıyla güncellendi!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Güncelleme başarısız: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }
}
