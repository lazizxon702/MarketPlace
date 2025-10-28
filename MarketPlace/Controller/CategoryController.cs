using MarketPlace.DTO.CategoryDTO;
using MarketPlace.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Controller;

[Route("api/[controller]")]
[ApiController]
[Authorize] 
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CategoryReadDTO>>> GetAll()
    {
        var categories = await categoryService.GetAll();
        return Ok(categories);
    }

   
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryReadDTO>> GetById(int id)
    {
        var category = await categoryService.GetById(id);
        return Ok(category);
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<string>> CreateCategory([FromBody] CategoryCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await categoryService.Create(dto);
        return Ok(result);
    }

   
    [Authorize(Roles = "Admin")] 
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDTO dto)
    {
        var updated = await categoryService.Update(id, dto);
        if (!updated) return NotFound("Category topilmadi");
        return NoContent();
    }

    
    [Authorize(Roles = "Admin")] 
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        var deleted = await categoryService.Delete(id);
        if (!deleted) return NotFound("Category topilmadi");
        return NoContent();
    }
}
