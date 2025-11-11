using FluentAssertions;
using Moq;
using TekusChallenge.Application.UseCases.Statistics.Queries.GetServicesByCountry;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.UnitTests.Application.Handlers.Statistics;

/// <summary>
/// Unit tests for GetServicesByCountryHandler
/// </summary>
public class GetServicesByCountryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IServiceRepository> _serviceRepositoryMock;
    private readonly Mock<ICountryRepository> _countryRepositoryMock;
    private readonly GetServicesByCountryHandler _handler;

    public GetServicesByCountryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _serviceRepositoryMock = new Mock<IServiceRepository>();
        _countryRepositoryMock = new Mock<ICountryRepository>();

        _unitOfWorkMock.Setup(x => x.Services).Returns(_serviceRepositoryMock.Object);
        _unitOfWorkMock.Setup(x => x.Countries).Returns(_countryRepositoryMock.Object);

        _handler = new GetServicesByCountryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ConDatosValidos_DebeRetornarEstadisticasPorPais()
    {
        // Arrange
        var query = new GetServicesByCountryQuery();

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
                ServiceCountries = new List<ServiceCountry>
                {
                    new ServiceCountry { CountryCode = "CO" },
                    new ServiceCountry { CountryCode = "PE" }
                }
            }
        };

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
        result.Data.TotalServices.Should().Be(2);
        result.Data.TotalCountries.Should().Be(2);
        result.Data.AverageHourlyRate.Should().Be(75m); // (50 + 100) / 2
        result.Data.ServicesByCountry.Should().HaveCount(2);
        result.Data.ServicesByCountry.Should().Contain(x => x.CountryCode == "CO" && x.Count == 2);
        result.Data.ServicesByCountry.Should().Contain(x => x.CountryCode == "PE" && x.Count == 1);
    }

    [Fact]
    public async Task Handle_ConServicioEnMultiplesPaises_DebeContarEnCadaPais()
    {
        // Arrange
        var query = new GetServicesByCountryQuery();

        var countries = new List<Country>
        {
            new Country { Code = "CO", CodeAlpha3 = "COL", Name = "Colombia" },
            new Country { Code = "PE", CodeAlpha3 = "PER", Name = "Peru" },
            new Country { Code = "MX", CodeAlpha3 = "MEX", Name = "Mexico" }
        };

        var services = new List<Service>
        {
            new Service 
            { 
                Id = Guid.NewGuid(), 
                Name = "International Service", 
                HourlyRate = 75m,
                ServiceCountries = new List<ServiceCountry>
                {
                    new ServiceCountry { CountryCode = "CO" },
                    new ServiceCountry { CountryCode = "PE" },
                    new ServiceCountry { CountryCode = "MX" }
                }
            }
        };

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
        result.Data.TotalServices.Should().Be(1);
        result.Data.TotalCountries.Should().Be(3);
        // El servicio debe ser contado en cada país
        result.Data.ServicesByCountry.Should().HaveCount(3);
        result.Data.ServicesByCountry.Should().OnlyContain(x => x.Count == 1);
    }

    [Fact]
    public async Task Handle_ConListasVacias_DebeRetornarEstadisticasVacias()
    {
        // Arrange
        var query = new GetServicesByCountryQuery();

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
        result.Data.TotalServices.Should().Be(0);
        result.Data.TotalCountries.Should().Be(0);
        result.Data.AverageHourlyRate.Should().Be(0);
        result.Data.ServicesByCountry.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ConExcepcion_DebeRetornarErrorResponse()
    {
        // Arrange
        var query = new GetServicesByCountryQuery();

        _serviceRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Contain("Error retrieving services statistics");
        result.Message.Should().Contain("Database error");
    }

    [Fact]
    public async Task Handle_DebeCalcularPromedioCorrectamente()
    {
        // Arrange
        var query = new GetServicesByCountryQuery();

        var countries = new List<Country>
        {
            new Country { Code = "CO", CodeAlpha3 = "COL", Name = "Colombia" }
        };

        var services = new List<Service>
        {
            new Service { Id = Guid.NewGuid(), Name = "Service 1", HourlyRate = 20m, ServiceCountries = new List<ServiceCountry> { new ServiceCountry { CountryCode = "CO" } } },
            new Service { Id = Guid.NewGuid(), Name = "Service 2", HourlyRate = 40m, ServiceCountries = new List<ServiceCountry> { new ServiceCountry { CountryCode = "CO" } } },
            new Service { Id = Guid.NewGuid(), Name = "Service 3", HourlyRate = 60m, ServiceCountries = new List<ServiceCountry> { new ServiceCountry { CountryCode = "CO" } } }
        };

        _serviceRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(services);
        _countryRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(countries);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Data.AverageHourlyRate.Should().Be(40m); // (20 + 40 + 60) / 3 = 40
    }

    [Fact]
    public async Task Handle_DebeOrdenarPorCantidadDescendente()
    {
        // Arrange
        var query = new GetServicesByCountryQuery();

        var countries = new List<Country>
        {
            new Country { Code = "CO", CodeAlpha3 = "COL", Name = "Colombia" },
            new Country { Code = "PE", CodeAlpha3 = "PER", Name = "Peru" },
            new Country { Code = "MX", CodeAlpha3 = "MEX", Name = "Mexico" }
        };

        var services = new List<Service>
        {
            new Service { Id = Guid.NewGuid(), HourlyRate = 50m, ServiceCountries = new List<ServiceCountry> { new ServiceCountry { CountryCode = "CO" } } },
            new Service { Id = Guid.NewGuid(), HourlyRate = 60m, ServiceCountries = new List<ServiceCountry> { new ServiceCountry { CountryCode = "CO" } } },
            new Service { Id = Guid.NewGuid(), HourlyRate = 70m, ServiceCountries = new List<ServiceCountry> { new ServiceCountry { CountryCode = "CO" } } },
            new Service { Id = Guid.NewGuid(), HourlyRate = 80m, ServiceCountries = new List<ServiceCountry> { new ServiceCountry { CountryCode = "PE" } } },
            new Service { Id = Guid.NewGuid(), HourlyRate = 90m, ServiceCountries = new List<ServiceCountry> { new ServiceCountry { CountryCode = "MX" } } }
        };

        _serviceRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(services);
        _countryRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(countries);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Data.ServicesByCountry.First().CountryCode.Should().Be("CO"); // 3 servicios
        result.Data.ServicesByCountry.First().Count.Should().Be(3);
        result.Data.ServicesByCountry.Last().Count.Should().Be(1);
    }
}

