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
        /// Returns a page without the image part
        /// </summary>
        public static ComicPage GetPage(URContext context, int id)
        {
            try
            {
                return (from c in context.ComicPage
                        where c.PageID == id
                        select new ComicPage
                        {
                            PageID = c.PageID,
                            Volume = c.Volume,
                            Issue = c.Issue,
                            ReleaseDate = c.ReleaseDate,
                        }).Single();
            }
            catch
            {
                ComicPage noPageFound = new ComicPage();
                return noPageFound;
            }
        }

        /// <summary>
        /// Returns a page with all parts, including image
        /// </summary>
        public static ComicPage GetFullPage(URContext context, int id)
        {
            try
            {
                return (from c in context.ComicPage
                        where c.PageID == id
                        select new ComicPage
                        {
                            PageID = c.PageID,
                            Volume = c.Volume,
                            Issue = c.Issue,
                            ReleaseDate = c.ReleaseDate,
                            Image = c.Image
                        }).Single();
            }
            catch
            {
                ComicPage noPageFound = new ComicPage();
                return noPageFound;
            }
        }

        /// <summary>
        /// Adds the comic page given into the database or
        /// Updates the page if the same page already existed
        /// </summary>
        /// <returns>
        /// Returns true if only one page was affected, returns false otherwise
        /// </returns>
        public static bool AddPage(URContext context, ComicPage page) {
            int pageID = IssueExists(context, page.Volume, page.Issue);
            if (pageID != -1)
            {
                page.PageID = pageID;
                return UpdatePage(context, page);
            }
            context.Add(page);
            if (context.SaveChanges() == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Updates the page given if they are already in the database
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
        /// Checks if a page with the same volume and issue exists already
        /// </summary>
        /// <returns>
        /// If the page already exists, returns its pageID, else it returns -1
        /// </returns>
        private static int IssueExists(URContext context, int volume, int issue)
        {
            try
            {
                int? pageID = (from c in context.ComicPage
                               where c.Volume == volume && c.Issue == issue
                               select c.PageID).First();
                return pageID.GetValueOrDefault(-1);
            }
            catch(InvalidOperationException ioe)
            {
                return -1;
            }
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
                List<ComicPage> noPages = new List<ComicPage>();
                return noPages;
            }
            finally {
                context.Dispose();
            }
        }
    }
}
