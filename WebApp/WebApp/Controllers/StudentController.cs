using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.ViewModels.Student;

namespace WebApp.Controllers;

public class StudentController : Controller
{
    public async Task<IActionResult> Index()
    {
        if(context.Student != null)
        {
            var model = new SearchStudent();
            model.Students = context.Student.Select(x => new StudentDetail
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Address = x.Address,
                Score = x.Score
            }).OrderBy(x => x.FirstName).ToList();
            return View(model);
        }
        
        return Problem("Entity set 'ApplicationDbContext.Student'  is null.");
    }

    [HttpPost]
    public async Task<IActionResult> Index(SearchStudent model)
    {
        if (context.Student != null)
        {
            model.Students = context.Student.Where(x =>
                x.FirstName.Contains(model.Criteria)
                || x.LastName.Contains(model.Criteria)
                || x.Address.Contains(model.Criteria)
                || x.Score.Contains(model.Criteria))
            .Select(x => new StudentDetail
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Address = x.Address,
                Score = x.Score
            }).OrderBy(x => x.FirstName).ToList();
            return View(model);
        }

        return Problem("Entity set 'ApplicationDbContext.Student'  is null.");
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || context.Student == null)
        {
            return NotFound();
        }

        var student = await context.Student
            .FirstOrDefaultAsync(m => m.Id == id);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Address,Score")] Student student)
    {
        if (ModelState.IsValid)
        {
            context.Add(student);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(student);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || context.Student == null)
        {
            return NotFound();
        }

        var student = await context.Student.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }
        return View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Address,Score")] Student student)
    {
        if (id != student.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(student);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(student.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(student);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || context.Student == null)
        {
            return NotFound();
        }

        var student = await context.Student
            .FirstOrDefaultAsync(m => m.Id == id);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (context.Student == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Student'  is null.");
        }
        var student = await context.Student.FindAsync(id);
        if (student != null)
        {
            context.Student.Remove(student);
        }
        
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool StudentExists(int id)
    {
      return (context.Student?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    private readonly ApplicationDbContext context;

    public StudentController(ApplicationDbContext context)
    {
        this.context = context;
    }

}
