using System;
using Microsoft.Owin;
using Owin;
using The_Book.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartupAttribute(typeof(The_Book.Startup))]
namespace The_Book
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if(!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var user = new ApplicationUser();
                byte[] imageData = new byte[0];
                user.UserName = "i.skngobese@live.co.za";
                user.Email = "i.skngobese@live.co.za";
                user.userPhoto = imageData;
                user.EmailConfirmed = true;
                string userPwd = "@AdminPass1*";
                var chkUser = userManager.Create(user, userPwd);
                if(chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "Admin");
                }
            }
            if(!roleManager.RoleExists("Manager"))
            {
                var role = new IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);
            }
            if(!roleManager.RoleExists("Teacher"))
            {
                var role = new IdentityRole();
                role.Name = "Teacher";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Student"))
            {
                var role = new IdentityRole();
                role.Name = "Student";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("GuestS"))
            {
                var role = new IdentityRole();
                role.Name = "GuestS";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("GuestT"))
            {
                var role = new IdentityRole();
                role.Name = "GuestT";
                roleManager.Create(role);
            }
        }
    }
}
