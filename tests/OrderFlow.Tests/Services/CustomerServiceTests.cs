using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using OrderFlow.Application.DTOs.Customer;
using OrderFlow.Application.Mappings;
using OrderFlow.Application.Services;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Exceptions;
using OrderFlow.Domain.Interfaces;

namespace OrderFlow.Tests.Services;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<CustomerService>> _loggerMock;
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        _repositoryMock = new Mock<ICustomerRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<CustomerService>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        _mapper = config.CreateMapper();

        _service = new CustomerService(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _mapper,
            _loggerMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCustomerExists_ReturnsCustomer()
    {
        // Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "João Silva",
            Document = "12345678901",
            Email = "joao@email.com",
            Phone = "(16) 99999-1234",
            Address = "Rua das Flores, 123",
            City = "Ribeirão Preto",
            State = "SP",
            ZipCode = "14000-000"
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);

        // Act
        var result = await _service.GetByIdAsync(customer.Id);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Name.Should().Be("João Silva");
        result.Data.Email.Should().Be("joao@email.com");
    }

    [Fact]
    public async Task GetByIdAsync_WhenCustomerNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Customer?)null);

        // Act
        var act = async () => await _service.GetByIdAsync(id);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_WhenDocumentIsUnique_CreatesCustomer()
    {
        // Arrange
        var dto = new CreateCustomerDto
        {
            Name = "Maria Santos",
            Document = "98765432100",
            Email = "maria@email.com",
            Phone = "(16) 99999-5678",
            Address = "Av. Brasil, 456",
            City = "Ribeirão Preto",
            State = "SP",
            ZipCode = "14025-000"
        };

        _repositoryMock.Setup(r => r.DocumentExistsAsync(dto.Document, null))
            .ReturnsAsync(false);

        _unitOfWorkMock.Setup(u => u.CommitAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Name.Should().Be("Maria Santos");
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenDocumentAlreadyExists_ThrowsBusinessRuleException()
    {
        // Arrange
        var dto = new CreateCustomerDto
        {
            Name = "Carlos Oliveira",
            Document = "12345678901",
            Email = "carlos@email.com",
            Phone = "(16) 99999-0001"
        };

        _repositoryMock.Setup(r => r.DocumentExistsAsync(dto.Document, null))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _service.CreateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage($"*{dto.Document}*");
    }

    [Fact]
    public async Task DeleteAsync_WhenCustomerExists_DeletesCustomer()
    {
        // Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Ana Paula",
            Document = "11122233344",
            Email = "ana@email.com",
            Phone = "(16) 99999-0002"
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);

        _unitOfWorkMock.Setup(u => u.CommitAsync())
            .ReturnsAsync(1);

        // Act
        await _service.DeleteAsync(customer.Id);

        // Assert
        _repositoryMock.Verify(r => r.Delete(It.IsAny<Customer>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }
}