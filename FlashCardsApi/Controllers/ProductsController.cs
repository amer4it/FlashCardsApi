using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlashCardsApi.Data;
using FlashCardsApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace FlashCardsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly FlashCardsContext _context;

        public ProductsController(FlashCardsContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
          if (_context.Products == null)
          {
              return NotFound();
          }

           var products = await _context.Products.ToListAsync();
          
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
                                                        lang.ToUpper().Contains((s.Language == null || s.Language.LanguageCode == null)?"":s.Language.LanguageCode.ToUpper())).First();
                            if (prod.ProductTranslations.Count() != 0 )
                                break;

                        }
                        if (prod.ProductTranslations.Count() == 0)
                            prod.ProductTranslations = (ICollection<ProductTranslation>)_context.ProductTranslations.Where(s => s.ProductId == prod.ID).First();

                    }
                    
                    if(prod.ProductTranslations != null)
                        foreach (ProductTranslation pt in prod.ProductTranslations)
                        {
                            pt.Language = _context.Languages.Where(la => la.ID == pt.TranslationLanguageId).First();
                        }
                }

            }

           //return await _context.Products.ToListAsync();
           return products;

        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            // Check product's start date and duration 
            if (!(DateTime.Compare(product.StartDate, DateTime.Now) <= 0 &&
            DateTime.Compare(product.StartDate.AddDays(Convert.ToDouble(product.Duration)), DateTime.Now) >= 0))
                return NotFound();


            // Fetch products' complete data 
            product.Category = _context.Categories.Where(ct => ct.ID == product.CategoryId).First();
            product.ProductCustomFieldKeysValues = _context.ProductCustomFieldKeyValues.Where(s => s.ProductId == product.ID).ToList();
            product.ProductTranslations = _context.ProductTranslations.Where(s => s.ProductId == product.ID).ToList();

            var langs = UserLanguages.GetUserLanguages(Request);

            if (product.ProductTranslations != null)
            {
                if (langs == null)
                {
                    product.ProductTranslations = (ICollection<ProductTranslation>?)product.ProductTranslations.First();
                }
                else
                {

                    foreach (var lang in langs)
                    {

                        product.ProductTranslations = (ICollection<ProductTranslation>)product.ProductTranslations.Where(s =>
                                                    lang.ToUpper().Contains((s.Language == null || s.Language.LanguageCode == null) ? "" : s.Language.LanguageCode.ToUpper())).First();
                        if (product.ProductTranslations.Count() != 0)
                            break;

                    }
                    if (product.ProductTranslations.Count() == 0)
                        product.ProductTranslations = (ICollection<ProductTranslation>)_context.ProductTranslations.Where(s => s.ProductId == product.ID).First();

                }

                if (product.ProductTranslations != null)
                    foreach (ProductTranslation pt in product.ProductTranslations)
                    {
                        pt.Language = _context.Languages.Where(la => la.ID == pt.TranslationLanguageId).First();
                    }
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ID)
            {
                return BadRequest();
            }

            //_context.Entry(product).State = EntityState.Modified;

            _context.Update(product);

            //if (product.ProductCustomFieldKeysValues != null)
            //{
            //    _context.Attach(product.ProductCustomFieldKeysValues);
            //    _context.Entry(product.ProductCustomFieldKeysValues).State = EntityState.Modified;
            //}
            //if (product.ProductTranslations != null)
            //{
            //    _context.Attach(product.ProductTranslations);
            //    _context.Entry(product.ProductTranslations).State = EntityState.Modified;
            //}
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
          if (_context.Products == null)
          {
              return Problem("Entity set 'FlashCardsContext.Products'  is null.");
          }


              if (product.ProductTranslations != null)
                {
                    foreach(ProductTranslation pt in product.ProductTranslations)
                    {
                        if (pt.ProductId != product.ID) 
                            pt.ProductId = product.ID;
                    }
                }

            if (product.ProductCustomFieldKeysValues != null)
            {
                foreach (ProductCustomFieldKeyValue pkv in product.ProductCustomFieldKeysValues)
                {
                    if (pkv.ProductId != product.ID)
                        pkv.ProductId = product.ID;
                }
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ID }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }


            // Remove related product translations
            if (product.ProductTranslations != null)
                {
                    foreach(ProductTranslation pt in product.ProductTranslations)
                    {
                    _context.ProductTranslations.Remove(pt);

                    }
            }

            // Remove related key values 
            if (product.ProductCustomFieldKeysValues != null)
            {
                foreach (ProductCustomFieldKeyValue pkv in product.ProductCustomFieldKeysValues)
                {
                    _context.ProductCustomFieldKeyValues.Remove(pkv);

                }
            }


            _context.Products.Remove(product);


            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
