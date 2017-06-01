using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fu.Module.Core.Data;
using Fu.Module.Core.Models;
using Fu.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Fu.Module.Core.Controllers.Backend
{
    public class RolesController : BackendController
    {
        private readonly IRepository<Role> _RoleRepository;

        private readonly RoleManager<Role> _RoleManager;

        public RolesController(IRepository<Role> RoleRepository, RoleManager<Role> RoleManager)
        {
            _RoleRepository = RoleRepository;
            _RoleManager = RoleManager;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            return View(await _RoleRepository.Query().ToListAsync());
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roles = await _RoleRepository.Query().SingleOrDefaultAsync(m => m.Id == id);
            if (roles == null)
            {
                return NotFound();
            }

            return View(roles);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NormalizedName")] Role roles)
        {
            if (ModelState.IsValid)
            {
                //_RoleRepository.Add(roles);
                //await _RoleRepository.SaveChangesAsync();
                await _RoleManager.CreateAsync(roles);
                return RedirectToAction("Index");
            }
            return View(roles);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roles = await _RoleRepository.Query().SingleOrDefaultAsync(m => m.Id == id);

            if (roles == null)
            {
                return NotFound();
            }
            return View(roles);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,NormalizedName")] Role roles)
        {
            if (id != roles.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    Role _Role = await _RoleManager.FindByIdAsync(roles.Id.ToString());
                    _Role.Name = roles.Name;
                    await _RoleManager.UpdateAsync(_Role);
                    //_RoleRepository.Update(roles);
                    //await _RoleRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RolesExists(roles.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(roles);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roles = await _RoleRepository.Query().SingleOrDefaultAsync(m => m.Id == id);
            if (roles == null)
            {
                return NotFound();
            }

            return View(roles);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var roles = await _RoleRepository.Query().SingleOrDefaultAsync(m => m.Id == id);

            await _RoleManager.DeleteAsync(roles);

            //_RoleRepository.Remove(roles);
            //await _RoleRepository.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private bool RolesExists(long id)
        {
            return _RoleRepository.Query().Any(e => e.Id == id);
        }
    }
}
