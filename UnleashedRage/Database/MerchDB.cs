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
        /// Returns one comic page with the same volume and issue given
        /// </summary>
        /// <returns>
        /// Returns the page if it is found, returns null otherwise
        /// </returns>
        public static Merch GetOneMerch(URContext context, int merchID) {
            try {
                Merch merch = (from m in context.Merch
                                  where m.MerchID == merchID
                                  select m).Single();
                return merch;
            }
            catch {
                return null;
            }
            finally {
                context.Dispose();
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
