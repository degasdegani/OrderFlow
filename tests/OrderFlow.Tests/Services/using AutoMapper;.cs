using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using OrderFlow.Application.DTOs.Product;
using OrderFlow.Application.Mappings;
using OrderFlow.Application.Services;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Exceptions;
using OrderFlow.Domain.Interfaces;

namespace OrderFlow.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<ProductService>> _loggerMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<ProductService>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        _mapper = config.CreateMapper();

        _service = new ProductService(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _mapper,
            _loggerMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductExists_ReturnsProduct()
    {
        // Arrange
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Notebook Dell",
            SKU = "NOTE-001",
            CostPrice = 2500m,
            SalePrice = 3299m,
            StockQuantity = 10,
            MinimumStock = 3,
            Unit = "UN",
            Category = new Category { Name = "Informática" },
            Supplier = new Supplier { CompanyName = "Tech Supply" }
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(product.Id))
            .ReturnsAsync(product);

        // Act
        var result = await _service.GetByIdAsync(product.Id);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Data!.Name.Should().Be("Notebook Dell");
        result.Data.SKU.Should().Be("NOTE-001");
        result.Data.IsLowStock.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Product?)null);

        // Act
        var act = async () => await _service.GetByIdAsync(id);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_WhenSkuIsUnique_CreatesProduct()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "Mouse Logitech",
            SKU = "MOUSE-001",
            CategoryId = Guid.NewGuid(),
            SupplierId = Guid.NewGuid(),
            CostPrice = 100m,
            SalePrice = 199m,
            StockQuantity = 20,
            MinimumStock = 5,
            Unit = "UN"
        };

        var createdProduct = new Product
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            SKU = dto.SKU,
            CostPrice = dto.CostPrice,
            SalePrice = dto.SalePrice,
            StockQuantity = dto.StockQuantity,
            MinimumStock = dto.MinimumStock,
            Unit = dto.Unit,
            Category = new Category { Name = "Acessórios" },
            Supplier = new Supplier { CompanyName = "Tech Supply" }
        };

        _repositoryMock.Setup(r => r.SkuExistsAsync(dto.SKU, null))
            .ReturnsAsync(false);

        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(createdProduct);

        _unitOfWorkMock.Setup(u => u.CommitAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Data!.Name.Should().Be("Mouse Logitech");
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenSkuAlreadyExists_ThrowsBusinessRuleException()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "Mouse Logitech",
            SKU = "MOUSE-001",
            CostPrice = 100m,
            SalePrice = 199m,
            Unit = "UN"
        };

        _repositoryMock.Setup(r => r.SkuExistsAsync(dto.SKU, null))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _service.CreateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage($"*{dto.SKU}*");
    }

    [Fact]
    public async Task GetLowStockAsync_ReturnsOnlyLowStockProducts()
    {
        // Arrange
        var lowStockProducts = new List<Product>
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Teclado Redragon",
                SKU = "TEC-001",
                StockQuantity = 2,
                MinimumStock = 5,
                Unit = "UN",
                Category = new Category { Name = "Acessórios" },
                Supplier = new Supplier { CompanyName = "Tech Supply" }
            }
        };

        _repositoryMock.Setup(r => r.GetLowStockAsync())
            .ReturnsAsync(lowStockProducts);

        // Act
        var result = await _service.GetLowStockAsync();

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Data.Should().HaveCount(1);
        result.Data![0].IsLowStock.Should().BeTrue();
    }
}