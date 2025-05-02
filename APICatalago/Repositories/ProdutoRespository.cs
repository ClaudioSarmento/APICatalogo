using APICatalago.Domain.Entities;
using APICatalago.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Repositories
{
    public class ProdutoRespository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRespository(AppDbContext context)
        {
            _context = context;
        }

        public Produto Create(Produto produto)
        {
            if (produto is null) throw new ArgumentNullException(nameof(produto));
            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return produto;
        }

        public Produto Delete(int id)
        {
            var produto = _context.Produtos.Find(id);
            if (produto is null) throw new KeyNotFoundException($"Categoria com ID {id} não encontrada.");
            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return produto;
        }

        public Produto GetProduto(int id)
        {
            var produtos = _context.Produtos.FirstOrDefault(c => c.Id == id);
            return produtos;
        }

        public IEnumerable<Produto> GetProdutos()
        {
            var produtos = _context.Produtos.ToList();
            return produtos;
        }

        public Produto Update(Produto produto)
        {
            if (produto is null) throw new ArgumentNullException(nameof(produto));
            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();
            return produto;
        }
    }
}
