using APICatalago.Domain.Entities;
using APICatalago.Infrastructure.Data.Context;
using APICatalago.Pagination;
using APICatalago.Repositories.Interfaces;

namespace APICatalago.Repositories;

public class ProdutoRespository : Repository<Produto>, IProdutoRepository
{
  
    public ProdutoRespository(AppDbContext context) : base(context) { }

    public PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters)
    {
        var produtos = GetAll()
            .OrderBy(p => p.Id)
            .AsQueryable();
        var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParameters.PageNumber, produtosParameters.PageSize);
        return produtosOrdenados;
    }   

    public IEnumerable<Produto> GetProdutosPorCategoria(int idCategoria)
    {
        return GetAll().Where(c => c.CategoriaId == idCategoria);
    }
}
