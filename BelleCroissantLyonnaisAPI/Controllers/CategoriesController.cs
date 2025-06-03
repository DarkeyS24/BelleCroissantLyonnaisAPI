using BelleCroissantLyonnaisAPI.AppContext;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BelleCroissantLyonnaisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        AppContextDB _context;

        public CategoriesController(AppContextDB contextDB)
        {
            _context = contextDB;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Categories.Select(c => c.category_name).ToList());
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id:int}")]
        public string Get(int id)
        {
            return "value";
        }
    }
}
