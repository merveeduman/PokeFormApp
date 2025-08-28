using PokeFormApp.Autofac;
using PokeFormApp.Services;
using PokemonReviewApp.Dto;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp.Owner
{
    public partial class OwnerUpdateForm : Form
    {
        private readonly int ownerId;
        private readonly IHttpRequest _httpRequest;

        public OwnerDto UpdatedOwner { get; private set; }

        public OwnerUpdateForm(int id, string firstName, string lastName, string gym)
        {
            InitializeComponent();

            ownerId = id;
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();

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
                bool success = await _httpRequest.PutAsync($"api/Owner/{ownerId}", updatedOwner);

                if (success)
                {
                    UpdatedOwner = updatedOwner;
                    MessageBox.Show("Owner başarıyla güncellendi!");
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
