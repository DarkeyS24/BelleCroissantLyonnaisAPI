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
        public async Task<IActionResult> Index()
        {
            var appContextDB = _context.Customers.Include(c => c.membership);
            return View(await appContextDB.ToListAsync());
        }

        // GET: Customers/Details/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.membership)
                .FirstOrDefaultAsync(m => m.customer_id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("customer_id,full_name,age,gender,postal_code,email,phone_number,membership_id,join_date,last_purchase_date,total_spending,average_order_value,frequency,preferred_category,churned")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["membership_id"] = new SelectList(_context.Memberships, "membership_id", "membership_id", customer.membership_id);
            return View(customer);
        }


        // PUT: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("customer_id,full_name,age,gender,postal_code,email,phone_number,membership_id,join_date,last_purchase_date,total_spending,average_order_value,frequency,preferred_category,churned")] Customer customer)
        {
            if (id != customer.customer_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["membership_id"] = new SelectList(_context.Memberships, "membership_id", "membership_id", customer.membership_id);
            return View(customer);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.customer_id == id);
        }
    }
}
