using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnleashedRage.Database;
using UnleashedRage.Models;

namespace UnleashedRage.Controllers
{
    public class UsersController : Controller
    {
        private readonly URContext _context;

        public UsersController(URContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            List<User> allUsers = UserDB.GetAllUsers(_context);
            return View(allUsers);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User user = UserDB.GetUser(_context, id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        #region Sign Up
        public IActionResult Create()
        {
            User user = new User();
            user.SendEmail = true;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind("UserID,Username,Password,Email,CurrPage")] 
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                user = UserDB.AddUser(_context, user);
                ViewBag.Welcome = "Welcome " + user.Username;
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }
        #endregion

        #region Log In
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            if (user.Username != null && user.Password != null)
            {
                bool? rightUser = UserDB.CheckUser(_context, user);
                if (rightUser == true)
                {
                    ViewBag.Welcome = "Welcome back " + user.Username;
                    return RedirectToAction("Index", "Home");
                }
                else if (rightUser == false)
                {
                    ViewBag.Error = "Username or Password is incorect";
                }
                else
                {
                    ViewBag.Error = "An Error occured, try again later";
                }
            }
            return View(user);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User user = UserDB.GetUser(_context, id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            User user = UserDB.GetUser(_context, id);
            UserDB.DeleteUser(_context, user);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserID == id);
        }
    }
}
