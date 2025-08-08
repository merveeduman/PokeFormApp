using System;
using System.Windows.Forms;

namespace PokeFormApp
{
    public partial class PokemonUpdateForm : Form
    {
        public string PokemonName { get; private set; }
        public DateTime PokemonBirthDate { get; private set; }

        // Parametresiz constructor — form boş açılır
        public PokemonUpdateForm()
        {
            InitializeComponent();
        }

        // Parametreli constructor — güncelleme için mevcut verilerle açılır
        public PokemonUpdateForm(string currentName, DateTime currentBirthDate) : this()
        {
            textBox1.Text = currentName;
            textBox2.Text = currentBirthDate.ToString("yyyy-MM-dd");
        }

        // Kapat butonu - button1
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Kaydet (Ekle/Güncelle) butonu - button2
        private void button2_Click(object sender, EventArgs e)
        {
            PokemonName = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(PokemonName))
            {
                MessageBox.Show("İsim boş olamaz!");
                return;
            }

            if (!DateTime.TryParse(textBox2.Text.Trim(), out DateTime parsedDate))
            {
                MessageBox.Show("Geçersiz tarih formatı. Lütfen yyyy-MM-dd şeklinde giriniz.");
                return;
            }

            PokemonBirthDate = parsedDate;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void PokemonUpdateForm_Load(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {

        }
    }
}
