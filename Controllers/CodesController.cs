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
using Microsoft.AspNet.OData;


namespace SimplePMServices.Controllers
{
    [Produces("application/json")]
    [Route("api/Codes")]
    public class CodesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CodesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Codes
        [EnableQuery]
        [HttpGet]
        public IQueryable<CodeWithType> GetCodes()
        {
            List<CodeWithType> codeWithTypes = new List<CodeWithType>();

            var query = from c in _context.Codes
                        join t in _context.CodeTypes on c.CodeTypeID equals t.ID
                        select new
                        {
                            ID = c.ID,
                            CodeTypeID = c.CodeTypeID,
                            CodeName = c.CodeName,
                            CodeDesc = c.CodeDesc,
                            TypeName = t.TypeName,
                            TypeDesc = t.TypeDesc
                        };

            foreach (var item in query)
            {
                var codeWithType = new CodeWithType
                {
                    ID = item.ID,
                    CodeTypeID = item.CodeTypeID,
                    CodeName = item.CodeName,
                    CodeDesc = item.CodeDesc,
                    TypeName = item.TypeName,
                    TypeDesc = item.TypeDesc
                };
                codeWithTypes.Add(codeWithType);
            }
        
            return codeWithTypes.AsQueryable();

           
        }

        // GET: api/Codes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCode([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var code = await _context.Codes.SingleOrDefaultAsync(m => m.ID == id);

            if (code == null)
            {
                return NotFound();
            }

            return Ok(code);
        }

        // PUT: api/Codes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCode([FromRoute] int id, [FromBody] Code code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != code.ID)
            {
                return BadRequest();
            }

            _context.Entry(code).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CodeExists(id))
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

        // POST: api/Codes
        [HttpPost]
        public async Task<IActionResult> PostCode([FromBody] Code code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Codes.Add(code);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCode", new { id = code.ID }, code);
        }

        // DELETE: api/Codes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCode([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var code = await _context.Codes.SingleOrDefaultAsync(m => m.ID == id);
            if (code == null)
            {
                return NotFound();
            }

            _context.Codes.Remove(code);
            await _context.SaveChangesAsync();

            return Ok(code);
        }

        private bool CodeExists(int id)
        {
            return _context.Codes.Any(e => e.ID == id);
        }
    }
}