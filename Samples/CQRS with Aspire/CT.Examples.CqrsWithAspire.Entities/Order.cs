namespace CT.Examples.CqrsWithAspire.Entities;

public class Order
{
    public Guid Id { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public string CustomerName { get; set; }
}