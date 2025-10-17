using DataServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NorthwindController : ControllerBase
{
    private readonly DataService _service;

    public NorthwindController()
    {
        _service = new DataService();
    }

    [HttpGet("categories")]
    public ActionResult<IEnumerable<Category>> GetCategories()
    {
        return Ok(_service.GetCategories());
    }

    [HttpGet("categories/{id:int}")]
    public ActionResult<Category> GetCategory(int id)
    {
        var category = _service.GetCategory(id);
        return category == null ? NotFound() : Ok(category);
    }

    [HttpPost("categories")]
    public ActionResult<Category> CreateCategory([FromBody] Category category)
    {
        var created = _service.CreateCategory(category.Name!, category.Description!);
        return CreatedAtAction(nameof(GetCategory), new { id = created.Id }, created);
    }

    [HttpPut("categories/{id:int}")]
    public IActionResult UpdateCategory(int id, [FromBody] Category category)
    {
        var updated = _service.UpdateCategory(id, category.Name!, category.Description!);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("categories/{id:int}")]
    public IActionResult DeleteCategory(int id)
    {
        var deleted = _service.DeleteCategory(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("products/{id:int}")]
    public ActionResult<Product> GetProduct(int id)
    {
        var product = _service.GetProduct(id);
        return product == null ? NotFound() : Ok(product);
    }

    [HttpGet("products/by-category/{categoryId:int}")]
    public ActionResult<IEnumerable<ProductByCategoryDto>> GetProductsByCategory(int categoryId)
    {
        return Ok(_service.GetProductByCategory(categoryId));
    }

    [HttpGet("products/search")]
    public ActionResult<IEnumerable<ProductByNameDto>> GetProductsByName([FromQuery] string name)
    {
        return Ok(_service.GetProductByName(name));
    }

    [HttpPost("products")]
    public ActionResult<Product> CreateProduct([FromBody] Product product)
    {
        var created = _service.CreateProduct(
            product.Name!,
            (float)product.UnitPrice,
            product.QuantityPerUnit,
            product.UnitsInStock,
            product.CategoryId
        );

        return CreatedAtAction(nameof(GetProduct), new { id = created.Id }, created);
    }

    [HttpGet("orders")]
    public ActionResult<IEnumerable<Order>> GetOrders()
    {
        return Ok(_service.GetOrders());
    }

    [HttpGet("orders/{id:int}")]
    public ActionResult<Order> GetOrder(int id)
    {
        var order = _service.GetOrder(id);
        return order == null ? NotFound() : Ok(order);
    }

    [HttpPost("orders")]
    public ActionResult<Order> CreateOrder([FromBody] Order order)
    {
        var created = _service.CreateOrder(
            order.Date,
            order.Required,
            order.Shipped,
            order.Freight,
            order.ShipName!,
            order.ShipCity!
        );

        return CreatedAtAction(nameof(GetOrder), new { id = created.Id }, created);
    }
    
    [HttpGet("orders/{orderId:int}/details")]
    public ActionResult<IEnumerable<OrderDetails>> GetOrderDetailsByOrderId(int orderId)
    {
        return Ok(_service.GetOrderDetailsByOrderId(orderId));
    }

    [HttpGet("products/{productId:int}/order-details")]
    public ActionResult<IEnumerable<OrderDetails>> GetOrderDetailsByProductId(int productId)
    {
        return Ok(_service.GetOrderDetailsByProductId(productId));
    }

    [HttpPost("order-details")]
    public ActionResult<OrderDetails> CreateOrderDetails([FromBody] OrderDetails details)
    {
        var created = _service.CreateOrderDetails(
            details.OrderId,
            details.ProductId,
            (float)details.UnitPrice,
            details.Quantity,
            details.Discount
        );

        return Created(string.Empty, created);
    }
}