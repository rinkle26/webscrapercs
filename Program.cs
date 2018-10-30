using System;
using WebScraperModularized.helpers;
using System.Net.Http;
using WebScraperModularized.parsers;
using WebScraperModularized.data;
using System.Collections.Generic;
using Z.Dapper.Plus;
using WebScraperModularized.wrappers;

namespace WebScraperModularized
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {   
            DapperPlusManager.Entity<URL>().Table("url").Identity(x => x.id);
            
            IEnumerable<URL> uRLs = DBHelper.getURLSFromDB(10, true);
            String urls = "";
            foreach (var item in uRLs)
            {
                urls = urls + item.url;
            }

            Console.WriteLine(urls);
        }
    }
}
