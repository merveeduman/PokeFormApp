namespace PokeFormApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = SystemColors.ActiveCaption;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Location = new Point(47, 54);
            button1.Name = "button1";
            button1.Size = new Size(280, 148);
            button1.TabIndex = 0;
            button1.Text = "Pokemon";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = SystemColors.Highlight;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Location = new Point(373, 54);
            button2.Name = "button2";
            button2.Size = new Size(280, 151);
            button2.TabIndex = 1;
            button2.Text = "Country";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.Salmon;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Location = new Point(723, 54);
            button3.Name = "button3";
            button3.Size = new Size(280, 153);
            button3.TabIndex = 2;
            button3.Text = "Category";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.BackColor = SystemColors.GrayText;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Location = new Point(35, 271);
            button4.Name = "button4";
            button4.Size = new Size(280, 153);
            button4.TabIndex = 3;
            button4.Text = "Owner";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.BackColor = Color.YellowGreen;
            button5.Location = new Point(386, 271);
            button5.Name = "button5";
            button5.Size = new Size(296, 153);
            button5.TabIndex = 4;
            button5.Text = "Review";
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.BackColor = SystemColors.GradientInactiveCaption;
            button6.Location = new Point(723, 271);
            button6.Name = "button6";
            button6.Size = new Size(280, 153);
            button6.TabIndex = 5;
            button6.Text = "Reviewer";
            button6.UseVisualStyleBackColor = false;
            button6.Click += button6_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1420, 677);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Form1";
            WindowState = FormWindowState.Maximized;
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;

    }
}
