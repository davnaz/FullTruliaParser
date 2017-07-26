using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using FTParser.DataProviders;
using AngleSharp.Dom.Html;
using AngleSharp.Dom;
using System.Threading;
using System.Net.NetworkInformation;
using FTParser.Components;
using System.Threading.Tasks;
using System.Diagnostics;
using TruliaParser;
using NLog;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium;
using System.Drawing;

namespace FTParser
{
    class Program
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            //logger.Info("Парсер начал работу.");
            
            List<string> links = new List<string>();
            links.Add("https://facebook.com");
            links.Add("https://vk.com");
            links.Add("https://mail.ru");
            links.Add("https://yandex.ru");
            links.Add("http://4pda.ru");
           // var driver = new PhantomJSDriver();
           // driver.Navigate().GoToUrl("https://vk.com");
           // File.WriteAllText(links[0].Split('/')[2] + ".html", driver.PageSource.ToString(), Encoding.Unicode);
           // driver.Quit();
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = 5;
            Parallel.ForEach(links, options, (link) =>
              {
                  var driver = new PhantomJSDriver();
                  driver.Manage().Window.Size = new Size(1920, 1080);
                  driver.Navigate().GoToUrl(link);
                  
                  driver.GetScreenshot().SaveAsFile(link.Split('/')[2]+".png",ScreenshotImageFormat.Png);
                  File.WriteAllText(link.Split('/')[2] + ".html", driver.PageSource.ToString(), Encoding.Unicode);
                  driver.Quit();
              });
            //var driver = new PhantomJSDriver();
            //driver.Navigate().GoToUrl("https://vk.com");
            //File.WriteAllText("file.html",driver.PageSource.ToString(),Encoding.Unicode);
            //driver.Quit();
            Console.WriteLine("Я все скачал!");
            Console.ReadKey();


















            /*
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Convert.ToInt32(Resources.MaxDegreeOfParallelism);
            Console.WriteLine("Получаю список неспарсенных регионов...");
            List<Region> regions =  DataProvider.Instance.GetRegionsFromDb();
            Console.WriteLine("Получено ссылок: {0}", regions.Count);
            ProxySolver.Instance.getNewProxy();
            foreach (Region reg in regions)
            {
                Console.WriteLine(reg);
            }
            */
            /*
            Parallel.ForEach(regions, options, (reg) =>
            {
                
                  Parser p = new Parser();
                  p.StartParsing(reg);
                
            //Parser p = new Parser();
            //
            //p.StartParsing(regions[830]);


            });

            


            Console.WriteLine("Работа парсера завершена. Для продолжения нажмите любую клавишу...");
    */        
    //Console.ReadKey();
        }
    }
}
