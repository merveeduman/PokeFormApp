using System.Xml.Linq;

namespace PokeFormApp
{
    partial class PokemonForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support — do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            dataGridView1 = new DataGridView();
            btnEkleme = new Button();
            btnSilme = new Button();
            btnGuncelleme = new Button();
            btnDetay = new Button();
            btnYenile = new Button();
            btnGeriDon = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(180, 15);
            label1.Name = "label1";
            label1.Size = new Size(76, 15);
            label1.TabIndex = 0;
            label1.Text = "PokemonList";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeight = 29;
            dataGridView1.Location = new Point(12, 40);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(350, 280);
            dataGridView1.TabIndex = 1;
            // 
            // btnEkleme
            // 
            btnEkleme.Location = new Point(530, 40);
            btnEkleme.Name = "btnEkleme";
            btnEkleme.Size = new Size(100, 30);
            btnEkleme.TabIndex = 2;
            btnEkleme.Text = "ekleme";
            btnEkleme.UseVisualStyleBackColor = true;
            btnEkleme.Click += btnEkleme_Click;
            // 
            // btnSilme
            // 
            btnSilme.Location = new Point(530, 101);
            btnSilme.Name = "btnSilme";
            btnSilme.Size = new Size(100, 30);
            btnSilme.TabIndex = 3;
            btnSilme.Text = "silme";
            btnSilme.UseVisualStyleBackColor = true;
            btnSilme.Click += btnSilme_Click;
            // 
            // btnGuncelleme
            // 
            btnGuncelleme.Location = new Point(530, 160);
            btnGuncelleme.Name = "btnGuncelleme";
            btnGuncelleme.Size = new Size(100, 30);
            btnGuncelleme.TabIndex = 4;
            btnGuncelleme.Text = "güncelleme";
            btnGuncelleme.UseVisualStyleBackColor = true;
            btnGuncelleme.Click += btnGuncelleme_Click;
            // 
            // btnDetay
            // 
            btnDetay.Location = new Point(530, 215);
            btnDetay.Name = "btnDetay";
            btnDetay.Size = new Size(100, 30);
            btnDetay.TabIndex = 5;
            btnDetay.Text = "detay";
            btnDetay.UseVisualStyleBackColor = true;
            btnDetay.Click += btnDetay_Click;
            // 
            // btnYenile
            // 
            btnYenile.Location = new Point(530, 330);
            btnYenile.Name = "btnYenile";
            btnYenile.Size = new Size(100, 30);
            btnYenile.TabIndex = 6;
            btnYenile.Text = "yenile";
            btnYenile.UseVisualStyleBackColor = true;
            btnYenile.Click += btnYenile_Click;
            // 
            // btnGeriDon
            // 
            btnGeriDon.Location = new Point(12, 330);
            btnGeriDon.Name = "btnGeriDon";
            btnGeriDon.Size = new Size(75, 30);
            btnGeriDon.TabIndex = 7;
            btnGeriDon.Text = "Geri Dön";
            btnGeriDon.UseVisualStyleBackColor = true;
            btnGeriDon.Click += btnGeriDon_Click;
            // 
            // PokemonForm
            // 
            ClientSize = new Size(676, 417);
            Controls.Add(btnGeriDon);
            Controls.Add(btnYenile);
            Controls.Add(btnDetay);
            Controls.Add(btnGuncelleme);
            Controls.Add(btnSilme);
            Controls.Add(btnEkleme);
            Controls.Add(dataGridView1);
            Controls.Add(label1);
            Name = "PokemonForm";
            Text = "PokemonForm";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();




        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnEkleme;
        private System.Windows.Forms.Button btnSilme;
        private System.Windows.Forms.Button btnGuncelleme;
        private System.Windows.Forms.Button btnDetay;
        private System.Windows.Forms.Button btnYenile;
        private System.Windows.Forms.Button btnGeriDon;
    }
}
