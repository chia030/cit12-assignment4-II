using DataServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace WebServiceLayer.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly DataService _service = new();

    [HttpGet("{id:int}")]
    public IActionResult GetProduct(int id)
    {
        var product = _service.GetProduct(id);
        if (product == null)
            return NotFound();

        var result = new
        {
            id = product.Id,
            name = product.Name,
            unitPrice = product.UnitPrice,
            unitsInStock = product.UnitsInStock,
            category = new
            {
                id = product.Category.Id,
                name = product.Category.Name
            }
        };

        return Ok(result);
    }

    [HttpGet("category/{categoryId:int}")]
    public IActionResult GetProductsByCategory(int categoryId)
    {
        var products = _service.GetProductByCategory(categoryId);
        if (products == null || !products.Any())
            return NotFound(products);
        return Ok(products);
    }

    [HttpGet("name/{substring}")]
    public IActionResult GetProductsByName(string substring)
    {
        var products = _service.GetProductByName(substring);
        if (products == null || !products.Any())
            return NotFound(products);
        return Ok(products);
    }
}
