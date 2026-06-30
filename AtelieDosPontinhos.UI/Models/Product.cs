using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeuProjeto.Models
{
    [Table("Produtos")] // Define o nome da tabela que será criada no banco de dados
    public class Product
    {
        [Key] // Define que o Id é a Chave Primária no banco
        public int Id { get; set; }

        [Required]
        [StringLength(100)] // Evita que o banco crie um campo de texto infinito (NVARCHAR(MAX))
        public string Nome { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")] // Garante que o banco salve o preço com duas casas decimais
        public decimal Preco { get; set; }

        public string Descricao { get; set; }

        public string Imagem { get; set; }

        [Required]
        public string Categoria { get; set; }
    }
}