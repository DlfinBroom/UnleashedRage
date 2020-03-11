using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnleashedRage.Models;

namespace UnleashedRage.Database
{
    public class MerchDB
    {
        /// <summary>
        /// Adds the comic page given into the database
        /// </summary>
        /// <returns>
        /// Returns true if only one page was affected, returns false otherwise
        /// </returns>
        public static bool AddMerch(URContext context, Merch merch) {
            context.Add(merch);
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
        public static bool UpdateMerch(URContext context, Merch merch) {
            context.Update(merch);
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
        public static bool DeleteMerch(URContext context, Merch merch) {
            context.Remove(merch);
            if (context.SaveChanges() == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns a single merch item with all properties except for image
        /// </summary>
        public static Merch GetMerch(URContext context, int merchID) {
            try {
                Merch merch = (from m in context.Merch
                               where m.MerchID == merchID
                               select new Merch
                               {
                                   MerchID = m.MerchID,
                                   Name = m.Name,
                                   Price = m.Price,
                               }).Single();
                return merch;
            }
            catch {
                Merch noMerchFound = new Merch();
                return noMerchFound;
            }
        }

        /// <summary>
        /// Returns a single merch item with all properties, including image
        /// </summary>
        public static Merch GetFullMerch(URContext context, int merchID)
        {
            try
            {
                Merch merch = (from m in context.Merch
                               where m.MerchID == merchID
                               select new Merch
                               {
                                   MerchID = m.MerchID,
                                   Name = m.Name,
                                   Price = m.Price,
                                   MerchImage = m.MerchImage
                               }).Single();
                return merch;
            }
            catch
            {
                Merch noMerchFound = new Merch();
                return noMerchFound;
            }
        }

        /// <summary>
        /// Returns all comic pages in the database
        /// </summary>
        /// <returns>
        /// returns the list of all pages if found, returns null otherwise
        /// </returns>
        public static List<Merch> GetAllMerch(URContext context) {
            try {
                List<Merch> merch = (from m in context.Merch
                                     select m).ToList<Merch>();
                return merch;
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
