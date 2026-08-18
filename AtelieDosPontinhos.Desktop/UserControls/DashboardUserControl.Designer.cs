namespace AtelieDosPontinhos.Desktop.UserControls
{
    partial class DashboardUserControl
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
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            lblCarregando = new Label();
            cardProdutos = new Panel();
            cardCategorias = new Panel();
            pnlCorProdutos = new Panel();
            pnlCorCategorias = new Panel();
            lblUltimosProdutos = new Label();
            gridUltimosProdutos = new DataGridView();
            cardProdutosLblTitulo = new Label();
            cardCategoriasLblTitulo = new Label();
            cardCategoriasLblNumero = new Label();
            cardCategoriasLblDesc = new Label();
            cardProdutosLblNumero = new Label();
            cardProdutosLblDesc = new Label();
            colId = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colPrice = new DataGridViewTextBoxColumn();
            colStock = new DataGridViewTextBoxColumn();
            colCategory = new DataGridViewTextBoxColumn();
            colIsFeatured = new DataGridViewCheckBoxColumn();
            cardProdutos.SuspendLayout();
            cardCategorias.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridUltimosProdutos).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(49, 29);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(43, 15);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Olá! 👋";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Location = new Point(49, 56);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(238, 15);
            lblSubtitulo.TabIndex = 0;
            lblSubtitulo.Text = "Bem vindo ao Ateliê dos Pontinhos Desktop";
            // 
            // lblCarregando
            // 
            lblCarregando.AutoSize = true;
            lblCarregando.Location = new Point(49, 86);
            lblCarregando.Name = "lblCarregando";
            lblCarregando.Size = new Size(162, 15);
            lblCarregando.TabIndex = 0;
            lblCarregando.Text = "⌛Carregando dados da API...";
            // 
            // cardProdutos
            // 
            cardProdutos.Controls.Add(cardProdutosLblDesc);
            cardProdutos.Controls.Add(cardProdutosLblNumero);
            cardProdutos.Controls.Add(cardProdutosLblTitulo);
            cardProdutos.Location = new Point(49, 112);
            cardProdutos.Name = "cardProdutos";
            cardProdutos.Size = new Size(234, 135);
            cardProdutos.TabIndex = 1;
            // 
            // cardCategorias
            // 
            cardCategorias.Controls.Add(cardCategoriasLblNumero);
            cardCategorias.Controls.Add(cardCategoriasLblDesc);
            cardCategorias.Controls.Add(cardCategoriasLblTitulo);
            cardCategorias.Location = new Point(306, 112);
            cardCategorias.Name = "cardCategorias";
            cardCategorias.Size = new Size(234, 137);
            cardCategorias.TabIndex = 1;
            // 
            // pnlCorProdutos
            // 
            pnlCorProdutos.Location = new Point(49, 112);
            pnlCorProdutos.Name = "pnlCorProdutos";
            pnlCorProdutos.Size = new Size(234, 20);
            pnlCorProdutos.TabIndex = 1;
            // 
            // pnlCorCategorias
            // 
            pnlCorCategorias.Location = new Point(306, 112);
            pnlCorCategorias.Name = "pnlCorCategorias";
            pnlCorCategorias.Size = new Size(234, 20);
            pnlCorCategorias.TabIndex = 1;
            // 
            // lblUltimosProdutos
            // 
            lblUltimosProdutos.AutoSize = true;
            lblUltimosProdutos.Location = new Point(49, 263);
            lblUltimosProdutos.Name = "lblUltimosProdutos";
            lblUltimosProdutos.Size = new Size(180, 15);
            lblUltimosProdutos.TabIndex = 0;
            lblUltimosProdutos.Text = "💾 Últimos produtos cadastrados";
            // 
            // gridUltimosProdutos
            // 
            gridUltimosProdutos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridUltimosProdutos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            gridUltimosProdutos.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colPrice, colStock, colCategory, colIsFeatured });
            gridUltimosProdutos.Location = new Point(49, 287);
            gridUltimosProdutos.Name = "gridUltimosProdutos";
            gridUltimosProdutos.RowHeadersVisible = false;
            gridUltimosProdutos.Size = new Size(673, 205);
            gridUltimosProdutos.TabIndex = 2;
            // 
            // cardProdutosLblTitulo
            // 
            cardProdutosLblTitulo.AutoSize = true;
            cardProdutosLblTitulo.Location = new Point(16, 30);
            cardProdutosLblTitulo.Name = "cardProdutosLblTitulo";
            cardProdutosLblTitulo.Size = new Size(70, 15);
            cardProdutosLblTitulo.TabIndex = 0;
            cardProdutosLblTitulo.Text = "🛍️ Produtos";
            // 
            // cardCategoriasLblTitulo
            // 
            cardCategoriasLblTitulo.AutoSize = true;
            cardCategoriasLblTitulo.Location = new Point(16, 30);
            cardCategoriasLblTitulo.Name = "cardCategoriasLblTitulo";
            cardCategoriasLblTitulo.Size = new Size(78, 15);
            cardCategoriasLblTitulo.TabIndex = 0;
            cardCategoriasLblTitulo.Text = "🏷️ Categorias";
            // 
            // cardCategoriasLblNumero
            // 
            cardCategoriasLblNumero.AutoSize = true;
            cardCategoriasLblNumero.Location = new Point(16, 65);
            cardCategoriasLblNumero.Name = "cardCategoriasLblNumero";
            cardCategoriasLblNumero.Size = new Size(13, 15);
            cardCategoriasLblNumero.TabIndex = 0;
            cardCategoriasLblNumero.Text = "0";
            // 
            // cardCategoriasLblDesc
            // 
            cardCategoriasLblDesc.AutoSize = true;
            cardCategoriasLblDesc.Location = new Point(16, 105);
            cardCategoriasLblDesc.Name = "cardCategoriasLblDesc";
            cardCategoriasLblDesc.Size = new Size(171, 15);
            cardCategoriasLblDesc.TabIndex = 0;
            cardCategoriasLblDesc.Text = "Total de categorias cadastradas";
            // 
            // cardProdutosLblNumero
            // 
            cardProdutosLblNumero.AutoSize = true;
            cardProdutosLblNumero.Location = new Point(16, 65);
            cardProdutosLblNumero.Name = "cardProdutosLblNumero";
            cardProdutosLblNumero.Size = new Size(13, 15);
            cardProdutosLblNumero.TabIndex = 0;
            cardProdutosLblNumero.Text = "0";
            // 
            // cardProdutosLblDesc
            // 
            cardProdutosLblDesc.AutoSize = true;
            cardProdutosLblDesc.Location = new Point(16, 105);
            cardProdutosLblDesc.Name = "cardProdutosLblDesc";
            cardProdutosLblDesc.Size = new Size(166, 15);
            cardProdutosLblDesc.TabIndex = 0;
            cardProdutosLblDesc.Text = "Total de produtos cadastrados";
            // 
            // colId
            // 
            colId.HeaderText = "ID";
            colId.Name = "colId";
            // 
            // colName
            // 
            colName.HeaderText = "Nome do produto";
            colName.Name = "colName";
            // 
            // colPrice
            // 
            colPrice.HeaderText = "Preço";
            colPrice.Name = "colPrice";
            // 
            // colStock
            // 
            colStock.HeaderText = "Estoque";
            colStock.Name = "colStock";
            // 
            // colCategory
            // 
            colCategory.HeaderText = "Categoria";
            colCategory.Name = "colCategory";
            // 
            // colIsFeatured
            // 
            colIsFeatured.HeaderText = "Destaque";
            colIsFeatured.Name = "colIsFeatured";
            // 
            // DashboardUserControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridUltimosProdutos);
            Controls.Add(pnlCorCategorias);
            Controls.Add(pnlCorProdutos);
            Controls.Add(cardCategorias);
            Controls.Add(cardProdutos);
            Controls.Add(lblUltimosProdutos);
            Controls.Add(lblCarregando);
            Controls.Add(lblSubtitulo);
            Controls.Add(lblTitulo);
            Name = "DashboardUserControl";
            Size = new Size(805, 501);
            cardProdutos.ResumeLayout(false);
            cardProdutos.PerformLayout();
            cardCategorias.ResumeLayout(false);
            cardCategorias.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridUltimosProdutos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblSubtitulo;
        private Label lblCarregando;
        private Panel cardProdutos;
        private Panel cardCategorias;
        private Panel pnlCorProdutos;
        private Panel pnlCorCategorias;
        private Label lblUltimosProdutos;
        private DataGridView gridUltimosProdutos;
        private Label cardProdutosLblDesc;
        private Label cardProdutosLblNumero;
        private Label cardProdutosLblTitulo;
        private Label cardCategoriasLblNumero;
        private Label cardCategoriasLblDesc;
        private Label cardCategoriasLblTitulo;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colPrice;
        private DataGridViewTextBoxColumn colStock;
        private DataGridViewTextBoxColumn colCategory;
        private DataGridViewCheckBoxColumn colIsFeatured;
    }
}
