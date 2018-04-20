using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplePMServices.Data;
using SimplePMServices.ViewModels;

namespace SimplePMServices.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ProjectMilestonesController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;


        public ProjectMilestonesController(ApplicationDbContext appDbContext)
        {

            _appDbContext = appDbContext;
        }

        // GET: api/values
        [HttpGet]
        public IQueryable<ProjectMilestone> Get()
        {

            return _appDbContext.ProjectMilestones.OrderBy( p => p.ProjectId ).OrderBy( p=> p.PhaseName );
        }
    }
}