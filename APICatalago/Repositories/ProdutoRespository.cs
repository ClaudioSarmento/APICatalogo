using APICatalago.Domain.Entities;
using APICatalago.Infrastructure.Data.Context;
using APICatalago.Pagination;
using APICatalago.Repositories.Interfaces;

namespace APICatalago.Repositories;

public class ProdutoRespository : Repository<Produto>, IProdutoRepository
{
  
    public ProdutoRespository(AppDbContext context) : base(context) { }

    public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters)
    {
        return GetAll()
            .OrderBy(p => p.Nome)
            .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
            .Take(produtosParameters.PageSize)
            .ToList();
    }   

    public IEnumerable<Produto> GetProdutosPorCategoria(int idCategoria)
    {
        return GetAll().Where(c => c.CategoriaId == idCategoria);
    }
}
