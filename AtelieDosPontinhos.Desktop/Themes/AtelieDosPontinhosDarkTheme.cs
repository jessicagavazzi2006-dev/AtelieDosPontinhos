using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace AtelieDosPontinhos.Desktop.Themes
{
    /// <summary>
    /// Versão Dark Mode do tema AtelieDosPontinhos ajustada para a paleta fornecida.
    /// Fundo escuro, sidebar roxa e destaques em lilás/roxo.
    /// </summary>
    public static class AtelieDosPontinhosDarkTheme
    {
        // Paleta inspirada na imagem
        public static Color FundoEscuro => Color.FromArgb(27, 27, 29);       // layout geral muito escuro
        public static Color Surface => Color.FromArgb(36, 36, 38);          // cartões / panels
        public static Color Cabecalho => Color.FromArgb(42, 42, 44);        // cabeçalhos e áreas escuras
        public static Color BordaEscura => Color.FromArgb(58, 58, 62);      // bordas sutis
        public static Color TextoClaro => Color.FromArgb(230, 230, 232);    // texto principal claro
        public static Color TextoSecundario => Color.FromArgb(160, 160, 170); // texto secundário

        // Accent roxo (sidebar) e variantes
        public static Color AccentRoxo => Color.FromArgb(155, 113, 206);     // sidebar / botões principais
        public static Color AccentRoxoHover => Color.FromArgb(122, 63, 176); // hover/pressed
        public static Color AccentLilas => Color.FromArgb(173, 126, 219);   // linha destacada do grid (opcional)

        // Linhas do grid
        public static Color LinhaPar => Color.FromArgb(31, 31, 33);
        public static Color LinhaImpar => Color.FromArgb(40, 38, 40);

        public static Color SelecionadoFundo => AccentRoxo;
        public static Color SelecionadoTexto => Color.White;

        // Tipografia (reaproveita fontes)
        public static string FonteBase => AtelieDosPontinhosTheme.FonteBase;
        public static Font FontePequena => AtelieDosPontinhosTheme.FontePequena;
        public static Font FonteNormal => AtelieDosPontinhosTheme.FonteNormal;
        public static Font FonteMedia => AtelieDosPontinhosTheme.FonteMedia;
        public static Font FonteCabecalhoGrid => AtelieDosPontinhosTheme.FonteCabecalhoGrid;
        public static Font FonteTitulo => AtelieDosPontinhosTheme.FonteTitulo;

        public static void AplicarEstiloGrid(DataGridView grid)
        {
            if (grid.Tag?.ToString() != "KeepEditable")
            {
                grid.ReadOnly = false;
            }
            grid.BackgroundColor = LinhaPar;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = BordaEscura;

            grid.ColumnHeadersDefaultCellStyle.BackColor = Cabecalho;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = TextoClaro;
            grid.ColumnHeadersDefaultCellStyle.Font = FonteCabecalhoGrid;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 6, 8, 6);
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersHeight = 42;
            grid.EnableHeadersVisualStyles = false;

            grid.DefaultCellStyle.BackColor = LinhaPar;
            grid.DefaultCellStyle.ForeColor = TextoClaro;
            grid.DefaultCellStyle.Font = FonteNormal;
            grid.DefaultCellStyle.SelectionBackColor = SelecionadoFundo;
            grid.DefaultCellStyle.SelectionForeColor = SelecionadoTexto;
            grid.DefaultCellStyle.Padding = new Padding(8, 6, 8, 6);

            grid.AlternatingRowsDefaultCellStyle.BackColor = LinhaImpar;

            grid.RowHeadersVisible = false;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grid.RowTemplate.Height = 36;

            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ReadOnly = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
        }

        public static void AplicarEstiloFormulario(Form form)
        {
            if (form.Tag?.ToString() != "KeepBackColor")
            {
                form.BackColor = FundoEscuro;
            }

            form.ForeColor = TextoClaro;

            ApplyToControlRecursive(form);
        }

        private static void ApplyToControlRecursive(Control control)
        {
            foreach (Control c in control.Controls)
            {
                bool keepBack = c?.Tag?.ToString() == "KeepBackColor";

                switch (c)
                {
                    case Label lbl:
                        lbl.ForeColor = TextoClaro;
                        break;

                    case Panel pnl:
                        if (!keepBack) pnl.BackColor = Surface;
                        break;

                    case DataGridView dgv:
                        AplicarEstiloGrid(dgv);
                        break;

                    //case Guna2Panel gPnl:
                    //    if (!keepBack) gPnl.FillColor = Surface;
                    //    gPnl.BorderColor = BordaEscura;
                    //    gPnl.ShadowDecoration.Enabled = false;
                    //    break;

                    case Guna2Button gBtn:
                        gBtn.FillColor = AccentRoxo;
                        gBtn.ForeColor = Color.White;
                        gBtn.Font = FonteNormal;
                        gBtn.BorderColor = Color.Transparent;
                        gBtn.HoverState.FillColor = AccentRoxoHover;
                        gBtn.HoverState.ForeColor = Color.White;
                        gBtn.DisabledState.FillColor = Color.FromArgb(80, 80, 88);
                        gBtn.DisabledState.ForeColor = TextoSecundario;
                        break;

                    case Guna2TextBox gTxt:
                        gTxt.FillColor = Color.FromArgb(34, 34, 36);
                        gTxt.ForeColor = TextoClaro;
                        gTxt.PlaceholderForeColor = TextoSecundario;
                        gTxt.BorderColor = BordaEscura;
                        gTxt.DisabledState.FillColor = Color.FromArgb(30, 30, 32);
                        break;

                    case Guna.UI2.WinForms.Guna2ComboBox gCombo:
                        gCombo.FillColor = Color.FromArgb(34, 34, 36);
                        gCombo.ForeColor = TextoClaro;
                        gCombo.BorderColor = BordaEscura;
                        gCombo.ItemHeight = 28;
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
                        // aplica fore color por padrão e, se o controle tiver BackColor definido explicitamente, atualiza se não marcado
                        try
                        {
                            c.ForeColor = TextoClaro;
                            if (c.BackColor != Color.Transparent && !keepBack)
                                c.BackColor = Surface;
                        }
                        catch { }
                        break;
                }

                if (c.HasChildren) ApplyToControlRecursive(c);
            }
        }
    }
}