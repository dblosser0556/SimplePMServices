using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplePMServices.Data;
using SimplePMServices.Models.Entities;

namespace SimplePMServices.Controllers
{
    [Produces("application/json")]
    [Route("api/GroupBudgets")]
    public class GroupBudgetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupBudgetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/GroupBudgets
        [EnableQuery]
        [HttpGet]
        public IQueryable<GroupBudget> GetGroupBudgets()
        {
            return _context.GroupBudgets.AsQueryable();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupBudget([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var groupBudget = await _context.GroupBudgets.SingleOrDefaultAsync(m => m.GroupBudgetId == id);

            if (groupBudget == null)
            {
                return NotFound();
            }

            return Ok(groupBudget);
        }

        // PUT: api/GroupBudgets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupBudget([FromRoute] int id, [FromBody] GroupBudget groupBudget)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != groupBudget.GroupBudgetId)
            {
                return BadRequest();
            }

            _context.Entry(groupBudget).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupBudgetExists(id))
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

        // POST: api/GroupBudgets
        [HttpPost]
        public async Task<IActionResult> PostGroupBudget([FromBody] GroupBudget groupBudget)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.GroupBudgets.Add(groupBudget);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroupBudget", new { id = groupBudget.GroupBudgetId }, groupBudget);
        }

        // DELETE: api/GroupBudgets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupBudget([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var groupBudget = await _context.GroupBudgets.SingleOrDefaultAsync(m => m.GroupBudgetId == id);
            if (groupBudget == null)
            {
                return NotFound();
            }

            _context.GroupBudgets.Remove(groupBudget);
            await _context.SaveChangesAsync();

            return Ok(groupBudget);
        }

        private bool GroupBudgetExists(int id)
        {
            return _context.GroupBudgets.Any(e => e.GroupBudgetId == id);
        }
    }
}