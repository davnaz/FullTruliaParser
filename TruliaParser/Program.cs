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
using FT.Components;

namespace FTParser
{ //
    class Program
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            logger.Info("Парсер начал работу.");


            List<City> cities = DataProvider.Instance.GetCitiesFromDb();
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Convert.ToInt32(Resources.MaxDegreeOfParallelism);
            Parallel.ForEach(cities, options, (city) =>
            {
                logger.Info("Getting streets of {0},{1}", city.StateName,city.CityName);
                Parser.GetSteetsToDb(city);
                Parser.FinalizeCity(city);
                logger.Info("Getting streets of {0},{1} is over", city.StateName, city.CityName);
            });
            logger.Info("Парсер закончил работу.");
             

           // Parser.GetCitiesToDb()
           // List<City> cities = 




            //Console.ReadKey();
        }

        private static void ParseCitiesFromStates()
        {
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Convert.ToInt32(Resources.MaxDegreeOfParallelism);

            List<State> states = DataProvider.Instance.GetStatesFromDb();

            Parallel.ForEach(states, options, (state) =>
            {
                logger.Info("Getting cities of {0}", state.StateName);
                Parser.GetCityListToDb(state);
                
            });
        }

        void TestDriver()
        {
            string link = "https://www.trulia.com/property-sitemap/AL/";
            var driver = new PhantomJSDriver();
            driver.Manage().Window.Size = new Size(1920, 1080);
            driver.Navigate().GoToUrl(link);
            var links = driver.FindElementsByCssSelector("dl a");
            foreach (var a in links)
            {
                //Console.WriteLine("City: {0}, Link: {1}, Count: {2}", a.Text.Replace(a.Text.Split(' ')[0],String.Empty).Replace(" Homes",String.Empty), a.GetAttribute(Constants.WebAttrsNames.href), Int32.Parse(a.Text.Split(' ')[0].Replace(",",String.Empty)));
            }
            driver.Quit();
        }
    }
}


















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

/*
List<string> links = new List<string>();
links.Add("https://facebook.com");
            links.Add("https://vk.com");
            links.Add("https://mail.ru");
            links.Add("https://yandex.ru");
            links.Add("http://4pda.ru");
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
            Console.WriteLine("Я все скачал!");
            */
