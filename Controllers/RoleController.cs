﻿using PayPal.Data;
using PayPal.Repositories;
using PayPal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PayPal.Controllers
{
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RoleController(ApplicationDbContext db)
        {
            _db = db;
        }

        public ActionResult Index(string message)
        {
            RoleRepo roleRepo = new RoleRepo(_db);
            ViewBag.Message = message ?? "";
            MyRegisteredUserRepo registeredUserRepo = new MyRegisteredUserRepo(_db);
            string userName = User.Identity.Name;
            string name = registeredUserRepo.GetUserName(userName);
            ViewBag.UserName = name;
            return View(roleRepo.GetAllRoles());
        }

        public ActionResult Create()
        {
            MyRegisteredUserRepo registeredUserRepo = new MyRegisteredUserRepo(_db);
            string userName = User.Identity.Name;
            string name = registeredUserRepo.GetUserName(userName);
            ViewBag.UserName = name;
            return View();
        }

        [HttpPost]
        public ActionResult Create(RoleVM roleVM)
        {
            MyRegisteredUserRepo registeredUserRepo = new MyRegisteredUserRepo(_db);
            string userName = User.Identity.Name;
            string name = registeredUserRepo.GetUserName(userName);
            ViewBag.UserName = name;
            if (ModelState.IsValid)
            {
                RoleRepo roleRepo = new RoleRepo(_db);
                bool isSuccess =
                    roleRepo.CreateRole(roleVM.RoleName);

                if (isSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState
                    .AddModelError("", "RoleName creation failed.");
                    ModelState
                    .AddModelError("", "The role may already" +
                                       " exist.");
                }
            }

            return View(roleVM);
        }

        [HttpGet]
        public ActionResult Delete(string roleName)
        {
            MyRegisteredUserRepo registeredUserRepo = new MyRegisteredUserRepo(_db);
            string userName = User.Identity.Name;
            string name = registeredUserRepo.GetUserName(userName);
            ViewBag.UserName = name;
            RoleRepo roleRepo = new RoleRepo(_db);

            return View(roleRepo.GetRole(roleName));
        }

        [HttpPost]
        public ActionResult Delete(RoleVM roleVM)
        {
            RoleRepo roleRepo = new RoleRepo(_db);
            string repoMessage = roleRepo.DeleteRole(roleVM.RoleName);

            if(repoMessage.Contains("Successfully"))
            {
                return RedirectToAction("Index", new { message = repoMessage });
            }
            else
            {
                ModelState.AddModelError("", "Role deletion failed");
                ModelState.AddModelError("", repoMessage);

                return View(roleVM);
            }
        }





    }
}
