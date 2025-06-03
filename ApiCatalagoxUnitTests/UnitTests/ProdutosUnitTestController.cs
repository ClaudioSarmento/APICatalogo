using APICatalago.DTOs.Mappings;
using APICatalago.Infrastructure.Data.Context;
using APICatalago.Repositories;
using APICatalago.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ApiCatalagoxUnitTests.UnitTests;

public class ProdutosUnitTestController
{
    public IUnitOfWork _repository;
    public IMapper mapper;
    public static DbContextOptions<AppDbContext> dbContextOptions { get; }
    public static readonly string connectionString;

    static ProdutosUnitTestController()
    {
        var config = ConfigHelper.LoadConfiguration();
        connectionString = config.GetConnectionString("DefaultConnection")!;

        dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .Options;
    }

    public ProdutosUnitTestController()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ProdutoDTOMappingProfile());
        });

        mapper = config.CreateMapper();
        var context = new AppDbContext(dbContextOptions);
        _repository = new UnitOfWork(context);
    }
}
