namespace AtelieDosPontinhos.Desktop.UserControls
{
    partial class PerfilUserControl
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            card = new Panel();
            btnAvatar = new Guna.UI2.WinForms.Guna2CircleButton();
            lblRolesValor = new Label();
            lblApiValor = new Label();
            lblEmailValor = new Label();
            lblRolesLabel = new Label();
            lblApiLabel = new Label();
            lblEmailLabel = new Label();
            lblBadge = new Label();
            sep = new Panel();
            lblNome = new Label();
            lblTitulo = new Label();
            card.SuspendLayout();
            SuspendLayout();
            // 
            // card
            // 
            card.BackColor = Color.White;
            card.Controls.Add(btnAvatar);
            card.Controls.Add(lblRolesValor);
            card.Controls.Add(lblApiValor);
            card.Controls.Add(lblEmailValor);
            card.Controls.Add(lblRolesLabel);
            card.Controls.Add(lblApiLabel);
            card.Controls.Add(lblEmailLabel);
            card.Controls.Add(lblBadge);
            card.Controls.Add(sep);
            card.Controls.Add(lblNome);
            card.Location = new Point(121, 57);
            card.Name = "card";
            card.Size = new Size(547, 406);
            card.TabIndex = 0;
            // 
            // btnAvatar
            // 
            btnAvatar.DisabledState.BorderColor = Color.DarkGray;
            btnAvatar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnAvatar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnAvatar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnAvatar.FillColor = Color.RoyalBlue;
            btnAvatar.Font = new Font("Yu Gothic UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAvatar.ForeColor = Color.White;
            btnAvatar.Location = new Point(228, 26);
            btnAvatar.Name = "btnAvatar";
            btnAvatar.ShadowDecoration.CustomizableEdges = customizableEdges1;
            btnAvatar.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            btnAvatar.Size = new Size(90, 90);
            btnAvatar.TabIndex = 2;
            btnAvatar.Text = "U";
            // 
            // lblRolesValor
            // 
            lblRolesValor.AutoSize = true;
            lblRolesValor.Location = new Point(22, 357);
            lblRolesValor.Name = "lblRolesValor";
            lblRolesValor.Size = new Size(16, 15);
            lblRolesValor.TabIndex = 1;
            lblRolesValor.Text = "...";
            // 
            // lblApiValor
            // 
            lblApiValor.AutoSize = true;
            lblApiValor.Location = new Point(22, 299);
            lblApiValor.Name = "lblApiValor";
            lblApiValor.Size = new Size(16, 15);
            lblApiValor.TabIndex = 1;
            lblApiValor.Text = "...";
            // 
            // lblEmailValor
            // 
            lblEmailValor.AutoSize = true;
            lblEmailValor.Location = new Point(22, 236);
            lblEmailValor.Name = "lblEmailValor";
            lblEmailValor.Size = new Size(16, 15);
            lblEmailValor.TabIndex = 1;
            lblEmailValor.Text = "...";
            // 
            // lblRolesLabel
            // 
            lblRolesLabel.AutoSize = true;
            lblRolesLabel.Font = new Font("Yu Gothic", 6.75F, FontStyle.Bold);
            lblRolesLabel.ForeColor = SystemColors.ControlDark;
            lblRolesLabel.Location = new Point(22, 336);
            lblRolesLabel.Name = "lblRolesLabel";
            lblRolesLabel.Size = new Size(66, 12);
            lblRolesLabel.TabIndex = 1;
            lblRolesLabel.Text = "PERMISSÕES";
            // 
            // lblApiLabel
            // 
            lblApiLabel.AutoSize = true;
            lblApiLabel.Font = new Font("Yu Gothic", 6.75F, FontStyle.Bold);
            lblApiLabel.ForeColor = SystemColors.ControlDark;
            lblApiLabel.Location = new Point(22, 278);
            lblApiLabel.Name = "lblApiLabel";
            lblApiLabel.Size = new Size(80, 12);
            lblApiLabel.TabIndex = 1;
            lblApiLabel.Text = "API CONECTADA";
            // 
            // lblEmailLabel
            // 
            lblEmailLabel.AutoSize = true;
            lblEmailLabel.Font = new Font("Yu Gothic", 6.75F, FontStyle.Bold);
            lblEmailLabel.ForeColor = SystemColors.ControlDark;
            lblEmailLabel.Location = new Point(22, 216);
            lblEmailLabel.Name = "lblEmailLabel";
            lblEmailLabel.Size = new Size(39, 12);
            lblEmailLabel.TabIndex = 1;
            lblEmailLabel.Text = "E-MAIL";
            // 
            // lblBadge
            // 
            lblBadge.BackColor = SystemColors.HotTrack;
            lblBadge.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBadge.ForeColor = Color.White;
            lblBadge.Location = new Point(202, 157);
            lblBadge.Name = "lblBadge";
            lblBadge.Size = new Size(139, 27);
            lblBadge.TabIndex = 2;
            lblBadge.Text = "Perfil";
            lblBadge.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // sep
            // 
            sep.BackColor = Color.FromArgb(224, 228, 235);
            sep.ForeColor = Color.FromArgb(224, 228, 235);
            sep.Location = new Point(22, 201);
            sep.Name = "sep";
            sep.Size = new Size(500, 1);
            sep.TabIndex = 0;
            // 
            // lblNome
            // 
            lblNome.AutoSize = true;
            lblNome.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNome.Location = new Point(242, 128);
            lblNome.Name = "lblNome";
            lblNome.Size = new Size(62, 20);
            lblNome.TabIndex = 1;
            lblNome.Text = "Usuário";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.Location = new Point(121, 22);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(119, 21);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "⚙️ Meu Perfil";
            // 
            // PerfilUserControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblTitulo);
            Controls.Add(card);
            Name = "PerfilUserControl";
            Size = new Size(805, 501);
            Load += PerfilUserControl_Load;
            card.ResumeLayout(false);
            card.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel card;
        private Label lblBadge;
        private Panel sep;
        private Label lblNome;
        private Label lblTitulo;
        private Label lblRolesValor;
        private Label lblApiValor;
        private Label lblEmailValor;
        private Label lblRolesLabel;
        private Label lblApiLabel;
        private Label lblEmailLabel;
        private Guna.UI2.WinForms.Guna2CircleButton btnAvatar;
    }
}
