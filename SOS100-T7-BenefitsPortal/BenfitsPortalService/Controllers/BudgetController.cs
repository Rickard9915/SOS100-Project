using BenfitsPortal_Api.Data;
using BenfitsPortal_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BenfitsPortal_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BudgetController(AppDbContext db) : ControllerBase
{
    // GET budget
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await db.BudgetLimits.ToListAsync());

    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var budget = await db.BudgetLimits.FindAsync(id);
        return budget is null ? NotFound() : Ok(budget);
    }

    // POST budget
    [HttpPost]
    public async Task<IActionResult> Create(BudgetLimit budget)
    {
        db.BudgetLimits.Add(budget);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = budget.Id }, budget);
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, BudgetLimit budget)
    {
        if (id != budget.Id) return BadRequest();
        db.Entry(budget).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE bugdet
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var budget = await db.BudgetLimits.FindAsync(id);
        if (budget is null) return NotFound();
        db.BudgetLimits.Remove(budget);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
