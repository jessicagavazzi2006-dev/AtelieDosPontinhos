namespace AtelieDosPontinhos.Desktop.Forms
{
    partial class LoginForm
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pbLogo = new PictureBox();
            lblBemVindo = new Label();
            lblTextoFacaLogin = new Label();
            lblEmail = new Label();
            txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            lblSenha = new Label();
            txtSenha = new Guna.UI2.WinForms.Guna2TextBox();
            btnEntrar = new Guna.UI2.WinForms.Guna2Button();
            lblCarregando = new Label();
            pnSeparador = new Panel();
            pnSeparador2 = new Panel();
            lblProblemas = new Label();
            lblApi = new Label();
            lblErro = new Label();
            lblVersao = new Label();
            btnFechar = new Guna.UI2.WinForms.Guna2CircleButton();
            ((System.ComponentModel.ISupportInitialize)pbLogo).BeginInit();
            SuspendLayout();
            // 
            // pbLogo
            // 
            pbLogo.Image = Properties.Resources.Logo_Ateliê_dos_pontinhos_maior;
            pbLogo.Location = new Point(76, 23);
            pbLogo.Name = "pbLogo";
            pbLogo.Size = new Size(261, 80);
            pbLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pbLogo.TabIndex = 0;
            pbLogo.TabStop = false;
            // 
            // lblBemVindo
            // 
            lblBemVindo.AutoSize = true;
            lblBemVindo.Font = new Font("Yu Gothic", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBemVindo.ForeColor = Color.FromArgb(58, 52, 64);
            lblBemVindo.Location = new Point(134, 110);
            lblBemVindo.Name = "lblBemVindo";
            lblBemVindo.Size = new Size(152, 31);
            lblBemVindo.TabIndex = 1;
            lblBemVindo.Text = "Bem-Vindo!";
            // 
            // lblTextoFacaLogin
            // 
            lblTextoFacaLogin.AutoSize = true;
            lblTextoFacaLogin.Font = new Font("Yu Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTextoFacaLogin.ForeColor = SystemColors.ControlDark;
            lblTextoFacaLogin.Location = new Point(131, 141);
            lblTextoFacaLogin.Name = "lblTextoFacaLogin";
            lblTextoFacaLogin.Size = new Size(158, 17);
            lblTextoFacaLogin.TabIndex = 1;
            lblTextoFacaLogin.Text = "faça login com sua conta";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEmail.ForeColor = Color.FromArgb(58, 52, 64);
            lblEmail.Location = new Point(22, 189);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(36, 15);
            lblEmail.TabIndex = 1;
            lblEmail.Text = "Email";
            // 
            // txtEmail
            // 
            txtEmail.BorderRadius = 5;
            txtEmail.CustomizableEdges = customizableEdges1;
            txtEmail.DefaultText = "";
            txtEmail.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtEmail.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtEmail.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtEmail.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtEmail.FillColor = Color.WhiteSmoke;
            txtEmail.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.Font = new Font("Segoe UI", 9F);
            txtEmail.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.Location = new Point(22, 207);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "seuemail@hotmail.com.br";
            txtEmail.SelectedText = "";
            txtEmail.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtEmail.Size = new Size(378, 36);
            txtEmail.TabIndex = 2;
            // 
            // lblSenha
            // 
            lblSenha.AutoSize = true;
            lblSenha.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSenha.ForeColor = Color.FromArgb(58, 52, 64);
            lblSenha.Location = new Point(22, 260);
            lblSenha.Name = "lblSenha";
            lblSenha.Size = new Size(41, 15);
            lblSenha.TabIndex = 1;
            lblSenha.Text = "Senha";
            // 
            // txtSenha
            // 
            txtSenha.BorderRadius = 5;
            txtSenha.CustomizableEdges = customizableEdges3;
            txtSenha.DefaultText = "";
            txtSenha.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSenha.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSenha.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSenha.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSenha.FillColor = Color.WhiteSmoke;
            txtSenha.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSenha.Font = new Font("Segoe UI", 9F);
            txtSenha.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSenha.Location = new Point(22, 278);
            txtSenha.Name = "txtSenha";
            txtSenha.PlaceholderText = "•••••••••••";
            txtSenha.SelectedText = "";
            txtSenha.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtSenha.Size = new Size(378, 36);
            txtSenha.TabIndex = 2;
            // 
            // btnEntrar
            // 
            btnEntrar.BorderRadius = 10;
            btnEntrar.CustomizableEdges = customizableEdges5;
            btnEntrar.DisabledState.BorderColor = Color.DarkGray;
            btnEntrar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnEntrar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnEntrar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnEntrar.FillColor = Color.FromArgb(177, 145, 217);
            btnEntrar.Font = new Font("Yu Gothic", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEntrar.ForeColor = Color.White;
            btnEntrar.Location = new Point(76, 333);
            btnEntrar.Name = "btnEntrar";
            btnEntrar.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnEntrar.Size = new Size(261, 45);
            btnEntrar.TabIndex = 3;
            btnEntrar.Text = "Entrar";
            // 
            // lblCarregando
            // 
            lblCarregando.AutoSize = true;
            lblCarregando.Font = new Font("Yu Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCarregando.ForeColor = SystemColors.ControlDark;
            lblCarregando.Location = new Point(167, 383);
            lblCarregando.Name = "lblCarregando";
            lblCarregando.Size = new Size(90, 16);
            lblCarregando.TabIndex = 1;
            lblCarregando.Text = "Autenticando...";
            // 
            // pnSeparador
            // 
            pnSeparador.BackColor = SystemColors.ActiveCaptionText;
            pnSeparador.Location = new Point(30, 171);
            pnSeparador.Name = "pnSeparador";
            pnSeparador.Size = new Size(370, 1);
            pnSeparador.TabIndex = 4;
            // 
            // pnSeparador2
            // 
            pnSeparador2.BackColor = SystemColors.ActiveCaptionText;
            pnSeparador2.Location = new Point(30, 405);
            pnSeparador2.Name = "pnSeparador2";
            pnSeparador2.Size = new Size(370, 1);
            pnSeparador2.TabIndex = 4;
            // 
            // lblProblemas
            // 
            lblProblemas.AutoSize = true;
            lblProblemas.Font = new Font("Yu Gothic", 9F);
            lblProblemas.ForeColor = SystemColors.ControlDark;
            lblProblemas.Location = new Point(22, 418);
            lblProblemas.Name = "lblProblemas";
            lblProblemas.Size = new Size(348, 16);
            lblProblemas.TabIndex = 1;
            lblProblemas.Text = "Problemas para acessar? Contate o administrador do sistema.";
            // 
            // lblApi
            // 
            lblApi.AutoSize = true;
            lblApi.Font = new Font("Yu Gothic", 9F);
            lblApi.ForeColor = SystemColors.ControlDark;
            lblApi.Location = new Point(22, 448);
            lblApi.Name = "lblApi";
            lblApi.Size = new Size(36, 16);
            lblApi.TabIndex = 1;
            lblApi.Text = "API...";
            // 
            // lblErro
            // 
            lblErro.AutoSize = true;
            lblErro.Font = new Font("Yu Gothic", 9F);
            lblErro.ForeColor = Color.Firebrick;
            lblErro.Location = new Point(22, 475);
            lblErro.Name = "lblErro";
            lblErro.Size = new Size(38, 16);
            lblErro.TabIndex = 1;
            lblErro.Text = "Erro...";
            // 
            // lblVersao
            // 
            lblVersao.AutoSize = true;
            lblVersao.Font = new Font("Yu Gothic", 9F);
            lblVersao.ForeColor = SystemColors.ControlDark;
            lblVersao.Location = new Point(87, 510);
            lblVersao.Name = "lblVersao";
            lblVersao.Size = new Size(252, 16);
            lblVersao.TabIndex = 1;
            lblVersao.Text = "Versão: 1.0.0 | ©️ Senac São Miguel Paulista";
            // 
            // btnFechar
            // 
            btnFechar.DisabledState.BorderColor = Color.DarkGray;
            btnFechar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnFechar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnFechar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnFechar.FillColor = Color.Brown;
            btnFechar.Font = new Font("Segoe UI", 9F);
            btnFechar.ForeColor = Color.White;
            btnFechar.Location = new Point(382, 12);
            btnFechar.Name = "btnFechar";
            btnFechar.ShadowDecoration.CustomizableEdges = customizableEdges7;
            btnFechar.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            btnFechar.Size = new Size(30, 30);
            btnFechar.TabIndex = 5;
            btnFechar.Text = "❌";
            btnFechar.Click += btnFechar_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(424, 540);
            Controls.Add(btnFechar);
            Controls.Add(pnSeparador2);
            Controls.Add(pnSeparador);
            Controls.Add(btnEntrar);
            Controls.Add(txtSenha);
            Controls.Add(txtEmail);
            Controls.Add(lblSenha);
            Controls.Add(lblEmail);
            Controls.Add(lblCarregando);
            Controls.Add(lblTextoFacaLogin);
            Controls.Add(lblVersao);
            Controls.Add(lblErro);
            Controls.Add(lblApi);
            Controls.Add(lblProblemas);
            Controls.Add(lblBemVindo);
            Controls.Add(pbLogo);
            FormBorderStyle = FormBorderStyle.None;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LoginForm";
            Load += LoginForm_Load;
            ((System.ComponentModel.ISupportInitialize)pbLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pbLogo;
        private Label lblBemVindo;
        private Label lblTextoFacaLogin;
        private Label lblEmail;
        private Guna.UI2.WinForms.Guna2TextBox txtEmail;
        private Label lblSenha;
        private Guna.UI2.WinForms.Guna2TextBox txtSenha;
        private Guna.UI2.WinForms.Guna2Button btnEntrar;
        private Label lblCarregando;
        private Panel pnSeparador;
        private Panel pnSeparador2;
        private Label lblProblemas;
        private Label lblApi;
        private Label lblErro;
        private Label lblVersao;
        private Guna.UI2.WinForms.Guna2CircleButton btnFechar;
    }
}