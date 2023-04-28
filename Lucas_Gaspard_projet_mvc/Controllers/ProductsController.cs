using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lucas_Gaspard_projet_mvc.Data;
using Lucas_Gaspard_projet_mvc.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LucasGaspardprojetmvc.Data.Migrations;

namespace Lucas_Gaspard_projet_mvc.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            ApplicationUser current_user = await _userManager.GetUserAsync(User);
            ViewBag.UserType = current_user?.Type ?? "";
            return _context.Products != null ?
                        View(await _context.Products.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Products'  is null.");
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ApplicationUser current_user = await _userManager.GetUserAsync(User);
            ViewBag.UserType = current_user?.Type ?? "";
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Products/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ApplicationUser current_user = await _userManager.GetUserAsync(User);
            ViewBag.UserType = current_user?.Type ?? "";
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Titre,Fabricant,Prix,Info,Type")] Product products)
        {
            ApplicationUser current_user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid && (current_user.Type == products.Type || User.IsInRole("Administrator")))
            {
                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            ApplicationUser current_user = await _userManager.GetUserAsync(User);
            ViewBag.UserType = current_user?.Type ?? "";
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (current_user.Type != product.Type && !User.IsInRole("Administrator"))
            {
                return RedirectToAction(nameof(Index));
            }
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titre,Fabricant,Prix,Info,Type")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            ApplicationUser current_user = await _userManager.GetUserAsync(User);
            var oldProdType = await _context.Products.Where(p => p.Id == id).Select(p => p.Type).FirstOrDefaultAsync();
            if (ModelState.IsValid && oldProdType == product.Type && (current_user.Type == product.Type || User.IsInRole("Administrator")))
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            ApplicationUser current_user = await _userManager.GetUserAsync(User);

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (current_user.Type != product.Type && !User.IsInRole("Administrator"))
            {
                return RedirectToAction(nameof(Index));
            }
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }

            var product = await _context.Products.FindAsync(id);
            ApplicationUser current_user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid && _context.Products.Any(p => p.Type == product.Type) && (current_user.Type == product.Type || User.IsInRole("Administrator")))
            {
                if (product != null)
                {
                    _context.Products.Remove(product);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                }
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
