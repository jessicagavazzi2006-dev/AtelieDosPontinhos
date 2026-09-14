
using AtelieDosPontinhos.Desktop.DTOs;
using AtelieDosPontinhos.Desktop.Themes;
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
using AtelieDosPontinhos.Desktop.Helpers;

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

            ThemeManager.ApplyTheme(this, animate: false);
            ThemeManager.ThemeChanged += OnThemeChanged;

            //FormClosing += ProdutoFormDialog_FormClosing; (corregir problema!)
        }

        public ProdutoFormDialog(List<CategoriaResponseDto> categorias, ProductResponseDto? produto)
        {
            _categorias = categorias;
            _produtoExistente = produto;

            InitializeComponent();

            ThemeManager.ApplyTheme(this, animate: false);
            ThemeManager.ThemeChanged += OnThemeChanged;

            //FormClosing += ProdutoFormDialog_FormClosing; (corrirgir problema!)
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
            //if (!_isDarkMode)
            //{
            //    // Aplicar tema claro
            //}
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
            txtEstoque.Text = _produtoExistente.Stock.ToString();
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

            if (!decimal.TryParse(
                    txtPreco.Text,
                    out decimal precoValido) ||
                precoValido <= 0)
            {
                MessageBox.Show(
                    "Informe um preço válido.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(txtEstoque.Text))
            {
                MessageBox.Show(
                    "Informe a quantidade em estoque.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!int.TryParse(
                    txtEstoque.Text,
                    out int estoqueValido) ||
                estoqueValido < 0)
            {
                MessageBox.Show(
                    "Informe um valor de estoque válido.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (cmbCategoria.SelectedIndex <= 0)
            {
                MessageBox.Show(
                    "Selecione uma categoria.",
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
                    Stock = estoqueValido,
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
                    Stock = estoqueValido,
                    CoverImageUrl = txtCoverUrl.Text,
                    CategoryId = categoriaId,
                    IsFeatured = chkDestaque.Checked
                };
            }

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        private void OnThemeChanged(bool isDark)
        {
            // Reaplica com animação leve (invocar no thread da UI)
            if (this.IsHandleCreated && !this.IsDisposed)
                this.Invoke(() => ThemeManager.ApplyTheme(this, animate: true, durationMs: 300));
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            // remove handler para evitar leak
            ThemeManager.ThemeChanged -= OnThemeChanged;
        }

        private void ProdutoFormDialog_FormClosing(
    object? sender,
    FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown)
                return;

            FormAnimator.AnimateOnClosing(
                this,
                e,
                closeWithSlide: true,
                durationMs: 320,
                offset: 40);
        }
    }
}
