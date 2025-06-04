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
        public IActionResult Index()
        {
            var appContextDB = _context.Orders.Include(o => o.channel).Include(o => o.customer).Include(o => o.payment).Include(o => o.product).ToList();
            return Ok(appContextDB);
        }

        // GET: Orders/Details/5
        [HttpGet("{id:int}")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound($"Id:{id} it's an inexistent order");
            }

            var order = _context.Orders
                .Include(o => o.channel)
                .Include(o => o.customer)
                .Include(o => o.payment)
                .Include(o => o.product)
                .FirstOrDefault(m => m.order_id == id);
            if (order == null)
            {
                return NotFound("Order not found");
            }

            return Ok(order);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Create(Order order)
        {
            int id = _context.Orders.Max(o => o.order_id);
            var newOrder = _context.Orders.FirstOrDefault(o => o.order_id == id);
            if (ModelState.IsValid)
            {
                order.order_id = newOrder.order_id + 1; // Assuming order_id is auto-incremented
                _context.Add(order);
                _context.SaveChanges();
                return Ok(order);
            }
            return BadRequest("Order can't be created");
        }

        // PUT: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id:int}")]
        public IActionResult Edit(Order order, int id)
        {
            if (id != order.order_id)
            {
                return NotFound("JSON id doesn't match with the parameter id");
            }
            else if (!OrderExists(order.order_id))
            {
                return NotFound($"Order with id: {order.order_id} doesn't exist");
            }
            else
            {
                try
                {
                    _context.Entry(order).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Ok(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest("Concurrency error: the order was modified by another user. Please reload the order and try again.");
                }
            }


                
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.order_id == id);
        }
    }
}
