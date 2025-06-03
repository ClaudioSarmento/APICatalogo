using APICatalago.Validations;
using System.ComponentModel.DataAnnotations;

namespace APICatalago.DTOs;

public class ProdutoDTO
{
    public int Id { get; set; }
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(80)]
    [PrimeiraLetraMaiuscula]
    public string? Nome { get; set; }
    [Required(ErrorMessage = "A descricao é obrigatória")]
    [StringLength(300)]
    public string? Descricao { get; set; }
    [Required(ErrorMessage = "O preco é obrigatório")]
    public decimal Preco { get; set; }
    [Required(ErrorMessage = "A imagemUrl é obrigatória")]
    [StringLength(300, ErrorMessage = "A imagemUrl deve ter no máximo {1} caracteres")]
    public string? ImagemUrl { get; set; }
    public int CategoriaId { get; set; }
    
}
