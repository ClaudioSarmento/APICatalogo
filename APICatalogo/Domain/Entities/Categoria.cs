using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalago.Domain.Entities;

[Table("Categorias")]
public class Categoria
{
    public Categoria() 
    {
        Produtos = new Collection<Produto>();
    }
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(80,ErrorMessage = "O nome deve ter no máximo {1} caracteres")]
    public string? Nome { get; set; }
    [Required]
    [StringLength(300, ErrorMessage = "A imagemUrl deve ter no máximo {1} caracteres")]
    public string? ImagemUrl { get; set; }
    [JsonIgnore]
    public ICollection<Produto>? Produtos { get; set; }
}
