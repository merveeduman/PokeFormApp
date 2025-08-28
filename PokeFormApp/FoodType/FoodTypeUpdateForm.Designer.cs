namespace PokeFormApp
{
    partial class FoodTypeUpdateForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Button buttonUpdate;
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
            buttonUpdate = new Button();
            buttonCancel = new Button();
            SuspendLayout();
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(50, 101);
            labelName.Name = "labelName";
            labelName.Size = new Size(42, 15);
            labelName.TabIndex = 0;
            labelName.Text = "Name:";
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(140, 93);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(250, 23);
            textBoxName.TabIndex = 1;
            // 
            // buttonUpdate
            // 
            buttonUpdate.Location = new Point(326, 196);
            buttonUpdate.Name = "buttonUpdate";
            buttonUpdate.Size = new Size(100, 30);
            buttonUpdate.TabIndex = 2;
            buttonUpdate.Text = "Güncelle";
            buttonUpdate.UseVisualStyleBackColor = true;
            buttonUpdate.Click += button1_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(50, 196);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(100, 30);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "İptal";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += button2_Click;
            // 
            // FoodTypeUpdateForm
            // 
            ClientSize = new Size(543, 284);
            Controls.Add(labelName);
            Controls.Add(textBoxName);
            Controls.Add(buttonUpdate);
            Controls.Add(buttonCancel);
            Name = "FoodTypeUpdateForm";
            Text = "FoodType Güncelle";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
