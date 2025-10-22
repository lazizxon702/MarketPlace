namespace MarketPlace;

public class Order
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public User User { get; set; }
 
    public DateTime? OrderDate { get; set; } = DateTime.Now;
    
    public decimal TotalAmount { get; set; }
    
    public string? Status { get; set; } = "GOOD";
    
    public string? PaymentType { get; set; }
    
    public string? Address { get; set; }
    
    public bool IsDeleted { get; set; } = false;
    
    public ICollection<OrderItem> OrderItems { get; set; }
}
