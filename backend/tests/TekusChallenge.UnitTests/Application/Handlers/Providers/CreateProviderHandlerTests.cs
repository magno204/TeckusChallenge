using AutoMapper;
using FluentAssertions;
using Moq;
using TekusChallenge.Application.DTO;
using TekusChallenge.Application.UseCases.Providers.Commands.CreateProvider;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.UnitTests.Application.Handlers.Providers;

/// <summary>
/// Pruebas unitarias para CreateProviderCommandHandler
/// </summary>
public class CreateProviderHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IProviderRepository> _providerRepositoryMock;
    private readonly Mock<IProviderCustomFieldRepository> _customFieldRepositoryMock;
    private readonly CreateProviderCommandHandler _handler;

    public CreateProviderHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _providerRepositoryMock = new Mock<IProviderRepository>();
        _customFieldRepositoryMock = new Mock<IProviderCustomFieldRepository>();

        // Configurar el UnitOfWork para devolver los repositorios mockeados
        _unitOfWorkMock.Setup(x => x.Providers).Returns(_providerRepositoryMock.Object);
        _unitOfWorkMock.Setup(x => x.ProviderCustomFields).Returns(_customFieldRepositoryMock.Object);

        _handler = new CreateProviderCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ConDatosValidos_DebeRetornarExito()
    {
        // Arrange
        var command = new CreateProviderCommand
        {
            Nit = "123456789",
            Name = "Proveedor Test",
            Email = "test@proveedor.com"
        };

        var providerId = Guid.NewGuid();
        var provider = new Provider
        {
            Id = providerId,
            Nit = command.Nit,
            Name = command.Name,
            Email = command.Email
        };

        var providerDto = new ProviderDto
        {
            Id = providerId,
            Nit = command.Nit,
            Name = command.Name,
            Email = command.Email
        };

        // Simular que no existe un proveedor con ese NIT
        _providerRepositoryMock
            .Setup(x => x.GetByNitAsync(command.Nit, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Provider?)null);

        // Simular que no existe un proveedor con ese Email
        _providerRepositoryMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Provider?)null);

        _mapperMock.Setup(x => x.Map<Provider>(command)).Returns(provider);
        _mapperMock.Setup(x => x.Map<ProviderDto>(provider)).Returns(providerDto);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Contain("successfully");
        result.Data.Should().NotBeNull();
        result.Data.Should().BeEquivalentTo(providerDto);

        _providerRepositoryMock.Verify(
            x => x.AddAsync(It.Is<Provider>(p => p.Nit == command.Nit), It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ConNitDuplicado_DebeRetornarError()
    {
        // Arrange
        var command = new CreateProviderCommand
        {
            Nit = "123456789",
            Name = "Proveedor Test",
            Email = "test@proveedor.com"
        };

        var existingProvider = new Provider
        {
            Id = Guid.NewGuid(),
            Nit = command.Nit,
            Name = "Proveedor Existente",
            Email = "otro@email.com"
        };

        _providerRepositoryMock
            .Setup(x => x.GetByNitAsync(command.Nit, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProvider);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Contain("NIT");
        result.Message.Should().Contain("already exists");
        result.Data.Should().BeNull();

        // Verificar que NO se intentó agregar el proveedor
        _providerRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Provider>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ConEmailDuplicado_DebeRetornarError()
    {
        // Arrange
        var command = new CreateProviderCommand
        {
            Nit = "123456789",
            Name = "Proveedor Test",
            Email = "test@proveedor.com"
        };

        var existingProvider = new Provider
        {
            Id = Guid.NewGuid(),
            Nit = "987654321",
            Name = "Proveedor Existente",
            Email = command.Email
        };

        // El NIT no existe
        _providerRepositoryMock
            .Setup(x => x.GetByNitAsync(command.Nit, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Provider?)null);

        // Pero el Email sí existe
        _providerRepositoryMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProvider);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Contain("email");
        result.Message.Should().Contain("already exists");
        result.Data.Should().BeNull();

        // Verificar que NO se intentó agregar el proveedor
        _providerRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Provider>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ConCamposPersonalizados_DebeGuardarCustomFields()
    {
        // Arrange
        var customFields = new List<CreateProviderCustomFieldDto>
        {
            new CreateProviderCustomFieldDto
            {
                FieldName = "Dirección",
                FieldValue = "Calle 123",
                FieldType = "text",
                Description = "Dirección principal",
                DisplayOrder = 1
            },
            new CreateProviderCustomFieldDto
            {
                FieldName = "Teléfono",
                FieldValue = "555-1234",
                FieldType = "text",
                Description = "Teléfono de contacto",
                DisplayOrder = 2
            }
        };

        var command = new CreateProviderCommand
        {
            Nit = "123456789",
            Name = "Proveedor Test",
            Email = "test@proveedor.com",
            CustomFields = customFields
        };

        var providerId = Guid.NewGuid();
        var provider = new Provider
        {
            Id = providerId,
            Nit = command.Nit,
            Name = command.Name,
            Email = command.Email
        };

        _providerRepositoryMock
            .Setup(x => x.GetByNitAsync(command.Nit, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Provider?)null);

        _providerRepositoryMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Provider?)null);

        _mapperMock.Setup(x => x.Map<Provider>(command)).Returns(provider);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        // Verificar que se guardó el proveedor
        _providerRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Provider>(), It.IsAny<CancellationToken>()),
            Times.Once);

        // Verificar que se agregaron los campos personalizados
        _customFieldRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<ProviderCustomField>(), It.IsAny<CancellationToken>()),
            Times.Exactly(customFields.Count));

        // Verificar que se llamó SaveChanges dos veces (una para el proveedor, otra para los campos)
        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.AtLeast(1));
    }

    [Fact]
    public async Task Handle_SinCamposPersonalizados_DebeCrearProveedorSinCustomFields()
    {
        // Arrange
        var command = new CreateProviderCommand
        {
            Nit = "123456789",
            Name = "Proveedor Test",
            Email = "test@proveedor.com",
            CustomFields = null
        };

        var providerId = Guid.NewGuid();
        var provider = new Provider
        {
            Id = providerId,
            Nit = command.Nit,
            Name = command.Name,
            Email = command.Email
        };

        var providerDto = new ProviderDto
        {
            Id = providerId,
            Nit = command.Nit,
            Name = command.Name,
            Email = command.Email
        };

        _providerRepositoryMock
            .Setup(x => x.GetByNitAsync(command.Nit, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Provider?)null);

        _providerRepositoryMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Provider?)null);

        _mapperMock.Setup(x => x.Map<Provider>(command)).Returns(provider);
        _mapperMock.Setup(x => x.Map<ProviderDto>(provider)).Returns(providerDto);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        // Verificar que se guardó el proveedor
        _providerRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Provider>(), It.IsAny<CancellationToken>()),
            Times.Once);

        // Verificar que NO se agregaron campos personalizados
        _customFieldRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<ProviderCustomField>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ConCamposPersonalizadosVacios_NoDebeAgregarCustomFields()
    {
        // Arrange
        var command = new CreateProviderCommand
        {
            Nit = "123456789",
            Name = "Proveedor Test",
            Email = "test@proveedor.com",
            CustomFields = new List<CreateProviderCustomFieldDto>()
        };

        var providerId = Guid.NewGuid();
        var provider = new Provider
        {
            Id = providerId,
            Nit = command.Nit,
            Name = command.Name,
            Email = command.Email
        };

        _providerRepositoryMock
            .Setup(x => x.GetByNitAsync(command.Nit, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Provider?)null);

        _providerRepositoryMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Provider?)null);

        _mapperMock.Setup(x => x.Map<Provider>(command)).Returns(provider);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        // Verificar que NO se agregaron campos personalizados
        _customFieldRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<ProviderCustomField>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_DebeLlamarSaveChangesAsync()
    {
        // Arrange
        var command = new CreateProviderCommand
        {
            Nit = "123456789",
            Name = "Proveedor Test",
            Email = "test@proveedor.com"
        };

        var provider = new Provider
        {
            Id = Guid.NewGuid(),
            Nit = command.Nit,
            Name = command.Name,
            Email = command.Email
        };

        _providerRepositoryMock
            .Setup(x => x.GetByNitAsync(command.Nit, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Provider?)null);

        _providerRepositoryMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Provider?)null);

        _mapperMock.Setup(x => x.Map<Provider>(command)).Returns(provider);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        // Verificar que se llamó SaveChangesAsync exactamente
        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task Handle_DebeMapearCommandAProvider()
    {
        // Arrange
        var command = new CreateProviderCommand
        {
            Nit = "123456789",
            Name = "Proveedor Test",
            Email = "test@proveedor.com"
        };

        var provider = new Provider
        {
            Id = Guid.NewGuid(),
            Nit = command.Nit,
            Name = command.Name,
            Email = command.Email
        };

        _providerRepositoryMock
            .Setup(x => x.GetByNitAsync(command.Nit, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Provider?)null);

        _providerRepositoryMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Provider?)null);

        _mapperMock.Setup(x => x.Map<Provider>(command)).Returns(provider);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapperMock.Verify(
            x => x.Map<Provider>(command),
            Times.Once);
    }
}

