using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LojaVirtual.Data.Model
{
    public class Produto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "A imagem é obrigatória")]
        [StringLength(500, ErrorMessage = "A imagem deve ter no máximo 500 caracteres")]
        public string Imagem { get; set; }

        [ForeignKey("Categoria")]
        [Required(ErrorMessage = "A categoria é obrigatória")]
        public int CategoriaId { get; set; }

        [ForeignKey("Vendedor")]
        [Required(ErrorMessage = "O vendedor é obrigatória")]
        public Guid VendedorId { get; set; }


        public Categoria Categoria { get; set; }
        public Vendedor Vendedor { get; set; }

    }
}
