using AtelieDosPontinhos.Desktop.DTOs;
using AtelieDosPontinhos.Desktop.Forms;
using AtelieDosPontinhos.Desktop.Helpers;
using AtelieDosPontinhos.Desktop.Services;
using AtelieDosPontinhos.Desktop.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtelieDosPontinhos.Desktop.UserControls
{
    public partial class PedidosUserControl : UserControl
    {
        //=================================================
        // SERVIÇOS (Inicilizados no Load)
        //=================================================
        private PedidosApiService _pedidosService = null!;
        private UsuariosApiService _usuariosService = null!;

        //=================================================
        // DADOS
        //=================================================
        private List<Pedido> _todosPedidos = new();

        //=================================================
        // CONSTRUTOR
        //=================================================
        public PedidosUserControl()
        {
            InitializeComponent();
        }

        private async void PedidosUserControl_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            _pedidosService = new PedidosApiService();
            _usuariosService = new UsuariosApiService();

            gridPedidos.Tag = "KeepEditable";
            AtelieDosPontinhosTheme.AplicarEstiloGrid(gridPedidos);

            ConfigurarGrid();

            // handlers para evitar a caixa de diálogo padrão e para commit imediato do combo
            gridPedidos.DataError += GridPedidos_DataError;
            gridPedidos.CurrentCellDirtyStateChanged += GridPedidos_CurrentCellDirtyStateChanged;
            gridPedidos.CellValueChanged += GridPedidos_CellValueChanged;
            gridPedidos.CellMouseDown += GridPedidos_CellMouseDown;

            await CarregarDadosAsync();
        }

        private async Task CarregarDadosAsync()
        {
            gridPedidos.Rows.Clear();

            try
            {
                var tarefaPedidos = _pedidosService.GetAllAsync();
                var tarefaUsuarios = _usuariosService.GetAllAsync();
                await Task.WhenAll(tarefaPedidos, tarefaUsuarios);
                _todosPedidos = tarefaPedidos.Result;
                var usuarios = tarefaUsuarios.Result;

                // cria dicionário para mapear userId -> userName
                _userMap = usuarios.ToDictionary(u => u.Id, u => u.UserName ?? string.Empty);
                PopularGrid(_todosPedidos);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar pedidos: {ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void PopularGrid(List<Pedido> pedidos)
        {
            gridPedidos.Rows.Clear();
            foreach (var p in pedidos)
            {
                var localidade = string.IsNullOrWhiteSpace(p.Estado) ? p.Cidade : $"{p.Cidade} - {p.Estado}";

                var status = NormalizeStatus(p.Status);

                // garante que o combo contenha o valor
                if (gridPedidos.Columns.Contains("colStatus") && gridPedidos.Columns["colStatus"] is DataGridViewComboBoxColumn combo)
                {
                    if (!combo.Items.Contains(status)) combo.Items.Add(status);
                }

                // mapeia userId -> userName quando possível
                var userName = _userMap != null && _userMap.TryGetValue(p.UserId, out var nm) ? nm : p.UserId;

                // mapeia metodo de pagamento (pode vir como id) para um nome legível
                var pagamento = MapPaymentName(p.MetodoPagamento);

                int rowIndex = gridPedidos.Rows.Add(
                    p.Id,
                    userName,
                    p.DataPedido.ToString("dd/MM/yyyy"),
                    localidade,
                    pagamento,
                    p.ValorTotal.ToString("C"),
                    status
                );

                // garante que a célula de status daquela linha NÃO seja ReadOnly
                gridPedidos.Rows[rowIndex].Cells["colStatus"].ReadOnly = false;
            }
        }

        private Dictionary<string, string> _userMap = new();

        private static string MapPaymentName(string metodo)
        {
            if (string.IsNullOrWhiteSpace(metodo)) return string.Empty;

            // se vier um número (id), converte para nome conhecido
            if (int.TryParse(metodo, out var id))
            {
                return id switch
                {
                    1 => "Cartão Crédito",
                    2 => "Cartão Débito",
                    3 => "Pix",
                    _ => metodo
                };
            }

            // caso já seja uma descrição, retorne ela mesma
            return metodo;
        }

        private void FiltrarPedidos()
        {
            var termo = txtPesquisa.Text.Trim();
            if (string.IsNullOrWhiteSpace(termo))
            {
                PopularGrid(_todosPedidos);
                return;
            }

            var filtrados = _todosPedidos.Where(p =>
                // busca por ID
                p.Id.ToString().IndexOf(termo, StringComparison.OrdinalIgnoreCase) >= 0

                // busca pelo nome do usuário através do dicionário userId -> userName
                || (_userMap != null && _userMap.TryGetValue(p.UserId, out var nome) && !string.IsNullOrWhiteSpace(nome)
                    && nome.IndexOf(termo, StringComparison.OrdinalIgnoreCase) >= 0)

                // cidade / estado (partial, case-insensitive)
                || (p.Cidade?.IndexOf(termo, StringComparison.OrdinalIgnoreCase) >= 0)
                || (p.Estado?.IndexOf(termo, StringComparison.OrdinalIgnoreCase) >= 0)

                // método de pagamento: compara tanto o valor bruto quanto o nome mapeado
                || (p.MetodoPagamento?.IndexOf(termo, StringComparison.OrdinalIgnoreCase) >= 0)
                || (MapPaymentName(p.MetodoPagamento).IndexOf(termo, StringComparison.OrdinalIgnoreCase) >= 0)

                // status
                || (NormalizeStatus(p.Status).IndexOf(termo, StringComparison.OrdinalIgnoreCase) >= 0)
            ).ToList();

            PopularGrid(filtrados);
        }

        private void ConfigurarGrid()
        {
            // garante que o grid seja editável quando necessário
            // gridPedidos.Tag já é setado no Load (KeepEditable)
            gridPedidos.Rows.Clear();
            gridPedidos.Columns.Clear();
            gridPedidos.AutoGenerateColumns = false;
            gridPedidos.EditMode = DataGridViewEditMode.EditOnEnter; // já estava assim, mantém
            gridPedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridPedidos.MultiSelect = false;
            gridPedidos.AllowUserToAddRows = false;

            gridPedidos.ReadOnly = false;

            // cria colunas no DataGridView (ID, Comprador, Data, Localidade, Pagamento, Total, Status)
            gridPedidos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "ID", ReadOnly = true });
            gridPedidos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colUser", HeaderText = "Comprador", ReadOnly = true });
            gridPedidos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colData", HeaderText = "Data", ReadOnly = true });
            gridPedidos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colLocalidade", HeaderText = "Localidade", ReadOnly = true });
            gridPedidos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPagamento", HeaderText = "Pagamento", ReadOnly = true });
            gridPedidos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTotal", HeaderText = "Total", ReadOnly = true });

            var colStatus = new DataGridViewComboBoxColumn
            {
                Name = "colStatus",
                HeaderText = "Status",
                ValueType = typeof(string),
                ReadOnly = false, // coluna editável
                                  // Mostrar como combo apenas para a célula atual para evitar problemas do estilo
                DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox,
                DisplayStyleForCurrentCellOnly = true,
                FlatStyle = FlatStyle.Standard
            };
            colStatus.Items.AddRange(new string[] { "Concluido", "Pendente", "Cancelado" });
            gridPedidos.Columns.Add(colStatus);
        }

        private static string NormalizeStatus(string status)
        {
            return status switch
            {
                "Concluído" or "Concluido" or "Pago" => "Concluido",
                "Cancelado" or "Cancelada" => "Cancelado",
                _ => "Pendente",
            };
        }

        private void GridPedidos_DataError(object? sender, DataGridViewDataErrorEventArgs e)
        {
            // suprime a caixa de diálogo padrão e evita que o app quebre
            e.ThrowException = false;
            // opcional: log
            // System.Diagnostics.Debug.WriteLine($"DataGrid error: {e.Exception?.Message}");
        }

        private void GridPedidos_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            if (gridPedidos.IsCurrentCellDirty && gridPedidos.CurrentCell is DataGridViewComboBoxCell)
            {
                gridPedidos.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private async void GridPedidos_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var changedColumn = gridPedidos.Columns[e.ColumnIndex].Name;
            if (changedColumn != "colStatus") return;

            // Desativa o evento temporariamente para evitar loops/reentrância
            gridPedidos.CellValueChanged -= GridPedidos_CellValueChanged;

            try
            {
                var row = gridPedidos.Rows[e.RowIndex];
                var id = Convert.ToInt32(row.Cells["colId"].Value);
                var novoStatus = row.Cells["colStatus"].Value?.ToString() ?? "Pendente";

                var pedido = _todosPedidos.FirstOrDefault(x => x.Id == id);
                if (pedido == null || pedido.Status == novoStatus) return;

                pedido.Status = novoStatus;

                try
                {
                    var (success, updated, error) = await _pedidosService.UpdateStatusAsync(id, novoStatus);
                    if (!success)
                    {
                        MessageBox.Show($"Erro ao atualizar status: {error}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        await CarregarDadosAsync();
                    }
                    else if (updated != null)
                    {
                        var idx = _todosPedidos.FindIndex(p => p.Id == id);
                        if (idx >= 0) _todosPedidos[idx] = updated;
                    }
                }
                catch
                {
                    var (success, updated, error) = await _pedidosService.UpdateAsync(id, pedido);
                    if (!success)
                    {
                        MessageBox.Show($"Erro ao atualizar pedido: {error}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        await CarregarDadosAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao processar alteração de status: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await CarregarDadosAsync();
            }
            finally
            {
                // Reativa o evento após concluir
                gridPedidos.CellValueChanged += GridPedidos_CellValueChanged;
            }
        }

        //=================================================
        // BOTÕES
        //=================================================

        private void btnDetalhes_Click(object sender, EventArgs e)
        {
            if (gridPedidos.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Selecione um pedido para ver os detalhes.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            var row = gridPedidos.SelectedRows[0];
            var id = Convert.ToInt32(row.Cells["colId"].Value);

            var pedido = _todosPedidos.FirstOrDefault(p => p.Id == id);

            if (pedido == null)
            {
                MessageBox.Show(
                    "Pedido não encontrado.",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            using var detalhesForm = new DetalhesPedidosForm(pedido);

            ThemeManager.ApplyTheme(
                detalhesForm,
                animate: false);

            // Este método já abre o formulário com ShowDialog().
            FormAnimator.ShowDialogWithSlideFade(
                detalhesForm,
                this,
                360,
                40);
        }

        private async void btnAtualizar_Click(object sender, EventArgs e) => await CarregarDadosAsync();

        private void txtPesquisa_TextChanged(object sender, EventArgs e) => FiltrarPedidos();

        // Abre o dropdown do combo quando o usuário clica na célula (works around problemas de tema/flat style)
        private void GridPedidos_CellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (gridPedidos.Columns[e.ColumnIndex].Name != "colStatus") return;

            // torna a célula atual e inicia edição
            gridPedidos.CurrentCell = gridPedidos.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (!gridPedidos.CurrentCell.IsInEditMode)
            {
                gridPedidos.BeginEdit(true);
            }

            // Se o controle de edição for o Combo, abre o dropdown
            if (gridPedidos.EditingControl is DataGridViewComboBoxEditingControl cbEditing)
            {
                cbEditing.DroppedDown = true;
            }
            else if (gridPedidos.EditingControl is ComboBox combo)
            {
                combo.DroppedDown = true;
            }
        }
    }
}
