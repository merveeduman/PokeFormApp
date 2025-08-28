using PokeFormApp.Autofac;
using PokeFormApp.Services;
using PokemonReviewApp.Dto;

namespace PokeFormApp
{
    public partial class PokemonForm : Form
    {
        private readonly IHttpRequest _httpRequest;
        private readonly string url = "api/Pokemon";
        private bool columnsCreated = false;

        public PokemonForm()
        {
            InitializeComponent();
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();
            this.Load += async (s, e) => await GetAllPokemons();
        }

        private async Task GetAllPokemons()
        {
            try
            {
                var pokemons = await _httpRequest.GetAllAsync<List<PokemonDto>>(url);

                if (!columnsCreated)
                {
                    dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.Columns.Clear();

                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Id",
                        HeaderText = "ID",
                        Width = 50
                    });

                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Name",
                        HeaderText = "Adı",
                        Width = 150
                    });

                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "BirthDate",
                        HeaderText = "Doğum Tarihi",
                        Width = 150
                    });

                    columnsCreated = true;
                }

                dataGridView1.DataSource = pokemons;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private async Task DeletePokemon(int id)
        {
            var success = await _httpRequest.DeleteAsync($"{url}/soft", id);
            if (success)
            {
                MessageBox.Show("Pokemon silindi.");
                await GetAllPokemons();
            }
            else
            {
                MessageBox.Show("Silme işlemi başarısız.");
            }
        }

        private async Task UpdatePokemon(PokemonDto updated)
        {
            var success = await _httpRequest.PutAsync($"{url}/{updated.Id}", updated);
            if (success)
            {
                MessageBox.Show("Güncellendi!");
                await GetAllPokemons();
            }
        }

        private async void btnEkleme_Click(object sender, EventArgs e)
        {
            var addForm = new PokemonAddForm();
            if (addForm.ShowDialog() == DialogResult.OK)
                await GetAllPokemons();
        }

        private async void btnSilme_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var secili = (PokemonDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var confirm = MessageBox.Show($"{secili.Name} adlı pokemonu silmek istiyor musunuz?", "Sil", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    await DeletePokemon(secili.Id);
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir pokemon seçin.");
            }
        }

        private async void btnGuncelleme_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var secili = (PokemonDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var pokemon = await _httpRequest.GetByIdAsync<PokemonDto>(url, secili.Id);

                var updateForm = new PokemonUpdateForm(pokemon.Id, pokemon.Name, pokemon.BirthDate);

                if (updateForm.ShowDialog() == DialogResult.OK)
                {
                    await UpdatePokemon(new PokemonDto
                    {
                        Id = pokemon.Id,
                        Name = updateForm.PokemonName,
                        BirthDate = updateForm.PokemonBirthDate
                    });

                }
            }
        }

        private async void btnYenile_Click(object sender, EventArgs e)
        {
            await GetAllPokemons();
        }

        private void btnGeriDon_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnDetay_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var secili = (PokemonDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var detay = await _httpRequest.GetByIdAsync<PokemonDto>(url, secili.Id);

                MessageBox.Show($"ID: {detay.Id}\nAd: {detay.Name}\nDoğum Tarihi: {detay.BirthDate:yyyy-MM-dd}", "Pokemon Detayı");
            }
        }
    }
}
