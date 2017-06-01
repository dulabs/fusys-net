using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fu.Module.Core.Models;
using Fu.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Fu.Module.Core.ViewModels.Users;
using AutoMapper;
using System.Collections.Generic;
using System;

namespace Fu.Module.Core.Controllers.Backend
{
    public class UsersController : BackendController
    {
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;
        private readonly IRepository<User> _UserRepository;

        public UsersController(IRepository<User> UserRepository, 
                               UserManager<User> UserManager,
                               RoleManager<Role> RoleManager) : base()
        {
            _UserManager = UserManager;
            _RoleManager = RoleManager;
            _UserRepository = UserRepository;

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<User, AddUserViewModel>().ReverseMap();
                cfg.CreateMap<User, EditUserViewModel>().ReverseMap();
                cfg.CreateMap<User, DetailsUserViewModel>().ReverseMap();
            });
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            List<User> model = await _UserRepository.Query().Where(q => q.IsDeleted.Equals(false)).ToListAsync();

            return View(model);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _UserManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            DetailsUserViewModel _UserViewModel = new DetailsUserViewModel();

            IMapper cfg = Mapper.Configuration.CreateMapper();

            _UserViewModel = cfg.Map<User, DetailsUserViewModel>(user, _UserViewModel);

            IList<String> _UserRoles = await _UserManager.GetRolesAsync(user);

            string RoleName = "";

            if (_UserRoles.Count > 0)
            {
                Role CurrentRole = await _RoleManager.FindByNameAsync(_UserRoles.SingleOrDefault());
                RoleName = CurrentRole.Name;
            }

            _UserViewModel.RoleName = RoleName;

            return View(_UserViewModel);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            AddUserViewModel model = new AddUserViewModel();

            model.Roles = ToSelectList(_RoleManager.Roles);

            return View(model);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccessFailedCount,ConcurrencyStamp,CreatedOn,Email,EmailConfirmed,FullName,IsDeleted,LockoutEnabled,LockoutEnd,NormalizedEmail,NormalizedUserName,PasswordHash,PhoneNumber,PhoneNumberConfirmed,SecurityStamp,TwoFactorEnabled,UpdatedOn,UserGuid,UserName")] AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Mapping 
                User _user = new Models.User();
                                    
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<AddUserViewModel, User>()
                        .IgnoreAllPropertiesWithAnInaccessibleSetter()
                        .ForMember(x => x.Id, y => y.Ignore());

                });

                _user = Mapper.Map(model, _user);

                await _UserManager.CreateAsync(_user);

                return RedirectToAction("Index");
            }
            
            model.Roles = ToSelectList(_RoleManager.Roles);

            return View(model);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            long RoleId = 0;

            if (id == null)
            {
                return NotFound();
            }

            var user = await _UserManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            EditUserViewModel _UserViewModel = new EditUserViewModel();

            IMapper cfg = Mapper.Configuration.CreateMapper();

            _UserViewModel = cfg.Map<User, EditUserViewModel>(user, _UserViewModel);

            IList<String> _UserRoles = await _UserManager.GetRolesAsync(user);
            
            if(_UserRoles.Count > 0)
            {
                Role CurrentRole = await _RoleManager.FindByNameAsync(_UserRoles.SingleOrDefault());
                RoleId = CurrentRole.Id;
            }    
           
            _UserViewModel.RoleId = RoleId;
  
            _UserViewModel.Roles = ToSelectList(_RoleManager.Roles);

            return View(_UserViewModel);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,AccessFailedCount,ConcurrencyStamp,CreatedOn,Email,EmailConfirmed,FullName,IsDeleted,LockoutEnabled,LockoutEnd,NormalizedEmail,NormalizedUserName,PasswordHash,PhoneNumber,PhoneNumberConfirmed,SecurityStamp,RoleId,TwoFactorEnabled,UpdatedOn,UserGuid,UserName")] EditUserViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                User _user = await _UserManager.FindByIdAsync(id.ToString());
                
                if(_user == null)
                {
                    return NotFound();
                }

                try
                {
                    long existingRoleId = 0;
                    string existingRole = "";
                    bool _RoleExists = false;
                    IList<string> _UserRoles = _UserManager.GetRolesAsync(_user).Result;

                    if(_UserRoles.Count > 0)
                    {
                        existingRole = _UserRoles.Single();
                        Role existingRoleEntity = await _RoleManager.FindByNameAsync(existingRole);
                        existingRoleId = existingRoleEntity.Id;
                        _RoleExists = true;
                    }



                    // Mapping 

                    Mapper.Initialize(cfg =>
                    {
                        cfg.CreateMap<EditUserViewModel, User>()
                            .IgnoreAllPropertiesWithAnInaccessibleSetter()
                            .ForMember(x => x.Id, y => y.Ignore());

                    });

                    _user = Mapper.Map(model, _user);

                    // save user date
                    await _UserManager.UpdateAsync(_user);
                    //await _UserRepository.SaveChangesAsync();

                    await _UserManager.UpdateSecurityStampAsync(_user);

                    if (model.RoleId != existingRoleId)
                    {
                        if (_RoleExists)
                        {
                            IdentityResult _RoleResult = await _UserManager.RemoveFromRoleAsync(_user, existingRole);

                            if (_RoleResult.Succeeded)
                            {
                                Role _Role = await _RoleManager.FindByIdAsync(model.RoleId.ToString());

                                if (_Role != null)
                                {
                                    IdentityResult _NewRoleResult = await _UserManager.AddToRoleAsync(_user, _Role.Name);

                                    if (_NewRoleResult.Succeeded)
                                    {
                                        return RedirectToAction("Index");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Role _Role = await _RoleManager.FindByIdAsync(model.RoleId.ToString());

                            if (_Role != null)
                            {
                                IdentityResult _NewRoleResult = await _UserManager.AddToRoleAsync(_user, _Role.Name);

                                if (_NewRoleResult.Succeeded)
                                {
                                    return RedirectToAction("Index");
                                }
                            }
                        }

                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(model.Id))
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

            model.Roles = ToSelectList(_RoleManager.Roles);

            return View(model);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _UserRepository.Query().SingleOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var user = await _UserRepository.Query().SingleOrDefaultAsync(m => m.Id == id);
            user.IsDeleted = true;
            _UserRepository.Update(user);
            await _UserRepository.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ChangePassword(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _UserRepository.Query().SingleOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            ChangePasswordViewModel model = new ChangePasswordViewModel();

            model.Id = user.Id;

            return View(model);
        }

        [HttpPost, ActionName("ChangePassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePasswordConfirmed(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _UserRepository.Query().SingleOrDefaultAsync(m => m.Id == id);

            return RedirectToAction("Index");
        }

        private bool UserExists(long id)
        {
            return _UserRepository.Query().Any(e => e.Id == id);
        }

        private List<SelectListItem> ToSelectList(IQueryable<Role> roles)
        {
            return roles.Select(r => new SelectListItem
                     {
                         Text = r.Name,
                         Value = r.Id.ToString()
                     }).ToList();
        }
    }
}
