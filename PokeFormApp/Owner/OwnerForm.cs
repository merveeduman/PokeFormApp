using Newtonsoft.Json;
using PokemonReviewApp.Dto; // <-- Dto burada da kullanılmalı
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp.Owner
{
    public partial class OwnerForm : Form
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7091/")
        };

        private bool columnsCreated = false;

        public OwnerForm()
        {
            InitializeComponent();
            this.Load += async (s, e) => await GetAllOwners();
        }

        private async Task GetAllOwners()
        {
            try
            {
                var response = await client.GetAsync("api/Owner");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var owners = JsonConvert.DeserializeObject<List<OwnerDto>>(json);

                    if (!columnsCreated)
                    {
                        dataGridView1.AutoGenerateColumns = false;
                        dataGridView1.Columns.Clear();

                        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Width = 50 });
                        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FirstName", HeaderText = "First Name", Width = 100 });
                        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "LastName", HeaderText = "Last Name", Width = 100 });
                        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Gym", HeaderText = "Gym", Width = 150 });

                        columnsCreated = true;
                    }

                    dataGridView1.DataSource = owners;
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

        private async Task DeleteOwner(int id)
        {
            var response = await client.DeleteAsync($"api/Owner/{id}");
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Owner silindi!");
                await GetAllOwners();
            }
            else
            {
                MessageBox.Show($"Silme başarısız: {response.StatusCode}");
            }
        }

        private async Task UpdateOwner(OwnerDto updated)
        {
            var json = JsonConvert.SerializeObject(updated);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/Owner/{updated.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Owner güncellendi!");
            }
            else
            {
                MessageBox.Show("Güncelleme başarısız: " + response.StatusCode);
            }
        }

        private async void button1_Click(object sender, EventArgs e) // Ekle
        {
            var addForm = new OwnerAddForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                await GetAllOwners();
            }
        }

        private async void button2_Click(object sender, EventArgs e) // Sil
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = (OwnerDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var confirm = MessageBox.Show($"{selected.FirstName} {selected.LastName} silinsin mi?", "Sil", MessageBoxButtons.YesNo);

                if (confirm == DialogResult.Yes)
                {
                    await DeleteOwner(selected.Id);
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir Owner seçin!");
            }
        }

        private async void button3_Click(object sender, EventArgs e) // Güncelle
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = (OwnerDto)dataGridView1.SelectedRows[0].DataBoundItem;

                var updateForm = new OwnerUpdateForm(selected.Id, selected.FirstName, selected.LastName, selected.Gym);
                if (updateForm.ShowDialog() == DialogResult.OK && updateForm.UpdatedOwner != null)
                {
                    await UpdateOwner(updateForm.UpdatedOwner);
                    await GetAllOwners();
                }
            }
        }

        private async void button4_Click(object sender, EventArgs e) // Detay
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = (OwnerDto)dataGridView1.SelectedRows[0].DataBoundItem;

                var response = await client.GetAsync($"api/Owner/{selected.Id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var detay = JsonConvert.DeserializeObject<OwnerDto>(json);

                    MessageBox.Show($"ID: {detay.Id}\nFirst Name: {detay.FirstName}\nLast Name: {detay.LastName}\nGym: {detay.Gym}", "Owner Detayı");
                }
            }
        }

        private async void button5_Click(object sender, EventArgs e) // Yenile
        {
            await GetAllOwners();
        }

        private void button6_Click(object sender, EventArgs e) // Geri Dön
        {
            var mainForm = new Form1();
            mainForm.Show();
            this.Close();
        }
    }
}
