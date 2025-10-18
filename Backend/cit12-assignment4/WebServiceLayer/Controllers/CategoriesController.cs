using DataServiceLayer;
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
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetCategory(int id)
    {
        var category = _service.GetCategory(id);
        if (category == null)
            return NotFound();
        return Ok(category);
    }

    [HttpPost]
    public IActionResult CreateCategory(Category category)
    {
        var created = _service.CreateCategory(category.Name, category.Description);
        return CreatedAtAction(nameof(GetCategory), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, Category category)
    {
        var success = _service.UpdateCategory(id, category.Name, category.Description);
        if (!success)
            return NotFound();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var success = _service.DeleteCategory(id);
        if (!success)
            return NotFound();
        return Ok();
    }
}
