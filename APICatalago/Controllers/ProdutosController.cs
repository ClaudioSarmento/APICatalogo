using APICatalago.Domain.Entities;
using APICatalago.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICatalago.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext? _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context?.Produtos?.ToList();
            if (produtos is null) return NotFound("Produtos não encontrados...");
            return produtos;
        }

        [HttpGet("{id:int}")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context?.Produtos?.FirstOrDefault(p => p.Id == id);
            if (produto is null) return NotFound($"Produto {id} não encontrado...");
            return produto;
        }
    }
}
