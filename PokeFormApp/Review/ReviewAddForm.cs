using PokeFormApp.Autofac;
using PokeFormApp.Services;  // IHttpRequest servisi için
using PokemonReviewApp.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeFormApp.Review
{
    public partial class ReviewAddForm : Form
    {
        private readonly IHttpRequest _httpRequest;

        public ReviewAddForm()
        {
            InitializeComponent();
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();

            this.Load += async (s, e) =>
            {
                await LoadReviewers();
                await LoadPokemons();
            };
        }

        private async Task LoadReviewers()
        {
            try
            {
                var reviewers = await _httpRequest.GetAllAsync<List<ReviewerDto>>("api/Reviewer");

                if (reviewers != null)
                {
                    var reviewerList = new List<object>();
                    foreach (var r in reviewers)
                    {
                        reviewerList.Add(new
                        {
                            Id = r.Id,
                            FullName = r.FirstName + " " + r.LastName
                        });
                    }

                    comboBox1.DataSource = reviewerList;
                    comboBox1.DisplayMember = "FullName";
                    comboBox1.ValueMember = "Id";
                    comboBox1.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("Reviewer verileri alınamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reviewer yüklenirken hata: " + ex.Message);
            }
        }

        private async Task LoadPokemons()
        {
            try
            {
                var pokemons = await _httpRequest.GetAllAsync<List<PokemonDto>>("api/Pokemon");

                if (pokemons != null)
                {
                    comboBox2.DataSource = pokemons;
                    comboBox2.DisplayMember = "Name";
                    comboBox2.ValueMember = "Id";
                    comboBox2.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("Pokemon verileri alınamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pokemon yüklenirken hata: " + ex.Message);
            }
        }

        private async void button1_Click(object sender, EventArgs e) // Ekle
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen bir reviewer seçin.");
                return;
            }
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen bir pokemon seçin.");
                return;
            }

            var createDto = new ReviewCreateDto
            {
                Title = textBox3.Text.Trim(),
                Text = textBox4.Text.Trim(),
                Rating = int.TryParse(textBox5.Text, out int rating) ? rating : 0
            };

            int reviewerId = Convert.ToInt32(comboBox1.SelectedValue);
            int pokemonId = Convert.ToInt32(comboBox2.SelectedValue);

            try
            {
                bool success = await _httpRequest.PostAsync($"api/Review?reviewerId={reviewerId}&pokeId={pokemonId}", createDto);


                if (success)
                {
                    MessageBox.Show("Review başarıyla eklendi.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ekleme başarısız oldu.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e) // Kapat
        {
            this.Close();
        }
    }
}
