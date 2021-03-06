﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnleashedRage.Database;
using UnleashedRage.Models;

namespace UnleashedRage.Controllers {
    public class MerchesController : Controller {
        private readonly URContext _context;

        public MerchesController(URContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Merch.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            Merch merch = MerchDB.GetOneMerch(_context, (int)id);
            ViewBag.Merch = merch;
            return View(merch);
        }

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Price,MerchImage")] InputMerch input) {
            if (ModelState.IsValid) {
                Merch merch = new Merch();
                merch.Name = input.Name;
                merch.Price = input.Price;

                if (input.MerchImage.ContentType.ToLower() != "image/jpeg" &&
                    input.MerchImage.ContentType.ToLower() != "image/png")
                {
                    // add error message here
                    throw new Exception(); 
                }

                var ms = new MemoryStream();
                input.MerchImage.OpenReadStream().CopyTo(ms);
                byte[] imageByteArray = ms.ToArray();

                merch.MerchImage = imageByteArray;
                if (MerchDB.AddMerch(_context, merch) == true)
                    ViewBag.Message = merch.ToString() + " was added!";
                else
                    ViewBag.ErrorMessage = "An error occured, try again later";

                return View();
            }
            ViewBag.ErrorMessage = "An error occured, try again later";
            return View(input);
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) {
                return NotFound();
            }

            var merch = await _context.Merch.FindAsync(id);
            if (merch == null) {
                return NotFound();
            }
            return View(merch);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MerchID,Price,MerchImage")] Merch merch) {
            if (id != merch.MerchID) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(merch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!MerchExists(merch.MerchID)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(merch);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var merch = await _context.Merch
                .FirstOrDefaultAsync(m => m.MerchID == id);
            if (merch == null) {
                return NotFound();
            }

            return View(merch);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var merch = await _context.Merch.FindAsync(id);
            _context.Merch.Remove(merch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        private bool MerchExists(int id)
        {
            return _context.Merch.Any(e => e.MerchID == id);
        }
    }
}
