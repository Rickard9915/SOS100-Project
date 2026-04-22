using ApplicationService.Data;
using ApplicationService.Dtos;
using ApplicationService.Filters;
using ApplicationService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace ApplicationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController(AppDbContext db, IHttpClientFactory httpClientFactory) : ControllerBase
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
    public async Task<IActionResult> Create([FromBody] Application application)
    {
        application.CreatedAt = DateTime.UtcNow;
        db.Applications.Add(application);
        await db.SaveChangesAsync();

        var httpClient = httpClientFactory.CreateClient();

        await httpClient.PostAsJsonAsync("http://localhost:5008/api/Notification", new
        {
            userId = application.UserId,
            applicationId = application.Id,
            status = "Pending",
            message = "Din ansökan har skickats in och väntar på granskning"
        });

        return CreatedAtAction(nameof(GetById), new { id = application.Id }, application);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Application updatedApplication)
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
    public async Task<IActionResult> ReviewApplication(int id, [FromBody] ReviewApplicationDto reviewDto)
    {
        var application = await db.Applications.FindAsync(id);

        if (application == null)
            return NotFound();

        var review = new ApplicationReview
        {
            ApplicationId = id,
            ReviewedBy = reviewDto.ReviewedBy,
            Comment = reviewDto.Comment,
            Decision = reviewDto.Decision,
            ReviewedAt = DateTime.UtcNow
        };

        db.ApplicationReviews.Add(review);

        application.Status = reviewDto.Decision;

        await db.SaveChangesAsync();

        var httpClient = httpClientFactory.CreateClient();

        await httpClient.PostAsJsonAsync("http://localhost:5008/api/Notification", new
        {
            userId = application.UserId,
            applicationId = application.Id,
            status = application.Status,
            message = application.Status == "Approved"
                ? "Ansökan godkändes"
                : "Ansökan avslogs"
        });

        return Ok(new
        {
            message = "Review saved successfully",
            applicationId = id,
            status = application.Status,
            reviewedBy = review.ReviewedBy,
            comment = review.Comment,
            decision = review.Decision,
            reviewedAt = review.ReviewedAt
        });
    }
}