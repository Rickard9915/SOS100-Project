using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SOS100_T7_BenefitsPortal.Models;
using SOS100_T7_BenefitsPortal.Services;

namespace SOS100_T7_BenefitsPortal.Controllers;

public class BenefitsController(BenefitService benefitService, CategoryService categoryService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var benefits = await benefitService.GetBenefitsAsync();
        return View(benefits);
    }

    public async Task<IActionResult> Details(int id)
    {
        var benefit = await benefitService.GetByIdAsync(id);
        if (benefit is null) return NotFound();
        return View(benefit);
    }

    public async Task<IActionResult> ByCategory(int id)
    {
        var category = await categoryService.GetByIdAsync(id);
        if (category is null) return NotFound();

        var benefits = await benefitService.GetByCategoryAsync(id);
        ViewBag.Category = category;
        return View(benefits);
    }

    // CREATE
    public async Task<IActionResult> Create()
    {
        await PopulateCategoryDropdown();
        return View(new BenefitFormModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BenefitFormModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCategoryDropdown();
            return View(model);
        }
        await benefitService.CreateAsync(model);
        return RedirectToAction(nameof(Index));
    }

    // EDIT
    public async Task<IActionResult> Edit(int id)
    {
        var benefit = await benefitService.GetByIdAsync(id);
        if (benefit is null) return NotFound();

        await PopulateCategoryDropdown(benefit.CategoryId);
        return View(new BenefitFormModel
        {
            Id = benefit.Id,
            Title = benefit.Title,
            Description = benefit.Description,
            Value = benefit.Value,
            CategoryId = benefit.CategoryId
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BenefitFormModel model)
    {
        if (id != model.Id) return BadRequest();

        if (!ModelState.IsValid)
        {
            await PopulateCategoryDropdown(model.CategoryId);
            return View(model);
        }
        await benefitService.UpdateAsync(model);
        return RedirectToAction(nameof(Index));
    }

    // DELETE
    public async Task<IActionResult> Delete(int id)
    {
        var benefit = await benefitService.GetByIdAsync(id);
        if (benefit is null) return NotFound();
        return View(benefit);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await benefitService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateCategoryDropdown(int? selectedId = null)
    {
        var categories = await categoryService.GetAllAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedId);
    }
}
