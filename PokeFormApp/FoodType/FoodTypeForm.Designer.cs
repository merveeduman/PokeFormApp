namespace PokeFormApp
{
    partial class FoodTypeForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Button buttonClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            buttonAdd = new Button();
            buttonDelete = new Button();
            buttonUpdate = new Button();
            buttonRefresh = new Button();
            buttonClose = new Button();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.Location = new Point(24, 52);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(400, 253);
            dataGridView1.TabIndex = 0;
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(480, 52);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(100, 30);
            buttonAdd.TabIndex = 1;
            buttonAdd.Text = "Ekle";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += button1_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.Location = new Point(480, 115);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(100, 30);
            buttonDelete.TabIndex = 2;
            buttonDelete.Text = "Sil";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += button2_Click;
            // 
            // buttonUpdate
            // 
            buttonUpdate.Location = new Point(480, 176);
            buttonUpdate.Name = "buttonUpdate";
            buttonUpdate.Size = new Size(100, 30);
            buttonUpdate.TabIndex = 3;
            buttonUpdate.Text = "Güncelle";
            buttonUpdate.UseVisualStyleBackColor = true;
            buttonUpdate.Click += button3_Click;
            // 
            // buttonRefresh
            // 
            buttonRefresh.Location = new Point(488, 239);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new Size(100, 30);
            buttonRefresh.TabIndex = 4;
            buttonRefresh.Text = "Yenile";
            buttonRefresh.UseVisualStyleBackColor = true;
            buttonRefresh.Click += button4_Click;
            // 
            // buttonClose
            // 
            buttonClose.Location = new Point(38, 331);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(100, 30);
            buttonClose.TabIndex = 5;
            buttonClose.Text = "Geri Dön";
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.Click += button5_Click;
            // 
            // button1
            // 
            button1.Location = new Point(488, 316);
            button1.Name = "button1";
            button1.Size = new Size(100, 30);
            button1.TabIndex = 6;
            button1.Text = "Detay";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button6_Click;
            // 
            // FoodTypeForm
            // 
            ClientSize = new Size(600, 395);
            Controls.Add(button1);
            Controls.Add(dataGridView1);
            Controls.Add(buttonAdd);
            Controls.Add(buttonDelete);
            Controls.Add(buttonUpdate);
            Controls.Add(buttonRefresh);
            Controls.Add(buttonClose);
            Name = "FoodTypeForm";
            Text = "Food Types";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }
        private Button button1;
    }
}
