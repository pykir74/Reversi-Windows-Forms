namespace reversi
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
            components = new System.ComponentModel.Container();
            PvP = new Button();
            PvCPU = new Button();
            title = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // PvP
            // 
            PvP.Font = new Font("TempleOS", 36F);
            PvP.Location = new Point(760, 500);
            PvP.Name = "PvP";
            PvP.Size = new Size(400, 150);
            PvP.TabIndex = 0;
            PvP.Text = "PvP";
            PvP.UseVisualStyleBackColor = true;
            PvP.Click += PvP_Click;
            // 
            // PvCPU
            // 
            PvCPU.Font = new Font("TempleOS", 36F);
            PvCPU.Location = new Point(760, 700);
            PvCPU.Name = "PvCPU";
            PvCPU.Size = new Size(400, 150);
            PvCPU.TabIndex = 1;
            PvCPU.Text = "PvCPU";
            PvCPU.UseVisualStyleBackColor = true;
            PvCPU.Click += PvCPU_Click;
            // 
            // title
            // 
            title.Font = new Font("TempleOS", 72F);
            title.Location = new Point(600, 200);
            title.Name = "title";
            title.Size = new Size(720, 96);
            title.TabIndex = 2;
            title.Text = "REVERSI";
            title.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            ClientSize = new Size(1904, 1041);
            Controls.Add(title);
            Controls.Add(PvCPU);
            Controls.Add(PvP);
            Name = "Form1";
            Text = "Reversi";
            WindowState = FormWindowState.Maximized;
            ResumeLayout(false);
        }

        #endregion

        private Button PvP;
        private Button PvCPU;
        private Label title;
        private System.Windows.Forms.Timer timer1;
    }
}
