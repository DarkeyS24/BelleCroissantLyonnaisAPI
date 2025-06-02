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
    public class OrdersController : Controller
    {
        private readonly AppContextDB _context;

        public OrdersController(AppContextDB context)
        {
            _context = context;
        }

        // GET: Orders
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var appContextDB = _context.Orders.Include(o => o.channel).Include(o => o.customer).Include(o => o.payment).Include(o => o.product);
            return View(await appContextDB.ToListAsync());
        }

        // GET: Orders/Details/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound($"Id:{id} it's an inexistent order");
            }

            var order = await _context.Orders
                .Include(o => o.channel)
                .Include(o => o.customer)
                .Include(o => o.payment)
                .Include(o => o.product)
                .FirstOrDefaultAsync(m => m.order_id == id);
            if (order == null)
            {
                return NotFound("Order not found");
            }

            return View(order);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("order_id,customer_id,date,time,product_id,quantity,price,payment_method_id,channel_id,store_id,promotion_id,discount_amount")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["channel_id"] = new SelectList(_context.Channels, "channel_id", "channel_id", order.channel_id);
            ViewData["customer_id"] = new SelectList(_context.Customers, "customer_id", "customer_id", order.customer_id);
            ViewData["payment_method_id"] = new SelectList(_context.Payment_Methods, "payment_method_id", "payment_method_id", order.payment_method_id);
            ViewData["product_id"] = new SelectList(_context.Products, "product_id", "product_id", order.product_id);
            return View(order);
        }

        // PUT: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("order_id,customer_id,date,time,product_id,quantity,price,payment_method_id,channel_id,store_id,promotion_id,discount_amount")] Order order)
        {
            if (id != order.order_id)
            {
                return NotFound("JSON id doesn't match with the parameter id");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.order_id))
                    {
                        return NotFound($"Order with id: {order.order_id} doesn't exist");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["channel_id"] = new SelectList(_context.Channels, "channel_id", "channel_id", order.channel_id);
            ViewData["customer_id"] = new SelectList(_context.Customers, "customer_id", "customer_id", order.customer_id);
            ViewData["payment_method_id"] = new SelectList(_context.Payment_Methods, "payment_method_id", "payment_method_id", order.payment_method_id);
            ViewData["product_id"] = new SelectList(_context.Products, "product_id", "product_id", order.product_id);
            return View(order);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.order_id == id);
        }
    }
}
