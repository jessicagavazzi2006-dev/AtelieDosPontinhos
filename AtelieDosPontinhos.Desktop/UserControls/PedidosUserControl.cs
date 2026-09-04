using AtelieDosPontinhos.Desktop.DTOs;
using AtelieDosPontinhos.Desktop.Forms;
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

            AtelieDosPontinhosTheme.AplicarEstiloGrid(gridPedidos);

            ConfigurarGrid();

            // handlers para evitar a caixa de diálogo padrão e para commit imediato do combo
            gridPedidos.DataError += GridPedidos_DataError;
            gridPedidos.CurrentCellDirtyStateChanged += GridPedidos_CurrentCellDirtyStateChanged;
            gridPedidos.CellValueChanged += GridPedidos_CellValueChanged;

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
                _userMap = usuarios.ToDictionary(u => u.Id, u => u.UserName);
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

                gridPedidos.Rows.Add(
                    p.Id,
                    userName,
                    p.DataPedido.ToString("dd/MM/yyyy"),
                    localidade,
                    pagamento,
                    p.ValorTotal.ToString("C"),
                    status
                );
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

        private void ConfigurarGrid()
        {
            gridPedidos.Rows.Clear();
            gridPedidos.Columns.Clear();
            gridPedidos.AutoGenerateColumns = false;
            gridPedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridPedidos.MultiSelect = false;
            gridPedidos.AllowUserToAddRows = false;

            gridPedidos.ReadOnly = false;

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
                FlatStyle = FlatStyle.Flat,
                ValueType = typeof(string)
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

            try
            {
                var row = gridPedidos.Rows[e.RowIndex];
                var id = Convert.ToInt32(row.Cells["colId"].Value);
                var novoStatus = row.Cells["colStatus"].Value?.ToString() ?? "Pendente";

                var pedido = _todosPedidos.FirstOrDefault(x => x.Id == id);
                if (pedido == null) return;

                if (pedido.Status == novoStatus) return;

                pedido.Status = novoStatus;

                // tenta usar o endpoint específico de status quando disponível
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
                    // fallback para UpdateAsync caso UpdateStatusAsync não exista
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
        }

        //=================================================
        // BOTÕES
        //=================================================

        private void btnDetalhes_Click(object sender, EventArgs e)
        {
            if (gridPedidos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um pedido para ver os detalhes.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var row = gridPedidos.SelectedRows[0];
            var id = Convert.ToInt32(row.Cells["colId"].Value);
            var pedido = _todosPedidos.FirstOrDefault(p => p.Id == id);
            if (pedido == null)
            {
                MessageBox.Show("Pedido não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using var detalhesForm = new DetalhesPedidosForm(pedido);
            detalhesForm.ShowDialog();
        }

        private async void btnAtualizar_Click(object sender, EventArgs e) => await CarregarDadosAsync();
    }
}
