using FluentAssertions;
using Moq;
using TekusChallenge.Application.UseCases.Statistics.Queries.GetProvidersByCountry;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.UnitTests.Application.Handlers.Statistics;

/// <summary>
/// Unit tests for GetProvidersByCountryHandler
/// </summary>
public class GetProvidersByCountryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProviderRepository> _providerRepositoryMock;
    private readonly Mock<IServiceRepository> _serviceRepositoryMock;
    private readonly Mock<ICountryRepository> _countryRepositoryMock;
    private readonly GetProvidersByCountryHandler _handler;

    public GetProvidersByCountryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _providerRepositoryMock = new Mock<IProviderRepository>();
        _serviceRepositoryMock = new Mock<IServiceRepository>();
        _countryRepositoryMock = new Mock<ICountryRepository>();

        _unitOfWorkMock.Setup(x => x.Providers).Returns(_providerRepositoryMock.Object);
        _unitOfWorkMock.Setup(x => x.Services).Returns(_serviceRepositoryMock.Object);
        _unitOfWorkMock.Setup(x => x.Countries).Returns(_countryRepositoryMock.Object);

        _handler = new GetProvidersByCountryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ConDatosValidos_DebeRetornarEstadisticasPorPais()
    {
        // Arrange
        var query = new GetProvidersByCountryQuery();

        var providerId1 = Guid.NewGuid();
        var providerId2 = Guid.NewGuid();

        var providers = new List<Provider>
        {
            new Provider { Id = providerId1, Nit = "123456789", Name = "Provider 1", Email = "test1@test.com" },
            new Provider { Id = providerId2, Nit = "987654321", Name = "Provider 2", Email = "test2@test.com" }
        };

        var countries = new List<Country>
        {
            new Country { Code = "CO", CodeAlpha3 = "COL", Name = "Colombia" },
            new Country { Code = "PE", CodeAlpha3 = "PER", Name = "Peru" }
        };

        var services = new List<Service>
        {
            new Service 
            { 
                Id = Guid.NewGuid(), 
                Name = "Service 1", 
                HourlyRate = 50m, 
                ProviderId = providerId1,
                ServiceCountries = new List<ServiceCountry>
                {
                    new ServiceCountry { CountryCode = "CO" }
                }
            },
            new Service 
            { 
                Id = Guid.NewGuid(), 
                Name = "Service 2", 
                HourlyRate = 100m, 
                ProviderId = providerId2,
                ServiceCountries = new List<ServiceCountry>
                {
                    new ServiceCountry { CountryCode = "PE" }
                }
            }
        };

        _providerRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(providers);

        _serviceRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(services);

        _countryRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(countries);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.TotalProviders.Should().Be(2);
        result.Data.TotalCountries.Should().Be(2);
        result.Data.ProvidersByCountry.Should().HaveCount(2);
        result.Data.ProvidersByCountry.Should().Contain(x => x.CountryCode == "CO" && x.Count == 1);
        result.Data.ProvidersByCountry.Should().Contain(x => x.CountryCode == "PE" && x.Count == 1);
    }

    [Fact]
    public async Task Handle_ConUnProveedorEnMultiplesPaises_DebeContarCorrectamente()
    {
        // Arrange
        var query = new GetProvidersByCountryQuery();

        var providerId = Guid.NewGuid();

        var providers = new List<Provider>
        {
            new Provider { Id = providerId, Nit = "123456789", Name = "Provider 1", Email = "test1@test.com" }
        };

        var countries = new List<Country>
        {
            new Country { Code = "CO", CodeAlpha3 = "COL", Name = "Colombia" },
            new Country { Code = "PE", CodeAlpha3 = "PER", Name = "Peru" }
        };

        // Un proveedor con servicios en ambos países
        var services = new List<Service>
        {
            new Service 
            { 
                Id = Guid.NewGuid(), 
                Name = "Service 1", 
                HourlyRate = 50m, 
                ProviderId = providerId,
                ServiceCountries = new List<ServiceCountry>
                {
                    new ServiceCountry { CountryCode = "CO" }
                }
            },
            new Service 
            { 
                Id = Guid.NewGuid(), 
                Name = "Service 2", 
                HourlyRate = 100m, 
                ProviderId = providerId,
                ServiceCountries = new List<ServiceCountry>
                {
                    new ServiceCountry { CountryCode = "PE" }
                }
            }
        };

        _providerRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(providers);

        _serviceRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(services);

        _countryRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(countries);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.TotalProviders.Should().Be(1);
        result.Data.TotalCountries.Should().Be(2);
        // El mismo proveedor aparece en ambos países
        result.Data.ProvidersByCountry.Should().Contain(x => x.CountryCode == "CO" && x.Count == 1);
        result.Data.ProvidersByCountry.Should().Contain(x => x.CountryCode == "PE" && x.Count == 1);
    }

    [Fact]
    public async Task Handle_ConListasVacias_DebeRetornarEstadisticasVacias()
    {
        // Arrange
        var query = new GetProvidersByCountryQuery();

        _providerRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Provider>());

        _serviceRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Service>());

        _countryRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Country>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.TotalProviders.Should().Be(0);
        result.Data.TotalCountries.Should().Be(0);
        result.Data.ProvidersByCountry.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ConExcepcion_DebeRetornarErrorResponse()
    {
        // Arrange
        var query = new GetProvidersByCountryQuery();

        _serviceRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Contain("Error retrieving providers statistics");
        result.Message.Should().Contain("Database error");
    }

    [Fact]
    public async Task Handle_DebeOrdenarPorCantidadDescendente()
    {
        // Arrange
        var query = new GetProvidersByCountryQuery();

        var provider1 = Guid.NewGuid();
        var provider2 = Guid.NewGuid();
        var provider3 = Guid.NewGuid();

        var providers = new List<Provider>
        {
            new Provider { Id = provider1, Nit = "111", Name = "Provider 1", Email = "test1@test.com" },
            new Provider { Id = provider2, Nit = "222", Name = "Provider 2", Email = "test2@test.com" },
            new Provider { Id = provider3, Nit = "333", Name = "Provider 3", Email = "test3@test.com" }
        };

        var countries = new List<Country>
        {
            new Country { Code = "CO", CodeAlpha3 = "COL", Name = "Colombia" },
            new Country { Code = "PE", CodeAlpha3 = "PER", Name = "Peru" }
        };

        var services = new List<Service>
        {
            new Service { Id = Guid.NewGuid(), ProviderId = provider1, ServiceCountries = new List<ServiceCountry> { new ServiceCountry { CountryCode = "CO" } } },
            new Service { Id = Guid.NewGuid(), ProviderId = provider2, ServiceCountries = new List<ServiceCountry> { new ServiceCountry { CountryCode = "CO" } } },
            new Service { Id = Guid.NewGuid(), ProviderId = provider3, ServiceCountries = new List<ServiceCountry> { new ServiceCountry { CountryCode = "PE" } } }
        };

        _providerRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(providers);
        _serviceRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(services);
        _countryRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(countries);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Data.ProvidersByCountry.First().CountryCode.Should().Be("CO"); // 2 proveedores
        result.Data.ProvidersByCountry.First().Count.Should().Be(2);
    }
}

