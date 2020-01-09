using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnleashedRage.Models;

namespace UnleashedRage.Database
{
    public class PagesController : Controller
    {
        private readonly URContext _context;

        public PagesController(URContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ComicPage.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comicPage = await _context.ComicPage
                .FirstOrDefaultAsync(m => m.PageID == id);
            if (comicPage == null)
            {
                return NotFound();
            }

            return View(comicPage);
        }

        #region Create
        [HttpGet]
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("PageID,Volume,Issue,Image,ReleaseDate")] ComicPage page) {
            page.ReleaseDate = DateTime.Today;
            page.Image = new byte[1]; // Have Image become file from the create page
            if (ModelState.IsValid) {
                if (ComicPageDB.AddPage(_context, page) == true)
                    ViewData["Massage"] = page.ToString() + " was added!";
                else
                    ViewData["Mesage"] = "An error occured, try again later";

                return View();
            }
            return View(page);
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comicPage = await _context.ComicPage.FindAsync(id);
            if (comicPage == null)
            {
                return NotFound();
            }
            return View(comicPage);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PageID,Volume,Issue,Image,ReleaseDate")] ComicPage comicPage)
        {
            if (id != comicPage.PageID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comicPage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComicPageExists(comicPage.PageID))
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
            return View(comicPage);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comicPage = await _context.ComicPage
                .FirstOrDefaultAsync(m => m.PageID == id);
            if (comicPage == null)
            {
                return NotFound();
            }

            return View(comicPage);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comicPage = await _context.ComicPage.FindAsync(id);
            _context.ComicPage.Remove(comicPage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        private bool ComicPageExists(int id)
        {
            return _context.ComicPage.Any(e => e.PageID == id);
        }
    }
}
