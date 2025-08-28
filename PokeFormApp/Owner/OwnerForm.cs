using PokeFormApp.Autofac;
using PokeFormApp.Services;
using PokemonReviewApp.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp.Owner
{
    public partial class OwnerForm : Form
    {
        private readonly IHttpRequest _httpRequest;
        private readonly string url = "api/Owner";
        private bool columnsCreated = false;

        public OwnerForm()
        {
            InitializeComponent();

            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();

            this.Load += async (s, e) => await GetAllOwners();
        }

        private async Task GetAllOwners()
        {
            try
            {
                var owners = await _httpRequest.GetAllAsync<List<OwnerDto>>(url);

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
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private async Task DeleteOwner(int id)
        {
            var success = await _httpRequest.DeleteAsync($"{url}/soft", id); // Eğer soft delete destekleniyorsa
            if (success)
            {
                MessageBox.Show("Owner silindi!");
                await GetAllOwners();
            }
            else
            {
                MessageBox.Show("Silme işlemi başarısız.");
            }
        }

        private async Task UpdateOwner(OwnerDto updated)
        {
            var success = await _httpRequest.PutAsync($"{url}/{updated.Id}", updated);
            if (success)
            {
                MessageBox.Show("Owner güncellendi!");
            }
            else
            {
                MessageBox.Show("Güncelleme başarısız.");
            }
        }

        private async void button1_Click(object sender, EventArgs e) // EKLE
        {
            var addForm = new OwnerAddForm();
            if (addForm.ShowDialog() == DialogResult.OK)
                await GetAllOwners();
        }

        private async void button2_Click(object sender, EventArgs e) // SİL
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = dataGridView1.SelectedRows[0].DataBoundItem as OwnerDto;
                if (selected == null)
                {
                    MessageBox.Show("Seçilen owner bilgisi alınamadı.");
                    return;
                }

                var confirm = MessageBox.Show($"{selected.FirstName} {selected.LastName} silinsin mi?", "Sil", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    await DeleteOwner(selected.Id);
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir owner seçin.");
            }
        }

        private async void button3_Click(object sender, EventArgs e) // GÜNCELLE
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = (OwnerDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var owner = await _httpRequest.GetByIdAsync<OwnerDto>(url, selected.Id);

                var updateForm = new OwnerUpdateForm(owner.Id, owner.FirstName, owner.LastName, owner.Gym);

                if (updateForm.ShowDialog() == DialogResult.OK && updateForm.UpdatedOwner != null)
                {
                    await UpdateOwner(updateForm.UpdatedOwner);
                    await GetAllOwners();
                }
            }
        }

        private async void button4_Click(object sender, EventArgs e) // DETAY
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selected = (OwnerDto)dataGridView1.SelectedRows[0].DataBoundItem;
                var detail = await _httpRequest.GetByIdAsync<OwnerDto>(url, selected.Id);

                MessageBox.Show($"ID: {detail.Id}\nFirst Name: {detail.FirstName}\nLast Name: {detail.LastName}\nGym: {detail.Gym}", "Owner Detayı");
            }
        }

        private async void button5_Click(object sender, EventArgs e) // YENİLE
        {
            await GetAllOwners();
        }

        private void button6_Click(object sender, EventArgs e) // GERİ DÖN
        {
            this.Close();
        }
    }
}
