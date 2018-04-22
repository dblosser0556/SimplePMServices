using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using SimplePMServices.Data;
using SimplePMServices.Models.Entities;
using System.Threading.Tasks;
using System;
using SimplePMServices.Helpers;
using SimplePMServices.ViewModels;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplePMServices.Controllers
{
    //[Authorize(Policy = "ApiUser")]
    [Route("api/[controller]")]
    public class PhasesController : Controller
    {

        private readonly ApplicationDbContext _appDbContext;
        

        public PhasesController(ApplicationDbContext appDbContext)
        {
           
            _appDbContext = appDbContext;
        }

        // GET: api/values
        [HttpGet]
        public IQueryable<Phase> Get()
        {
           
            return _appDbContext.Phases;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var phase = _appDbContext.Phases.FirstOrDefault(p => p.PhaseId == id);
            if (phase == null)
            {
                return NotFound();
            }
            return new ObjectResult(phase);
        }

        

        // POST api/phases
        [HttpPost]
        public IActionResult Create([FromBody]Phase phase)
        {
            try
            {
                

                _appDbContext.Phases.Add(phase);
                _appDbContext.SaveChanges();



                return new OkObjectResult("'Phase created'");
            }
            catch (Exception e)
            {
                throw e;

            }
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]Phase item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var phase = _appDbContext.Phases.FirstOrDefault(p => p.PhaseId == id);
            if (phase == null)
            {
                return NotFound();
            }

            phase.PhaseName = item.PhaseName;
            phase.PhaseDesc = item.PhaseDesc;
            phase.Order = item.Order;

            _appDbContext.Phases.Update(phase);
            _appDbContext.SaveChanges();
            return new OkObjectResult("Phase updated");

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _appDbContext.Dispose();
            }
            base.Dispose(disposing);
        }

       
    }
}
