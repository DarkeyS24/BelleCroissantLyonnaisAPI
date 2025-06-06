using System.Security.Cryptography;
using System.Text;
using BelleCroissantLyonnaisAPI.AppContext;
using BelleCroissantLyonnaisAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BelleCroissantLyonnaisAPI.Controllers
{
    [ApiController]
    [Route("api/")]
    public class AuthenticationController : Controller
    {
        private AppContextDB _context;

        public AuthenticationController(AppContextDB context)
        {
            _context = context;
        }

        //[HttpGet("{id:int}")]
        //public IActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest("Id is null");
        //    }

        //    var login = _context.UserLogin
        //        .Include(c => c.Addresses)
        //        .FirstOrDefault(m => m.login_id == id);
        //    if (login == null)
        //    {
        //        return NotFound("Login not founded");
        //    }

        //    return Ok(login);
        //}

        [HttpPost("register")]
        public ActionResult Register(User_Login login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if the email already exists
                    var existingUser = _context.User_Login
                        .FirstOrDefault(u => u.email == login.email);

                    if (existingUser != null)
                    {
                        return BadRequest("Email already exists. Please use a different email address.");
                    }
                    else
                    {
                        // Add the new user login to the database
                        _context.User_Login.Add(login);
                        _context.SaveChanges();
                        // Optionally, you can redirect to a confirmation page or return a success message
                        return CreatedAtRoute("",new { id = login.login_id} ,login);
                    }
                }
                return NotFound("Invalid user model");
            }
            catch (SqlException e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("login")]
        public ActionResult Login(ValidateLogin login)
        {
                var existingUser = _context.User_Login
                       .FirstOrDefault(u => u.email.Equals(login.email) && u.password.Equals(login.password));

                if (existingUser != null)
                {
                    return Ok(existingUser);

                }
                else
                {
                    return BadRequest("Invalid user");
                }
        }

        [HttpPost("Forgot-password")]
        public ActionResult Forgot_Password(User_Login user)
        {
            var existingUser = _context.User_Login
                .FirstOrDefault(u => u.email.Equals(user.email));
            existingUser.security_answer = HashAnswer(existingUser.security_answer); // Hash the security answer

            if (existingUser != null)
            {
                // Here you would typically send an email with a reset link or code
                // For simplicity, we will just return a success message
                return Ok(existingUser);
            }
            else
            {
                return NotFound("Email not found.");
            }
        }

        private string HashAnswer(string answer)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(answer));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }


        [HttpPost("Reset-password")]
        public ActionResult Reset_Password(User_Login login)
        {
            var existingUser = _context.User_Login
                .FirstOrDefault(u => u.login_id == login.login_id && u.security_answer.Equals(login.security_answer));
            if (existingUser != null)
            {
                return Ok("The answer is correct");
            }
            else
            {
                return NotFound("Incorrect answer");
            }
        }

        public class ValidateLogin()
        {
            public string email { get; set; }
            public string password { get; set; }
        }
    }
}
