using APICatalago.Controllers;
using APICatalago.DTOs;
using ApiCatalagoxUnitTests.UnitTests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoxUnitTests.UnitTests
{
    public class PutProdutosUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;
        public PutProdutosUnitTests(ProdutosUnitTestController controller) 
        {
            _controller = new ProdutosController(controller._repository, controller.mapper);
        }

        [Fact]
        public async Task PutProduto_Return_OkResult()
        {
            // Arrange
            var product = new ProdutoDTO()
            {
                Nome = "Novo Produto 2",
                Descricao = "Descrição do Novo Produto",
                Preco = 10.99m,
                ImagemUrl = "imagemfake1.jpg",
                CategoriaId = 2,
                Id = 683
            };

            // Act
            var data = await _controller.Put(683,product);

            // Assert
            var okResult = data.Should().BeOfType<OkObjectResult>();
            okResult.Subject.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task PutProduto_Return_BadRequest()
        {
            // Arrange
            var product = new ProdutoDTO()
            {
                Nome = "Novo Produto 2",
                Descricao = "Descrição do Novo Produto",
                Preco = 10.99m,
                ImagemUrl = "imagemfake1.jpg",
                CategoriaId = 2,
                Id = 65
            };

            // Act
            var data = await _controller.Put(683, product);

            // Assert
            var badRequestResult = data.Should().BeOfType<BadRequestResult>();
            badRequestResult.Subject.StatusCode.Should().Be(400);

        }
    }
}
