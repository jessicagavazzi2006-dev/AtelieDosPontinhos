namespace AtelieDosPontinhos.Desktop.UserControls
{
    partial class PedidosUserControl
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
            gridPedidos = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colDateTime = new DataGridViewTextBoxColumn();
            colLocation = new DataGridViewTextBoxColumn();
            colPay = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewComboBoxColumn();
            pnlToolbar = new Panel();
            btnAtualizar = new Guna.UI2.WinForms.Guna2Button();
            btnDetalhes = new Guna.UI2.WinForms.Guna2Button();
            txtPesquisa = new Guna.UI2.WinForms.Guna2TextBox();
            lblTitulo = new Label();
            ((System.ComponentModel.ISupportInitialize)gridPedidos).BeginInit();
            pnlToolbar.SuspendLayout();
            SuspendLayout();
            // 
            // gridPedidos
            // 
            gridPedidos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridPedidos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            gridPedidos.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colDateTime, colLocation, colPay, colTotal, colStatus });
            gridPedidos.Location = new Point(46, 141);
            gridPedidos.Name = "gridPedidos";
            gridPedidos.RowHeadersVisible = false;
            gridPedidos.Size = new Size(713, 336);
            gridPedidos.TabIndex = 5;
            // 
            // colId
            // 
            colId.FillWeight = 48.27308F;
            colId.HeaderText = "ID";
            colId.Name = "colId";
            // 
            // colName
            // 
            colName.FillWeight = 136.654144F;
            colName.HeaderText = "Comprador";
            colName.Name = "colName";
            // 
            // colDateTime
            // 
            colDateTime.FillWeight = 124.315414F;
            colDateTime.HeaderText = "Data";
            colDateTime.Name = "colDateTime";
            // 
            // colLocation
            // 
            colLocation.FillWeight = 97.6189F;
            colLocation.HeaderText = "Localidade";
            colLocation.Name = "colLocation";
            // 
            // colPay
            // 
            colPay.FillWeight = 97.6189F;
            colPay.HeaderText = "Pagamento";
            colPay.Name = "colPay";
            // 
            // colTotal
            // 
            colTotal.FillWeight = 97.6189F;
            colTotal.HeaderText = "Total";
            colTotal.Name = "colTotal";
            // 
            // colStatus
            // 
            colStatus.FillWeight = 97.6189F;
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.Resizable = DataGridViewTriState.True;
            colStatus.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // pnlToolbar
            // 
            pnlToolbar.Controls.Add(btnAtualizar);
            pnlToolbar.Controls.Add(btnDetalhes);
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
            btnAtualizar.Location = new Point(590, 14);
            btnAtualizar.Name = "btnAtualizar";
            btnAtualizar.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnAtualizar.Size = new Size(89, 39);
            btnAtualizar.TabIndex = 1;
            btnAtualizar.Text = "🔃 Atualizar";
            btnAtualizar.Click += btnAtualizar_Click;
            // 
            // btnDetalhes
            // 
            btnDetalhes.BorderRadius = 5;
            btnDetalhes.CustomizableEdges = customizableEdges3;
            btnDetalhes.DisabledState.BorderColor = Color.DarkGray;
            btnDetalhes.DisabledState.CustomBorderColor = Color.DarkGray;
            btnDetalhes.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnDetalhes.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnDetalhes.FillColor = Color.RoyalBlue;
            btnDetalhes.Font = new Font("Segoe UI", 9F);
            btnDetalhes.ForeColor = Color.White;
            btnDetalhes.Location = new Point(485, 14);
            btnDetalhes.Name = "btnDetalhes";
            btnDetalhes.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnDetalhes.Size = new Size(99, 39);
            btnDetalhes.TabIndex = 1;
            btnDetalhes.Text = "🔎 Detalhes";
            btnDetalhes.Click += btnDetalhes_Click;
            // 
            // txtPesquisa
            // 
            txtPesquisa.BorderRadius = 5;
            txtPesquisa.CustomizableEdges = customizableEdges5;
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
            txtPesquisa.PlaceholderText = "🔎 Pesquisar pelo email, local, status...";
            txtPesquisa.SelectedText = "";
            txtPesquisa.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtPesquisa.Size = new Size(255, 36);
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
            lblTitulo.Size = new Size(225, 19);
            lblTitulo.TabIndex = 3;
            lblTitulo.Text = "🛍️ Gerenciamento de Pedidos";
            // 
            // PedidosUserControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridPedidos);
            Controls.Add(pnlToolbar);
            Controls.Add(lblTitulo);
            Name = "PedidosUserControl";
            Size = new Size(805, 501);
            Load += PedidosUserControl_Load;
            ((System.ComponentModel.ISupportInitialize)gridPedidos).EndInit();
            pnlToolbar.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView gridPedidos;
        private DataGridViewTextBoxColumn colPrice;
        private DataGridViewTextBoxColumn colStock;
        private DataGridViewTextBoxColumn colCategory;
        private DataGridViewCheckBoxColumn colIsFeatured;
        private Panel pnlToolbar;
        private Guna.UI2.WinForms.Guna2Button btnAtualizar;
        private Guna.UI2.WinForms.Guna2Button btnDetalhes;
        private Guna.UI2.WinForms.Guna2Button btnNovo;
        private Guna.UI2.WinForms.Guna2TextBox txtPesquisa;
        private Label lblTitulo;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colDateTime;
        private DataGridViewTextBoxColumn colLocation;
        private DataGridViewTextBoxColumn colPay;
        private DataGridViewTextBoxColumn colTotal;
        private DataGridViewComboBoxColumn colStatus;
    }
}
