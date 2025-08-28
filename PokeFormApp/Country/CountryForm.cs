using PokeFormApp.Autofac;
using PokeFormApp.Services;
using PokemonReviewApp.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class CountryForm : Form
    {
        private readonly IHttpRequest _httpRequest;
        private readonly string url = "api/Country";
        private bool columnsCreated = false;

        public CountryForm()
        {
            InitializeComponent();

            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();

            this.Load += async (s, e) => await GetAllCountries();
        }

        private async Task GetAllCountries()
        {
            try
            {
                var countries = await _httpRequest.GetAllAsync<List<CountryDto>>(url);

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
                        HeaderText = "Name",
                        Width = 150
                    });

                    columnsCreated = true;
                }

                dataGridView1.DataSource = countries;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private async Task DeleteCountry(int id)
        {
            var success = await _httpRequest.DeleteAsync($"{url}/soft", id);
            if (success)
            {
                MessageBox.Show("Ülke silindi.");
                await GetAllCountries();
            }
            else
            {
                MessageBox.Show("Silme işlemi başarısız.");
            }
        }


        private async Task UpdateCountry(CountryDto updated)
        {
            var success = await _httpRequest.PutAsync($"{url}/{updated.Id}", updated);
         
        }



        private async void button1_Click(object sender, EventArgs e) // EKLE
        {
            var addForm = new CountryAddForm();
            if (addForm.ShowDialog() == DialogResult.OK)
                await GetAllCountries();
        }

        private async void button2_Click(object sender, EventArgs e) // SİL
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var secili = dataGridView1.SelectedRows[0].DataBoundItem as CountryDto;
                if (secili == null)
                {
                    MessageBox.Show("Seçilen ülke bilgisi alınamadı.");
                    return;
                }

                var confirm = MessageBox.Show($"{secili.Name} ülkesini silmek istiyor musunuz?", "Sil", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    await DeleteCountry(secili.Id);
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir ülke seçin.");
            }
        }


        private async void button3_Click(object sender, EventArgs e) // GÜNCELLE
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var secili = (CountryDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var country = await _httpRequest.GetByIdAsync<CountryDto>(url, secili.Id);

                var updateForm = new CountryUpdateForm(country.Id, country.Name);


                if (updateForm.ShowDialog() == DialogResult.OK)
                {
                    await UpdateCountry(new CountryDto
                    {
                        Id = country.Id,
                        Name = updateForm.UpdatedCountryName
                    });

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
            this.Close();
        }

        private async void button6_Click(object sender, EventArgs e) // DETAY
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var secili = (CountryDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var detay = await _httpRequest.GetByIdAsync<CountryDto>(url, secili.Id);

                MessageBox.Show($"ID: {detay.Id}\nName: {detay.Name}", "Ülke Detayı");
            }
        }
    }
}
