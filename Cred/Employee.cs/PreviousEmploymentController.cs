using System.Threading.Tasks;
using Cred.dbcontex;
using Cred.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagement.Controllers
{
    public class PreviousEmploymentController : Controller
    {
        private readonly AppDbContext _context;

        public PreviousEmploymentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PreviousEmployment
        public async Task<IActionResult> Index()
        {
            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "Name");
            var employments = await _context.PreviousEmployments.Include(p => p.Employee).ToListAsync();
            return View(employments);
        }

        // POST: PreviousEmployment/Create
        [HttpPost]
        public async Task<IActionResult> Create(PreviousEmployment employment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Employment record created successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "Name");
            var employments = await _context.PreviousEmployments.Include(p => p.Employee).ToListAsync();
            return View("Index", employments);
        }

        // GET: PreviousEmployment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var employment = await _context.PreviousEmployments.FindAsync(id);
            if (employment == null) return NotFound();

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "Name", employment.EmployeeId);
            return View(employment);
        }

        // POST: PreviousEmployment/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, PreviousEmployment employment)
        {
            if (id != employment.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employment);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Employment record updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await EmploymentExistsAsync(id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "Name", employment.EmployeeId);
            return View(employment);
        }

        // GET: PreviousEmployment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var employment = await _context.PreviousEmployments
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employment == null) return NotFound();

            return View(employment);
        }

        // POST: PreviousEmployment/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employment = await _context.PreviousEmployments.FindAsync(id);
            if (employment != null)
            {
                _context.PreviousEmployments.Remove(employment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Employment record deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        // Reset form
        [HttpPost]
        public IActionResult Reset()
        {
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EmploymentExistsAsync(int id)
        {
            return await _context.PreviousEmployments.AnyAsync(e => e.Id == id);
        }
    }
}
