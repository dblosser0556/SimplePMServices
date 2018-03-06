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
    [Route("api/FixedPriceTypes")]
    public class FixedPriceTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FixedPriceTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FixedPriceTypes
        [HttpGet]
        public IEnumerable<FixedPriceType> GetFixedPriceTypes()
        {
            return _context.FixedPriceTypes;
        }

        // GET: api/FixedPriceTypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFixedPriceType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fixedPriceType = await _context.FixedPriceTypes.SingleOrDefaultAsync(m => m.FixedPriceTypeId == id);

            if (fixedPriceType == null)
            {
                return NotFound();
            }

            return Ok(fixedPriceType);
        }

        // PUT: api/FixedPriceTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFixedPriceType([FromRoute] int id, [FromBody] FixedPriceType fixedPriceType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fixedPriceType.FixedPriceTypeId)
            {
                return BadRequest();
            }

            _context.Entry(fixedPriceType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FixedPriceTypeExists(id))
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

        // POST: api/FixedPriceTypes
        [HttpPost]
        public async Task<IActionResult> PostFixedPriceType([FromBody] FixedPriceType fixedPriceType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.FixedPriceTypes.Add(fixedPriceType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFixedPriceType", new { id = fixedPriceType.FixedPriceTypeId }, fixedPriceType);
        }

        // DELETE: api/FixedPriceTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFixedPriceType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fixedPriceType = await _context.FixedPriceTypes.SingleOrDefaultAsync(m => m.FixedPriceTypeId == id);
            if (fixedPriceType == null)
            {
                return NotFound();
            }

            _context.FixedPriceTypes.Remove(fixedPriceType);
            await _context.SaveChangesAsync();

            return Ok(fixedPriceType);
        }

        private bool FixedPriceTypeExists(int id)
        {
            return _context.FixedPriceTypes.Any(e => e.FixedPriceTypeId == id);
        }
    }
}