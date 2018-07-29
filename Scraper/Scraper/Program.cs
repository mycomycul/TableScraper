using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapySharp.Html.Forms;
using System.Web;
using System.Text.RegularExpressions;

namespace Scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            List<TrailConditions> ConditionsList = new List<TrailConditions>();
            //Setup Browser and download page 
            ScrapingBrowser Browser = new ScrapingBrowser();
            Browser.AllowAutoRedirect = true;
            Browser.AllowMetaRedirect = true;
            WebPage PageResult = Browser.NavigateToPage(new Uri("https://www.nps.gov/olym/planyourvisit/wilderness-trail-conditions.htm"));

            //Get tables from page
            IEnumerable<HtmlNode> TableNode = PageResult.Html.CssSelect("tbody");

            //Loop through tables skipping table one
            for (int i = 1; i < TableNode.Count(); i++)
            {
                var tableRows = TableNode.ElementAt(i).CssSelect("tr");
                //Loop Through Rows adding to database skipping rows one and 2
                for (int j = 2; j < tableRows.Count(); j++)
                {
                    var rowCells = tableRows.ElementAt(j).CssSelect("td");
                    var rowZone = tableRows.ElementAt(0).InnerText;
                    TrailConditions tc = new TrailConditions()
                    {
                        TrailName = CleanFromHTML(rowCells.ElementAt(0).InnerText),
                        Description = CleanFromHTML(rowCells.ElementAt(1).InnerText),
                        Elevation = ExtractElevations(CleanFromHTML(rowCells.ElementAt(2).InnerText)),
                        Conditions = CleanFromHTML(rowCells.ElementAt(3).InnerText),
                        Updated = DateTime.Parse(CleanFromHTML(rowCells.ElementAt(4).InnerText)),
                        InfoLink = rowCells.ElementAt(0).CssSelect("a").Any() ? PageResult.AbsoluteUrl.Host + CleanFromHTML(rowCells.ElementAt(0).CssSelect("a").First().GetAttributeValue("href")) : null,
                        Zone = CleanFromHTML(rowZone),
                        Miles = ExtractMiles(CleanFromHTML(rowCells.ElementAt(2).InnerText))

                    };

                    Console.WriteLine(tc.TrailName);
                    Console.WriteLine(tc.Description);
                    ConditionsList.Add(tc);
                }

            }


            Console.ReadLine();

            ///<summary>
            ///Removes comments and newlines from strings and replaces character codes
            /// </summary>
            string CleanFromHTML(string stringToClean)
            {
                Regex HTMLCommentRegEx = new Regex("<[^>]*>", RegexOptions.IgnoreCase);
                stringToClean = HTMLCommentRegEx.Replace(HttpUtility.HtmlDecode(stringToClean).Replace("\n", String.Empty), "");
                return stringToClean;
            }
            //Need to account for oddities in elevations or mileagelike multiple mileage sets
            float? ExtractMiles(string milesElevation)
            {
                try
                {
                    milesElevation = milesElevation.Remove(milesElevation.IndexOf(" miles"));
                    return float.Parse(milesElevation);
                }
                catch
                {
                    return null;
                }
            }
            //Need to account for oddities in elevations or mileagelike multiple mileage sets
            string ExtractElevations(string milesElevation)
            {
                milesElevation = milesElevation.Remove(0, milesElevation.IndexOf("miles") + 5);
                return milesElevation;
            }


        }


        public class TrailConditions
        {
            public string TrailName { get; set; }
            public string Zone { get; set; }
            public string Description { get; set; }
            public string Elevation { get; set; }
            public float? Miles { get; set; }
            public string Conditions { get; set; }
            /// <summary>
            /// Link to information page on NPS.gov
            /// </summary>
            public string InfoLink { get; set; }
            public DateTime Updated { get; set; }

        }
    }
}
