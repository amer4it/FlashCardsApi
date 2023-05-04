using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlashCardsApi.Data;
using FlashCardsApi.Models;

namespace FlashCardsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTranslationsController : ControllerBase
    {
        private readonly FlashCardsContext _context;

        public ProductTranslationsController(FlashCardsContext context)
        {
            _context = context;
        }

        // GET: api/ProductTranslations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductTranslation>>> GetProductTranslations()
        {
          if (_context.ProductTranslations == null)
          {
              return NotFound();
          }
            return await _context.ProductTranslations.ToListAsync();
        }

        // GET: api/ProductTranslations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductTranslation>> GetProductTranslation(int id)
        {
          if (_context.ProductTranslations == null)
          {
              return NotFound();
          }
            var productTranslation = await _context.ProductTranslations.FindAsync(id);

            if (productTranslation == null)
            {
                return NotFound();
            }

            return productTranslation;
        }

        // PUT: api/ProductTranslations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductTranslation(int id, ProductTranslation productTranslation)
        {
            if (id != productTranslation.ID)
            {
                return BadRequest();
            }

            _context.Entry(productTranslation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductTranslationExists(id))
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

        // POST: api/ProductTranslations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductTranslation>> PostProductTranslation(ProductTranslation productTranslation)
        {
          if (_context.ProductTranslations == null)
          {
              return Problem("Entity set 'FlashCardsContext.ProductTranslations'  is null.");
          }
            _context.ProductTranslations.Add(productTranslation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductTranslation", new { id = productTranslation.ID }, productTranslation);
        }

        // DELETE: api/ProductTranslations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductTranslation(int id)
        {
            if (_context.ProductTranslations == null)
            {
                return NotFound();
            }
            var productTranslation = await _context.ProductTranslations.FindAsync(id);
            if (productTranslation == null)
            {
                return NotFound();
            }

            _context.ProductTranslations.Remove(productTranslation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductTranslationExists(int id)
        {
            return (_context.ProductTranslations?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
