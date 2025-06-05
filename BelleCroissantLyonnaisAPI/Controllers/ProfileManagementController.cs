using System.Net;
using BelleCroissantLyonnaisAPI.AppContext;
using BelleCroissantLyonnaisAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BelleCroissantLyonnaisAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ProfileManagementController : ControllerBase
    {
        private AppContextDB _context;
        public ProfileManagementController(AppContextDB context)
        {
            _context = context;
        }

        // GET: api/profile
        [HttpGet("profile/{id:int}")]
        public IActionResult Profile(int id)
        {
            return Ok(_context.UserLogin.Include(u => u.Addresses).FirstOrDefault(u => u.login_id.Equals(id)));
        }

        // PUT api/profile + Request body(update)
        [HttpPut("profile")]
        public IActionResult Profile(UserLogin login)
        {
            if (login == null || login.login_id <= 0)
            {
                return BadRequest("Invalid user login data.");
            }
            else
            {
                _context.Entry(login).State = EntityState.Modified;
                try
                {
                    _context.SaveChanges();
                    return Ok(login);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound("User not found or update failed.");
                }
            }
        }

        // GET api/addresses
        [HttpGet("addresses")]
        public IActionResult Addresses()
        {
            var address = _context.Delivery_Address.ToList();

            if (address == null)
            {
                return BadRequest("Error finding addresses.");
            }
            return Ok(address);
        }

        // POST api/addresses + Request body(save)
        [HttpPost("addresses")]
        public IActionResult Addresses(Delivery_Address address)
        {
            if (address == null || address.login_id <= 0 || string.IsNullOrEmpty(address.address))
            {
                return BadRequest("Invalid address data.");
            }else if (address.delivery_address_id > 0)
            {
                return BadRequest("Delivery Address Id field cannot content a value.");
            }
            _context.Delivery_Address.Add(address);
            _context.SaveChanges();
            return Ok("Address was successfully added");
        }

        // PUT api/addresses/5 + Request body(update)
        [HttpPut("addresses/{id:int}")]
        public IActionResult Addresses(Delivery_Address address, int id)
        {
            if (address == null || address.login_id <= 0 || string.IsNullOrEmpty(address.address))
            {
                return BadRequest("Invalid address data.");
            }else if (id != address.delivery_address_id)
            {
                return BadRequest("Address Ids doesn't match.");
            }
            _context.Entry(address).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(address);
        }

        // DELETE api/addresses/5
        [HttpDelete("addresses/{id:int}")]
        public IActionResult Addresses(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid address ID.");
            }
            var address = _context.Delivery_Address.Find(id);
            if (address == null)
            {
                return NotFound("Address not found.");
            }
            _context.Delivery_Address.Remove(address);
            _context.SaveChanges();
            return Ok("Address deleted successfully.");
        }
    }
}
