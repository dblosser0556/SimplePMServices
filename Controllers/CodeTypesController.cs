using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplePMServices.Data;
using SimplePMServices.Models.Entities;
using SimplePMServices.ViewModels;

namespace SimplePMServices.Controllers
{
    [Produces("application/json")]
    [Route("api/CodeTypes")]
    public class CodeTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CodeTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CodeTypes
        [HttpGet]
        public IEnumerable<CodeType> GetCodeTypes()
        {
            return _context.CodeTypes;
        }

        // GET: api/CodeTypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCodeType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var codeType = await _context.CodeTypes.SingleOrDefaultAsync(m => m.ID == id);

            if (codeType == null)
            {
                return NotFound();
            }

            return Ok(codeType);
        }

        // PUT: api/CodeTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCodeType([FromRoute] int id, [FromBody] CodeType codeType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != codeType.ID)
            {
                return BadRequest();
            }

            _context.Entry(codeType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CodeTypeExists(id))
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

        // POST: api/CodeTypes
        [HttpPost]
        public async Task<IActionResult> PostCodeType([FromBody] CodeType codeType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CodeTypes.Add(codeType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCodeType", new { id = codeType.ID }, codeType);
        }

        // DELETE: api/CodeTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCodeType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var codeType = await _context.CodeTypes.SingleOrDefaultAsync(m => m.ID == id);
            if (codeType == null)
            {
                return NotFound();
            }

            _context.CodeTypes.Remove(codeType);
            await _context.SaveChangesAsync();

            return Ok(codeType);
        }

        private bool CodeTypeExists(int id)
        {
            return _context.CodeTypes.Any(e => e.ID == id);
        }
    }
}