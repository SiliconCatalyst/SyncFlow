using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductEntriesController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        // GET: api/ProductEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductEntry>>> GetProductEntries()
        {
            return await _context.ProductEntries.ToListAsync();
        }

        // GET: api/ProductEntries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductEntry>> GetProductEntry(int id)
        {
            var entry = await _context.ProductEntries.FindAsync(id);

            if (entry == null)
            {
                return NotFound();
            }

            return entry;
        }

        // POST: api/ProductEntries
        [HttpPost]
        public async Task<ActionResult<ProductEntry>> PostProductEntry(ProductEntry entry)
        {
            // CRITICAL: Always reset ID to 0 so database auto-generates it
            // This handles offline sync where negative IDs are sent
            entry.Id = 0;

            // Set timestamps server-side
            entry.EntryDateTime = DateTime.Now;
            entry.CreatedAt = DateTime.Now;

            _context.ProductEntries.Add(entry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductEntry), new { id = entry.Id }, entry);
        }

        // PUT: api/ProductEntries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductEntry(int id, ProductEntry entry)
        {
            if (id != entry.Id)
            {
                return BadRequest();
            }

            _context.Entry(entry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductEntryExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/ProductEntries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductEntry(int id)
        {
            var entry = await _context.ProductEntries.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            _context.ProductEntries.Remove(entry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductEntryExists(int id)
        {
            return _context.ProductEntries.Any(e => e.Id == id);
        }
    }
}