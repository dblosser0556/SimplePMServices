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
        public async Task<IActionResult> Post([FromBody]RegistrationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userIdentity = _mapper.Map<AppUser>(model);

                var result = await _userManager.CreateAsync(userIdentity, model.Password);

                if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

                await _appDbContext.SaveChangesAsync();

                return new OkObjectResult("Account created");
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        // GET: api/accounts/users
        [HttpGet(template: "users")]
        public async Task<IEnumerable<AppUser>> GetUsers()
        {

            List<AppUser> appUsers = await _appDbContext.AppUsers.ToListAsync();
            return appUsers;
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
            IList<string> _roles =  await _userManager.GetRolesAsync(currentUser);

            List<string> userRoles = new List<string>();

            foreach (string _role in _roles)
            {
                userRoles.Add(_role);
            }

            var loggedInUser = new LoggedInUser();
            loggedInUser.CurrentUser = user;
            loggedInUser.Roles = userRoles;
            return loggedInUser;
        }




    }






}
