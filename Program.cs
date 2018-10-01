using System;
using WebScraperModularized.helpers;
using System.Net.Http;
using WebScraperModularized.parsers;
using WebScraperModularized.data;
using System.Collections.Generic;

namespace WebScraperModularized
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            string url = URLHelper.getNextURL().url;
            string url1 = URLHelper.getNextURL().url;
            Console.WriteLine("Parsing URL {0}", url);
            var response = client.GetAsync(url).Result;

            string content = response.Content.ReadAsStringAsync().Result;
            PropertyParser parser = new PropertyParser(content);
            List<URL> urlList = parser.parse();
            new DBHelper().insertParsedProperties(urlList);
            Console.WriteLine("Got list of size: {0}", urlList.Count);
        }
    }
}
