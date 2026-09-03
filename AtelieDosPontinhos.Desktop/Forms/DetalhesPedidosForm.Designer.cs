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
            TituloDetalhes.Location = new Point(12, 9);
            TituloDetalhes.Name = "TituloDetalhes";
            TituloDetalhes.Size = new Size(110, 15);
            TituloDetalhes.TabIndex = 0;
            TituloDetalhes.Text = "Detalhes Do Pedido";
            // 
            // ClienteLbl
            // 
            ClienteLbl.AutoSize = true;
            ClienteLbl.Location = new Point(12, 35);
            ClienteLbl.Name = "ClienteLbl";
            ClienteLbl.Size = new Size(47, 15);
            ClienteLbl.TabIndex = 1;
            ClienteLbl.Text = "Cliente:";
            // 
            // DataLbl
            // 
            DataLbl.AutoSize = true;
            DataLbl.Location = new Point(12, 60);
            DataLbl.Name = "DataLbl";
            DataLbl.Size = new Size(34, 15);
            DataLbl.TabIndex = 2;
            DataLbl.Text = "Data:";
            // 
            // EnderecoLbl
            // 
            EnderecoLbl.AutoSize = true;
            EnderecoLbl.Location = new Point(12, 86);
            EnderecoLbl.Name = "EnderecoLbl";
            EnderecoLbl.Size = new Size(59, 15);
            EnderecoLbl.TabIndex = 3;
            EnderecoLbl.Text = "Endereço:";
            // 
            // itemCompradosGrid
            // 
            itemCompradosGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            itemCompradosGrid.Columns.AddRange(new DataGridViewColumn[] { itemCol, quantidadeCol, precoUnitarioCol, precoCol });
            itemCompradosGrid.Location = new Point(12, 120);
            itemCompradosGrid.Name = "itemCompradosGrid";
            itemCompradosGrid.Size = new Size(712, 224);
            itemCompradosGrid.TabIndex = 4;
            // 
            // itemCol
            // 
            itemCol.HeaderText = "Item";
            itemCol.Name = "itemCol";
            // 
            // quantidadeCol
            // 
            quantidadeCol.HeaderText = "Quantidade";
            quantidadeCol.Name = "quantidadeCol";
            // 
            // precoUnitarioCol
            // 
            precoUnitarioCol.HeaderText = "Preço Unitário";
            precoUnitarioCol.Name = "precoUnitarioCol";
            // 
            // precoCol
            // 
            precoCol.HeaderText = "Total";
            precoCol.Name = "precoCol";
            // 
            // TotalDoPedido
            // 
            TotalDoPedido.AutoSize = true;
            TotalDoPedido.Location = new Point(12, 361);
            TotalDoPedido.Name = "TotalDoPedido";
            TotalDoPedido.Size = new Size(94, 15);
            TotalDoPedido.TabIndex = 5;
            TotalDoPedido.Text = "Total Do Pedido:";
            // 
            // totalLbl
            // 
            totalLbl.AutoSize = true;
            totalLbl.Location = new Point(12, 399);
            totalLbl.Name = "totalLbl";
            totalLbl.Size = new Size(20, 15);
            totalLbl.TabIndex = 6;
            totalLbl.Text = "R$";
            // 
            // guna2Button1
            // 
            guna2Button1.CustomizableEdges = customizableEdges1;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.Font = new Font("Segoe UI", 9F);
            guna2Button1.ForeColor = Color.White;
            guna2Button1.Location = new Point(587, 9);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Button1.Size = new Size(180, 45);
            guna2Button1.TabIndex = 7;
            guna2Button1.Text = "Fechar Detalhe";
            guna2Button1.Click += guna2Button1_Click;
            // 
            // DetalhesPedidosForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
        private DataGridViewTextBoxColumn itemCol;
        private DataGridViewTextBoxColumn quantidadeCol;
        private DataGridViewTextBoxColumn precoUnitarioCol;
        private DataGridViewTextBoxColumn precoCol;
        private Label TotalDoPedido;
        private Label totalLbl;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
    }
}