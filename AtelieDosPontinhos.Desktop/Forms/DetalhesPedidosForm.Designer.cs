namespace AtelieDosPontinhos.Desktop.Forms
{
    partial class DetalhesPedidosForm
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
            panel1 = new Panel();
            btnFechar = new Guna.UI2.WinForms.Guna2Button();
            totalLbl = new Label();
            TotalDoPedido = new Label();
            itemCompradosGrid = new DataGridView();
            itemCol = new DataGridViewTextBoxColumn();
            quantidadeCol = new DataGridViewTextBoxColumn();
            precoUnitarioCol = new DataGridViewTextBoxColumn();
            precoCol = new DataGridViewTextBoxColumn();
            EnderecoLbl = new Label();
            DataLbl = new Label();
            ClienteLbl = new Label();
            TituloDetalhesLbl = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)itemCompradosGrid).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(btnFechar);
            panel1.Controls.Add(totalLbl);
            panel1.Controls.Add(TotalDoPedido);
            panel1.Controls.Add(itemCompradosGrid);
            panel1.Controls.Add(EnderecoLbl);
            panel1.Controls.Add(DataLbl);
            panel1.Controls.Add(ClienteLbl);
            panel1.Controls.Add(TituloDetalhesLbl);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(495, 412);
            panel1.TabIndex = 0;
            // 
            // btnFechar
            // 
            btnFechar.BackColor = Color.Transparent;
            btnFechar.BorderRadius = 10;
            btnFechar.CustomizableEdges = customizableEdges1;
            btnFechar.DisabledState.BorderColor = Color.DarkGray;
            btnFechar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnFechar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnFechar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnFechar.FillColor = Color.DarkRed;
            btnFechar.Font = new Font("Segoe UI", 9F);
            btnFechar.ForeColor = Color.White;
            btnFechar.Location = new Point(378, 18);
            btnFechar.Name = "btnFechar";
            btnFechar.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnFechar.Size = new Size(95, 41);
            btnFechar.TabIndex = 23;
            btnFechar.Text = "Fechar";
            btnFechar.Click += btnFechar_Click;
            // 
            // totalLbl
            // 
            totalLbl.AutoSize = true;
            totalLbl.BackColor = Color.Transparent;
            totalLbl.Font = new Font("Yu Gothic", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            totalLbl.ForeColor = Color.ForestGreen;
            totalLbl.Location = new Point(148, 378);
            totalLbl.Name = "totalLbl";
            totalLbl.Size = new Size(25, 17);
            totalLbl.TabIndex = 22;
            totalLbl.Text = "R$";
            // 
            // TotalDoPedido
            // 
            TotalDoPedido.AutoSize = true;
            TotalDoPedido.BackColor = Color.Transparent;
            TotalDoPedido.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TotalDoPedido.ForeColor = Color.FromArgb(58, 52, 64);
            TotalDoPedido.Location = new Point(21, 375);
            TotalDoPedido.Name = "TotalDoPedido";
            TotalDoPedido.Size = new Size(121, 20);
            TotalDoPedido.TabIndex = 21;
            TotalDoPedido.Text = "Total Do Pedido:";
            // 
            // itemCompradosGrid
            // 
            itemCompradosGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            itemCompradosGrid.Columns.AddRange(new DataGridViewColumn[] { itemCol, quantidadeCol, precoUnitarioCol, precoCol });
            itemCompradosGrid.Location = new Point(21, 136);
            itemCompradosGrid.Name = "itemCompradosGrid";
            itemCompradosGrid.RowHeadersVisible = false;
            itemCompradosGrid.Size = new Size(452, 224);
            itemCompradosGrid.TabIndex = 20;
            // 
            // itemCol
            // 
            itemCol.HeaderText = "Item";
            itemCol.Name = "itemCol";
            itemCol.Width = 120;
            // 
            // quantidadeCol
            // 
            quantidadeCol.HeaderText = "Quantidade";
            quantidadeCol.Name = "quantidadeCol";
            quantidadeCol.Width = 110;
            // 
            // precoUnitarioCol
            // 
            precoUnitarioCol.HeaderText = "Preço Unitário";
            precoUnitarioCol.Name = "precoUnitarioCol";
            precoUnitarioCol.Width = 120;
            // 
            // precoCol
            // 
            precoCol.HeaderText = "Total";
            precoCol.Name = "precoCol";
            // 
            // EnderecoLbl
            // 
            EnderecoLbl.AutoSize = true;
            EnderecoLbl.BackColor = Color.Transparent;
            EnderecoLbl.Font = new Font("Yu Gothic", 9F);
            EnderecoLbl.Location = new Point(21, 106);
            EnderecoLbl.Name = "EnderecoLbl";
            EnderecoLbl.Size = new Size(62, 16);
            EnderecoLbl.TabIndex = 19;
            EnderecoLbl.Text = "Endereço:";
            // 
            // DataLbl
            // 
            DataLbl.AutoSize = true;
            DataLbl.BackColor = Color.Transparent;
            DataLbl.Font = new Font("Yu Gothic", 9F);
            DataLbl.Location = new Point(21, 80);
            DataLbl.Name = "DataLbl";
            DataLbl.Size = new Size(37, 16);
            DataLbl.TabIndex = 18;
            DataLbl.Text = "Data:";
            // 
            // ClienteLbl
            // 
            ClienteLbl.AutoSize = true;
            ClienteLbl.BackColor = Color.Transparent;
            ClienteLbl.Font = new Font("Yu Gothic", 9F);
            ClienteLbl.Location = new Point(21, 55);
            ClienteLbl.Name = "ClienteLbl";
            ClienteLbl.Size = new Size(49, 16);
            ClienteLbl.TabIndex = 17;
            ClienteLbl.Text = "Cliente:";
            // 
            // TituloDetalhesLbl
            // 
            TituloDetalhesLbl.AutoSize = true;
            TituloDetalhesLbl.BackColor = Color.Transparent;
            TituloDetalhesLbl.Font = new Font("Yu Gothic", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TituloDetalhesLbl.ForeColor = Color.FromArgb(58, 52, 64);
            TituloDetalhesLbl.Location = new Point(21, 18);
            TituloDetalhesLbl.Name = "TituloDetalhesLbl";
            TituloDetalhesLbl.Size = new Size(174, 19);
            TituloDetalhesLbl.TabIndex = 16;
            TituloDetalhesLbl.Text = "🛍️ Detalhes Do Pedido";
            // 
            // DetalhesPedidosForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(177, 145, 217);
            ClientSize = new Size(519, 436);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "DetalhesPedidosForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DetalhesPedidos";
            Load += DetalhesPedidosForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)itemCompradosGrid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Guna.UI2.WinForms.Guna2Button btnFechar;
        private Label totalLbl;
        private Label TotalDoPedido;
        private DataGridView itemCompradosGrid;
        private DataGridViewTextBoxColumn itemCol;
        private DataGridViewTextBoxColumn quantidadeCol;
        private DataGridViewTextBoxColumn precoUnitarioCol;
        private DataGridViewTextBoxColumn precoCol;
        private Label EnderecoLbl;
        private Label DataLbl;
        private Label ClienteLbl;
        private Label TituloDetalhesLbl;
    }
}