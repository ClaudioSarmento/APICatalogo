using APICatalago.Domain.Entities;
using APICatalago.DTOs;
using APICatalago.DTOs.Mappings;
using APICatalago.Filters;
using APICatalago.Pagination;
using APICatalago.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            var categoriasDto = categorias.ToCategoriaDTOList();
            return Ok(categoriasDto);

        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            var categoriasDto = categorias.ToCategoriaDTOList();
            return Ok(categoriasDto);
        }

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {

            var categorias = _unitOfWork.CategoriaRepository.GetCategorias(categoriasParameters);
            return ObterCategorias(categorias);

        }

        [HttpGet("filter/nome/pagination")]
        public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasFiltroNome categoriasFiltroNome)
        {

            var categorias = _unitOfWork.CategoriaRepository.GetCategoriasFiltroNome(categoriasFiltroNome);
            return ObterCategorias(categorias);

        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.Get(c => c.Id == id);
            if (categoria is null) return NotFound($"Categoria {id} não encontrada");
            var categoriaDto = categoria.ToCategoriaDTO();
            return categoriaDto!;

        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post([FromBody] CategoriaDTO categoriaDto)
        {

            if (categoriaDto is null) return BadRequest("Dados inválidos");
            var categoria = categoriaDto.ToCategoria();
            var categoriaPost = _unitOfWork.CategoriaRepository.Add(categoria!);
            _unitOfWork.Commit();
            var categoriaDtoPost = categoriaPost.ToCategoriaDTO();
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaDtoPost!.Id });

        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult<CategoriaDTO> Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {

            if (id != categoriaDto.Id) return BadRequest("Dados inválidos");
            var categoria = categoriaDto.ToCategoria();
            var categoriaUpdate = _unitOfWork.CategoriaRepository.Update(categoria!);
            _unitOfWork.Commit();
            var categoriaDtoUpdate = categoriaUpdate.ToCategoriaDTO();
            return Ok(categoriaDtoUpdate);

        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.Get(c =>c.Id == id);
            if (categoria is null) return NotFound($"Categoria {id} não encontrada");
            var categoriaDeletada = _unitOfWork.CategoriaRepository.Delete(categoria);
            _unitOfWork.Commit();
            var categoriaDto = categoriaDeletada.ToCategoriaDTO();
            return Ok(categoriaDto);

        }

    }
}
