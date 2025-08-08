using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PokemonReviewApp.Dto;

namespace PokeFormApp
{
    public partial class PokemonForm : Form
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7091/")
        };

        public PokemonForm()
        {
            InitializeComponent();
            this.Load += async (s, e) => await GetAllPokemons();
          

        }

        private async Task GetAllPokemons()
        {
            try
            {
                var response = await client.GetAsync("api/Pokemon");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var pokemons = JsonConvert.DeserializeObject<List<PokemonDto>>(json);
                    dataGridView1.DataSource = pokemons;

                   
                }
                else
                {
                    MessageBox.Show($"Veriler çekilemedi. StatusCode: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        private async Task AddPokemon(PokemonDto yeni)
        {
            try
            {
                var json = JsonConvert.SerializeObject(yeni);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/Pokemon", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Pokemon başarıyla eklendi!");
                    await GetAllPokemons();
                }
                else
                {
                    MessageBox.Show("Ekleme başarısız oldu!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private async Task DeletePokemon(int pokeId)
        {
            var response = await client.DeleteAsync($"api/Pokemon/{pokeId}");
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Silindi!");
                await GetAllPokemons();
            }
        }

        private async Task UpdatePokemon(PokemonDto guncel)
        {
            var json = JsonConvert.SerializeObject(guncel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/Pokemon/{guncel.Id}", content);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Güncellendi!");
                await GetAllPokemons();
            }
        }

        private async Task ShowPokemonDetails(int id)
        {
            var response = await client.GetAsync($"api/Pokemon/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var pokemon = JsonConvert.DeserializeObject<PokemonDto>(json);
                MessageBox.Show($"ID: {pokemon.Id}\nAd: {pokemon.Name}\nDoğum Tarihi: {pokemon.BirthDate:yyyy-MM-dd}", "Detay");
            }
        }

        private void btnGeriDon_Click(object sender, EventArgs e)
        {
            Form1 anaForm = new Form1();
            anaForm.Show();
            this.Close();
        }

        private async void btnEkleme_Click(object sender, EventArgs e)
        {
            var addForm = new PokemonAddForm();
            var result = addForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                await GetAllPokemons();
            }
        }

        private async void btnSilme_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var secili = (PokemonDto)dataGridView1.SelectedRows[0].DataBoundItem;
                await DeletePokemon(secili.Id);
            }
        }

        private async void btnGuncelleme_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var secili = (PokemonDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var updateForm = new PokemonUpdateForm(secili.Name, secili.BirthDate);
                var result = updateForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    secili.Name = updateForm.PokemonName;
                    secili.BirthDate = updateForm.PokemonBirthDate;
                    await UpdatePokemon(secili);
                }
            }
        }

        private async void btnDetay_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var secili = (PokemonDto)dataGridView1.SelectedRows[0].DataBoundItem;
                await ShowPokemonDetails(secili.Id);
            }
        }

        private async void btnYenile_Click(object sender, EventArgs e)
        {
            await GetAllPokemons();
        }
    }
}
