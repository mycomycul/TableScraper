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

namespace Scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            List<TrailConditions> ConditionsList = new List<TrailConditions>();

            ScrapingBrowser Browser = new ScrapingBrowser();
            Browser.AllowAutoRedirect = true;
            Browser.AllowMetaRedirect = true;
            WebPage PageResult = Browser.NavigateToPage(new Uri("https://www.nps.gov/olym/planyourvisit/wilderness-trail-conditions.htm"));
            IEnumerable<HtmlNode> TableNode = PageResult.Html.CssSelect("tbody");
            //Loop through tables skipping table one
            for (int i = 1; i < TableNode.Count(); i++)
            {
                var tableRows = TableNode.ElementAt(i).CssSelect("tr");
                //Loop Through Rows adding to database skipping rows one and 2
                for (int j = 2; j < tableRows.Count(); j++)
                {

                    var rowCells = tableRows.ElementAt(j).CssSelect("td");
                    TrailConditions tc = new TrailConditions()
                    {
                        TrailName = HttpUtility.HtmlDecode(rowCells.ElementAt(0).InnerText).Replace("\n", String.Empty),
                        Description = HttpUtility.HtmlDecode(rowCells.ElementAt(1).InnerText).Replace("\n", String.Empty),
                        MilesElevation = HttpUtility.HtmlDecode(rowCells.ElementAt(2).InnerText).Replace("\n", String.Empty),
                        Conditions = HttpUtility.HtmlDecode(rowCells.ElementAt(3).InnerText).Replace("\n", String.Empty),
                        Updated = DateTime.Parse(HttpUtility.HtmlDecode(rowCells.ElementAt(4).InnerText).Replace("\n", String.Empty))
                    };
                    Console.WriteLine(tc.TrailName);
                    Console.WriteLine(tc.Description);
                    ConditionsList.Add(tc);
                }
            }
            Console.ReadLine();
        }
        public class TrailConditions
        {
            public string TrailName { get; set; }
            public string Description { get; set; }
            public string MilesElevation { get; set; }
            public string Conditions { get; set; }
            public DateTime Updated { get; set; }
            public TrailConditions()
            {

            }
        }
    }
}
