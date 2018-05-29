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
    [Route("api/[controller]")]
    public class CharacteristicsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CharacteristicsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Characteristics
        [EnableQuery]
        [HttpGet]
        public IQueryable<Characteristic> GetCharacteristics()
        {
            var characteristics = _context.Characteristics
                             .OrderBy(c => c.Lft);

          
            return characteristics.AsQueryable();

        }

        // GET: api/Characteristics/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharacteristic([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var characteristic = await _context.Characteristics.FindAsync(id);

            if (characteristic == null)
            {
                return NotFound();
            }

            return Ok(characteristic);
        }

        // PUT: api/Characteristics/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharacteristic([FromRoute] int id, [FromBody] Characteristic characteristic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != characteristic.CharacteristicId)
            {
                return BadRequest();
            }

            _context.Entry(characteristic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await RebuildHierarchyOrder();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CharacteristicExists(id))
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

        // POST: api/Characteristics
        [HttpPost]
        public async Task<IActionResult> PostCharacteristic([FromBody] Characteristic characteristic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Characteristics.Add(characteristic);
            await _context.SaveChangesAsync();
            await RebuildHierarchyOrder();
            return CreatedAtAction("GetCharacteristic", new { id = characteristic.CharacteristicId }, characteristic);
        }

        // DELETE: api/Characteristics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacteristic([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var characteristic = await _context.Characteristics.FindAsync(id);
            if (characteristic == null)
            {
                return NotFound();
            }

            _context.Characteristics.Remove(characteristic);
            await _context.SaveChangesAsync();
            await RebuildHierarchyOrder();
            return Ok(characteristic);
        }

        private bool CharacteristicExists(int id)
        {
            return _context.Characteristics.Any(e => e.CharacteristicId == id);
        }

        private async Task<ActionResult> RebuildHierarchyOrder()
        {
            try
            {
                List<Characteristic> characteristics = new List<Characteristic>();

                // get an updated list of the hierarchy order by hierarchy.
                var sql = "With CharacteristicList AS " +
                    "(SELECT Parent.CharacteristicId, Parent.ParentId, Parent.Lft, " +
                    "Parent.Rgt, Parent.CharacteristicName, Parent.CharacteristicDesc, 1 as Level " +
                    "FROM Characteristics AS Parent " +
                    "WHERE Parent.ParentId is NULL OR Parent.ParentId = 0 " +
                    "UNION ALL " +
                    "SELECT Child.CharacteristicId, Child.ParentId, Child.Lft, " +
                    "Child.Rgt, Child.CharacteristicName, Child.CharacteristicDesc, CL.Level + 1 " +
                    "FROM Characteristics AS Child " +
                    "INNER JOIN CharacteristicList AS CL " +
                    "ON Child.ParentId = CL.CharacteristicId " +
                    "WHERE Child.ParentId is NOT NULL or Child.ParentId > 0) " +
                    "SELECT * FROM CharacteristicList";





                var results = await _context.Characteristics.FromSql(sql).ToListAsync();

                int right = 1;
                foreach (var item in results)
                {
                    int left = right;
                    if (item.ParentId == 0)
                    {
                        right = await RebuildTree(item, left, 1);
                    }

                }
                return Ok();
                //return groups;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }


        //count the children 
        private async Task<int> RebuildTree(Characteristic characteristic, int left, int level)
        {
            int right = left + 1;

            var results = await _context.Characteristics.FromSql(
                "Select * from characteristics WHERE ParentId={0}", characteristic.CharacteristicId).ToListAsync();

            // go through and get all children
            foreach (Characteristic item in results)
            {
                right = await RebuildTree(item, right, level + 1);
            }
            _context.Attach(characteristic);
            characteristic.Lft = left;
            characteristic.Rgt = right;
            characteristic.Level = level;
            await _context.SaveChangesAsync();

            return right + 1;
        }
    }
}
