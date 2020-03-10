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
            InputUser user = new InputUser();
            user.SendEmail = true;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind("UserID,Username,Password,Email,CurrPage")] 
        public async Task<IActionResult> Create(InputUser input)
        {
            if (ModelState.IsValid)
            {
                input.Username = input.Username.ToLower();
                input.Email = input.Email.ToLower();
                if (input.Password != input.CheckPassword)
                {
                    ViewBag.Error = "Passwords must match";
                }
                else if (UserDB.UsernameExists(_context, input.Username))
                {
                    ViewBag.Error = "Username is already taken";
                    input.Username = "";
                }
                else if (UserDB.EmailExists(_context, input.Email))
                {
                    ViewBag.Error = "Email is already taken";
                    input.Email = "";
                }
                else
                {
                    User user = new User(
                        input.Username,
                        input.Email,
                        input.SendEmail
                    );
                    // Hash the password before entering it
                    user.Password = input.Password;
                    user = UserDB.AddUser(_context, user);
                    ViewBag.Welcome = "Welcome " + user.Username;
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(input);
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

        public IActionResult ForgetPassword()
        {
            User user = new User();
            return View(user);
        }
        [HttpPost]
        public IActionResult ForgetPassword(User account)
        {
            User originalAccount = UserDB.GetUser(_context, account.Username);
            string email = originalAccount.Email;
            if (!string.IsNullOrEmpty(email))
            {
                /*
                 * Instead of linking to the webpage, send an email 
                 * with the link attached that expires in 10 minutes
                 */
                //// Send an email to change that users password
                //ViewBag.Success = "An email was sent to change the password of this account";
                //return RedirectToAction("Login", "Users");
                int id = UserDB.GetUser(_context, account.Username).UserID;
                return RedirectToAction("RecoverPassword", new { userID = id });
            }
            ViewBag.Error = "Username was not found, was it spelled right?";
            return View(account);
        }

        public IActionResult RecoverPassword(int userID)
        {
            InputUser input = new InputUser(UserDB.GetUser(_context, userID));
            return View(input);
        }
        [HttpPost]
        public IActionResult RecoverPassword(InputUser input)
        {
            if (ModelState.IsValid)
            {
                if(input.Password != input.CheckPassword)
                {
                    ViewBag.Error = "Passwords do not match";
                }
                else
                {
                    User user = UserDB.GetUser(_context, input.Username);
                    user.Password = input.Password;
                    if (UserDB.UpdateUser(_context, user) == user)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ViewBag.Error = "An Error Occured, Try Again Later";
                    }
                }
            }
            return View(input);
        }
        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserID == id);
        }
    }
}
