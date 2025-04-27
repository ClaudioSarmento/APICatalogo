using APICatalago.Domain.Entities;
using APICatalago.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext? _context;

        public CategoriasController(AppDbContext? context)
        {
            _context = context;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            var categoriaProdutos = _context?.Categorias?.AsNoTracking().Include(p => p.Produtos).ToList();
            if (categoriaProdutos is null) return NotFound("CategoriasProdutos não encontrado...");
            return categoriaProdutos;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _context?.Categorias?.AsNoTracking().ToList();
            if(categorias is null) return NotFound("Categorias não encontradas...");
            return categorias;
        }

        [HttpGet("{id:int}", Name="ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _context?.Categorias?
                .AsNoTracking().FirstOrDefault(c => c.Id == id);
            if (categoria is null) return NotFound($"Categoria {id} não encontrada");
            return categoria;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            if (categoria is null) return BadRequest();
            _context?.Categorias?.Add(categoria);
            _context?.SaveChanges();
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.Id });
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.Id || _context == null) return BadRequest();
            _context.Entry(categoria).State = EntityState.Modified;
            _context?.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context?.Produtos?.FirstOrDefault(p => p.Id == id);
            if (produto is null || _context == null) return NotFound("Categoria não localizada...");
            _context.Produtos?.Remove(produto);
            _context?.SaveChanges();
            return Ok(produto);
        }
    }
}
