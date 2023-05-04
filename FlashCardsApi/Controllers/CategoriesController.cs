using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlashCardsApi.Data;
using FlashCardsApi.Models;
using Microsoft.AspNetCore.Authorization;


namespace FlashCardsApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly FlashCardsContext _context;

        public CategoriesController(FlashCardsContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
          if (_context.Categories == null)
          {
              return NotFound();
          }
            return await _context.Categories.ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
          if (_context.Categories == null)
          {
              return NotFound();
          }
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // GET: api/Categories/5/products
        [HttpGet("{id}/products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetCategoryProducts(int id)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }
            
            // Fetch the products of that category
            var products = _context.Products.Where(pr => pr.CategoryId == category.ID).ToList();
            if (products == null)
            {
                return NotFound();
            }

            // Filter products by start date and duration  
            products = products.Where(prod =>
            DateTime.Compare(prod.StartDate, DateTime.Now) <= 0 &&
            DateTime.Compare(prod.StartDate.AddDays(Convert.ToDouble(prod.Duration)), DateTime.Now) >= 0).ToList();


            if (products == null)
            {
                return NotFound();
            }

            // Fetch products' complete data 
            foreach (Product prod in products)
            {
                prod.Category = _context.Categories.Where(ct => ct.ID == prod.CategoryId).First();
                prod.ProductCustomFieldKeysValues = _context.ProductCustomFieldKeyValues.Where(s => s.ProductId == prod.ID).ToList();
                prod.ProductTranslations = _context.ProductTranslations.Where(s => s.ProductId == prod.ID).ToList();
                var langs = UserLanguages.GetUserLanguages(Request);

                if (prod.ProductTranslations != null)
                {
                    if (langs == null)
                    {
                        prod.ProductTranslations = (ICollection<ProductTranslation>?)prod.ProductTranslations.First();
                    }
                    else
                    {

                        foreach (var lang in langs)
                        {

                            prod.ProductTranslations = (ICollection<ProductTranslation>)prod.ProductTranslations.Where(s =>
                                                        lang.ToUpper().Contains((s.Language == null || s.Language.LanguageCode == null) ? "" : s.Language.LanguageCode.ToUpper())).First();
                            if (prod.ProductTranslations.Count() != 0)
                                break;

                        }
                        if (prod.ProductTranslations.Count() == 0)
                            prod.ProductTranslations = (ICollection<ProductTranslation>)_context.ProductTranslations.Where(s => s.ProductId == prod.ID).First();

                    }

                    if (prod.ProductTranslations != null)
                        foreach (ProductTranslation pt in prod.ProductTranslations)
                        {
                            pt.Language = _context.Languages.Where(la => la.ID == pt.TranslationLanguageId).First();
                        }
                }
            }

            return products;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.ID)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
          if (_context.Categories == null)
          {
              return Problem("Entity set 'FlashCardsContext.Categories'  is null.");
          }
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.ID }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            category.Products = _context.Products.Where(pr => pr.CategoryId == category.ID).ToList();

            // Remove related products
            if (category.Products != null)
            {
                foreach (Product pr in category.Products)
                {

                    // Remove related product translations
                    if (pr.ProductTranslations != null)
                    {
                        foreach (ProductTranslation pt in pr.ProductTranslations)
                        {
                            _context.ProductTranslations.Remove(pt);

                        }
                    }

                    // Remove related key values 
                    if (pr.ProductCustomFieldKeysValues != null)
                    {
                        foreach (ProductCustomFieldKeyValue pkv in pr.ProductCustomFieldKeysValues)
                        {
                            _context.ProductCustomFieldKeyValues.Remove(pkv);

                        }
                    }

                    _context.Products.Remove(pr);

                }
            }


            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
