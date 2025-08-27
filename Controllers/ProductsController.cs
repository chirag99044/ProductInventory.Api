using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductInventory.Api.Data;
using ProductInventory.Api.Dtos;
using ProductInventory.Api.Models;

namespace ProductInventory.Api.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;        
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductDto>>> Get(
        string? search = null,
        int? categoryId = null,
        string? sort = null, 
        int page = 1,
        int pageSize = 10)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0 || pageSize > 100) pageSize = 10;


            var query = _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .AsQueryable();


            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search));


            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId);


            // Sorting
            query = sort?.ToLower() switch
            {
                "name:desc" => query.OrderByDescending(p => p.Name),
                "price:asc" => query.OrderBy(p => p.Price),
                "price:desc" => query.OrderByDescending(p => p.Price),
                "quantity:asc" => query.OrderBy(p => p.Quantity),
                "quantity:desc" => query.OrderByDescending(p => p.Quantity),
                _ => query.OrderBy(p => p.Name)
            };


            var total = await query.CountAsync();
            var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDto(
            p.Id, p.Name, p.Price, p.Quantity,
            p.CategoryId, p.Category!.Name, p.Created))
            .ToListAsync();


            return new PagedResult<ProductDto> { Items = items, TotalCount = total };
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var p = await _context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (p is null) return NotFound();
            return new ProductDto(p.Id, p.Name, p.Price, p.Quantity, p.CategoryId, p.Category!.Name, p.Created);
        }


        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateUpdateProductDto dto)
        {
            if (!await _context.categories.AnyAsync(c => c.Id == dto.CategoryId))
                return BadRequest($"Invalid CategoryId {dto.CategoryId}");


            var p = new Product
            {
                Name = dto.Name.Trim(),
                Price = dto.Price,
                Quantity = dto.Quantity,
                CategoryId = dto.CategoryId
            };


            _context.Products.Add(p);
            await _context.SaveChangesAsync();


            var created = await _context.Products.Include(x => x.Category).FirstAsync(x => x.Id == p.Id);
            var result = new ProductDto(created.Id, created.Name, created.Price, created.Quantity, created.CategoryId, created.Category!.Name, created.Created);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] CreateUpdateProductDto dto)
        {
            var p = await _context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (p is null) return NotFound();


            if (!await _context.categories.AnyAsync(c => c.Id == dto.CategoryId))
                return BadRequest($"Invalid CategoryId {dto.CategoryId}");


            p.Name = dto.Name.Trim();
            p.Price = dto.Price;
            p.Quantity = dto.Quantity;
            p.CategoryId = dto.CategoryId;


            await _context.SaveChangesAsync();


            return new ProductDto(p.Id, p.Name, p.Price, p.Quantity, p.CategoryId, p.Category!.Name, p.Created);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _context.Products.FindAsync(id);
            if (p is null) return NotFound();
            _context.Products.Remove(p);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
