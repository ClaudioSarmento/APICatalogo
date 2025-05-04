using APICatalago.Domain.Entities;
using APICatalago.DTOs;
using APICatalago.Filters;
using APICatalago.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APICatalago.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriasController(IUnitOfWork unitOfWork)
        {
          _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {

            var categorias = _unitOfWork.CategoriaRepository.GetAll();
            var categoriasDto = categorias
                .Select(
                    c => new CategoriaDTO
                    {
                        Id = c.Id,
                        Nome = c.Nome,
                        ImagemUrl = c.ImagemUrl,
                    }
                );
            return Ok(categoriasDto);

        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.Get(c => c.Id == id);
            if (categoria is null) return NotFound($"Categoria {id} não encontrada");

            var categoriaDto = new CategoriaDTO()
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
                
            };
            return categoriaDto;

        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post([FromBody] CategoriaDTO categoriaDto)
        {

            if (categoriaDto is null) return BadRequest("Dados inválidos");
            var categoria = new Categoria()
            {
                Id = categoriaDto.Id,
                Nome = categoriaDto.Nome,
                ImagemUrl = categoriaDto.ImagemUrl
            };
            var categoriaPost = _unitOfWork.CategoriaRepository.Add(categoria);
            _unitOfWork.Commit();
            var categoriaDtoPost = new CategoriaDTO()
            {
                Id = categoriaPost.Id,
                Nome = categoriaPost.Nome,
                ImagemUrl = categoriaPost.ImagemUrl

            };
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaDtoPost.Id });

        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult<CategoriaDTO> Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {

            if (id != categoriaDto.Id) return BadRequest("Dados inválidos");
            var categoria = new Categoria()
            {
                Id = categoriaDto.Id,
                Nome = categoriaDto.Nome,
                ImagemUrl = categoriaDto.ImagemUrl
            };
            var categoriaUpdate = _unitOfWork.CategoriaRepository.Update(categoria);
            _unitOfWork.Commit();
            var categoriaDtoUpdate = new CategoriaDTO()
            {
                Id = categoriaUpdate.Id,
                Nome = categoriaUpdate.Nome,
                ImagemUrl = categoriaUpdate.ImagemUrl

            };
            return Ok(categoriaDtoUpdate);

        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.Get(c =>c.Id == id);
            if (categoria is null) return NotFound($"Categoria {id} não encontrada");
            var categoriaDeletada = _unitOfWork.CategoriaRepository.Delete(categoria);
            _unitOfWork.Commit();
            var categoriaDto = new CategoriaDTO()
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl

            };
            return Ok(categoriaDto);

        }

    }
}
