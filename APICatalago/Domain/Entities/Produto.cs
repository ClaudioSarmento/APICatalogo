using APICatalago.Validations;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalago.Domain.Entities;

[Table("Produtos")]
public class Produto : IValidatableObject
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(80)]
    [PrimeiraLetraMaiuscula]
    public string? Nome { get; set; }
    [Required(ErrorMessage = "A descricao é obrigatória")]
    [StringLength(300)]
    public string? Descricao { get; set; }
    [Required(ErrorMessage = "O preco é obrigatório")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco { get; set; }
    [Required(ErrorMessage = "A imagemUrl é obrigatória")]
    [StringLength(300, ErrorMessage = "A imagemUrl deve ter no máximo {1} caracteres")]
    public string? ImagemUrl { get; set; }
    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }
    public int CategoriaId { get; set; }
    [JsonIgnore]
    public Categoria? Categoria { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrWhiteSpace(Nome)) 
        {
            var primeiraLetra = this.Nome[0].ToString();
            if (primeiraLetra != primeiraLetra.ToUpper())
            {
                yield return new ValidationResult("A primeira letra do produto deve ser maiúscula", new[] {nameof(Nome)});
            }

            if(Estoque <= 0)
            {
                yield return new ValidationResult("O estoque deve ser maior que zero", new[] { nameof(Estoque) });
            }
        }
    }
}
