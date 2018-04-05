using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplePMServices.Data;
using SimplePMServices.Models.Entities;

namespace SimplePMServices.Controllers
{
    [Produces("application/json")]
    [Route("api/Milestones")]
    public class MilestonesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MilestonesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Milestones
        [HttpGet]
        public IEnumerable<Milestone> GetMilestones()
        {
            return _context.Milestones;
        }

        // GET: api/Milestones/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMilestone([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var milestone = await _context.Milestones.SingleOrDefaultAsync(m => m.MilestoneId == id);

            if (milestone == null)
            {
                return NotFound();
            }

            return Ok(milestone);
        }

        // PUT: api/Milestones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMilestone([FromRoute] int id, [FromBody] Milestone milestone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != milestone.MilestoneId)
            {
                return BadRequest();
            }

            _context.Entry(milestone).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MilestoneExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Milestones
        [HttpPost]
        public async Task<IActionResult> PostMilestone([FromBody] Milestone milestone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Milestones.Add(milestone);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMilestone", new { id = milestone.MilestoneId }, milestone);
        }

        // DELETE: api/Milestones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMilestone([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var milestone = await _context.Milestones.SingleOrDefaultAsync(m => m.MilestoneId == id);
            if (milestone == null)
            {
                return NotFound();
            }

            _context.Milestones.Remove(milestone);
            await _context.SaveChangesAsync();

            return Ok(milestone);
        }

        private bool MilestoneExists(int id)
        {
            return _context.Milestones.Any(e => e.MilestoneId == id);
        }
    }
}