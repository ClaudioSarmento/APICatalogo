using APICatalago.Domain.Entities;
using APICatalago.DTOs;
using APICatalago.Infrastructure.Mocks;
using APICatalago.Pagination;
using APICatalago.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalago.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ProdutosController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;   
            _mapper = mapper;
        }

        private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(PagedList<Produto> produtos)
        {
            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDto);
        }

        [HttpGet("produtos/{categoriaId}")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosCategoriaAsync(int categoriaId)
        {
            var produtos = await _unitOfWork.ProdutoRepository.GetProdutosPorCategoriaAsync(categoriaId);
            if (produtos is null) return NotFound();
            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetAsync()
        {

           
            var produtos = await _unitOfWork.ProdutoRepository.GetAllAsync();
            if (!produtos.Any()) return NotFound("Produtos não encontrados...");
            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDto);

        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> GetAsync(int id)
        {

           
            var produto = _unitOfWork.ProdutoRepository.GetAsync(p => p.Id == id);
            if (produto is null) return NotFound($"Produto {id} não encontrado...");
            var produtoDto = _mapper.Map<ProdutoDTO>(produto);
            return Ok(produtoDto);

        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetAsync([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = await _unitOfWork.ProdutoRepository.GetProdutosAsync(produtosParameters);
            return ObterProdutos(produtos);
        }

        [HttpGet("filter/preco/pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetAsync([FromQuery] ProdutosFiltroPreco produtosFiltroPreco)
        {
            var produtos = await _unitOfWork.ProdutoRepository.GetProdutosFiltroPrecoAsync(produtosFiltroPreco);
            return ObterProdutos(produtos);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProdutoDTO produtoDto)
        {

            if (produtoDto is null) return BadRequest();
            var produto = _mapper.Map<Produto>(produtoDto);
            var produtoCriado = _unitOfWork.ProdutoRepository.Add(produto);
            await _unitOfWork.CommitAsync();
            var produtoDtoCriado = _mapper.Map<ProdutoDTO>(produtoCriado);
            return new CreatedAtRouteResult("ObterProduto", new { id = produtoDtoCriado.Id }, produtoDtoCriado);
           
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProdutoDTO produtoDto)
        {

            if (id != produtoDto.Id) return BadRequest();
            var produto = _mapper.Map<Produto>(produtoDto);
            var produtoAlterado = _unitOfWork.ProdutoRepository.Update(produto);
            await _unitOfWork.CommitAsync();
            var produtoAlteradoDto = _mapper.Map<ProdutoDTO>(produtoAlterado);
            return Ok(produtoAlteradoDto);

        }

        [HttpPatch("{id}/UpdatePartial")]
        public async Task<ActionResult<ProdutoDTOUpdateResponse>> PatchAsync(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDto)
        {
            if (patchProdutoDto is null || id <= 0) return BadRequest();
            var produto = await _unitOfWork.ProdutoRepository.GetAsync(p =>  p.Id == id);

            if (produto is null) return NotFound();

            var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

            patchProdutoDto.ApplyTo(produtoUpdateRequest, ModelState);
            if(!ModelState.IsValid || !TryValidateModel(produtoUpdateRequest)) 
                return BadRequest(ModelState);
            _mapper.Map(produtoUpdateRequest, produto);
            _unitOfWork.ProdutoRepository.Update(produto);
            await _unitOfWork.CommitAsync();
            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
        }


        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var produto = await _unitOfWork.ProdutoRepository.GetAsync(p => p.Id == id);
            if (produto is null) return NotFound($"Produto {id} não encontrada");
            var produtoDeletado = _unitOfWork.ProdutoRepository.Delete(produto);
            await _unitOfWork.CommitAsync();
            var produtoDeletadoDto = _mapper.Map<ProdutoDTO>(produtoDeletado);
            return Ok(produtoDeletadoDto);

        }
        
    }
}
