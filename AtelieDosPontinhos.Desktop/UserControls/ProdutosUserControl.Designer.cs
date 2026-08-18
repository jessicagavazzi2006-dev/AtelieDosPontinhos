namespace AtelieDosPontinhos.Desktop.UserControls
{
    partial class ProdutosUserControl
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges61 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges62 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges63 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges64 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges65 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges66 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges67 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges68 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges69 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges70 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges71 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges72 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            lblTitulo = new Label();
            pnlToolbar = new Panel();
            txtPesquisa = new Guna.UI2.WinForms.Guna2TextBox();
            gridProdutos = new DataGridView();
            btnPesquisar = new Guna.UI2.WinForms.Guna2Button();
            btnNovo = new Guna.UI2.WinForms.Guna2Button();
            btnEditar = new Guna.UI2.WinForms.Guna2Button();
            btnExcluir = new Guna.UI2.WinForms.Guna2Button();
            btnAtualizar = new Guna.UI2.WinForms.Guna2Button();
            colIsFeatured = new DataGridViewCheckBoxColumn();
            colCategory = new DataGridViewTextBoxColumn();
            colStock = new DataGridViewTextBoxColumn();
            colPrice = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colId = new DataGridViewTextBoxColumn();
            pnlToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridProdutos).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(45, 26);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(170, 15);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "🛍️ Gerenciamento de Produtos";
            // 
            // pnlToolbar
            // 
            pnlToolbar.Controls.Add(btnAtualizar);
            pnlToolbar.Controls.Add(btnExcluir);
            pnlToolbar.Controls.Add(btnEditar);
            pnlToolbar.Controls.Add(btnNovo);
            pnlToolbar.Controls.Add(btnPesquisar);
            pnlToolbar.Controls.Add(txtPesquisa);
            pnlToolbar.Location = new Point(45, 59);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(713, 66);
            pnlToolbar.TabIndex = 1;
            // 
            // txtPesquisa
            // 
            txtPesquisa.CustomizableEdges = customizableEdges61;
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
            txtPesquisa.PlaceholderText = "🔎 Pesquisar por nome...";
            txtPesquisa.SelectedText = "";
            txtPesquisa.ShadowDecoration.CustomizableEdges = customizableEdges62;
            txtPesquisa.Size = new Size(205, 36);
            txtPesquisa.TabIndex = 0;
            // 
            // gridProdutos
            // 
            gridProdutos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            gridProdutos.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colPrice, colStock, colCategory, colIsFeatured });
            gridProdutos.Location = new Point(45, 143);
            gridProdutos.Name = "gridProdutos";
            gridProdutos.RowHeadersVisible = false;
            gridProdutos.Size = new Size(713, 336);
            gridProdutos.TabIndex = 2;
            // 
            // btnPesquisar
            // 
            btnPesquisar.CustomizableEdges = customizableEdges63;
            btnPesquisar.DisabledState.BorderColor = Color.DarkGray;
            btnPesquisar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnPesquisar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnPesquisar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnPesquisar.Font = new Font("Segoe UI", 9F);
            btnPesquisar.ForeColor = Color.White;
            btnPesquisar.Location = new Point(223, 13);
            btnPesquisar.Name = "btnPesquisar";
            btnPesquisar.ShadowDecoration.CustomizableEdges = customizableEdges64;
            btnPesquisar.Size = new Size(86, 39);
            btnPesquisar.TabIndex = 1;
            btnPesquisar.Text = "Pesquisar";
            // 
            // btnNovo
            // 
            btnNovo.CustomizableEdges = customizableEdges65;
            btnNovo.DisabledState.BorderColor = Color.DarkGray;
            btnNovo.DisabledState.CustomBorderColor = Color.DarkGray;
            btnNovo.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnNovo.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnNovo.Font = new Font("Segoe UI", 9F);
            btnNovo.ForeColor = Color.White;
            btnNovo.Location = new Point(328, 13);
            btnNovo.Name = "btnNovo";
            btnNovo.ShadowDecoration.CustomizableEdges = customizableEdges66;
            btnNovo.Size = new Size(86, 39);
            btnNovo.TabIndex = 1;
            btnNovo.Text = "➕ Novo Produto";
            // 
            // btnEditar
            // 
            btnEditar.CustomizableEdges = customizableEdges67;
            btnEditar.DisabledState.BorderColor = Color.DarkGray;
            btnEditar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnEditar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnEditar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnEditar.Font = new Font("Segoe UI", 9F);
            btnEditar.ForeColor = Color.White;
            btnEditar.Location = new Point(420, 13);
            btnEditar.Name = "btnEditar";
            btnEditar.ShadowDecoration.CustomizableEdges = customizableEdges68;
            btnEditar.Size = new Size(86, 39);
            btnEditar.TabIndex = 1;
            btnEditar.Text = "✏️ Editar";
            // 
            // btnExcluir
            // 
            btnExcluir.CustomizableEdges = customizableEdges69;
            btnExcluir.DisabledState.BorderColor = Color.DarkGray;
            btnExcluir.DisabledState.CustomBorderColor = Color.DarkGray;
            btnExcluir.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnExcluir.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnExcluir.Font = new Font("Segoe UI", 9F);
            btnExcluir.ForeColor = Color.White;
            btnExcluir.Location = new Point(512, 13);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.ShadowDecoration.CustomizableEdges = customizableEdges70;
            btnExcluir.Size = new Size(86, 39);
            btnExcluir.TabIndex = 1;
            btnExcluir.Text = "🗑️ Excluir";
            // 
            // btnAtualizar
            // 
            btnAtualizar.CustomizableEdges = customizableEdges71;
            btnAtualizar.DisabledState.BorderColor = Color.DarkGray;
            btnAtualizar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnAtualizar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnAtualizar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnAtualizar.Font = new Font("Segoe UI", 9F);
            btnAtualizar.ForeColor = Color.White;
            btnAtualizar.Location = new Point(604, 13);
            btnAtualizar.Name = "btnAtualizar";
            btnAtualizar.ShadowDecoration.CustomizableEdges = customizableEdges72;
            btnAtualizar.Size = new Size(89, 39);
            btnAtualizar.TabIndex = 1;
            btnAtualizar.Text = "🔃 Atualizar";
            // 
            // colIsFeatured
            // 
            colIsFeatured.FillWeight = 99.667984F;
            colIsFeatured.HeaderText = "Destaque";
            colIsFeatured.Name = "colIsFeatured";
            colIsFeatured.Width = 118;
            // 
            // colCategory
            // 
            colCategory.FillWeight = 100.614677F;
            colCategory.HeaderText = "Categoria";
            colCategory.Name = "colCategory";
            colCategory.Width = 119;
            // 
            // colStock
            // 
            colStock.FillWeight = 99.72201F;
            colStock.HeaderText = "Estoque";
            colStock.Name = "colStock";
            colStock.Width = 118;
            // 
            // colPrice
            // 
            colPrice.FillWeight = 99.6654358F;
            colPrice.HeaderText = "Preço";
            colPrice.Name = "colPrice";
            colPrice.Width = 118;
            // 
            // colName
            // 
            colName.FillWeight = 100.611565F;
            colName.HeaderText = "Nome do Produto";
            colName.Name = "colName";
            colName.Width = 119;
            // 
            // colId
            // 
            colId.FillWeight = 99.7183F;
            colId.HeaderText = "ID";
            colId.Name = "colId";
            colId.Width = 118;
            // 
            // ProdutosUserControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridProdutos);
            Controls.Add(pnlToolbar);
            Controls.Add(lblTitulo);
            Name = "ProdutosUserControl";
            Size = new Size(805, 501);
            pnlToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridProdutos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Panel pnlToolbar;
        private Guna.UI2.WinForms.Guna2Button btnAtualizar;
        private Guna.UI2.WinForms.Guna2Button btnExcluir;
        private Guna.UI2.WinForms.Guna2Button btnEditar;
        private Guna.UI2.WinForms.Guna2Button btnNovo;
        private Guna.UI2.WinForms.Guna2Button btnPesquisar;
        private Guna.UI2.WinForms.Guna2TextBox txtPesquisa;
        private DataGridView gridProdutos;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colPrice;
        private DataGridViewTextBoxColumn colStock;
        private DataGridViewTextBoxColumn colCategory;
        private DataGridViewCheckBoxColumn colIsFeatured;
    }
}
