using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace AtelieDosPontinhos.Desktop.Themes
{
    /// <summary>
    /// Versão Dark Mode do tema AtelieDosPontinhos com aplicação aprofundada a controles Guna2
    /// e suporte explícito a panels do Windows Forms.
    /// </summary>
    public static class AtelieDosPontinhosDarkTheme
    {
        // =====================================================================
        // PALETA PRINCIPAL (Dark)
        // =====================================================================

        public static Color FundoEscuro => Color.FromArgb(24, 24, 28);
        public static Color Surface => Color.FromArgb(32, 32, 36);
        public static Color Cabecalho => Color.FromArgb(42, 38, 48);
        public static Color BordaEscura => Color.FromArgb(55, 50, 64);
        public static Color TextoClaro => Color.FromArgb(235, 235, 238);
        public static Color TextoSecundario => Color.FromArgb(170, 170, 180);
        public static Color AccentRoxo => Color.FromArgb(155, 113, 206);
        public static Color AccentRoxoHover => Color.FromArgb(115, 85, 148);
        public static Color LinhaPar => Color.FromArgb(32, 32, 36);
        public static Color LinhaImpar => Color.FromArgb(36, 36, 40);
        public static Color SelecionadoFundo => AccentRoxo;
        public static Color SelecionadoTexto => Color.White;

        // TIPOGRAFIA (reaproveita as fontes do tema claro)
        public static string FonteBase => AtelieDosPontinhosTheme.FonteBase;
        public static Font FontePequena => AtelieDosPontinhosTheme.FontePequena;
        public static Font FonteNormal => AtelieDosPontinhosTheme.FonteNormal;
        public static Font FonteMedia => AtelieDosPontinhosTheme.FonteMedia;
        public static Font FonteCabecalhoGrid => AtelieDosPontinhosTheme.FonteCabecalhoGrid;
        public static Font FonteTitulo => AtelieDosPontinhosTheme.FonteTitulo;

        /// <summary>
        /// Aplica o estilo dark e moderno a um DataGridView.
        /// </summary>
        public static void AplicarEstiloGrid(DataGridView grid)
        {
            grid.BackgroundColor = BordaEscura;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = BordaEscura;

            grid.ColumnHeadersDefaultCellStyle.BackColor = Cabecalho;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = TextoClaro;
            grid.ColumnHeadersDefaultCellStyle.Font = FonteCabecalhoGrid;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 8, 10, 8);
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersHeight = 44;
            grid.EnableHeadersVisualStyles = false;

            grid.DefaultCellStyle.BackColor = LinhaPar;
            grid.DefaultCellStyle.ForeColor = TextoClaro;
            grid.DefaultCellStyle.Font = FonteNormal;
            grid.DefaultCellStyle.SelectionBackColor = SelecionadoFundo;
            grid.DefaultCellStyle.SelectionForeColor = SelecionadoTexto;
            grid.DefaultCellStyle.Padding = new Padding(10, 6, 10, 6);

            grid.AlternatingRowsDefaultCellStyle.BackColor = LinhaImpar;

            grid.RowHeadersVisible = false;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grid.RowTemplate.Height = 40;

            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
        }

        /// <summary>
        /// Aplica estilos dark a um formulário e a todos os controles filhos.
        /// Uso após InitializeComponent().
        /// </summary>
        public static void AplicarEstiloFormulario(Form form)
        {
            form.BackColor = BordaEscura;
            form.ForeColor = TextoClaro;

            ApplyToControlRecursive(form);
        }

        // Aplica recursivamente estilos especificos por tipo (inclui Guna2 controls e Panel)
        private static void ApplyToControlRecursive(Control control)
        {
            

            foreach (Control c in control.Controls)
            {
                bool keepBack = c?.Tag?.ToString() == "KeepBackColor";
                // Aplique estilos por tipo
                switch (c)
                {
                    case Label lbl:
                        lbl.ForeColor = TextoClaro;
                        lbl.Font = lbl.Font;
                        break;

                    case Panel pnl:
                        if (!keepBack) pnl.BackColor = BordaEscura;
                        break;



                    case DataGridView dgv:
                        AplicarEstiloGrid(dgv);
                        break;

                    case Guna2Button gBtn:
                        gBtn.FillColor = AccentRoxo;
                        gBtn.ForeColor = Color.White;
                        gBtn.Font = FonteNormal;
                        gBtn.BorderColor = Color.Transparent;
                        gBtn.HoverState.FillColor = AccentRoxoHover;
                        gBtn.HoverState.ForeColor = Color.White;
                        gBtn.PressedColor = AccentRoxoHover;
                        gBtn.DisabledState.FillColor = Color.FromArgb(80, 80, 88);
                        gBtn.DisabledState.ForeColor = TextoSecundario;
                        gBtn.FillColor = AccentRoxo;
                        break;

                    case Guna2TextBox gTxt:
                        gTxt.FillColor = Color.FromArgb(40, 40, 44);
                        gTxt.ForeColor = TextoClaro;
                        gTxt.PlaceholderForeColor = TextoSecundario;
                        gTxt.BorderColor = BordaEscura;
                        gTxt.DisabledState.FillColor = Color.FromArgb(38, 38, 42);
                        break;

                    case Guna2ComboBox gCombo:
                        gCombo.FillColor = Color.FromArgb(40, 40, 44);
                        gCombo.ForeColor = TextoClaro;
                        gCombo.BorderColor = BordaEscura;
                        gCombo.ItemHeight = 30;
                        break;

                    case Guna2CheckBox gChk:
                        gChk.ForeColor = TextoClaro;
                        gChk.CheckedState.BorderColor = AccentRoxo;
                        gChk.CheckedState.FillColor = AccentRoxo;
                        break;

                    case Button btn:
                        btn.BackColor = AccentRoxo;
                        btn.ForeColor = Color.White;
                        btn.FlatStyle = FlatStyle.Flat;
                        break;

                    default:
                        // Para outros controles, aplica cor de texto quando fizer sentido
                        c.ForeColor = TextoClaro;
                        break;
                }

                // Se o controle for um Guna2DataGridView (especializado), aplica grid style também
                if (c is Guna2DataGridView gDgv)
                {
                    AplicarEstiloGrid(gDgv);
                }

                // Reaplicar para filhos
                if (c.HasChildren)
                    ApplyToControlRecursive(c);
            }
        }
    }
}