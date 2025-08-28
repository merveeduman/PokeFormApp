using Autofac;
using PokeFormApp.Owner;
using PokeFormApp.Review;
using PokeFormApp.Reviewer;
using System;
using System.Windows.Forms;
using static System.Formats.Asn1.AsnWriter;

namespace PokeFormApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("Token: " + SessionManager.Token);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PokemonForm pokemonForm = new PokemonForm();
            pokemonForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CountryForm countryForm = new CountryForm();
            countryForm.Show();
        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CategoryForm categoryForm = new CategoryForm();
            categoryForm.Show();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OwnerForm ownerForm = new OwnerForm();
            ownerForm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ReviewForm reviewForm = new ReviewForm();
            reviewForm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ReviewerForm reviewerForm = new ReviewerForm();
            reviewerForm.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FoodForm foodForm = new FoodForm();
            foodForm.Show(); 
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FoodTypeForm foodTypeForm = new FoodTypeForm();
            foodTypeForm.Show();
        }
    }
}
