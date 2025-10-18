using DataServiceLayer;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.DTOs;


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

        var dto = new ProductDetailDTO
        {
            Id = product.Id,
            Name = product.Name,
            Category = new CategoryDTO
            {
                Id = product.Category.Id,
                Name = product.Category.Name,
                Description = product.Category.Description
            }
        };

        return Ok(dto);
    }

    [HttpGet("category/{categoryId:int}")]
    public IActionResult GetProductsByCategory(int categoryId)
    {
        var products = _service.GetProductByCategory(categoryId);
        if (products == null || !products.Any())
            return NotFound(products);

        var dtos = products.Select(p => new ProductDTO
        {
            Id = p.Id,
            Name = p.Name,
            CategoryName = p.CategoryName
        });

        return Ok(dtos);
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
