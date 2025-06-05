using APICatalago.Controllers;
using APICatalago.DTOs;
using ApiCatalagoxUnitTests.UnitTests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoxUnitTests.UnitTests
{
    public class PostProdutosUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public PostProdutosUnitTests(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller._repository, controller.mapper);
        }

        [Fact]
        public async Task PostProduto_Return_CreatedAtRouteResult()
        {
            // Arrange
            var product = new ProdutoDTO() 
            {
                Nome = "Novo Produto",
                Descricao = "Descrição do Novo Produto",
                Preco = 10.99m,
                ImagemUrl = "imagemfake1.jpg",
                CategoriaId = 2
            };

            // Act
            var data = await _controller.Post(product);

            //data.Should().BeOfType<CreatedAtRouteResult>()
            //    .Which
            //    .StatusCode
            //    .Should()
            //    .Be(201);

            // Assert
            var createdResult = data.Result.Should().BeOfType<CreatedAtRouteResult>();
            createdResult.Subject.StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task PostProduto_Return_BadRequest()
        {
            // Arrange
            ProdutoDTO product = null!;
            // Act
            var data = await _controller.Post(product!);

            //data.Should().BeOfType<CreatedAtRouteResult>()
            //    .Which
            //    .StatusCode
            //    .Should()
            //    .Be(201);

            // Assert
            var badRequestResult = data.Result.Should().BeOfType<BadRequestResult>();
            badRequestResult.Subject.StatusCode.Should().Be(400);

        }
    }
}
