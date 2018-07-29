using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapySharp.Html.Forms;

namespace Scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            //ScrapingBrowser Browser = new ScrapingBrowser();
            //Browser.AllowAutoRedirect = true;
            //Browser.AllowMetaRedirect = true;
            //WebPage PageResult = Browser.NavigateToPage(new Uri("https://www.nps.gov/olym/planyourvisit/wilderness-trail-conditions.htm"));
            //HtmlNodeCollection TableNode = PageResult.Html.SelectNodes("tbody");
            //foreach (var item in TableNode.CssSelect()
            //{

            //}

            var web = new HtmlWeb();
            var document = web.Load("https://www.nps.gov/olym/planyourvisit/wilderness-trail-conditions.htm");
            var nodes = document.DocumentNode.CssSelect("table");

            for (int i = 1; i < nodes.Count(); i++)
            {

            }





          






            //Console.WriteLine(pageTitle);
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