using MarketPlace.DTO.ProductDTO;
using MarketPlace.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Controller;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductController(IProductService productService, AppDbContext db) : ControllerBase
{
    
    [HttpGet]
    public async Task<ActionResult<List<ProductReadDTO>>> GetAll()
    {
        var products = await productService.GetAll();
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductReadDTO>> GetById(int id)
    {
        var product = await productService.GetById(id);
        if (product == null)
            return NotFound("Product topilmadi");
        return Ok(product);
    }
    
    [HttpPost]
    public async Task<ActionResult<string>> CreateProduct([FromBody] ProductCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var createP = await productService.Create(dto);
        return Ok(createP);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<bool>> UpdateProduct(int id, ProductUpdateDTO dto)
    {
        var updateP = await productService.Update(id, dto);
        if (!updateP) return NotFound();
        return NoContent();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var deleted = await productService.Delete(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
    
}