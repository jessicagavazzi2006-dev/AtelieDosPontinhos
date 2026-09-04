namespace AtelieDosPontinhos.Desktop.UserControls
{
    partial class UsuariosUserControl
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pnlToolbar = new Panel();
            btnAtualizar = new Guna.UI2.WinForms.Guna2Button();
            btnExcluir = new Guna.UI2.WinForms.Guna2Button();
            btnEditar = new Guna.UI2.WinForms.Guna2Button();
            btnNovo = new Guna.UI2.WinForms.Guna2Button();
            txtPesquisa = new Guna.UI2.WinForms.Guna2TextBox();
            lblTitulo = new Label();
            gridUsuarios = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colEmail = new DataGridViewTextBoxColumn();
            colPerfil = new DataGridViewTextBoxColumn();
            pnlToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridUsuarios).BeginInit();
            SuspendLayout();
            // 
            // pnlToolbar
            // 
            pnlToolbar.Controls.Add(btnAtualizar);
            pnlToolbar.Controls.Add(btnExcluir);
            pnlToolbar.Controls.Add(btnEditar);
            pnlToolbar.Controls.Add(btnNovo);
            pnlToolbar.Controls.Add(txtPesquisa);
            pnlToolbar.Location = new Point(46, 57);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(713, 66);
            pnlToolbar.TabIndex = 4;
            // 
            // btnAtualizar
            // 
            btnAtualizar.BorderRadius = 5;
            btnAtualizar.CustomizableEdges = customizableEdges1;
            btnAtualizar.DisabledState.BorderColor = Color.DarkGray;
            btnAtualizar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnAtualizar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnAtualizar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnAtualizar.FillColor = Color.Goldenrod;
            btnAtualizar.Font = new Font("Segoe UI", 9F);
            btnAtualizar.ForeColor = Color.White;
            btnAtualizar.Location = new Point(604, 13);
            btnAtualizar.Name = "btnAtualizar";
            btnAtualizar.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnAtualizar.Size = new Size(89, 39);
            btnAtualizar.TabIndex = 1;
            btnAtualizar.Text = "🔃 Atualizar";
            btnAtualizar.Click += btnAtualizar_Click;
            // 
            // btnExcluir
            // 
            btnExcluir.BorderRadius = 5;
            btnExcluir.CustomizableEdges = customizableEdges3;
            btnExcluir.DisabledState.BorderColor = Color.DarkGray;
            btnExcluir.DisabledState.CustomBorderColor = Color.DarkGray;
            btnExcluir.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnExcluir.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnExcluir.FillColor = Color.DarkRed;
            btnExcluir.Font = new Font("Segoe UI", 9F);
            btnExcluir.ForeColor = Color.White;
            btnExcluir.Location = new Point(512, 13);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnExcluir.Size = new Size(86, 39);
            btnExcluir.TabIndex = 1;
            btnExcluir.Text = "🗑️ Excluir";
            btnExcluir.Click += btnExcluir_Click;
            // 
            // btnEditar
            // 
            btnEditar.BorderRadius = 5;
            btnEditar.CustomizableEdges = customizableEdges5;
            btnEditar.DisabledState.BorderColor = Color.DarkGray;
            btnEditar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnEditar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnEditar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnEditar.FillColor = Color.RoyalBlue;
            btnEditar.Font = new Font("Segoe UI", 9F);
            btnEditar.ForeColor = Color.White;
            btnEditar.Location = new Point(420, 13);
            btnEditar.Name = "btnEditar";
            btnEditar.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnEditar.Size = new Size(86, 39);
            btnEditar.TabIndex = 1;
            btnEditar.Text = "✏️ Editar";
            btnEditar.Click += btnEditar_Click;
            // 
            // btnNovo
            // 
            btnNovo.BorderRadius = 5;
            btnNovo.CustomizableEdges = customizableEdges7;
            btnNovo.DisabledState.BorderColor = Color.DarkGray;
            btnNovo.DisabledState.CustomBorderColor = Color.DarkGray;
            btnNovo.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnNovo.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnNovo.FillColor = Color.Green;
            btnNovo.Font = new Font("Segoe UI", 9F);
            btnNovo.ForeColor = Color.White;
            btnNovo.Location = new Point(328, 13);
            btnNovo.Name = "btnNovo";
            btnNovo.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnNovo.Size = new Size(86, 39);
            btnNovo.TabIndex = 1;
            btnNovo.Text = "➕ Novo Usuário";
            btnNovo.Click += btnNovo_Click;
            // 
            // txtPesquisa
            // 
            txtPesquisa.BorderRadius = 5;
            txtPesquisa.CustomizableEdges = customizableEdges9;
            txtPesquisa.DefaultText = "";
            txtPesquisa.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtPesquisa.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtPesquisa.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtPesquisa.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtPesquisa.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPesquisa.Font = new Font("Segoe UI", 9F);
            txtPesquisa.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPesquisa.Location = new Point(12, 14);
            txtPesquisa.Name = "txtPesquisa";
            txtPesquisa.PlaceholderText = "🔎 Pesquisar pelo nome ou email...";
            txtPesquisa.SelectedText = "";
            txtPesquisa.ShadowDecoration.CustomizableEdges = customizableEdges10;
            txtPesquisa.Size = new Size(310, 36);
            txtPesquisa.TabIndex = 0;
            txtPesquisa.TextChanged += txtPesquisa_TextChanged;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Yu Gothic", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(58, 52, 64);
            lblTitulo.Location = new Point(46, 24);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(232, 19);
            lblTitulo.TabIndex = 3;
            lblTitulo.Text = "👤 Gerenciamento de Usuários";
            // 
            // gridUsuarios
            // 
            gridUsuarios.AllowUserToDeleteRows = false;
            gridUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            gridUsuarios.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colEmail, colPerfil });
            gridUsuarios.Location = new Point(46, 141);
            gridUsuarios.Name = "gridUsuarios";
            gridUsuarios.RowHeadersVisible = false;
            gridUsuarios.Size = new Size(713, 336);
            gridUsuarios.TabIndex = 5;
            // 
            // colId
            // 
            colId.HeaderText = "ID";
            colId.Name = "colId";
            // 
            // colName
            // 
            colName.HeaderText = "Nome do Usuário";
            colName.Name = "colName";
            // 
            // colEmail
            // 
            colEmail.HeaderText = "Email";
            colEmail.Name = "colEmail";
            // 
            // colPerfil
            // 
            colPerfil.HeaderText = "Perfil";
            colPerfil.Name = "colPerfil";
            // 
            // UsuariosUserControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlToolbar);
            Controls.Add(lblTitulo);
            Controls.Add(gridUsuarios);
            Name = "UsuariosUserControl";
            Size = new Size(805, 501);
            Load += UsuariosUserControl_Load;
            pnlToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridUsuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlToolbar;
        private Guna.UI2.WinForms.Guna2Button btnAtualizar;
        private Guna.UI2.WinForms.Guna2Button btnExcluir;
        private Guna.UI2.WinForms.Guna2Button btnEditar;
        private Guna.UI2.WinForms.Guna2Button btnNovo;
        private Guna.UI2.WinForms.Guna2TextBox txtPesquisa;
        private Label lblTitulo;
        private DataGridView gridUsuarios;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colEmail;
        private DataGridViewTextBoxColumn colPerfil;
    }
}
