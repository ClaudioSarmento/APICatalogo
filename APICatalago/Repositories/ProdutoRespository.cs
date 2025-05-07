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

    public PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroPreco)
    {
        var produtos = GetAll().AsQueryable();
        if (produtosFiltroPreco.Preco.HasValue &&
            !string.IsNullOrWhiteSpace(produtosFiltroPreco.PrecoCriterio))
        {
            var preco = produtosFiltroPreco.Preco.Value;
            produtos = produtosFiltroPreco.PrecoCriterio.ToLowerInvariant() switch
            {
                "maior" => produtos.Where(p => p.Preco > preco),
                "menor" => produtos.Where(p => p.Preco < preco),
                "igual" => produtos.Where(p => p.Preco == preco),
                _ => produtos
            };
        }
        produtos = produtos.OrderBy(p => p.Preco);
        var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, produtosFiltroPreco.PageNumber, produtosFiltroPreco.PageSize);
        return produtosFiltrados;

    }

    public IEnumerable<Produto> GetProdutosPorCategoria(int idCategoria)
    {
        return GetAll().Where(c => c.CategoriaId == idCategoria);
    }
}
