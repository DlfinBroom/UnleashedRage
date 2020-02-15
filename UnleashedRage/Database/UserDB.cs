using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnleashedRage.Models;

namespace UnleashedRage.Database
{
    public class UserDB
    {
        /// <summary>
        /// Adds the user given into the database
        /// </summary>
        /// <returns>
        /// Returns true if only one user was affected, returns false otherwise
        /// </returns>
        public static bool AddUser(URContext context, User user) {
            context.Add(user);
            if (context.SaveChanges() == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Updates the user given if they are already in the database
        /// </summary>
        /// <returns>
        /// Returns true if only one user was affected, returns false otherwise
        /// </returns>
        public static bool UpdateUser(URContext context, User user) {
            context.Update(user);
            if (context.SaveChanges() == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Deletes the user given from the database
        /// </summary>
        /// <returns>
        /// Returns true if only one user was affected, returns false otherwise
        /// </returns>
        public static bool DeleteUser(URContext context, User user) {
            context.Remove(user);
            if (context.SaveChanges() == 1)
                return true;
            else
                return false;

        }

        /// <summary>
        /// Compares the user given to the user in the database with the same username
        /// </summary>
        /// <returns>
        /// Returns true if the user given matches the one in the database, returns false otherwise
        /// If the a user with the username was not found, returns null
        /// </returns>
        public static bool? CheckUser(URContext context, User user) {
            try {
                User OriginalUser = (from u in context.User
                                     where u.Username == user.Username
                                     select u).Single();
                if (OriginalUser.Password == user.Password)
                    return true;
                else
                    return false;
            }
            catch {
                return null;
            }
            finally {
                context.Dispose();
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
            try {
                return (from u in context.User
                        orderby u.Username
                        select u).ToList();
            }
            catch {
                return null;
            }
            finally {
                context.Dispose();
            }
        }
    }
}
