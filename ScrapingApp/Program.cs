using HtmlAgilityPack;
using CsvHelper;
using System.Globalization;


namespace ScrapingApp;

class Program
{
    static void Main(string[] args)
    {
        var web = new HtmlWeb();

         
        var document = web.Load("https://jijimoo.com/mobile/");

        var mobiles = new List<Mobile>();

        var productHTMLElements = document.DocumentNode.
            QuerySelectorAll("div.products__item-img-color-wrapper");

        foreach (var productHTMLElement in productHTMLElements)
        {
            var url = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("a").Attributes["href"].Value);
            var image = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("img").Attributes["src"].Value);
            var name = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("a").Attributes["title"].Value);
            var price = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("bdi").InnerText)!;
            
            var mobileProduct = new Mobile() 
            {
                Url = url,
                Image = image,
                Name = name,
                Price = price,
            };

            mobiles.Add(mobileProduct);
        }

        using (var writer = new StreamWriter("mobile-products.csv"))
         
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(mobiles);
        }


        Console.WriteLine("DONE!");
    }
}