using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enums;

namespace OrderFlow.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var admin = new User
        {
            FullName = "Administrador",
            Email = "admin@orderflow.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = UserRole.Admin
        };

        var manager = new User
        {
            FullName = "Gerente",
            Email = "gerente@orderflow.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Gerente@123"),
            Role = UserRole.Manager
        };

        var seller = new User
        {
            FullName = "Vendedor",
            Email = "vendedor@orderflow.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Vendedor@123"),
            Role = UserRole.Seller
        };

        await context.Users.AddRangeAsync(admin, manager, seller);

        var category1 = new Category { Name = "Eletrônicos", Description = "Produtos eletrônicos em geral" };
        var category2 = new Category { Name = "Informática", Description = "Computadores e periféricos" };
        var category3 = new Category { Name = "Acessórios", Description = "Acessórios diversos" };

        await context.Categories.AddRangeAsync(category1, category2, category3);

        var supplier1 = new Supplier
        {
            CompanyName = "Tech Supply Ltda",
            Document = "12345678000195",
            ContactName = "João Silva",
            Email = "contato@techsupply.com",
            Phone = "(11) 99999-0001",
            Address = "Rua das Indústrias, 100",
            City = "São Paulo",
            State = "SP",
            ZipCode = "01000-000"
        };

        var supplier2 = new Supplier
        {
            CompanyName = "Distribuidora InfoParts",
            Document = "98765432000110",
            ContactName = "Maria Santos",
            Email = "contato@infoparts.com",
            Phone = "(11) 99999-0002",
            Address = "Av. Comercial, 500",
            City = "Ribeirão Preto",
            State = "SP",
            ZipCode = "14000-000"
        };

        await context.Suppliers.AddRangeAsync(supplier1, supplier2);
        await context.SaveChangesAsync();

        var product1 = new Product
        {
            Name = "Notebook Dell Inspiron 15",
            Description = "Notebook Dell com processador Intel Core i5",
            SKU = "NOTE-DELL-001",
            CategoryId = category2.Id,
            SupplierId = supplier1.Id,
            CostPrice = 2500.00m,
            SalePrice = 3299.99m,
            StockQuantity = 10,
            MinimumStock = 3,
            Unit = "UN"
        };

        var product2 = new Product
        {
            Name = "Mouse Logitech MX Master",
            Description = "Mouse sem fio ergonômico",
            SKU = "MOUSE-LOG-001",
            CategoryId = category3.Id,
            SupplierId = supplier2.Id,
            CostPrice = 150.00m,
            SalePrice = 249.99m,
            StockQuantity = 30,
            MinimumStock = 5,
            Unit = "UN"
        };

        var product3 = new Product
        {
            Name = "Teclado Mecânico Redragon",
            Description = "Teclado mecânico gamer RGB",
            SKU = "TECLADO-RED-001",
            CategoryId = category3.Id,
            SupplierId = supplier2.Id,
            CostPrice = 180.00m,
            SalePrice = 299.99m,
            StockQuantity = 2,
            MinimumStock = 5,
            Unit = "UN"
        };

        await context.Products.AddRangeAsync(product1, product2, product3);

        var customer1 = new Customer
        {
            Name = "Carlos Oliveira",
            Document = "12345678901",
            Email = "carlos@email.com",
            Phone = "(16) 99999-1001",
            Address = "Rua das Flores, 123",
            City = "Ribeirão Preto",
            State = "SP",
            ZipCode = "14020-000"
        };

        var customer2 = new Customer
        {
            Name = "Ana Paula Ferreira",
            Document = "98765432100",
            Email = "ana@email.com",
            Phone = "(16) 99999-2002",
            Address = "Av. Brasil, 456",
            City = "Ribeirão Preto",
            State = "SP",
            ZipCode = "14025-000"
        };

        await context.Customers.AddRangeAsync(customer1, customer2);
        await context.SaveChangesAsync();
    }
}