using APICatalago.Controllers;
using ApiCatalagoxUnitTests.UnitTests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests.UnitTests
{
    public class GetProdutosUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;
        public GetProdutosUnitTests(ProdutosUnitTestController controller) 
        {
            _controller = new ProdutosController(controller._repository, controller.mapper);
        }

        [Fact]
        public async Task GetProdutoById_Return_OkResult() 
        {
            //Arrange
            var prodId = 2;

            //Act
            var data = await _controller.GetAsync(prodId);

            //Assert (xunit)
            //var okResult = Assert.IsType<OkObjectResult>(data.Result);
            //Assert.Equal(200, okResult.StatusCode);

            //Assert (fluentassertions)
            data.Result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200); 

        }
        [Fact]
        public async Task GetProdutoById_Return_NotFound() { }
        [Fact]
        public async Task GetProdutoById_Return_BadRequest() { }
        [Fact]
        public async Task GetProdutos_Return_ListOfProdutoDTO() { }
        [Fact]
        public async Task GetProdutos_Return_BadRequestResult() { }
    }
}
