using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BelleCroissantLyonnaisAPI.AppContext;
using BelleCroissantLyonnaisAPI.Models;

namespace BelleCroissantLyonnaisAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly AppContextDB _context;

        public CustomersController(AppContextDB context)
        {
            _context = context;
        }

        // GET: Customers
        [HttpGet]
        public IActionResult Index()
        {
            var appContextDB = _context.Customers.Include(c => c.membership).ToList();
            return Ok(appContextDB);
        }

        // GET: Customers/Details/5
        [HttpGet("{id:int}")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = _context.Customers
                .Include(c => c.membership)
                .FirstOrDefault(m => m.customer_id == id);
            if (customer == null)
            {
                return NotFound("Customer not founded");
            }

            return Ok(customer);
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            int id = _context.Customers.Max(c => c.customer_id);
            var newCustomer = _context.Customers.FirstOrDefault(c => c.customer_id == id);
            if (ModelState.IsValid)
            {
                customer.customer_id = newCustomer.customer_id + 1;
                _context.Add(customer);
                _context.SaveChanges();
                return Ok(customer);
            }
            return BadRequest("Customer not created");
        }


        // PUT: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id:int}")]
        public IActionResult Edit(int id, [Bind("customer_id,full_name,age,gender,postal_code,email,phone_number,membership_id,join_date,last_purchase_date,total_spending,average_order_value,frequency,preferred_category,churned")] Customer customer)
        {
            if (id != customer.customer_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(customer).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.customer_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(customer);
            }
            return BadRequest("Customer not updated");
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.customer_id == id);
        }
    }
}
