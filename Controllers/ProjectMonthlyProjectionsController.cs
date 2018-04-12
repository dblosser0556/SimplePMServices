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
    public class ProjectMonthlyProjectionsController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;


        public ProjectMonthlyProjectionsController(ApplicationDbContext appDbContext)
        {

            _appDbContext = appDbContext;
        }

        // GET: api/values
        [HttpGet]
        public IQueryable<ProjectMonthlyProjection> Get()
        {

            return _appDbContext.ProjectMonthlyProjections.OrderBy(m => m.Month);
        }
    }
}