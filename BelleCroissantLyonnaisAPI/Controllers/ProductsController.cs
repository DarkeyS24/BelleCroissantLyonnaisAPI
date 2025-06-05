using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BelleCroissantLyonnaisAPI.AppContext;
using BelleCroissantLyonnaisAPI.Models;

namespace BelleCroissantLyonnaisAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly AppContextDB _context;

        public ProductsController(AppContextDB context)
        {
            _context = context;
        }

        // GET: Products
        [HttpGet]
        public List<Product> Index()
        {
            var appContextDB = _context.Products.Include(p => p.category).ToList();
            return  appContextDB;
        }

        // GET: Products/Details/5
        [HttpGet("{id:int}")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound($"invalid product id: {id}");
            }

            var product = _context.Products
                .Include(p => p.category)
                .FirstOrDefault(m => m.product_id == id);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            return Ok(product);
        }


        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Create(Product product)
        {
            int id = _context.Products.Max(o => o.product_id);
            var newProduct = _context.Products.FirstOrDefault(o => o.product_id == id);
            if (newProduct != null)
            {
                product.product_id = newProduct.product_id + 1;
                _context.Add(product);
                _context.SaveChanges();
                return Ok(product);
            }
            return BadRequest("Error adding product");
        }

        // PUT: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id:int}")]
        public IActionResult Edit(Product product, int id)
        {
            if (id != product.product_id)
            {
                return NotFound("Id's doesn't match");
            }

                try
                {
                    _context.Entry(product).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.product_id))
                    {
                        return NotFound("Product not found");
                    }
                    else
                    {
                        throw;
                    }
                }
            return Ok(product);
        }


        // POST: Products/Delete/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.product_id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            _context.SaveChanges();
            return Ok(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.product_id == id);
        }
    }
}
