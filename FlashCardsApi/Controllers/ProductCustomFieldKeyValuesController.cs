using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlashCardsApi.Data;
using FlashCardsApi.Models;

namespace FlashCardsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCustomFieldKeyValuesController : ControllerBase
    {
        private readonly FlashCardsContext _context;

        public ProductCustomFieldKeyValuesController(FlashCardsContext context)
        {
            _context = context;
        }

        // GET: api/ProductCustomFieldKeyValues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCustomFieldKeyValue>>> GetProductCustomFieldKeyValues()
        {
          if (_context.ProductCustomFieldKeyValues == null)
          {
              return NotFound();
          }
            return await _context.ProductCustomFieldKeyValues.ToListAsync();
        }

        // GET: api/ProductCustomFieldKeyValues/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCustomFieldKeyValue>> GetProductCustomFieldKeyValue(int id)
        {
          if (_context.ProductCustomFieldKeyValues == null)
          {
              return NotFound();
          }
            var productCustomFieldKeyValue = await _context.ProductCustomFieldKeyValues.FindAsync(id);

            if (productCustomFieldKeyValue == null)
            {
                return NotFound();
            }

            return productCustomFieldKeyValue;
        }

        // PUT: api/ProductCustomFieldKeyValues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCustomFieldKeyValue(int id, ProductCustomFieldKeyValue productCustomFieldKeyValue)
        {
            if (id != productCustomFieldKeyValue.ID)
            {
                return BadRequest();
            }

            _context.Entry(productCustomFieldKeyValue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCustomFieldKeyValueExists(id))
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

        // POST: api/ProductCustomFieldKeyValues
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductCustomFieldKeyValue>> PostProductCustomFieldKeyValue(ProductCustomFieldKeyValue productCustomFieldKeyValue)
        {
          if (_context.ProductCustomFieldKeyValues == null)
          {
              return Problem("Entity set 'FlashCardsContext.ProductCustomFieldKeyValues'  is null.");
          }
            _context.ProductCustomFieldKeyValues.Add(productCustomFieldKeyValue);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductCustomFieldKeyValue", new { id = productCustomFieldKeyValue.ID }, productCustomFieldKeyValue);
        }

        // DELETE: api/ProductCustomFieldKeyValues/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCustomFieldKeyValue(int id)
        {
            if (_context.ProductCustomFieldKeyValues == null)
            {
                return NotFound();
            }
            var productCustomFieldKeyValue = await _context.ProductCustomFieldKeyValues.FindAsync(id);
            if (productCustomFieldKeyValue == null)
            {
                return NotFound();
            }

            _context.ProductCustomFieldKeyValues.Remove(productCustomFieldKeyValue);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductCustomFieldKeyValueExists(int id)
        {
            return (_context.ProductCustomFieldKeyValues?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
