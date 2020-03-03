using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnleashedRage.Models;
using System.Net.Mail;
using System.Net;

namespace UnleashedRage.Database {
    public class PagesController : Controller {
        private readonly URContext _context;

        public PagesController(URContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            return View(ComicPageDB.GetAllPages(_context));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) {
                return NotFound();
            }

            ComicPage comicPage = ComicPageDB.GetOnePage(_context, (int)id);
            ViewBag.ComicPage = comicPage;
            return View(comicPage);
        }

        #region Create
        [HttpGet]
        public IActionResult Create() {
            InputComicPage page = new InputComicPage();
            return View(page);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Volume,Issue,Image")] InputComicPage input) {
            if (ModelState.IsValid) {
                ComicPage page = new ComicPage();
                page.Issue = input.Issue;
                page.Volume = input.Volume;

                // Makes sure the
                if (input.Image.ContentType.ToLower() != "image/jpeg" &&
                    input.Image.ContentType.ToLower() != "image/png")
                {
                    // add error message here
                    throw new Exception();
                }

                // Transforms the file into a byte[]
                var ms = new MemoryStream();
                input.Image.OpenReadStream().CopyTo(ms);
                byte[] imageByteArray = ms.ToArray();
                page.Image = imageByteArray;

                // Sets the date added to today
                page.ReleaseDate = DateTime.Today;

                // Tries to add the page to the database, and displays a message if it worked or not
                bool? pageAdded = ComicPageDB.AddPage(_context, page);
                if (pageAdded == true)
                    ViewBag.Message = page.ToString() + " was added!";
                else if (pageAdded == null)
                    ViewBag.Error = input.ToString() + " already exists, edit that page or make a new one";
                else
                    ViewBag.Error = "An error occured, try again later";

                if (input.SendEmail == true)
                {
                    SendPageUpdateEmail(input);
                }

                return View();
            }
            ViewData["ErrorMessage"] = "An error occured, try again later";
            return View();
        }

        private void SendPageUpdateEmail(InputComicPage input)
        {
            List<string> emails = UserDB.GetAllEmails(_context);
            if (emails.Count == 0)
            {
                ViewBag.EmailFail = "No Email addresses were found to send to";
                return;
            }
            MailMessage message = new MailMessage
            {
                IsBodyHtml = true,
                Body = "text",
                Subject = "subject"
            };
            message.From = new MailAddress("sender@email.com");
            foreach (string email in emails)
            {
                message.To.Add(email);
            }
            SmtpClient client = new SmtpClient("email sending service", 465)
            {
                Credentials = new NetworkCredential("sending email address", "password"),
                EnableSsl = true
            };
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) {
                return NotFound();
            }

            var comicPage = await _context.ComicPage.FindAsync(id);
            if (comicPage == null) {
                return NotFound();
            }
            return View(comicPage);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PageID,Volume,Issue,Image,ReleaseDate")] ComicPage comicPage)
        {
            if (id != comicPage.PageID) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(comicPage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!ComicPageExists(comicPage.PageID)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(comicPage);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var comicPage = await _context.ComicPage
                .FirstOrDefaultAsync(m => m.PageID == id);
            if (comicPage == null) {
                return NotFound();
            }

            return View(comicPage);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var comicPage = await _context.ComicPage.FindAsync(id);
            _context.ComicPage.Remove(comicPage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        private bool ComicPageExists(int id) {
            return _context.ComicPage.Any(e => e.PageID == id);
        }
    }
}
