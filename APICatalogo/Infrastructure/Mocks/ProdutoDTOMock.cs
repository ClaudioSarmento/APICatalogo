using APICatalago.Domain.Entities;
using APICatalago.DTOs;
using Bogus;

namespace APICatalago.Infrastructure.Mocks
{
    public class ProdutoDTOMock
    {
        public static ProdutoDTO GerarProdutoDTO()
        {
            var faker = new Faker<ProdutoDTO>("pt_BR")
                .RuleFor(p => p.Nome, f =>
                {
                    var nome = f.Commerce.ProductName();
                    return char.ToUpper(nome[0]) + nome.Substring(1); // Primeira letra maiúscula
                })
                .RuleFor(p => p.Descricao, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Preco, f => f.Random.Decimal(1, 1000))
                .RuleFor(p => p.ImagemUrl, f => f.Image.PicsumUrl())
                .RuleFor(p => p.CategoriaId, f => f.Random.Int(1, 3));

            return faker.Generate();
        }

        public static List<ProdutoDTO> GerarListaProdutoDTO(int quantidade)
        {
            var faker = new Faker<ProdutoDTO>("pt_BR")
                .RuleFor(p => p.Nome, f =>
                {
                    var nome = f.Commerce.ProductName();
                    return char.ToUpper(nome[0]) + nome.Substring(1); // Primeira letra maiúscula
                })
                .RuleFor(p => p.Descricao, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Preco, f => f.Random.Decimal(1, 1000))
                .RuleFor(p => p.ImagemUrl, f => f.Image.PicsumUrl())
                .RuleFor(p => p.CategoriaId, f => f.Random.Int(1, 3));

            return faker.Generate(quantidade);
        }
    }
}
