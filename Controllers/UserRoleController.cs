using IfRolesExample.Data;
using IfRolesExample.Repositories;
using IfRolesExample.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IfRolesExample.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRoleController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public UserRoleController(ApplicationDbContext context,
                                 UserManager<IdentityUser> userManager)
        {
            _db = context;
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            UserRepo userRepo = new UserRepo(_db);
            IEnumerable<UserVM> users = userRepo.GetAllUsers();

            return View(users);
        }

        public async Task<IActionResult> Detail(string userName,
                                                string message = "")
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);
            MyRegisteredUserRepo myRegisteredUserRepo = new MyRegisteredUserRepo(_db);

            var roles = await userRoleRepo.GetUserRolesAsync(userName);
            var name = myRegisteredUserRepo.GetUserName(userName);

            ViewBag.Message = message;
            ViewBag.UserName = name;

            return View(roles);
        }

        public ActionResult Create()
        {
            RoleRepo roleRepo = new RoleRepo(_db);
            ViewBag.RoleSelectList = roleRepo.GetRoleSelectList();


            UserRepo userRepo = new UserRepo(_db);
            ViewBag.UserSelectList = userRepo.GetUserSelectList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserRoleVM userRoleVM)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);

            if (ModelState.IsValid)
            {
                try
                {
                    var addUR =
                    await userRoleRepo.AddUserRoleAsync(userRoleVM.Email,
                                                        userRoleVM.RoleName);

                    string message = $"{userRoleVM.RoleName} permissions" +
                                     $" successfully added to " +
                                     $"{userRoleVM.Email}.";

                    return RedirectToAction("Detail", "UserRole",
                                      new
                                      {
                                          userName = userRoleVM.Email,
                                          message = message
                                      });
                }
                catch
                {
                    ModelState.AddModelError("", "UserRole creation failed.");
                    ModelState.AddModelError("", "The RoleName may exist " +
                                                 "for this user.");
                }
            }

            RoleRepo roleRepo = new RoleRepo(_db);
            ViewBag.RoleSelectList = roleRepo.GetRoleSelectList();

            UserRepo userRepo = new UserRepo(_db);
            ViewBag.UserSelectList = userRepo.GetUserSelectList();

            return View();
        }

        public async Task<IActionResult> Delete(string role, string email)
        {
            //UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);
            //var roles = await userRoleRepo.GetUserRolesAsync(role);

            var viewModel = new UserRoleVM
            {
                Email = email,
                RoleName = role
            };

            MyRegisteredUserRepo myRegisteredUserRepo = new MyRegisteredUserRepo(_db);

            ViewBag.UserName = myRegisteredUserRepo.GetUserName(email);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserRoleVM userRoleVM)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);

            if (ModelState.IsValid)
            {
                try
                {
                    bool repoPassed =
                             await userRoleRepo.RemoveUserRoleAsync(userRoleVM.Email, userRoleVM.RoleName);

                    if (repoPassed)
                    {
                        string message = "Role successfully removed.";
                        return RedirectToAction("Detail", "UserRole",
                                                   new { userName = userRoleVM.Email, message = message });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Role deletion failed.");
                    }
                }
                                catch
                {
                    return RedirectToAction("Detail", "UserRole",

                            "", "Role deletion failed.");
                }
            }
            return RedirectToAction("Detail", "UserRole",

                            "", "Role deletion failed."); ;
        }

    }
}
