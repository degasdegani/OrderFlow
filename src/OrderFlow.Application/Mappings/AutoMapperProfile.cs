using AutoMapper;
using OrderFlow.Application.DTOs.Category;
using OrderFlow.Application.DTOs.Customer;
using OrderFlow.Application.DTOs.Order;
using OrderFlow.Application.DTOs.Payment;
using OrderFlow.Application.DTOs.Product;
using OrderFlow.Application.DTOs.Stock;
using OrderFlow.Application.DTOs.Supplier;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Customer
        CreateMap<Customer, CustomerResponseDto>();
        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>();

        // Supplier
        CreateMap<Supplier, SupplierResponseDto>();
        CreateMap<CreateSupplierDto, Supplier>();
        CreateMap<UpdateSupplierDto, Supplier>();

        // Category
        CreateMap<Category, CategoryResponseDto>()
            .ForMember(dest => dest.ProductCount,
                opt => opt.MapFrom(src => src.Products != null ? src.Products.Count : 0));
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();

        // Product
        CreateMap<Product, ProductResponseDto>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
            .ForMember(dest => dest.SupplierName,
                opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.CompanyName : string.Empty))
            .ForMember(dest => dest.IsLowStock,
                opt => opt.MapFrom(src => src.IsLowStock))
            .ForMember(dest => dest.ProfitMargin,
                opt => opt.MapFrom(src => src.ProfitMargin));
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        // Order
        CreateMap<Order, OrderResponseDto>()
            .ForMember(dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Name : string.Empty))
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<CreateOrderDto, Order>();

        // OrderItem
        CreateMap<OrderItem, OrderItemResponseDto>()
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
            .ForMember(dest => dest.ProductSKU,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.SKU : string.Empty));

        // Payment
        CreateMap<Payment, PaymentResponseDto>()
            .ForMember(dest => dest.OrderNumber,
                opt => opt.MapFrom(src => src.Order != null ? src.Order.OrderNumber : 0))
            .ForMember(dest => dest.PaymentMethod,
                opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()));

        // StockMovement
        CreateMap<StockMovement, StockMovementResponseDto>()
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
            .ForMember(dest => dest.ProductSKU,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.SKU : string.Empty))
            .ForMember(dest => dest.MovementType,
                opt => opt.MapFrom(src => src.MovementType.ToString()))
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty));
    }
}