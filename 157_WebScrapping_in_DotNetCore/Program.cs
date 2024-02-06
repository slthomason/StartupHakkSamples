using System.Collections.Generic;
using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;


namespace WebScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            var html = GetHtml();
            var data = ParseHtmlUsingHtmlAgilityPack(html);
        }

        private static string GetHtml()
        {
            var options = new ChromeOptions
            {
                BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe"
            };

            options.AddArguments("headless");

            var chrome = new ChromeDriver(options);
            chrome.Navigate().GoToUrl("https://edition.cnn.com/");

            return chrome.PageSource;
        }

        private static string ParseHtmlUsingHtmlAgilityPack(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var data = htmlDoc.ParsedText;

            
            return data;
        }
    }
}