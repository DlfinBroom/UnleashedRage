using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UnleashedRage.Database;
using UnleashedRage.Models;

namespace UnleashedRage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();

            using (var db = new URContext())
            {
                // Create and save a new Blog
                Console.Write("Enter a name for a new User: ");
                var name = Console.ReadLine();

                User user = new User(name);
                db.User.Add(user);
                db.SaveChanges();

                // Display all Blogs from the database
                var query = from b in db.User
                            orderby b.Username
                            select b;

                Console.WriteLine("All users in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Username);
                }

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
