using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductEntriesController(ApplicationDbContext db) : ControllerBase
    {
        private readonly ApplicationDbContext _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entries = await _db.ProductEntries.OrderByDescending(e => e.CreatedAt).ToListAsync();

            return Ok(entries);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entry = await _db.ProductEntries.FindAsync(id);

            if (entry is null) return NotFound();

            return Ok(entry);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductEntryCreateDto dto)
        {

        }
    }
}