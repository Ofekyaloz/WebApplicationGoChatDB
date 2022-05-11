#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationGoChat.Data;
using WebApplicationGoChat.Models;

namespace WebApplicationGoChat.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly WebApplicationGoChatContext _context;
        public ContactsController(WebApplicationGoChatContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            return Json(UsersController._users.Find(m => m.Username == userId).Contacts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            List<Contact> contacts = UsersController._users.Find(m => m.Username == userId).Contacts;
            var contact = contacts.Find(m => m.id == id);

            if (contact == null)
            {
                return NotFound();
            }

            return Json(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,server,last,lastdate")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return Json(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,name,server,last,lastdate")] Contact contact)
        {
            if (id != contact.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.id))
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
            return Json(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var contact = await _context.Contact.FindAsync(id);
            _context.Contact.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(string id)
        {
            return _context.Contact.Any(e => e.id == id);
        }
    }
}
