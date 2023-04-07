using Lucas_Gaspard_projet_mvc.Data;
using Lucas_Gaspard_projet_mvc.Data.Model;
using LucasGaspardprojetmvc.Data.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Lucas_Gaspard_projet_mvc.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly List<string> acceptedTypes = new() { "Carrosserie", "Peinture", "Moteur" };
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            List<ApplicationUser> users = await _context.Users.ToListAsync();
            List<(string, ApplicationUser)> usersWithRoles = new();
            foreach (ApplicationUser u in users)
            {
                if (_context.UserRoles.Any(ur => ur.UserId == u.Id))
                {
                    usersWithRoles.Add((_context.Roles.First(r => r.Id == _context.UserRoles.First(ur => ur.UserId == u.Id).RoleId).Name, u));
                }
                else
                {
                    usersWithRoles.Add(("", u));
                }
            }
            return _context.Users != null ?
                        View(usersWithRoles) :
                        Problem("Entity set 'ApplicationDbContext.Users'  is null.");
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var Users = await _context.Users.FindAsync(id);
            if (Users == null)
            {
                return NotFound();
            }
            return View(Users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Type")] ApplicationUser UserType)
        {
            if (ModelState.IsValid && acceptedTypes.Any(t => t == UserType.Type)) { 
            try
            {
                ApplicationUser user = await _context.Users.FindAsync(id);
                //IdentityUserRole<string> test = new()
                //{
                //    UserId = UserType.Id,
                //    RoleId = _context.Roles.First(r => r.Name == UserType.UserName).Id
                //};
                user.Type = UserType.Type;
                _context.Update(user);
                //_context.Update(test);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
            }
            return View(UserType);
        }
    }
}