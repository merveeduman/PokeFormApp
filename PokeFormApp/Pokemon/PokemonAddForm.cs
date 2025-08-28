using PokeFormApp.Autofac;
using PokeFormApp.Services;
using PokemonReviewApp.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class PokemonAddForm : Form
    {
        private readonly IHttpRequest _httpRequest;

        private List<OwnerDto> _owners;
        private List<CategoryDto> _categories;
        private List<FoodDto> _foods;

        public PokemonAddForm()
        {
            InitializeComponent();
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();

            button1.Click += button1_Click; // Kapat
            button2.Click += button2_Click; // Ekle

            this.Load += async (s, e) => await LoadFormData();
        }

        private async Task LoadFormData()
        {
            await LoadOwnersAsync();
            await LoadCategoriesAsync();
            await LoadFoodsAsync();
        }

        private async Task LoadOwnersAsync()
        {
            try
            {
                _owners = await _httpRequest.GetAllAsync<List<OwnerDto>>("api/Owner");

                var ownerList = _owners.ConvertAll(o => new
                {
                    Id = o.Id,
                    FullName = $"{o.FirstName} {o.LastName}"
                });

                comboBox1.DataSource = ownerList;
                comboBox1.DisplayMember = "FullName";
                comboBox1.ValueMember = "Id";
                comboBox1.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Owner verileri alınamadı: " + ex.Message);
            }
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                _categories = await _httpRequest.GetAllAsync<List<CategoryDto>>("api/Category");

                comboBox2.DataSource = _categories;
                comboBox2.DisplayMember = "Name";
                comboBox2.ValueMember = "Id";
                comboBox2.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kategori verileri alınamadı: " + ex.Message);
            }
        }

        private async Task LoadFoodsAsync()
        {
            try
            {
                _foods = await _httpRequest.GetAllAsync<List<FoodDto>>("api/Food");

                comboBox3.DataSource = _foods;
                comboBox3.DisplayMember = "Name";
                comboBox3.ValueMember = "Id";
                comboBox3.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Food verileri alınamadı: " + ex.Message);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string birthDateStr = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(name) || !DateTime.TryParse(birthDateStr, out DateTime birthDate))
            {
                MessageBox.Show("Geçerli bir ad ve doğum tarihi giriniz.");
                return;
            }

            if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1 || comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz (Owner, Category, Food).");
                return;
            }

            int ownerId = (int)comboBox1.SelectedValue;
            int categoryId = (int)comboBox2.SelectedValue;
            int foodId = (int)comboBox3.SelectedValue;

            var newPokemon = new PokemonDto
            {
                Name = name,
                BirthDate = birthDate
            };

            try
            {
                string endpoint = $"api/Pokemon?ownerId={ownerId}&catId={categoryId}&foodId={foodId}";
                bool success = await _httpRequest.PostAsync(endpoint, newPokemon);

                if (success)
                {
                    MessageBox.Show("Pokemon başarıyla eklendi!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ekleme başarısız.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
