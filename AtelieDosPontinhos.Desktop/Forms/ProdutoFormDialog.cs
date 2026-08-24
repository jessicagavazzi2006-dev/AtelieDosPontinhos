
using AtelieDosPontinhos.Desktop.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtelieDosPontinhos.Desktop.Forms
{
    public partial class ProdutoFormDialog : Form
    {
        // =====================================================================
        // PROPRIEDADES DE SAÍDA
        // =====================================================================

        /// <summary>DTO preenchido quando no modo de criação (OK)</summary>
        public CreateProductDto? ProdutoDto { get; private set; }

        /// <summary>DTO preenchido quando no modo de edição (OK)</summary>
        public UpdateProductDto? UpdateDto { get; private set; }

        // =====================================================================
        // CAMPOS PRIVADOS
        // =====================================================================
        private List<CategoriaResponseDto> _categorias = new();
        private ProductResponseDto? _produtoExistente;

        // =====================================================================
        // CONSTRUTORES
        // =====================================================================
        public ProdutoFormDialog()
        {
            InitializeComponent();
        }

        public ProdutoFormDialog(List<CategoriaResponseDto> categorias, ProductResponseDto? produto)
        {
            _categorias = categorias;
            _produtoExistente = produto;
            InitializeComponent();
        }

        // =====================================================================
        // EVENTO LOAD
        // =====================================================================
        private void ProdutoFormDialog_Load(object sender, EventArgs e)
        {
            //Guard
            if (DesignMode) return;

            // Configura título baseado no modo (criação/edição)
            this.Text = _produtoExistente == null ? "Novo Produto" : "Editar Produto";
            lblTituloForm.Text = _produtoExistente == null ? "➕ Novo Produto" : "✏️ Editar Produto";

            //Popula o ComboBox de categorias
            cmbCategoria.Items.Clear();
            cmbCategoria.Items.Add("Selecione uma categoria...");
            foreach (var cat in _categorias)
                cmbCategoria.Items.Add(cat.Name);
            cmbCategoria.SelectedIndex = 0;

            //Preenche campos se estiver no modo edição
            PreencherCampos();
        }

        // =====================================================================
        // PREENCHIMENTO (MODO EDIÇÃO)
        // =====================================================================
        private void PreencherCampos()
        {
            if (_produtoExistente == null) return;

            txtNome.Text = _produtoExistente.Name;
            txtDescricao.Text = _produtoExistente.Description;
            txtPreco.Text = _produtoExistente.Price.ToString("F2", CultureInfo.CurrentCulture);
            txtCoverUrl.Text = _produtoExistente.CoverImageUrl;
            chkDestaque.Checked = _produtoExistente.IsFeatured;

            var idx = _categorias.FindIndex(c => c.Id == _produtoExistente.CategoryId);
            if (idx >= 0) cmbCategoria.SelectedIndex = idx + 1;
        }

        // =====================================================================
        // SALVAR
        // =====================================================================
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show(
                    "Informe o nome do produto.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPreco.Text, out decimal precoValido) || precoValido <= 0)
            {
                MessageBox.Show(
                    "Informe um preço válido",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (cmbCategoria.SelectedIndex <= 0)
            {
                MessageBox.Show(
                    "Selecione uma categoria",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var categoriaIdx = cmbCategoria.SelectedIndex - 1;
            var categoriaId = _categorias[categoriaIdx].Id;

            if (_produtoExistente == null)
            {
                ProdutoDto = new CreateProductDto
                {
                    Name = txtNome.Text.Trim(),
                    Description = txtDescricao.Text.Trim(),
                    Price = precoValido,
                    CoverImageUrl = txtCoverUrl.Text,
                    CategoryId = categoriaId,
                    IsFeatured = chkDestaque.Checked
                };
            }
            else
            {
                UpdateDto = new UpdateProductDto
                {
                    Name = txtNome.Text.Trim(),
                    Description = txtDescricao.Text.Trim(),
                    Price = precoValido,
                    CoverImageUrl = txtCoverUrl.Text,
                    CategoryId = categoriaId,
                    IsFeatured = chkDestaque.Checked
                };
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
