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
        public static bool? AddPage(URContext context, ComicPage page) {
            if (!pageExists(context, page.Volume + " " + page.Issue))
            {
                context.Add(page);
                if (context.SaveChanges() == 1)
                    return true;
                else
                    return false;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns if a page with the same volume and issue exists in the database
        /// </summary>
        private static bool pageExists(URContext context, string volumeIssue)
        {
            return false;
            //List<String> things = (from c in context.ComicPage
            //                       orderby c.Volume, c.Issue
            //                       select c.Volume + " " + c.Issue).ToList<String>();
            //if (things.Contains(volumeIssue))
            //{
            //    return true;
            //}
            //return false;
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

        public static ComicPage GetLatestPage(URContext context)
        {
            try
            {
                ComicPage page = (from c in context.ComicPage
                                  orderby c.Volume, c.Issue
                                  select c).Last();
                return page;
            }
            catch
            {
                ComicPage noPage = new ComicPage();
                return noPage;
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
        }

        /// <summary>
        /// Returns a list of all of the volumes and issues of each page
        /// as a string with a space inbetween.
        /// Returns an empty list if an error occures
        /// </summary>
        public static List<string> GetAllPageNumbers(URContext context)
        {
            List<string> allPages = (from c in context.ComicPage
                                     orderby c.Volume, c.Issue
                                     select ("" + c.Volume + " " + c.Issue)).ToList<string>();
            return allPages;
            //try {
            //    List<string> allPages = (from c in context.ComicPage
            //                             orderby c.Volume, c.Issue
            //                             select c.Volume + " " + c.Issue).ToList<string>();
            //    return allPages;
            //}
            //catch {
            //    List<string> noPages = new List<string>();
            //    return noPages;
            //}
            //finally {
            //    context.Dispose();
            //}
        }

        /// <summary>
        /// Returns the byte array of the image of the comic page 
        /// with the same volume or issue given.
        /// Returns an empty list if an error occures while trying
        /// </summary>
        public static byte[] GetPageImage(URContext context, int volume, int issue)
        {
            try {
                byte[] image = (from c in context.ComicPage
                                where c.Volume == volume && c.Issue == issue
                                select c.Image).Single();
                return image;
            }
            catch {
                byte[] noImage = { 0 };
                return noImage;
            }
        }
    }
}
