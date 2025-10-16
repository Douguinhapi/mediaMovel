namespace Estudo_Instramed43
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
            formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            TxtJanela = new TextBox();
            TxtPassos = new TextBox();
            BtEnviar = new Button();
            Lb1 = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            BtIniciar1 = new Button();
            BtParar1 = new Button();
            LbTimer = new Label();
            BtReiniciar = new Button();
            SuspendLayout();
            // 
            // formsPlot1
            // 
            formsPlot1.DisplayScale = 1.25F;
            formsPlot1.Location = new Point(36, 71);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(776, 221);
            formsPlot1.TabIndex = 0;
            formsPlot1.Load += formsPlot1_Load;
            // 
            // TxtJanela
            // 
            TxtJanela.Location = new Point(87, 38);
            TxtJanela.Name = "TxtJanela";
            TxtJanela.PlaceholderText = "Digite a Janela";
            TxtJanela.Size = new Size(142, 27);
            TxtJanela.TabIndex = 1;
            // 
            // TxtPassos
            // 
            TxtPassos.Location = new Point(244, 38);
            TxtPassos.Name = "TxtPassos";
            TxtPassos.PlaceholderText = "Digite os Passos";
            TxtPassos.Size = new Size(142, 27);
            TxtPassos.TabIndex = 2;
            // 
            // BtEnviar
            // 
            BtEnviar.Location = new Point(403, 38);
            BtEnviar.Name = "BtEnviar";
            BtEnviar.Size = new Size(108, 27);
            BtEnviar.TabIndex = 4;
            BtEnviar.Text = "Enviar";
            BtEnviar.UseVisualStyleBackColor = true;
            BtEnviar.Click += BtEnviar_Click;
            // 
            // Lb1
            // 
            Lb1.AutoSize = true;
            Lb1.Location = new Point(12, 9);
            Lb1.Name = "Lb1";
            Lb1.Size = new Size(50, 20);
            Lb1.TabIndex = 13;
            Lb1.Text = "label1";
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // BtIniciar1
            // 
            BtIniciar1.Location = new Point(608, 31);
            BtIniciar1.Name = "BtIniciar1";
            BtIniciar1.Size = new Size(83, 28);
            BtIniciar1.TabIndex = 14;
            BtIniciar1.Text = "Iniciar";
            BtIniciar1.UseVisualStyleBackColor = true;
            BtIniciar1.Click += BtIniciar1_Click;
            // 
            // BtParar1
            // 
            BtParar1.Location = new Point(697, 31);
            BtParar1.Name = "BtParar1";
            BtParar1.Size = new Size(83, 28);
            BtParar1.TabIndex = 15;
            BtParar1.Text = "parar";
            BtParar1.UseVisualStyleBackColor = true;
            BtParar1.Click += BtParar1_Click;
            // 
            // LbTimer
            // 
            LbTimer.AutoSize = true;
            LbTimer.Location = new Point(12, 295);
            LbTimer.Name = "LbTimer";
            LbTimer.Size = new Size(50, 20);
            LbTimer.TabIndex = 16;
            LbTimer.Text = "label1";
            // 
            // BtReiniciar
            // 
            BtReiniciar.Location = new Point(786, 31);
            BtReiniciar.Name = "BtReiniciar";
            BtReiniciar.Size = new Size(83, 29);
            BtReiniciar.TabIndex = 17;
            BtReiniciar.Text = "reiniciar";
            BtReiniciar.UseVisualStyleBackColor = true;
            BtReiniciar.Click += BtReiniciar_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.MenuBar;
            ClientSize = new Size(881, 327);
            Controls.Add(BtReiniciar);
            Controls.Add(LbTimer);
            Controls.Add(BtParar1);
            Controls.Add(BtIniciar1);
            Controls.Add(Lb1);
            Controls.Add(BtEnviar);
            Controls.Add(TxtPassos);
            Controls.Add(TxtJanela);
            Controls.Add(formsPlot1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private TextBox TxtJanela;
        private TextBox TxtPassos;
        private Button BtEnviar;
        private Label Lb1;
        private System.Windows.Forms.Timer timer1;
        private Button BtIniciar1;
        private Button BtParar1;
        private Label LbTimer;
        private Button BtReiniciar;
    }
}
