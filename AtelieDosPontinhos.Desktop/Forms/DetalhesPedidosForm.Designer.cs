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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            TituloDetalhes = new Label();
            ClienteLbl = new Label();
            DataLbl = new Label();
            EnderecoLbl = new Label();
            itemCompradosGrid = new DataGridView();
            itemCol = new DataGridViewTextBoxColumn();
            quantidadeCol = new DataGridViewTextBoxColumn();
            precoUnitarioCol = new DataGridViewTextBoxColumn();
            precoCol = new DataGridViewTextBoxColumn();
            TotalDoPedido = new Label();
            totalLbl = new Label();
            guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)itemCompradosGrid).BeginInit();
            SuspendLayout();
            // 
            // TituloDetalhes
            // 
            TituloDetalhes.AutoSize = true;
            TituloDetalhes.Font = new Font("Yu Gothic", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TituloDetalhes.ForeColor = Color.FromArgb(58, 52, 64);
            TituloDetalhes.Location = new Point(12, 22);
            TituloDetalhes.Name = "TituloDetalhes";
            TituloDetalhes.Size = new Size(174, 19);
            TituloDetalhes.TabIndex = 0;
            TituloDetalhes.Text = "🛍️ Detalhes Do Pedido";
            // 
            // ClienteLbl
            // 
            ClienteLbl.AutoSize = true;
            ClienteLbl.Font = new Font("Yu Gothic", 9F);
            ClienteLbl.Location = new Point(12, 59);
            ClienteLbl.Name = "ClienteLbl";
            ClienteLbl.Size = new Size(49, 16);
            ClienteLbl.TabIndex = 1;
            ClienteLbl.Text = "Cliente:";
            // 
            // DataLbl
            // 
            DataLbl.AutoSize = true;
            DataLbl.Font = new Font("Yu Gothic", 9F);
            DataLbl.Location = new Point(12, 84);
            DataLbl.Name = "DataLbl";
            DataLbl.Size = new Size(37, 16);
            DataLbl.TabIndex = 2;
            DataLbl.Text = "Data:";
            // 
            // EnderecoLbl
            // 
            EnderecoLbl.AutoSize = true;
            EnderecoLbl.Font = new Font("Yu Gothic", 9F);
            EnderecoLbl.Location = new Point(12, 110);
            EnderecoLbl.Name = "EnderecoLbl";
            EnderecoLbl.Size = new Size(62, 16);
            EnderecoLbl.TabIndex = 3;
            EnderecoLbl.Text = "Endereço:";
            EnderecoLbl.Click += EnderecoLbl_Click;
            // 
            // itemCompradosGrid
            // 
            itemCompradosGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            itemCompradosGrid.Columns.AddRange(new DataGridViewColumn[] { itemCol, quantidadeCol, precoUnitarioCol, precoCol });
            itemCompradosGrid.Location = new Point(12, 140);
            itemCompradosGrid.Name = "itemCompradosGrid";
            itemCompradosGrid.RowHeadersVisible = false;
            itemCompradosGrid.Size = new Size(503, 224);
            itemCompradosGrid.TabIndex = 4;
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
            precoUnitarioCol.Width = 140;
            // 
            // precoCol
            // 
            precoCol.HeaderText = "Total";
            precoCol.Name = "precoCol";
            precoCol.Width = 130;
            // 
            // TotalDoPedido
            // 
            TotalDoPedido.AutoSize = true;
            TotalDoPedido.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TotalDoPedido.ForeColor = Color.FromArgb(58, 52, 64);
            TotalDoPedido.Location = new Point(12, 379);
            TotalDoPedido.Name = "TotalDoPedido";
            TotalDoPedido.Size = new Size(121, 20);
            TotalDoPedido.TabIndex = 5;
            TotalDoPedido.Text = "Total Do Pedido:";
            // 
            // totalLbl
            // 
            totalLbl.AutoSize = true;
            totalLbl.Font = new Font("Yu Gothic", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            totalLbl.ForeColor = Color.ForestGreen;
            totalLbl.Location = new Point(139, 382);
            totalLbl.Name = "totalLbl";
            totalLbl.Size = new Size(25, 17);
            totalLbl.TabIndex = 6;
            totalLbl.Text = "R$";
            // 
            // guna2Button1
            // 
            guna2Button1.BorderRadius = 10;
            guna2Button1.CustomizableEdges = customizableEdges3;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = Color.DarkRed;
            guna2Button1.Font = new Font("Segoe UI", 9F);
            guna2Button1.ForeColor = Color.White;
            guna2Button1.Location = new Point(420, 22);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2Button1.Size = new Size(95, 41);
            guna2Button1.TabIndex = 7;
            guna2Button1.Text = "Fechar";
            guna2Button1.Click += guna2Button1_Click;
            // 
            // DetalhesPedidosForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(548, 418);
            Controls.Add(guna2Button1);
            Controls.Add(totalLbl);
            Controls.Add(TotalDoPedido);
            Controls.Add(itemCompradosGrid);
            Controls.Add(EnderecoLbl);
            Controls.Add(DataLbl);
            Controls.Add(ClienteLbl);
            Controls.Add(TituloDetalhes);
            FormBorderStyle = FormBorderStyle.None;
            Name = "DetalhesPedidosForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DetalhesPedidos";
            Load += DetalhesPedidosForm_Load;
            ((System.ComponentModel.ISupportInitialize)itemCompradosGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label TituloDetalhes;
        private Label ClienteLbl;
        private Label DataLbl;
        private Label EnderecoLbl;
        private DataGridView itemCompradosGrid;
        private Label TotalDoPedido;
        private Label totalLbl;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private DataGridViewTextBoxColumn itemCol;
        private DataGridViewTextBoxColumn quantidadeCol;
        private DataGridViewTextBoxColumn precoUnitarioCol;
        private DataGridViewTextBoxColumn precoCol;
    }
}