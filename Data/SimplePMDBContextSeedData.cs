using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SimplePMServices.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.Data
{
    

    public class SimplePMDBContextSeedData
    {
        private ApplicationDbContext _context;

        public SimplePMDBContextSeedData(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> SeedAminUser()
        {
            var user = new AppUser
            {
                UserName = "SimplePMAdmin",
                NormalizedUserName = "SimplePMAdmin",
                Email = "SimplePMAdmin@SimplePM.com",
                NormalizedEmail = "SimplePMAdmin@SimplePM.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var roleStore = new RoleStore<IdentityRole>(_context);

            if (!_context.Roles.Any(r => r.Name == "admin"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "admin", NormalizedName = "admin" });
            }

            if (!_context.Roles.Any(r => r.Name == "editPrograms"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "editPrograms", NormalizedName = "editPrograms" });
            }
            if (!_context.Roles.Any(r => r.Name == "editProjects")) { 
                await roleStore.CreateAsync(new IdentityRole { Name = "editProjects", NormalizedName = "editProjects" });
                
            }


            if (!_context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<AppUser>();
                var hashed = password.HashPassword(user, "password");
                user.PasswordHash = hashed;
                var userStore = new UserStore<AppUser>(_context);
                await userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, "admin");
                await userStore.AddToRoleAsync(user, "editPrograms");
                await userStore.AddToRoleAsync(user, "editProjects");
            }

            await _context.SaveChangesAsync();
            return new OkObjectResult("Account created");


        }
    }
}
