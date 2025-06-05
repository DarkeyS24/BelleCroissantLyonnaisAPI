using System.Net.Mail;
using BelleCroissantLyonnaisAPI.AppContext;
using BelleCroissantLyonnaisAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BelleCroissantLyonnaisAPI.Controllers
{
    [ApiController]
    [Route("api/")]
    public class SubscriptionController : Controller
    {
        private AppContextDB _context;

        public SubscriptionController(AppContextDB context)
        {
            _context = context;
        }

        // POST: SubscriptionController/Create
        [HttpPost("subscribe")]
        public IActionResult Subscribe(Mailing_Subscription mailing)
        {
            if (mailing == null)
            {
                return BadRequest("Invalid subscription data.");
            }
                // Check if the user is already subscribed
                var existingSubscription = _context.Mailing_Subscription
                    .FirstOrDefault(s => s.login_id == mailing.login_id);
                if (existingSubscription != null)
                {
                    if (existingSubscription.is_subscribed)
                    {
                        return BadRequest("You are already subscribed to this mailing list.");
                    }
                    else
                    {
                    existingSubscription.is_subscribed = true;
                        _context.Entry(existingSubscription).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();
                        return Ok("Subscription successful.");
                    }
            }
            else
            {
                _context.Mailing_Subscription.Add(mailing);
                _context.SaveChanges();
                return Ok("Subscription successful.");
            }
        }

        // POST: SubscriptionController/Edit/5
        [HttpPost("unsubscribe")]
        public IActionResult Unsubscribe(Mailing_Subscription mailing)
        {
            if (mailing == null)
            {
                return BadRequest("Invalid subscription data.");
            }
            
                // Check if the user is already subscribed
                var existingSubscription = _context.Mailing_Subscription
                    .FirstOrDefault(s => s.login_id == mailing.login_id);
                if (existingSubscription != null)
                {
                    if (existingSubscription.is_subscribed == false)
                    {
                        return BadRequest("You are already unsubscribed from this mailing list.");
                    }
                    else
                    {
                        existingSubscription.is_subscribed = false;
                        _context.Entry(existingSubscription).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();
                        return Ok("Subscription successfully updated.");
                    }
            }
            else
            {
                return NotFound("Unsubscription can be executed because you're not subscribe to this mailing list.");

            }
        }
    }
}
