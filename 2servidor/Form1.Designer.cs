namespace _2servidor
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
            btnStart = new Button();
            listViewClientes = new ListView();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.BackColor = Color.FromArgb(166, 177, 225);
            btnStart.ForeColor = SystemColors.ActiveCaptionText;
            btnStart.Location = new Point(341, 57);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(168, 33);
            btnStart.TabIndex = 0;
            btnStart.Text = "START";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // listViewClientes
            // 
            listViewClientes.BackColor = Color.FromArgb(244, 238, 255);
            listViewClientes.Location = new Point(18, 140);
            listViewClientes.Name = "listViewClientes";
            listViewClientes.Size = new Size(278, 214);
            listViewClientes.TabIndex = 1;
            listViewClientes.UseCompatibleStateImageBehavior = false;
            listViewClientes.View = View.List;
            listViewClientes.SelectedIndexChanged += listViewClientes_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(66, 72, 116);
            ClientSize = new Size(800, 450);
            Controls.Add(listViewClientes);
            Controls.Add(btnStart);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnStart;
        private ListView listViewClientes;
    }
}
