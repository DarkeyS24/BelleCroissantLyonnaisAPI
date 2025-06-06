using BelleCroissantLyonnaisAPI.AppContext;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BelleCroissantLyonnaisAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CustomerMatchingController : ControllerBase
    {
        private AppContextDB _context;

        public CustomerMatchingController(AppContextDB context)
        {
            _context = context;
        }

        // GET api/<CustomerMatchingController>/5
        [HttpGet("customers/{email:length(1,100)}")]
        public IActionResult Customers(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email cannot be null or empty.");
            }
            var login = _context.User_Login
                .FirstOrDefault(u => u.email.Equals(email));
            if (login == null)
            {
                return NotFound("No user found with the provided email.");
            }
            return Ok(login);
        }
    }
}
