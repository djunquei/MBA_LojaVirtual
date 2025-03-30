using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LojaVirtual.Data.Model
{
    public class Vendedor
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O último nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O último nome deve ter no máximo 100 caracteres")]
        public string UltimoNome { get; set; }

        [Required(ErrorMessage = "O sexo é obrigatório")]
        [StringLength(1, ErrorMessage = "O sexo deve ter no máximo 1 caracter")]
        public string Sexo { get; set; }


        public IEnumerable<Produto> Produtos { get; set; }

    }
}
