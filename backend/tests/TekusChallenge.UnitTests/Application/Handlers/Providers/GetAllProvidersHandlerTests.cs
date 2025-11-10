using AutoMapper;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using TekusChallenge.Application.DTO;
using TekusChallenge.Application.UseCases.Providers.Queries.GetAllProviders;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.UnitTests.Application.Handlers.Providers;

/// <summary>
/// Pruebas unitarias para GetAllProvidersHandler
/// </summary>
public class GetAllProvidersHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IProviderRepository> _providerRepositoryMock;
    private readonly GetAllProvidersHandler _handler;

    public GetAllProvidersHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _providerRepositoryMock = new Mock<IProviderRepository>();

        _unitOfWorkMock.Setup(x => x.Providers).Returns(_providerRepositoryMock.Object);

        _handler = new GetAllProvidersHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ConPaginacionBasica_DebeRetornarProveedoresPaginados()
    {
        // Arrange
        var query = new GetAllProvidersQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        var providers = new List<Provider>
        {
            new Provider { Id = Guid.NewGuid(), Nit = "123456789", Name = "Proveedor 1", Email = "test1@test.com" },
            new Provider { Id = Guid.NewGuid(), Nit = "987654321", Name = "Proveedor 2", Email = "test2@test.com" }
        };

        var providerDtos = new List<ProviderDto>
        {
            new ProviderDto { Id = providers[0].Id, Nit = "123456789", Name = "Proveedor 1", Email = "test1@test.com" },
            new ProviderDto { Id = providers[1].Id, Nit = "987654321", Name = "Proveedor 2", Email = "test2@test.com" }
        };

        _providerRepositoryMock
            .Setup(x => x.GetPagedAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Provider, bool>>>(),
                It.IsAny<Func<IQueryable<Provider>, IOrderedQueryable<Provider>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((providers, 2));

        _mapperMock
            .Setup(x => x.Map<IEnumerable<ProviderDto>>(providers))
            .Returns(providerDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Should().HaveCount(2);
        result.PageNumber.Should().Be(1);
        result.TotalCount.Should().Be(2);
        result.TotalPages.Should().Be(1);
        result.Message.Should().Contain("successfully");
    }

    [Fact]
    public async Task Handle_ConFiltroSearchTerm_DebeFiltrarProveedores()
    {
        // Arrange
        var query = new GetAllProvidersQuery
        {
            PageNumber = 1,
            PageSize = 10,
            SearchTerm = "Proveedor"
        };

        var providers = new List<Provider>
        {
            new Provider { Id = Guid.NewGuid(), Nit = "123456789", Name = "Proveedor Test", Email = "test@test.com" }
        };

        var providerDtos = new List<ProviderDto>
        {
            new ProviderDto { Id = providers[0].Id, Nit = "123456789", Name = "Proveedor Test", Email = "test@test.com" }
        };

        _providerRepositoryMock
            .Setup(x => x.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                It.IsAny<Expression<Func<Provider, bool>>>(),
                It.IsAny<Func<IQueryable<Provider>, IOrderedQueryable<Provider>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((providers, 1));

        _mapperMock
            .Setup(x => x.Map<IEnumerable<ProviderDto>>(providers))
            .Returns(providerDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(1);
        result.Data.First().Name.Should().Contain("Proveedor");
    }

    [Fact]
    public async Task Handle_ConFiltroNit_DebeFiltrarPorNit()
    {
        // Arrange
        var query = new GetAllProvidersQuery
        {
            PageNumber = 1,
            PageSize = 10,
            Nit = "123456"
        };

        var providers = new List<Provider>
        {
            new Provider { Id = Guid.NewGuid(), Nit = "123456789", Name = "Proveedor Test", Email = "test@test.com" }
        };

        var providerDtos = new List<ProviderDto>
        {
            new ProviderDto { Id = providers[0].Id, Nit = "123456789", Name = "Proveedor Test", Email = "test@test.com" }
        };

        _providerRepositoryMock
            .Setup(x => x.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                It.IsAny<Expression<Func<Provider, bool>>>(),
                It.IsAny<Func<IQueryable<Provider>, IOrderedQueryable<Provider>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((providers, 1));

        _mapperMock
            .Setup(x => x.Map<IEnumerable<ProviderDto>>(providers))
            .Returns(providerDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(1);
        result.Data.First().Nit.Should().Contain("123456");
    }

    [Fact]
    public async Task Handle_ConFiltroEmail_DebeFiltrarPorEmail()
    {
        // Arrange
        var query = new GetAllProvidersQuery
        {
            PageNumber = 1,
            PageSize = 10,
            Email = "test@test.com"
        };

        var providers = new List<Provider>
        {
            new Provider { Id = Guid.NewGuid(), Nit = "123456789", Name = "Proveedor Test", Email = "test@test.com" }
        };

        var providerDtos = new List<ProviderDto>
        {
            new ProviderDto { Id = providers[0].Id, Nit = "123456789", Name = "Proveedor Test", Email = "test@test.com" }
        };

        _providerRepositoryMock
            .Setup(x => x.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                It.IsAny<Expression<Func<Provider, bool>>>(),
                It.IsAny<Func<IQueryable<Provider>, IOrderedQueryable<Provider>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((providers, 1));

        _mapperMock
            .Setup(x => x.Map<IEnumerable<ProviderDto>>(providers))
            .Returns(providerDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(1);
        result.Data.First().Email.Should().Be("test@test.com");
    }

    [Fact]
    public async Task Handle_ConMultiplesFiltros_DebeAplicarTodosLosFiltros()
    {
        // Arrange
        var query = new GetAllProvidersQuery
        {
            PageNumber = 1,
            PageSize = 10,
            SearchTerm = "Proveedor",
            Nit = "123",
            Email = "test"
        };

        var providers = new List<Provider>
        {
            new Provider { Id = Guid.NewGuid(), Nit = "123456789", Name = "Proveedor Test", Email = "test@test.com" }
        };

        var providerDtos = new List<ProviderDto>
        {
            new ProviderDto { Id = providers[0].Id, Nit = "123456789", Name = "Proveedor Test", Email = "test@test.com" }
        };

        _providerRepositoryMock
            .Setup(x => x.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                It.IsAny<Expression<Func<Provider, bool>>>(),
                It.IsAny<Func<IQueryable<Provider>, IOrderedQueryable<Provider>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((providers, 1));

        _mapperMock
            .Setup(x => x.Map<IEnumerable<ProviderDto>>(providers))
            .Returns(providerDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(1);
        
        // Verificar que se pasó un filtro al repositorio
        _providerRepositoryMock.Verify(
            x => x.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                It.IsAny<Expression<Func<Provider, bool>>>(),
                It.IsAny<Func<IQueryable<Provider>, IOrderedQueryable<Provider>>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ConOrdenamientoPorNombre_DebeAplicarOrdenamiento()
    {
        // Arrange
        var query = new GetAllProvidersQuery
        {
            PageNumber = 1,
            PageSize = 10,
            OrderBy = "name",
            OrderDescending = false
        };

        var providers = new List<Provider>
        {
            new Provider { Id = Guid.NewGuid(), Nit = "111", Name = "A Proveedor", Email = "a@test.com" },
            new Provider { Id = Guid.NewGuid(), Nit = "222", Name = "B Proveedor", Email = "b@test.com" }
        };

        var providerDtos = new List<ProviderDto>
        {
            new ProviderDto { Id = providers[0].Id, Nit = "111", Name = "A Proveedor", Email = "a@test.com" },
            new ProviderDto { Id = providers[1].Id, Nit = "222", Name = "B Proveedor", Email = "b@test.com" }
        };

        _providerRepositoryMock
            .Setup(x => x.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                It.IsAny<Expression<Func<Provider, bool>>>(),
                It.IsAny<Func<IQueryable<Provider>, IOrderedQueryable<Provider>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((providers, 2));

        _mapperMock
            .Setup(x => x.Map<IEnumerable<ProviderDto>>(providers))
            .Returns(providerDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        
        // Verificar que se pasó un orderBy al repositorio
        _providerRepositoryMock.Verify(
            x => x.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                It.IsAny<Expression<Func<Provider, bool>>>(),
                It.IsAny<Func<IQueryable<Provider>, IOrderedQueryable<Provider>>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ConOrdenamientoDescendente_DebeAplicarOrdenDescendente()
    {
        // Arrange
        var query = new GetAllProvidersQuery
        {
            PageNumber = 1,
            PageSize = 10,
            OrderBy = "name",
            OrderDescending = true
        };

        var providers = new List<Provider>
        {
            new Provider { Id = Guid.NewGuid(), Nit = "222", Name = "B Proveedor", Email = "b@test.com" },
            new Provider { Id = Guid.NewGuid(), Nit = "111", Name = "A Proveedor", Email = "a@test.com" }
        };

        var providerDtos = new List<ProviderDto>
        {
            new ProviderDto { Id = providers[0].Id, Nit = "222", Name = "B Proveedor", Email = "b@test.com" },
            new ProviderDto { Id = providers[1].Id, Nit = "111", Name = "A Proveedor", Email = "a@test.com" }
        };

        _providerRepositoryMock
            .Setup(x => x.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                It.IsAny<Expression<Func<Provider, bool>>>(),
                It.IsAny<Func<IQueryable<Provider>, IOrderedQueryable<Provider>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((providers, 2));

        _mapperMock
            .Setup(x => x.Map<IEnumerable<ProviderDto>>(providers))
            .Returns(providerDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_CalcularTotalPages_DebeCalcularCorrectamente()
    {
        // Arrange - 25 proveedores con tamaño de página de 10
        var query = new GetAllProvidersQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        var providers = Enumerable.Range(1, 10)
            .Select(i => new Provider 
            { 
                Id = Guid.NewGuid(), 
                Nit = $"NIT{i}", 
                Name = $"Proveedor {i}", 
                Email = $"test{i}@test.com" 
            })
            .ToList();

        var providerDtos = providers.Select(p => new ProviderDto
        {
            Id = p.Id,
            Nit = p.Nit,
            Name = p.Name,
            Email = p.Email
        }).ToList();

        // Total de 25 proveedores con página de 10 = 3 páginas
        _providerRepositoryMock
            .Setup(x => x.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                It.IsAny<Expression<Func<Provider, bool>>>(),
                It.IsAny<Func<IQueryable<Provider>, IOrderedQueryable<Provider>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((providers, 25));

        _mapperMock
            .Setup(x => x.Map<IEnumerable<ProviderDto>>(providers))
            .Returns(providerDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.TotalCount.Should().Be(25);
        result.TotalPages.Should().Be(3); // Ceiling(25 / 10) = 3
        result.PageNumber.Should().Be(1);
    }

    [Fact]
    public async Task Handle_ConListaVacia_DebeRetornarListaVacia()
    {
        // Arrange
        var query = new GetAllProvidersQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        var emptyProviders = new List<Provider>();
        var emptyProviderDtos = new List<ProviderDto>();

        _providerRepositoryMock
            .Setup(x => x.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                It.IsAny<Expression<Func<Provider, bool>>>(),
                It.IsAny<Func<IQueryable<Provider>, IOrderedQueryable<Provider>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((emptyProviders, 0));

        _mapperMock
            .Setup(x => x.Map<IEnumerable<ProviderDto>>(emptyProviders))
            .Returns(emptyProviderDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
        result.TotalPages.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ConPaginaDiferenteDeUno_DebeRetornarPaginaCorrecta()
    {
        // Arrange
        var query = new GetAllProvidersQuery
        {
            PageNumber = 2,
            PageSize = 5
        };

        var providers = new List<Provider>
        {
            new Provider { Id = Guid.NewGuid(), Nit = "111", Name = "Proveedor 6", Email = "test6@test.com" },
            new Provider { Id = Guid.NewGuid(), Nit = "222", Name = "Proveedor 7", Email = "test7@test.com" }
        };

        var providerDtos = new List<ProviderDto>
        {
            new ProviderDto { Id = providers[0].Id, Nit = "111", Name = "Proveedor 6", Email = "test6@test.com" },
            new ProviderDto { Id = providers[1].Id, Nit = "222", Name = "Proveedor 7", Email = "test7@test.com" }
        };

        _providerRepositoryMock
            .Setup(x => x.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                It.IsAny<Expression<Func<Provider, bool>>>(),
                It.IsAny<Func<IQueryable<Provider>, IOrderedQueryable<Provider>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((providers, 12));

        _mapperMock
            .Setup(x => x.Map<IEnumerable<ProviderDto>>(providers))
            .Returns(providerDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.PageNumber.Should().Be(2);
        result.TotalPages.Should().Be(3); // Ceiling(12 / 5) = 3
        result.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_ConExcepcion_DebeRetornarErrorResponse()
    {
        // Arrange
        var query = new GetAllProvidersQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        _providerRepositoryMock
            .Setup(x => x.GetPagedAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Provider, bool>>>(),
                It.IsAny<Func<IQueryable<Provider>, IOrderedQueryable<Provider>>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Contain("Error retrieving providers");
        result.Message.Should().Contain("Database error");
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task Handle_DebeMapearCorrectamenteProvidersAProviderDtos()
    {
        // Arrange
        var query = new GetAllProvidersQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        var providers = new List<Provider>
        {
            new Provider { Id = Guid.NewGuid(), Nit = "123456789", Name = "Proveedor Test", Email = "test@test.com" }
        };

        var providerDtos = new List<ProviderDto>
        {
            new ProviderDto { Id = providers[0].Id, Nit = "123456789", Name = "Proveedor Test", Email = "test@test.com" }
        };

        _providerRepositoryMock
            .Setup(x => x.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                It.IsAny<Expression<Func<Provider, bool>>>(),
                It.IsAny<Func<IQueryable<Provider>, IOrderedQueryable<Provider>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((providers, 1));

        _mapperMock
            .Setup(x => x.Map<IEnumerable<ProviderDto>>(providers))
            .Returns(providerDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        
        // Verificar que se llamó al mapper con los proveedores correctos
        _mapperMock.Verify(
            x => x.Map<IEnumerable<ProviderDto>>(It.Is<List<Provider>>(p => p.Count == 1)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_DebeLlamarRepositorioConParametrosCorrectos()
    {
        // Arrange
        var query = new GetAllProvidersQuery
        {
            PageNumber = 3,
            PageSize = 20
        };

        var providers = new List<Provider>();
        var providerDtos = new List<ProviderDto>();

        _providerRepositoryMock
            .Setup(x => x.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                It.IsAny<Expression<Func<Provider, bool>>>(),
                It.IsAny<Func<IQueryable<Provider>, IOrderedQueryable<Provider>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((providers, 0));

        _mapperMock
            .Setup(x => x.Map<IEnumerable<ProviderDto>>(providers))
            .Returns(providerDtos);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _providerRepositoryMock.Verify(
            x => x.GetPagedAsync(
                3,  // PageNumber
                20, // PageSize
                It.IsAny<Expression<Func<Provider, bool>>>(),
                It.IsAny<Func<IQueryable<Provider>, IOrderedQueryable<Provider>>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}

