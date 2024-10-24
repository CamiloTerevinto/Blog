using CT.Examples.CqrsWithAspire.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CT.Examples.CqrsWithAspire.Query.Api.Controllers;

[Route("orders")]
[ApiController]
public class OrdersController(ShopDbContext dbContext) : ControllerBase
{
    private readonly ShopDbContext _dbContext = dbContext;

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(Guid id)
    {
        var order = await _dbContext.Orders
            .Where(x => x.Id == id)
            .Select(x => new OrderDto
            {
                CustomerName = x.CustomerName,
                Id = x.Id,
            })
            .FirstOrDefaultAsync();

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }
}

public class OrderDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
}