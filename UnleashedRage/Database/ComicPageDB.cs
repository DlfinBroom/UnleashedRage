using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnleashedRage.Models;

namespace UnleashedRage.Database
{
    public class ComicPageDB
    {
        /// <summary>
        /// Adds the comic page given into the database
        /// </summary>
        /// <returns>
        /// Returns true if only one page was affected, returns false otherwise
        /// </returns>
        public static bool AddPage(URContext context, ComicPage page) {
            context.Add(page);
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
        public static bool UpdatePage(URContext context, ComicPage page) {
            context.Update(page);
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
        public static bool DeletePage(URContext context, ComicPage page) {
            context.Remove(page);
            if (context.SaveChanges() == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns the comic page with the most recent Released Date
        /// </summary>
        public static ComicPage GetLatestPage(URContext context)
        {
            try
            {
                ComicPage page = (from c in context.ComicPage
                                  select c).Single<ComicPage>();
                return page;
            }
            catch
            {
                return null;
            }
            finally
            {
                context.Dispose();
            }
        }

        /// <summary>
        /// Returns one comic page with the same volume and issue given
        /// </summary>
        /// <returns>
        /// Returns the page if it is found, returns null otherwise
        /// </returns>
        public static ComicPage GetOnePage(URContext context, int pageID) {
            try {
                ComicPage page = (from c in context.ComicPage
                                  where c.PageID == pageID
                                  select c).Single();
                return page;
            }
            catch {
                return null;
            }
            finally {
                context.Dispose();
            }
        }

        /// <summary>
        /// Returns a list of all comic pages in the volume given
        /// </summary>
        /// <returns>
        /// Returns the list if it is found, returns null otherwise
        /// </returns>
        public static List<ComicPage> GetOneVolume(URContext context, int Volume) {
            try {
                List<ComicPage> page = (from c in context.ComicPage
                                        where c.Volume == Volume
                                        select c).ToList<ComicPage>();
                return page;
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
        public static List<ComicPage> GetAllPages(URContext context) {
            try {
                List<ComicPage> page = (from c in context.ComicPage
                                        orderby c.Volume, c.Issue
                                        select c).ToList<ComicPage>();
                return page;
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
