using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPG_BD.Data;
using RPG_BD.Models;

namespace RPG_BD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ItemsController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/items
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Item>>> Get([FromQuery] string? name)
    {
        var query = _db.Items.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(i => EF.Functions.Like(i.Name, $"%{name}%"));

        var list = await query.OrderBy(i => i.Name).ToListAsync();
        return Ok(list);
    }

    // GET: api/items/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Item>> GetById(int id)
    {
        var item = await _db.Items.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    // POST: api/items
    [HttpPost]
    public async Task<ActionResult<Item>> Create(Item input)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // check unique name
        if (await _db.Items.AnyAsync(i => i.Name == input.Name))
            return Conflict(new { message = "Um item com esse nome já existe." });

        _db.Items.Add(input);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = input.Id }, input);
    }

    // PUT: api/items/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Item input)
    {
        if (id != input.Id) return BadRequest(new { message = "Id da rota diferente do corpo." });
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var item = await _db.Items.FindAsync(id);
        if (item == null) return NotFound();

        // update fields
        item.Name = input.Name;
        item.Price = input.Price;
        item.Rarity = input.Rarity;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase) == true)
        {
            return Conflict(new { message = "Nome duplicado." });
        }

        return NoContent();
    }

    // DELETE: api/items/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Items.FindAsync(id);
        if (item == null) return NotFound();

        _db.Items.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
