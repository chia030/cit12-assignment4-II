using DataServiceLayer;
using WebServiceLayer.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebServiceLayer.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly DataService _service = new();

    [HttpGet]
    public IActionResult GetAll()
    {
        var categories = _service.GetCategories();
        if (categories == null || !categories.Any())
            return NotFound();

        // TODO: find a way to map their attributes automatically
        var dtos = categories.Select(c => new CategoryDTO
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        });

        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetCategory(int id)
    {
        var category = _service.GetCategory(id);
        if (category == null)
            return NotFound();

        var dto = new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };

        return Ok(dto);
    }

    [HttpPost]
    public IActionResult CreateCategory([FromBody] CategoryDTO categoryDto)
    {
        var created = _service.CreateCategory(categoryDto.Name, categoryDto.Description);

        var dto = new CategoryDTO
        {
            Id = created.Id,
            Name = created.Name,
            Description = created.Description
        };

        return CreatedAtAction(nameof(GetCategory), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateCategory(int id, [FromBody] CategoryDTO categoryDto)
    {
        var updated = _service.UpdateCategory(id, categoryDto.Name, categoryDto.Description);
        if (!updated)
            return NotFound();

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var deleted = _service.DeleteCategory(id);
        if (!deleted)
            return NotFound();

        return Ok();
    }
}
