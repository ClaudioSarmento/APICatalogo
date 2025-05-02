using APICatalago.Domain.Entities;
using APICatalago.Infrastructure.Data.Context;
using APICatalago.Repositories.Interfaces;

namespace APICatalago.Repositories;

public class ProdutoRespository : Repository<Produto>, IProdutoRepository
{
  
    public ProdutoRespository(AppDbContext context) : base(context) { }

    public IEnumerable<Produto> GetProdutosPorCategoria(int idCategoria)
    {
        return GetAll().Where(c => c.CategoriaId == idCategoria);
    }
}
