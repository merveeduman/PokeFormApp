using PokeFormApp.Autofac;
using PokeFormApp.Services;
using PokemonReviewApp.Dto;
using System;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class PokemonUpdateForm : Form
    {
        private readonly int _pokemonId;
        private readonly IHttpRequest _httpRequest;

        public string PokemonName => textBox1.Text.Trim();
        public DateTime PokemonBirthDate => DateTime.Parse(textBox2.Text.Trim());

        public PokemonUpdateForm(int id, string name, DateTime birthDate)
        {
            InitializeComponent();
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();

            _pokemonId = id;
            textBox1.Text = name;
            textBox2.Text = birthDate.ToString("yyyy-MM-dd");
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string name = PokemonName;
            string dateStr = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Pokemon adı boş olamaz.");
                return;
            }

            if (!DateTime.TryParse(dateStr, out DateTime birthDate))
            {
                MessageBox.Show("Geçerli bir doğum tarihi giriniz. (örn: 2023-01-01)");
                return;
            }

            var updatedPokemon = new PokemonDto
            {
                Id = _pokemonId,
                Name = name,
                BirthDate = birthDate
            };

            try
            {
                bool success = await _httpRequest.PutAsync($"api/Pokemon/{_pokemonId}", updatedPokemon);

                if (success)
                {
                    MessageBox.Show("Pokemon güncellendi.");
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
