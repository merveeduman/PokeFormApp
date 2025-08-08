using PokeFormApp;
using PokeFormApp.Owner;
using PokeFormApp.Review;
using PokeFormApp.Reviewer;

namespace PokeFormApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PokemonForm form = new PokemonForm(); // Yeni form nesnesi oluþtur
            form.Show(); 

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var form = new CountryForm();
            form.Show(); // veya form.ShowDialog(); ama Show yeterli
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var form = new CategoryForm();
            form.Show(); // veya form.ShowDialog(); ama Show yeterli
        }
        

        private void button5_Click(object sender, EventArgs e)
        {
            var form = new ReviewForm();
            form.Show(); // veya form.ShowDialog(); ama Show yeterli
        }
        private void button4_Click(object sender, EventArgs e)
        {
            var form = new OwnerForm
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            form.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var form = new ReviewerForm();
            form.ShowDialog();
        }
    }
}
