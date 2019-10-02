using SimpleWebScraper.Builders;
using SimpleWebScraper.Data;
using SimpleWebScraper.Workers;
using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace SimpleWebScraper
{
    class Program
    {
        private const string Method = "search";
        static void Main(string[] args)
        {
            try
            {
                // Get city and category from user
                Console.Write("City to scrape information for: ");
                string city = Console.ReadLine() ?? string.Empty;
                Console.Write("CraigsList category: ");
                string category = Console.ReadLine() ?? string.Empty;

                // Use WebClient to pull web page then scrape the listing URL and descriptions
                using (WebClient client = new WebClient())
                {
                    Console.WriteLine($"Scraping page http://{city.Replace(" ", string.Empty)}.craigslist.org/{Method}/{category}");
                    string content = client.DownloadString($"http://{city.Replace(" ", string.Empty)}.craigslist.org/{Method}/{category}");

                    ScrapeCriteria scrapeCriteria = new ScrapeCriteriaBuilder()
                        .WithData(content)
                        // RegEx for entire listing element
                        .WithRegex(@"<a href=\""(.*?)\"" data-id=\""(.*?)\"" class=\""result-title hdrlnk\"">(.*?)</a>")
                        .WithRegexOption(RegexOptions.ExplicitCapture)
                        // Build scraper for listing description part
                        .WithPart(new ScrapeCriteriaPartBuilder()
                            .WithRegex(@">(.*?)</a>")
                            .WithRegexOption(RegexOptions.Singleline)
                            .Build())
                        // Build scraper for listing URL part
                        .WithPart(new ScrapeCriteriaPartBuilder()
                            .WithRegex(@"href=\""(.*?)\""")
                            .WithRegexOption(RegexOptions.Singleline)
                            .Build())
                        .Build();

                    // Call scraper to extract listing elements from page then extract parts from listing elements
                    Scraper scraper = new Scraper();
                    var scrapedElements = scraper.Scrape(scrapeCriteria);

                    // Display scraped parts if any exists
                    if (scrapedElements.Any())
                    {
                        foreach (var scrapedElement in scrapedElements) Console.WriteLine(scrapedElement);
                    }
                    else
                    {
                        Console.WriteLine("There were no matches for the entered city and category.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.Write("Press any key to exit.");
                Console.ReadLine();
            }
        }
    }
}
