﻿using PayPal.Data;
using PayPal.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class RoleRepo
{
    private readonly ApplicationDbContext _db;

    public RoleRepo(ApplicationDbContext context)
    {
        this._db = context;
        CreateInitialRole();
    }

    public IEnumerable<RoleVM> GetAllRoles()
    {
        var roles =
            _db.Roles.Select(r => new RoleVM
            {
                RoleName = r.Name
            });

        return roles;
    }

    public RoleVM GetRole(string roleName)
    {


        var role = _db.Roles.Where(r => r.Name == roleName)
                            .FirstOrDefault();

        if (role != null)
        {
            return new RoleVM() { RoleName = role.Name };
        }
        return null;
    }

    public bool CreateRole(string roleName)
    {
        bool isSuccess = true;

        try
        {
            _db.Roles.Add(new IdentityRole
            {
                Id = roleName.ToLower(),
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            });
            _db.SaveChanges();
        }
        catch (Exception)
        {
            isSuccess = false;
        }

        return isSuccess;
    }

    public SelectList GetRoleSelectList()
    {
        var roles = GetAllRoles().Select(r => new
        SelectListItem
        {
            Value = r.RoleName,
            Text = r.RoleName
        });

        var roleSelectList = new SelectList(roles,
                                           "Value",
                                           "Text");
        return roleSelectList;
    }

    public void CreateInitialRole()
    {
        const string ADMIN = "Admin";

        var role = GetRole(ADMIN);

        if (role == null)
        {
            CreateRole(ADMIN);
        }
    }


    public string DeleteRole(string roleName)
    {
        string result = "";

        try
        {
            var role = _db.Roles.Where(r => r.Name == roleName)
                                     .FirstOrDefault();

            if (role != null)
            {
                if(_db.UserRoles.Any(ur => ur.RoleId == role.Name.ToLower()))
                {
                    result = "Role cannot be deleted because it is assigned to a user.";
                    return result;
                }
                _db.Roles.Remove(role);
                _db.SaveChanges();
                return "Role Deleted Successfully"; 
            }
        }
        catch (Exception ex)
        {
            result = ex.Message;
        }

        return result;
    }
    // Logic for role deletion can be included here.
}
