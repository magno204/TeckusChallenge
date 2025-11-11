using FluentAssertions;
using Moq;
using TekusChallenge.Application.UseCases.Statistics.Queries.GetSummaryReport;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.UnitTests.Application.Handlers.Statistics;

/// <summary>
/// Unit tests for GetSummaryReportHandler
/// </summary>
public class GetSummaryReportHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProviderRepository> _providerRepositoryMock;
    private readonly Mock<IServiceRepository> _serviceRepositoryMock;
    private readonly Mock<ICountryRepository> _countryRepositoryMock;
    private readonly GetSummaryReportHandler _handler;

    public GetSummaryReportHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _providerRepositoryMock = new Mock<IProviderRepository>();
        _serviceRepositoryMock = new Mock<IServiceRepository>();
        _countryRepositoryMock = new Mock<ICountryRepository>();

        _unitOfWorkMock.Setup(x => x.Providers).Returns(_providerRepositoryMock.Object);
        _unitOfWorkMock.Setup(x => x.Services).Returns(_serviceRepositoryMock.Object);
        _unitOfWorkMock.Setup(x => x.Countries).Returns(_countryRepositoryMock.Object);

        _handler = new GetSummaryReportHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ConDatosValidos_DebeRetornarResumenCompleto()
    {
        // Arrange
        var query = new GetSummaryReportQuery();

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
                Provider = providers[0],
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
                Provider = providers[1],
                ServiceCountries = new List<ServiceCountry>
                {
                    new ServiceCountry { CountryCode = "CO" },
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
        result.Data.TotalServices.Should().Be(2);
        result.Data.TotalCountriesCovered.Should().Be(2);
        result.Data.AverageHourlyRate.Should().Be(75m);
        result.Data.CountryStatistics.Should().NotBeEmpty();
        result.Data.MostExpensiveService.Should().NotBeNull();
        result.Data.MostExpensiveService!.HourlyRate.Should().Be(100m);
        result.Data.CheapestService.Should().NotBeNull();
        result.Data.CheapestService!.HourlyRate.Should().Be(50m);
    }

    [Fact]
    public async Task Handle_ConListasVacias_DebeRetornarResumenVacio()
    {
        // Arrange
        var query = new GetSummaryReportQuery();

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
        result.Data.TotalServices.Should().Be(0);
        result.Data.TotalCountriesCovered.Should().Be(0);
        result.Data.AverageHourlyRate.Should().Be(0);
        result.Data.CountryStatistics.Should().BeEmpty();
        result.Data.MostExpensiveService.Should().BeNull();
        result.Data.CheapestService.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ConExcepcion_DebeRetornarErrorResponse()
    {
        // Arrange
        var query = new GetSummaryReportQuery();

        _providerRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Contain("Error retrieving summary report");
        result.Message.Should().Contain("Database error");
    }

    [Fact]
    public async Task Handle_DebeLlamarTodosLosRepositorios()
    {
        // Arrange
        var query = new GetSummaryReportQuery();

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
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _providerRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _serviceRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _countryRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DebeCalcularPromedioCorrectamente()
    {
        // Arrange
        var query = new GetSummaryReportQuery();
        var providerId = Guid.NewGuid();

        var providers = new List<Provider>
        {
            new Provider { Id = providerId, Nit = "123", Name = "Provider", Email = "test@test.com" }
        };

        var services = new List<Service>
        {
            new Service { Id = Guid.NewGuid(), Name = "Service 1", HourlyRate = 30m, ProviderId = providerId, Provider = providers[0], ServiceCountries = new List<ServiceCountry>() },
            new Service { Id = Guid.NewGuid(), Name = "Service 2", HourlyRate = 60m, ProviderId = providerId, Provider = providers[0], ServiceCountries = new List<ServiceCountry>() },
            new Service { Id = Guid.NewGuid(), Name = "Service 3", HourlyRate = 90m, ProviderId = providerId, Provider = providers[0], ServiceCountries = new List<ServiceCountry>() }
        };

        _providerRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(providers);
        _serviceRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(services);
        _countryRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<Country>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Data.AverageHourlyRate.Should().Be(60m); // (30 + 60 + 90) / 3 = 60
    }
}

