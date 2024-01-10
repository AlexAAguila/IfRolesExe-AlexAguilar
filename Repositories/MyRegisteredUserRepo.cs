using IfRolesExample.Data;
using IfRolesExample.Models;
using IfRolesExample.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace IfRolesExample.Repositories
{
    public class MyRegisteredUserRepo
    {
        private readonly ApplicationDbContext _db;

        public MyRegisteredUserRepo(ApplicationDbContext context)
        {
            this._db = context;

        }
        public void CreateUser(string email, string firstname, string lastName)
        {
            MyRegisteredUser registerUser = new MyRegisteredUser()
            {
                Email = email,
                FirstName = firstname,
                LastName = lastName
            };
            _db.MyRegisteredUsers.Add(registerUser);
            _db.SaveChanges();
        }

        // Get all roles of a specific user.
        public string? GetUserName(string email)
        {
            var user =  _db.MyRegisteredUsers.FirstOrDefault(u => u.Email == email);
            return user.FirstName+ " " +user.LastName;
           
        }
    }
}
