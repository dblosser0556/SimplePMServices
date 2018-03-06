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
    [Route("api/ResourceTypes")]
    public class ResourceTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResourceTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ResourceTypes
        [HttpGet]
        public IEnumerable<ResourceType> GetResourceTypes()
        {
            return _context.ResourceTypes;
        }

        // GET: api/ResourceTypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResourceType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resourceType = await _context.ResourceTypes.SingleOrDefaultAsync(m => m.ResourceTypeId == id);

            if (resourceType == null)
            {
                return NotFound();
            }

            return Ok(resourceType);
        }

        // PUT: api/ResourceTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResourceType([FromRoute] int id, [FromBody] ResourceType resourceType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != resourceType.ResourceTypeId)
            {
                return BadRequest();
            }

            _context.Entry(resourceType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceTypeExists(id))
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

        // POST: api/ResourceTypes
        [HttpPost]
        public async Task<IActionResult> PostResourceType([FromBody] ResourceType resourceType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ResourceTypes.Add(resourceType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResourceType", new { id = resourceType.ResourceTypeId }, resourceType);
        }

        // DELETE: api/ResourceTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResourceType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resourceType = await _context.ResourceTypes.SingleOrDefaultAsync(m => m.ResourceTypeId == id);
            if (resourceType == null)
            {
                return NotFound();
            }

            _context.ResourceTypes.Remove(resourceType);
            await _context.SaveChangesAsync();

            return Ok(resourceType);
        }

        private bool ResourceTypeExists(int id)
        {
            return _context.ResourceTypes.Any(e => e.ResourceTypeId == id);
        }
    }
}