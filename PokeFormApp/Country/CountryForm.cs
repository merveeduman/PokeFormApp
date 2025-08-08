using PokemonReviewApp.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class CountryForm : Form
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7091/")
        };

        private bool columnsCreated = false;

        public CountryForm()
        {
            InitializeComponent();
            this.Load += async (s, e) => await GetAllCountries();
        }

        private async Task GetAllCountries()
        {
            try
            {
                var response = await client.GetAsync("api/Country");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var countries = JsonConvert.DeserializeObject<List<CountryDto>>(json);

                    if (!columnsCreated)
                    {
                        dataGridView1.AutoGenerateColumns = false;
                        dataGridView1.Columns.Clear();

                        var colId = new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Width = 50 };
                        var colName = new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Name", Width = 150 };

                        dataGridView1.Columns.Add(colId);
                        dataGridView1.Columns.Add(colName);

                        columnsCreated = true;
                    }

                    dataGridView1.DataSource = countries;
                }
                else
                {
                    MessageBox.Show("Veriler çekilemedi. Kod: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private async Task DeleteCountry(int id)
        {
            var response = await client.DeleteAsync($"api/Country/{id}");
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Silindi!");
                await GetAllCountries();
            }
            else
            {
                MessageBox.Show("Silme işlemi başarısız: " + response.StatusCode);
            }
        }

        private async Task UpdateCountry(CountryDto updated)
        {
            var json = JsonConvert.SerializeObject(updated);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/Country/{updated.Id}", content);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Güncellendi!");
            }
            else
            {
                MessageBox.Show("Güncelleme başarısız: " + response.StatusCode);
            }
        }

        private async void button1_Click(object sender, EventArgs e) // EKLE
        {
            var addForm = new CountryAddForm();
            var result = addForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                await GetAllCountries();
            }
        }

        private async void button2_Click(object sender, EventArgs e) // SİL
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var secili = (CountryDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var confirm = MessageBox.Show($"{secili.Name} ülkesini silmek istiyor musunuz?", "Sil", MessageBoxButtons.YesNo);

                if (confirm == DialogResult.Yes)
                {
                    await DeleteCountry(secili.Id);
                }
            }
        }

        private async void button3_Click(object sender, EventArgs e) // GÜNCELLE
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var secili = (CountryDto)dataGridView1.SelectedRows[0].DataBoundItem;

                var updateForm = new CountryUpdateForm(secili.Id, secili.Name);
                var result = updateForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    secili.Name = updateForm.UpdatedCountryName;
                    await UpdateCountry(secili);
                    await GetAllCountries();
                }
            }
        }

        private async void button4_Click(object sender, EventArgs e) // YENİLE
        {
            await GetAllCountries();
        }

        private void button5_Click(object sender, EventArgs e) // GERİ DÖN
        {
            var anaForm = new Form1();
            anaForm.Show();
            this.Close();
        }

        private async void button6_Click(object sender, EventArgs e) // DETAY
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var secili = (CountryDto)dataGridView1.SelectedRows[0].DataBoundItem;

                var response = await client.GetAsync($"api/Country/{secili.Id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var detay = JsonConvert.DeserializeObject<CountryDto>(json);

                    MessageBox.Show($"ID: {detay.Id}\nName: {detay.Name}", "Ülke Detayı");
                }
            }
        }
    }
}
