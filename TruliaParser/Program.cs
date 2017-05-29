using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using TruliaParser.DataProviders;
using AngleSharp.Dom.Html;
using AngleSharp.Dom;
using System.Threading;
using System.Net.NetworkInformation;
using TruliaParser.Components;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TruliaParser
{
    class Program
    {
        static void Main(string[] args)
        {

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


            Parallel.ForEach(regions, options, (reg) =>
            {
                
                  Parser p = new Parser();
                  p.StartParsing(reg);
                
            //Parser p = new Parser();
            //
            //p.StartParsing(regions[830]);


            });

            


            Console.WriteLine("Работа парсера завершена. Для продолжения нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
