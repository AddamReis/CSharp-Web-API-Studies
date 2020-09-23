using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_Catalogo.Validations;

namespace WebAPI_Catalogo.Models
{
    [Table("Produtos")]
    public class Produto : IValidatableObject
    {
        [Key]
        public int ProdutoId { get; set; }
        //[Required]
        //[MaxLength(100)]
        //[PrimeiraLetraMaiuscula]
        public string Nome { get; set; }
        [Required]
        [MaxLength(500)]
        public string Descricao { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName ="decimal(8,2)")]
        [Range(1, 10000, ErrorMessage ="O preco deve estar entre {1} e {2}")]
        public decimal Preco { get; set; }
        [Required]
        [MaxLength(500)]
        public string ImagemUrl { get; set; }
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }

        public Categoria Categoria { get; set; }
        public int CategoriaId { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(this.Nome))
            {
                var primeiraLetra = this.Nome[0].ToString();
                if (primeiraLetra != primeiraLetra.ToUpper())
                {
                    yield return new ValidationResult("A primeira letra do nome do produto deve ser maiúscula", new[] { nameof(this.Nome) });
                }
            }
            if(this.Estoque <= 0)
            {
                yield return new ValidationResult("O estoque deve ser maior que zero", new[] { nameof(this.Estoque) });
            }
        }
    }
}
