namespace PokeFormApp
{
    partial class FoodTypeAddForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            labelName = new Label();
            textBoxName = new TextBox();
            buttonAdd = new Button();
            buttonCancel = new Button();
            SuspendLayout();
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(95, 134);
            labelName.Name = "labelName";
            labelName.Size = new Size(42, 15);
            labelName.TabIndex = 0;
            labelName.Text = "Name:";
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(198, 126);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(250, 23);
            textBoxName.TabIndex = 1;
            // 
            // buttonAdd
            // 
            buttonAdd.ForeColor = Color.ForestGreen;
            buttonAdd.Location = new Point(348, 246);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(100, 30);
            buttonAdd.TabIndex = 2;
            buttonAdd.Text = "Ekle";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += button1_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.ForeColor = Color.DarkRed;
            buttonCancel.Location = new Point(66, 246);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(100, 30);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "Kapat";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += button2_Click;
            // 
            // FoodTypeAddForm
            // 
            ClientSize = new Size(563, 337);
            Controls.Add(labelName);
            Controls.Add(textBoxName);
            Controls.Add(buttonAdd);
            Controls.Add(buttonCancel);
            Name = "FoodTypeAddForm";
            Text = "Yeni FoodType Ekle";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
