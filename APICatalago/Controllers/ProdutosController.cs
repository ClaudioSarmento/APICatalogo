using APICatalago.Domain.Entities;
using APICatalago.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        {

            if (_context?.Produtos is null) return NotFound("Produtos não encontrados...");
            var produtos = await _context.Produtos.AsNoTracking().ToListAsync();
            if (!produtos.Any()) return NotFound("Produtos não encontrados...");
            return produtos;

        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> GetAsync(int id)
        {

            if (_context?.Produtos is null) return NotFound("Produtos não encontrados...");
            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (produto is null) return NotFound($"Produto {id} não encontrado...");
            return produto;

        }

        [HttpPost]
        public ActionResult Post([FromBody] Produto produto)
        {

            if (produto is null) return BadRequest();
            _context?.Produtos?.Add(produto);
            _context?.SaveChanges();
            return new CreatedAtRouteResult("ObterProduto", new { id = produto.Id }, produto);

        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {

            if (id != produto.Id || _context == null) return BadRequest();
            _context.Entry(produto).State = EntityState.Modified;
            _context?.SaveChanges();
            return Ok(produto);

        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult Delete(int id)
        {

            var produto = _context?.Produtos?.FirstOrDefault(p => p.Id == id);
            if (produto is null || _context == null) return NotFound("Produto não localizado...");
            _context.Produtos?.Remove(produto);
            _context?.SaveChanges();
            return Ok(produto);

        }
    }
}
