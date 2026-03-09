using Microsoft.AspNetCore.Mvc;
using SOS100_T7_BenefitsPortal.Models;
using SOS100_T7_BenefitsPortal.Services;

namespace SOS100_T7_BenefitsPortal.Controllers;

public class CategoriesController(CategoryService categoryService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var categories = await categoryService.GetAllAsync();
        return View(categories);
    }

    // CREATE
    public IActionResult Create() => View(new CategoryFormModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryFormModel model)
    {
        if (!ModelState.IsValid) return View(model);
        await categoryService.CreateAsync(model.Name);
        return RedirectToAction(nameof(Index));
    }

    // EDIT
    public async Task<IActionResult> Edit(int id)
    {
        var category = await categoryService.GetByIdAsync(id);
        if (category is null) return NotFound();
        return View(new CategoryFormModel { Id = category.Id, Name = category.Name });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryFormModel model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);
        await categoryService.UpdateAsync(model.Id, model.Name);
        return RedirectToAction(nameof(Index));
    }

    // DELETE
    public async Task<IActionResult> Delete(int id)
    {
        var category = await categoryService.GetByIdAsync(id);
        if (category is null) return NotFound();
        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await categoryService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
