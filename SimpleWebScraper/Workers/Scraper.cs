using SimpleWebScraper.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SimpleWebScraper.Workers
{
    class Scraper
    {
        public List<string> Scrape(ScrapeCriteria scrapeCriteria)
        {
            List<string> scrapedElements = new List<string>();

            // Find all matches for specified RegEx
            MatchCollection matches = Regex.Matches(scrapeCriteria.Data, scrapeCriteria.Regex, scrapeCriteria.RegexOption);

            // Loop through found matches to extract smaller parts if specified
            foreach (Match match in matches)
            {
                // If no parts exist to drill into, add the scraped elements
                // Else loop through list of parts to extract parts of the matched elements
                if (!scrapeCriteria.Parts.Any())
                {
                    scrapedElements.Add(match.Groups[0].Value);
                }
                else
                {
                    // Loop through each part and scrape matching RegEx from parent match element
                    foreach (var part in scrapeCriteria.Parts)
                    {
                        Match matchedPart = Regex.Match(match.Groups[0].Value, part.Regex, part.RegexOption);

                        if (matchedPart.Success) scrapedElements.Add(matchedPart.Groups[0].Value);
                    }
                }
            }

            return scrapedElements;
        }
    }
}
