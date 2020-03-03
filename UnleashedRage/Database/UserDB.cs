using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnleashedRage.Models;

namespace UnleashedRage.Database
{
    public static class UserDB
    {
        public static User GetUser(URContext context, int? id)
        {
            try
            {
                return (from u in context.User
                        where u.UserID == id
                        select new User
                        {
                            UserID = u.UserID,
                            Username = u.Username,
                            Email = u.Email,
                            SendEmail = (u.SendEmail == true ? true : false),
                            CurrPage = u.CurrPage
                        }).Single();
            }
            catch
            {
                User noUserFound = new User();
                return noUserFound;
            }
        }

        public static User GetUser(URContext context, string username)
        {
            try
            {
                return (from u in context.User
                        where u.Username == username
                        select new User
                        {
                            UserID = u.UserID,
                            Username = u.Username,
                            Email = u.Email,
                            SendEmail = (u.SendEmail == true ? true : false),
                            CurrPage = u.CurrPage
                        }).Single();
            }
            catch
            {
                User noUserFound = new User();
                return noUserFound;
            }
        }

        public static List<string> GetAllEmails(URContext context)
        {
            try
            {
                return (from u in context.User
                        where u.SendEmail == true
                        select u.Email).ToList<string>();
            }
            catch
            {
                List<string> noEmails = new List<string>();
                return noEmails;
            }
        }

        /// <summary>
        /// Adds the user given into the database
        /// </summary>
        /// <returns>
        /// Returns true if only one user was affected, returns false otherwise
        /// </returns>
        public static User AddUser(URContext context, User user)
        {
            context.Add(user);
            context.SaveChanges();
            return user;
        }

        /// <summary>
        /// Updates the user given if they are already in the database
        /// </summary>
        /// <returns>
        /// Returns true if only one user was affected, returns false otherwise
        /// </returns>
        public static User UpdateUser(URContext context, User user)
        {
            context.Update(user);
            context.SaveChanges();
            return user;
        }

        /// <summary>
        /// Deletes the user given from the database
        /// </summary>
        /// <returns>
        /// Returns true if only one user was affected, returns false otherwise
        /// </returns>
        public static User DeleteUser(URContext context, User user)
        {
            context.Remove(user);
            context.SaveChanges();
            return user;
        }

        /// <summary>
        /// Compares the user given to the user in the database with the same username
        /// </summary>
        /// <returns>
        /// Returns true if the user given matches the one in the database, returns false otherwise
        /// If the a user with the username was not found, returns null
        /// </returns>
        public static bool? CheckUser(URContext context, User user)
        {
            try
            {
                string password = (from u in context.User
                                   where u.Username == user.Username
                                   select u.Password).Single();
                return password == user.Password;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Grabs a list of all of the users in the database
        /// </summary>
        /// <returns>
        /// Returns null if any kind of error occures
        /// </returns>
        public static List<User> GetAllUsers(URContext context)
        {
            return (from u in context.User
                    orderby u.Username
                    select new User
                    {
                        UserID = u.UserID,
                        Username = u.Username,
                        Email = u.Email,
                        SendEmail = (u.SendEmail == true? true : false),
                        CurrPage = u.CurrPage
                    }).ToList();
        }
    }
}