using ApplicationService.Data;
using ApplicationService.Filters;
using ApplicationService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var applications = await db.Applications
            .Include(a => a.Reviews)
            .ToListAsync();

        return Ok(applications);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var application = await db.Applications
            .Include(a => a.Reviews)
            .FirstOrDefaultAsync(a => a.Id == id);

        return application is null ? NotFound() : Ok(application);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Application application)
    {
        application.CreatedAt = DateTime.UtcNow;
        db.Applications.Add(application);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = application.Id }, application);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Application updatedApplication)
    {
        var application = await db.Applications.FindAsync(id);
        if (application is null)
            return NotFound();

        application.EmployeeName = updatedApplication.EmployeeName;
        application.BenefitId = updatedApplication.BenefitId;
        application.Status = updatedApplication.Status;

        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var application = await db.Applications.FindAsync(id);
        if (application is null)
            return NotFound();

        db.Applications.Remove(application);
        await db.SaveChangesAsync();

        return NoContent();
    }

    [ServiceFilter(typeof(ApiKeyFilter))]
    [HttpPost("{id}/review")]
    public async Task<IActionResult> ReviewApplication(int id, ApplicationReview review)
    {
        var application = await db.Applications.FindAsync(id);

        if (application == null)
            return NotFound();

        review.ApplicationId = id;
        review.ReviewedAt = DateTime.UtcNow;

        db.ApplicationReviews.Add(review);

        application.Status = review.Decision;

        await db.SaveChangesAsync();

        return Ok(review);
    }
}