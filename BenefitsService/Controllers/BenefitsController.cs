using BenefitsService.Data;
using BenefitsService.Filters;
using BenefitsService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BenefitsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BenefitsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await db.Benefits.Include(b => b.Category).ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var benefit = await db.Benefits.Include(b => b.Category).FirstOrDefaultAsync(b => b.Id == id);
        return benefit is null ? NotFound() : Ok(benefit);
    }

    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetByCategory(int categoryId) =>
        Ok(await db.Benefits.Where(b => b.CategoryId == categoryId).ToListAsync());

    [HttpPost]
    [TypeFilter<ApiKeyFilter>]
    public async Task<IActionResult> Create(Benefit benefit)
    {
        db.Benefits.Add(benefit);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = benefit.Id }, benefit);
    }

    [HttpPut("{id}")]
    [TypeFilter<ApiKeyFilter>]
    public async Task<IActionResult> Update(int id, Benefit benefit)
    {
        if (id != benefit.Id) return BadRequest();

        db.Entry(benefit).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [TypeFilter<ApiKeyFilter>]
    public async Task<IActionResult> Delete(int id)
    {
        var benefit = await db.Benefits.FindAsync(id);
        if (benefit is null) return NotFound();

        db.Benefits.Remove(benefit);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
