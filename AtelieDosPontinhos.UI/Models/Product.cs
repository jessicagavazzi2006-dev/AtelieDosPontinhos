using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtelieDosPontinhos.UI.Models // Ou o namespace correspondente ao seu projeto
{
    [Table("Produtos")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Preco { get; set; }

        public string Descricao { get; set; }

        public string Imagem { get; set; }

        [Required]
        public string Categoria { get; set; }

        // Adicione esta linha abaixo para corrigir o erro:
        public bool Destaque { get; set; }
    }
}