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
    class Sample
    {
        static void SampleMain(string[] args)
        {
            ScrapingBrowser Browser = new ScrapingBrowser();
            Browser.AllowAutoRedirect = true;
            Browser.AllowMetaRedirect = true;
            WebPage PageResult = Browser.NavigateToPage(new Uri("http://localhost:57661/"));
            HtmlNode TitleNode = PageResult.Html.CssSelect(".navbar-brand").First();
            string pageTitle = TitleNode.InnerText;

            List<string> Names = new List<string>();
            var Table = PageResult.Html.CssSelect("#PersonTable").First();

            foreach (var row in Table.SelectNodes("tbody/tr"))
            {
                foreach (var cell in row.SelectNodes("td[1]"))
                {
                    Names.Add(cell.InnerText);
                }
            }
            foreach (var name in Names)
            {
                Console.WriteLine(name);
            }
            //Console.ReadLine();

            PageWebForm form = PageResult.FindFormById("dataForm");
            form["UserName"] = "Mary";
            form["Gender"] = "M";
            form.Method = HttpVerb.Post;
            WebPage resultsPage = form.Submit();
            var Page = resultsPage.Html.CssSelect(".body-content").First().InnerText;
            Console.WriteLine(Page);
        }
    }
}
