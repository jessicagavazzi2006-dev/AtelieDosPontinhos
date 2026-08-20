// =============================================================================
// AtelieDosPontinhos.Desktop - Themes/AppTheme.cs
// =============================================================================
//  CONCEITO: Design System / Theme Manager
//
// Centraliza TODAS as cores, fontes e estilos da aplicação baseados na 
// paleta de tons pastéis (Lavanda e Creme).
// =============================================================================

using System.Drawing;
using System.Windows.Forms;

namespace AtelieDosPontinhos.Desktop.Themes
{
    /// <summary>
    /// Tema visual moderno focado em tons pastéis e design limpo.
    /// Define todas as cores, fontes e dimensões usadas na interface.
    /// </summary>
    public static class AppTheme
    {
        // =====================================================================
        // PALETA DE CORES PRINCIPAL (Baseada na imagem)
        // =====================================================================

        /// <summary>Lavanda Suave — usado no cabeçalho do grid, painéis e destaques sutis</summary>
        public static Color LavandaSuave => Color.FromArgb(228, 217, 242);     // #E4D9F2

        /// <summary>Bege Creme — usado em fundos secundários e hover de linhas</summary>
        public static Color BegeCreme => Color.FromArgb(243, 235, 225);        // #F3EBE1

        /// <summary>Roxo Primário — botões principais, paginação e seleção ativa</summary>
        public static Color RoxoPrimario => Color.FromArgb(140, 107, 177);     // #8C6BB1

        /// <summary>Roxo Variante — hover de botões primários</summary>
        public static Color RoxoVariante => Color.FromArgb(115, 85, 148);      // Tom mais escuro do roxo

        /// <summary>Branco puro — fundos de formulários e grids</summary>
        public static Color Branco => Color.White;                             // #FFFFFF

        /// <summary>Cinza de fundo — fundo da janela principal (off-white)</summary>
        public static Color CinzaFundo => Color.FromArgb(250, 250, 250);       // #FAFAFA

        /// <summary>Borda suave — linhas divisórias do datagrid e cards</summary>
        public static Color BordaSuave => Color.FromArgb(214, 207, 224);       // #D6CFE0

        /// <summary>Texto principal — grafite arroxeado para leitura confortável</summary>
        public static Color TextoPrincipal => Color.FromArgb(58, 52, 64);      // #3A3440

        /// <summary>Texto secundário — placeholders e textos de apoio</summary>
        public static Color TextoSecundario => Color.FromArgb(120, 112, 130);  // #787082

        // =====================================================================
        // BOTÕES
        // =====================================================================

        public static Color BotaoPrimarioFundo => RoxoPrimario;
        public static Color BotaoPrimarioTexto => Color.White;
        public static Color BotaoPrimarioHover => RoxoVariante;

        public static Color BotaoSecundarioFundo => Color.White;
        public static Color BotaoSecundarioTexto => RoxoPrimario;
        public static Color BotaoSecundarioBorda => BordaSuave;

        // =====================================================================
        // DATAGRIDVIEW
        // =====================================================================

        public static Color GridCabecalhoFundo => LavandaSuave;
        public static Color GridCabecalhoTexto => TextoPrincipal;
        public static Color GridLinhaPar => Color.White;
        public static Color GridLinhaImpar => BegeCreme;
        public static Color GridLinhaSelecionada => RoxoPrimario;
        public static Color GridTextoSelecionado => Color.White;
        public static Color GridBorda => BordaSuave;

        // =====================================================================
        // TIPOGRAFIA
        // =====================================================================

        /// <summary>Fonte padrão do sistema</summary>
        public static string FonteBase => "Segoe UI";

        public static Font FontePequena => new(FonteBase, 8f);
        public static Font FonteNormal => new(FonteBase, 9f);
        public static Font FonteMedia => new(FonteBase, 10f);
        public static Font FonteCabecalhoGrid => new(FonteBase, 10f, FontStyle.Bold);
        public static Font FonteTitulo => new(FonteBase, 14f, FontStyle.Bold);

        // =====================================================================
        // MÉTODOS UTILITÁRIOS
        // =====================================================================

        /// <summary>
        /// Aplica o estilo moderno e em tons pastéis a um DataGridView.
        /// </summary>
        public static void AplicarEstiloGrid(DataGridView grid)
        {
            // Estilo geral (fundo branco, bordas sutis)
            grid.BackgroundColor = Branco;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = GridBorda;

            // Cabeçalho (Usando a cor Lavanda da sua imagem)
            grid.ColumnHeadersDefaultCellStyle.BackColor = GridCabecalhoFundo;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = GridCabecalhoTexto;
            grid.ColumnHeadersDefaultCellStyle.Font = FonteCabecalhoGrid;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 8, 10, 8);
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersHeight = 45;
            grid.EnableHeadersVisualStyles = false; // Obrigatório no WinForms para a cor de fundo funcionar

            // Estilo das Linhas
            grid.DefaultCellStyle.BackColor = GridLinhaPar;
            grid.DefaultCellStyle.ForeColor = TextoPrincipal;
            grid.DefaultCellStyle.Font = FonteNormal;
            grid.DefaultCellStyle.SelectionBackColor = GridLinhaSelecionada; // Fica Roxo ao selecionar
            grid.DefaultCellStyle.SelectionForeColor = GridTextoSelecionado;
            grid.DefaultCellStyle.Padding = new Padding(10, 5, 10, 5);

            // Linhas alternadas (Efeito zebrado usando o Bege da sua imagem)
            grid.AlternatingRowsDefaultCellStyle.BackColor = GridLinhaImpar;

            // Configurações visuais de estrutura
            grid.RowHeadersVisible = false; // Esconde aquela setinha feia da esquerda
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grid.RowTemplate.Height = 40;

            // Comportamento
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
        }
    }
}