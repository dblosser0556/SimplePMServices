using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

using SimplePMServices.Data;
using SimplePMServices.Models.Entities;
using SimplePMServices.ViewModels;
using SimplePMServices.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SimplePMServices.Controllers
{
    //[Route("api/Accounts")]
    [Route("api/Accounts")]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager, IMapper mapper, ApplicationDbContext appDbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        // POST api/accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]LoggedInUser model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                var userIdentity = new AppUser();
                userIdentity.Email = model.CurrentUser.email;
                userIdentity.FirstName = model.CurrentUser.FirstName;
                userIdentity.LastName = model.CurrentUser.LastName;
                userIdentity.UserName = model.CurrentUser.UserName;


                //var result = await _userManager.CreateAsync(userIdentity, model.CurrentUser.password);

                var userStore = new UserStore<AppUser>(_appDbContext);
                var result = await userStore.CreateAsync(userIdentity);
               

                if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));


                foreach (UserRole role in model.Roles)
                {
                    if (role.Selected)
                    {
                        await userStore.AddToRoleAsync(userIdentity, role.RoleName);

                    }
                }
                await _appDbContext.SaveChangesAsync();

                return new OkObjectResult("Account created");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody]LoggedInUser item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var user = await _appDbContext.AppUsers.FirstOrDefaultAsync(p => p.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            user.LastName = item.CurrentUser.LastName;
            user.FirstName = item.CurrentUser.FirstName;
            user.Email = item.CurrentUser.email;
            user.UserName = item.CurrentUser.UserName;
            
            if (user.PasswordHash != item.CurrentUser.password)
            {
                var password = new PasswordHasher<AppUser>();
                var hashed = password.HashPassword(user, item.CurrentUser.password);
                user.PasswordHash = hashed;
            }


            // remove any roles that have been removed from the user.
            IList<string> userCurRoles = await _userManager.GetRolesAsync(user);
            if (userCurRoles != null)
            {
                foreach (string userCurRole in userCurRoles)
                {
                    if (!item.Roles.Any(r => r.RoleName == userCurRole  && r.Selected))
                    {
                        await _userManager.RemoveFromRoleAsync(user, userCurRole);
                    }
                }   
            }

            // update the roles that where assigned passed to the user.
            if (item.Roles != null )
            {
                //add the user roles
                foreach (var role in item.Roles)
                {
                    if (role.Selected)
                    {
                        if (!userCurRoles.Any(r => r == role.RoleName))
                            await _userManager.AddToRoleAsync(user, role.RoleName);
                    }
                }
            }

            //save the changes.
            _appDbContext.AppUsers.Update(user);

            // work on roles
            int count = await _appDbContext.SaveChangesAsync();
            return new OkObjectResult("User updated");

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]

        public IActionResult Delete(long id)
        {
            var phase = _appDbContext.Phases.FirstOrDefault(p => p.PhaseId == id);
            if (phase == null)
            {
                return NotFound();
            }

            _appDbContext.Phases.Remove(phase);
            _appDbContext.SaveChanges();

            return new OkObjectResult("Phase deleted");
        }

        // GET: api/accounts/users
        [HttpGet(template: "users")]
        public async Task<IEnumerable<LoggedInUser>> GetUsers()
        {

            List<AppUser> appUsers = await _appDbContext.AppUsers.ToListAsync();

            List<LoggedInUser> loggedInUsers = new List<LoggedInUser>();

            foreach (AppUser appUser in appUsers)
            {
                LoggedInUser loggedInUser = new LoggedInUser();
                loggedInUser = await GetLoggedInUser(appUser.UserName);
                loggedInUsers.Add(loggedInUser);
            }
            return loggedInUsers;
        }

        // GET: api/accounts/roles
        [HttpGet(template: "roles")]
        public async Task<IEnumerable<string>> GetRoles()
        {

            List<IdentityRole> roles = await _appDbContext.Roles.ToListAsync();

            List<string> _roles = new List<string>();

            roles.ForEach(r => _roles.Add(r.Name));

            return _roles;
        }

        // GET: api/accounts/roles
        [HttpGet(template: "role/{roleName}")]
        public async Task<IEnumerable<LoggedInUser>> GetUserByRole(string roleName)
        {

            IList<AppUser> appUsers = await _userManager.GetUsersInRoleAsync(roleName);

            List<LoggedInUser> loggedInUsers = new List<LoggedInUser>();

            foreach (AppUser appUser in appUsers)
            {
                LoggedInUser loggedInUser = new LoggedInUser();
                loggedInUser = await GetLoggedInUser(appUser.UserName);
                loggedInUsers.Add(loggedInUser);
            }
            return loggedInUsers;
        }



        [HttpGet(template: "user/{userName}")]
        public async Task<IActionResult> GetUser(string userName)
        {


            var loggedInUser = await GetLoggedInUser(userName);

            if (loggedInUser == null)
            {
                return BadRequest();
            }


            return Ok(loggedInUser);

        }

        private async Task<LoggedInUser> GetLoggedInUser(string userName)
        {
            // get the user to verifty
            var currentUser = await _userManager.FindByNameAsync(userName);

            if (currentUser == null) return null;

            var user = new User() { UserId = currentUser.Id, LastName = currentUser.LastName, FirstName = currentUser.FirstName, UserName = currentUser.UserName, email = currentUser.Email, password = currentUser.PasswordHash };

            // Get a list of the user roles from the userManager
            IList<string> _userCurRoles =  await _userManager.GetRolesAsync(currentUser);


            // Get a list of all the roles.
            List<IdentityRole> _roles = await _appDbContext.Roles.ToListAsync();


            //return a list of all the roles and whether the logged in user has that role.
            List<UserRole> userRoles = new List<UserRole>();
            foreach (IdentityRole _role in _roles)
            {
                UserRole userRole = new UserRole();
                userRole.RoleName = _role.Name;
                userRole.Selected = _userCurRoles.Any(r => r == _role.Name);
                userRoles.Add(userRole);
            }

            var loggedInUser = new LoggedInUser();
            loggedInUser.CurrentUser = user;
            loggedInUser.Roles = userRoles;
            return loggedInUser;
        }




    }






}
