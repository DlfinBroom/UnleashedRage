﻿using System;
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

            ComicPage comicPage = ComicPageDB.GetPage(_context, id.GetValueOrDefault(-1));
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
        public IActionResult Create(InputComicPage input) {
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
                if (pageAdded == false)
                {
                    ViewBag.Error = "An error occured, try again later";
                }
                else if (pageAdded == null)
                {
                    ViewBag.Error = input.ToString() + " already exists, edit that page or make a new one";
                }

                ViewBag.Message = page.ToString() + " was added!";
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
            ComicPage page = ComicPageDB.GetFullPage(_context, id.GetValueOrDefault(-1));
            if (page == null) {
                return NotFound();
            }
            return View(page);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ComicPage page)
        {
            if (ModelState.IsValid) {
                if (ComicPageDB.UpdatePage(_context, page))
                {
                    ViewBag.Error = "An Error has occured, try again later";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(page);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }
            ComicPage page = ComicPageDB.GetPage(_context, id.GetValueOrDefault(-1));
            if (page == null) {
                return NotFound();
            }

            return View(page);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            ComicPage page = ComicPageDB.GetPage(_context, id);
            ComicPageDB.DeletePage(_context, page);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        private bool ComicPageExists(int id) {
            return _context.ComicPage.Any(e => e.PageID == id);
        }
    }
}
