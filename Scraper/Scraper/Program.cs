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
                    Regex HTMLCommentRegEx = new Regex("<[^>]*>", RegexOptions.IgnoreCase);

                    TrailConditions tc = new TrailConditions()
                    {
                        TrailName = HTMLCommentRegEx.Replace(HttpUtility.HtmlDecode(rowCells.ElementAt(0).InnerText).Replace("\n", String.Empty),""),
                        Description = HTMLCommentRegEx.Replace(HttpUtility.HtmlDecode(rowCells.ElementAt(1).InnerText).Replace("\n", String.Empty),""),
                        MilesElevation = HTMLCommentRegEx.Replace(HttpUtility.HtmlDecode(rowCells.ElementAt(2).InnerText).Replace("\n", String.Empty), ""),
                        Conditions = HTMLCommentRegEx.Replace(HttpUtility.HtmlDecode(rowCells.ElementAt(3).InnerText).Replace("\n", String.Empty), ""),
                        Updated = DateTime.Parse(HTMLCommentRegEx.Replace(HttpUtility.HtmlDecode(rowCells.ElementAt(4).InnerText).Replace("\n", String.Empty),"")),
                        InfoLink = rowCells.ElementAt(0).CssSelect("a").Any() ?PageResult.AbsoluteUrl.Host + HTMLCommentRegEx.Replace(rowCells.ElementAt(0).CssSelect("a").First().GetAttributeValue("href"),""):null
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
        public string InfoLink { get; set; }
        public DateTime Updated { get; set; }
        public TrailConditions()
        {

        }
    }
}
}
