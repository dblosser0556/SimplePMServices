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
    [Route("api/VendorInvoices")]
    public class VendorInvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendorInvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/VendorInvoices
        [HttpGet]
        public IEnumerable<VendorInvoice> GetInvoices()
        {
            return _context.Invoices;
        }

        // GET: api/VendorInvoices/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVendorInvoice([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vendorInvoice = await _context.Invoices.SingleOrDefaultAsync(m => m.VendorInvoiceId == id);

            if (vendorInvoice == null)
            {
                return NotFound();
            }

            return Ok(vendorInvoice);
        }

        // PUT: api/VendorInvoices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVendorInvoice([FromRoute] int id, [FromBody] VendorInvoice vendorInvoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vendorInvoice.VendorInvoiceId)
            {
                return BadRequest();
            }

            _context.Entry(vendorInvoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorInvoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return AcceptedAtAction("GetVendorInvoice", vendorInvoice);
        }

        // POST: api/VendorInvoices
        [HttpPost]
        public async Task<IActionResult> PostVendorInvoice([FromBody] VendorInvoice vendorInvoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Invoices.Add(vendorInvoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVendorInvoice", new { id = vendorInvoice.VendorInvoiceId }, vendorInvoice);
        }

        // DELETE: api/VendorInvoices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVendorInvoice([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vendorInvoice = await _context.Invoices.SingleOrDefaultAsync(m => m.VendorInvoiceId == id);
            if (vendorInvoice == null)
            {
                return NotFound();
            }

            _context.Invoices.Remove(vendorInvoice);
            await _context.SaveChangesAsync();

            return Ok(vendorInvoice);
        }

        private bool VendorInvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.VendorInvoiceId == id);
        }
    }
}