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
            var RainierConditions = RainierTrailConditions();
            var OlympicConditions = OlympicTrailConditions();
            foreach(var location in OlympicConditions)
            {
                Console.WriteLine("Current Conditions for {0}", location.TrailName);
                Console.WriteLine(location.Conditions.PadLeft(1, '\u0009') + '\n');
                
            }
            Console.ReadLine();
        }
        ///<summary>
        ///Removes comments and newlines from strings and replaces character codes
        /// </summary>
        static string CleanFromHTML(string stringToClean)
        {
            Regex HTMLCommentRegEx = new Regex("<[^>]*>", RegexOptions.IgnoreCase);
            stringToClean = HTMLCommentRegEx.Replace(HttpUtility.HtmlDecode(stringToClean).Replace("\n", String.Empty), "");
            return stringToClean;
        }
        //Need to account for oddities in elevations or mileagelike multiple mileage sets
        static public float? ExtractMiles(string milesElevation)
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
        static public string ExtractElevations(string milesElevation)
        {
            milesElevation = milesElevation.Remove(0, milesElevation.IndexOf("miles") + 5);
            return milesElevation;
        }

        static List<TrailConditions> OlympicTrailConditions()
        {
            List<TrailConditions> ConditionsList = new List<TrailConditions>();
            //Setup Browser and download page 
            var baseUrl = new Uri("https://www.nps.gov/olym/planyourvisit/wilderness-trail-conditions.htm");
            HtmlWeb web = new HtmlWeb();
            var PageResult = web.Load(baseUrl.ToString());


            //Get tables from page
            IEnumerable<HtmlNode> TableNode = PageResult.DocumentNode.CssSelect("tbody");

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
                        InfoHTMLLink = rowCells.ElementAt(0).CssSelect("a").Any() ? baseUrl.Host + CleanFromHTML(rowCells.ElementAt(0).CssSelect("a").First().GetAttributeValue("href")) : null,
                        Zone = CleanFromHTML(rowZone),
                        Miles = ExtractMiles(CleanFromHTML(rowCells.ElementAt(2).InnerText))

                    };
                    ConditionsList.Add(tc);
                }
            }
            return ConditionsList;
        }

        static List<TrailConditions> RainierTrailConditions()
        {
            List<TrailConditions> ConditionsList = new List<TrailConditions>();
            //Setup Browser and download page 
            var baseUrl = new Uri("https://www.nps.gov/mora/planyourvisit/trails-and-backcountry-camp-conditions.htm");
            HtmlWeb web = new HtmlWeb();
            var PageResult = web.Load(baseUrl.ToString());
            //Get tables from page
            IEnumerable<HtmlNode> TableNode = PageResult.DocumentNode.CssSelect("tbody");

            //Loop through tables skipping table one
            for (int i = 0; i < TableNode.Count(); i++)
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
                        PercentSnowCover = int.TryParse(CleanFromHTML(rowCells.ElementAt(1).InnerText), out int snow) ? snow : 0,
                        Conditions = CleanFromHTML(rowCells.ElementAt(2).InnerText),
                        Updated = DateTime.TryParse(CleanFromHTML(rowCells.ElementAt(3).InnerText), out DateTime update) ? update : (DateTime?)null
                        //Zone = CleanFromHTML(rowZone),
                    };
                    ConditionsList.Add(tc);
                }          
            }
            return ConditionsList;

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
            public string InfoHTMLLink { get; set; }
            public int PercentSnowCover { get; set; }
            public DateTime? Updated { get; set; }

        }
    }
}

