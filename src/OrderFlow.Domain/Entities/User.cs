using OrderFlow.Domain.Enums;

namespace OrderFlow.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}