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

        // GET: Pages
        public async Task<IActionResult> Index()
        {
            return View(await _context.ComicPage.ToListAsync());
        }

        // GET: Pages/Details/5
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

        // GET: Pages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PageID,Volume,Issue,Image,ReleaseDate")] ComicPage comicPage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comicPage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comicPage);
        }

        // GET: Pages/Edit/5
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

        // POST: Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Pages/Delete/5
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

        // POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comicPage = await _context.ComicPage.FindAsync(id);
            _context.ComicPage.Remove(comicPage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComicPageExists(int id)
        {
            return _context.ComicPage.Any(e => e.PageID == id);
        }
    }
}
