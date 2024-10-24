using CT.Examples.CqrsWithAspire.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CT.Examples.CqrsWithAspire.Command.Api.Controllers;

[Route("orders")]
[ApiController]
public class OrdersController(ShopDbContext dbContext) : ControllerBase
{
    private readonly ShopDbContext _dbContext = dbContext;

    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderCommand command)
    {
        var order = new Order
        {
            CustomerName = command.CustomerName,
            Id = Guid.NewGuid(),
            CreationDate = DateTime.UtcNow,
        };

        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();

        return Ok(new OrderDto
        {
            Id = order.Id,
            CustomerName = command.CustomerName,
        });
    }
}

public class CreateOrderCommand
{
    public string CustomerName { get; set; }
}

public class OrderDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
}