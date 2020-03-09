using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            user.Password = HashPassword(user.Password);
            context.Add(user);
            context.SaveChanges();
            return user;
        }

        /// <summary>
        /// Hashes the password before it is put into the database for security reasons
        /// </summary>
        private static string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
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
                string originalPassword =
                    (from u in context.User
                     where u.Username == user.Username
                     select u.Password).Single();

                /* Fetch the stored value */
                byte[] hashBytes = Convert.FromBase64String(originalPassword);
                /* Get the salt */
                byte[] salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);
                /* Compute the hash on the password the user entered */
                var pbkdf2 = new Rfc2898DeriveBytes(user.Password, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);

                for (int i = 0; i < 20; i++) { 
                    if (hashBytes[i + 16] != hash[i])
                    {
                        return false;
                    }
                }
                return true;
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