using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using TruliaParser.DataProviders;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using AngleSharp;
using AngleSharp.Scripting.JavaScript.Services;
using Jint.Native;
using AngleSharp.Extensions;
using Jint.Native.Object;

namespace TruliaParser.Components
{
    class Parser
    {
        private static WebProxy currentProxy;
        private static HtmlParser parser;

        public Parser()
        {
            currentProxy = ProxySolver.Instance.getNewProxy();
            var config = new Configuration()
                .WithJavaScript();
            parser = new HtmlParser(config); //создание экземпляра парсера, он можнт быть использован несколько раз для одного потока(экземпляра класса Parser)
            
        }

        private WebProxy UpdateInternalProxy()
        {
            return currentProxy = ProxySolver.Instance.getNewProxy();
        }

        private void ParseRegionsToDb()
        {
            DataProvider.Instance.ExecureSP(DataProvider.Instance.CreateSQLCommandForSP(Resources.ClearRegionsSPName)); //чистим таблицу с ссылками на регионы
            SqlCommand insertLink = DataProvider.Instance.CreateSQLCommandForSP(Resources.InsertRegionSP); //подготовка процедуры для занесения ссылок в БД
            string rentMapLink = "https://www.trulia.com/rent-sitemap/";
            string statePageHtml;
            while (true)
            {
                statePageHtml = WebHelpers.GetHtmlThrowProxy(rentMapLink, UpdateInternalProxy());
                if (statePageHtml != Constants.WebAttrsNames.NotFound)
                {
                    break;
                }
            }
            var document = parser.Parse(statePageHtml);
            List<IElement> stateLinks = document.QuerySelectorAll(".activeLink.h7").ToList(); //добыли список штатов

            stateLinks.ForEach(i =>
            {
                string stateName = i.TextContent.Trim().Replace(" real estate", "");
                Console.WriteLine(Resources.BaseLink + i.GetAttribute(Constants.WebAttrsNames.href));
                var statePage = parser.Parse(WebHelpers.GetHtmlThrowProxy(Resources.BaseLink + i.GetAttribute(Constants.WebAttrsNames.href), UpdateInternalProxy()));
                List<IElement> countryLinks = statePage.QuerySelectorAll(".mts li a").ToList();
                countryLinks.ForEach(link =>
                {
                    string countyName = link.TextContent.Trim().Replace(" County", "");
                    string countyPageHtml; //string for all state counties
                    while (true)
                    {
                        countyPageHtml = WebHelpers.GetHtmlThrowProxy(Resources.BaseLink + link.GetAttribute(Constants.WebAttrsNames.href), UpdateInternalProxy());
                        if (countyPageHtml != Constants.WebAttrsNames.NotFound)
                        {
                            break;
                        }
                    }

                    string regionLink = Resources.BaseLink + parser.Parse(countyPageHtml).QuerySelector(".mbl a").GetAttribute(Constants.WebAttrsNames.href);
                    int offersCount = getOffersCount(regionLink);
                    Console.WriteLine("{0}, {1}, {2}", stateName, countyName, regionLink);
                    insertLink.Parameters.Clear();
                    insertLink.Parameters.AddWithValue(Constants.RegionLinkDbParams.State, stateName);
                    insertLink.Parameters.AddWithValue(Constants.RegionLinkDbParams.RegionName, countyName);
                    insertLink.Parameters.AddWithValue(Constants.RegionLinkDbParams.Link, regionLink);
                    insertLink.Parameters.AddWithValue(Constants.RegionLinkDbParams.OffersCount, offersCount);
                    DataProvider.Instance.ExecureSP(insertLink);
                });
            });
        }

       

        /// <summary>
        /// Парсит регион сайта Trulia  
        /// </summary>
        /// <param name="regionLink">Полный адрес сайта региона для выборки</param>
        public void StartParsing(Region region) //
        {
            string regionLink = region.Link;
            Console.WriteLine("Beginning the parsing new region:\nState: {0}, County: {1}, Link: {2}", region.State, region.RegionName, region.Link);
            List<string> offerLinks = new List<string>(GetOffersLinks(regionLink));
            offerLinks.ForEach(i => ParseOffer(i));
            if(offerLinks.Count >= region.OffersCount*0.9 || (offerLinks.Count- region.OffersCount < 20))
            {
                DataProvider.Instance.FinalizeRegion(region);
            }
            else
            {
                Console.WriteLine("Кажется, регион неправильно спарсился(забрал ссылки)собрано/ожидалось: {0},{1}",offerLinks.Count,region.OffersCount);
            }
            //Console.ReadKey();
            
        }

        private int getOffersCount(string regionLink)
        {
            string regionPageHtml;
            try
            {
                while (true) //Получаем стартовую страницу региона для того, чтобы узнать количество заявок в регионе
                {
                    Console.WriteLine("Попытка скачать: {0}", regionLink);
                    regionPageHtml = WebHelpers.GetHtmlThrowProxy(regionLink, UpdateInternalProxy());
                    if (regionPageHtml != Constants.WebAttrsNames.NotFound)
                    {
                        break;
                    }
                }
                var regionPageDom = parser.Parse(regionPageHtml);
                try
                {
                    int offersCount = Convert.ToInt32(regionPageDom.QuerySelector(Constants.OfferListSelectors.OffersCount).TextContent.Trim('(',')'));
                    return offersCount;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка при получении количества предложений региона:{0},{1}", regionLink, e.Message);
                    //Console.ReadKey();
                    return -1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка при получении страницы количества предложений региона:{0},{1}", regionLink, e.Message);
                //Console.ReadKey();
                return -1;
            }

        }

        private void ParseOffer(string offerLink)
        {
            string offerHtml = Constants.WebAttrsNames.NotFound;
            IDocument offerDom;
            int numOfRetrying = Convert.ToInt32(Resources.NumberOfLoadRetrying);
            while(true)
            {
                offerHtml = WebHelpers.GetHtmlThrowProxy(offerLink,currentProxy);
                if(offerHtml == Constants.WebAttrsNames.NotFound)
                {
                    UpdateInternalProxy();
                    Console.WriteLine("Хреновый прокси! {0}", offerLink);
                }
                else
                {
                    break;
                }
            }
            
            if(offerHtml != Constants.WebAttrsNames.NotFound)
            {
                string offerPageHtmlReplacedOurData = offerHtml.Replace("trulia.propertyData.set", "var ourdata = ");
                offerDom = parser.Parse(offerPageHtmlReplacedOurData); //костыль, призванный решить проблему с не работающими методами сайта в голом HTML(без внешних JS)
                System.Threading.Thread.Sleep(500);
                try
                {
                    ObjectInstance basicData = offerDom.ExecuteScript("ourdata") as ObjectInstance; //получаем JS-переменную, и теперь по ключам вытаскиваем данные
                    Console.WriteLine("Получена страница: {0}", offerLink);
                    Offer o = new Offer(basicData);
                    o.directLink = offerLink;
                    o.FillFromHtmlDocument(offerDom);
                    DataProvider.Instance.InsertOfferToDb(o);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Ошибка,{0},{1}",ex.Message,offerHtml);
                }
            }
            else
            {
                Console.WriteLine("Ссылка на предложение битая, {0}", offerLink);
            }
        }

        private List<string> GetOffersLinks(string regionLink)
        {
            int switchProxyRemindCounter = 0; //счетчик количества использования одного прокси
            string nextPageLink = regionLink;
            IElement nextPageLinkDom = null;            
            List<string> offerLinks = new List<string>();
            while (true)
            {
                switchProxyRemindCounter++;
                string searchResultPageHtml;
                while (true)
                {
                    searchResultPageHtml = WebHelpers.GetHtmlThrowProxy(nextPageLink, currentProxy); //скачали страницу с выдачей
                    if(searchResultPageHtml != Constants.WebAttrsNames.NotFound)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Хреновый прокси! ");
                        UpdateInternalProxy();
                    }
                }
                
                var searchResultPageDom = parser.Parse(searchResultPageHtml); //перегнали в DOM
                var offerLinksDom = searchResultPageDom.QuerySelectorAll(Constants.OfferListSelectors.OfferLinks); //получили все ссылки на предложения
                if (offerLinksDom != null) //если ссылки на странице есть, собираем в список
                {
                    offerLinks.AddRange(offerLinksDom.ToList().Select(i => Resources.BaseLink+i.GetAttribute(Constants.WebAttrsNames.href)));
                    Console.WriteLine("Добавлено предложений в список/всего: {0}/{1}, {2}",offerLinksDom.Length, offerLinks.Count, nextPageLink);
                }
                else
                {
                    Console.WriteLine("На этой странице нет ссылок: {0}", nextPageLink);
                    break;
                }
                var findNextPageButton = searchResultPageDom.QuerySelectorAll(Constants.OfferListSelectors.NextPage); //ищем кнопку на след страницу
                nextPageLinkDom = null;
                for(int o = 0;o < findNextPageButton.Length;o++)
                {
                    if(findNextPageButton[o].TextContent == ">>")
                    {
                        nextPageLinkDom = findNextPageButton[o];
                        break;
                    }
                }
                //nextPageLinkDom = searchResultPageDom.QuerySelector(Constants.OfferListSelectors.NextPage); //обращаемся к элементу, где сидит ссылка на следующую страницу выдачи
                if (nextPageLinkDom != null)
                {
                    if (nextPageLinkDom.GetAttribute(Constants.WebAttrsNames.href) != null)
                    {
                        nextPageLink = nextPageLinkDom.GetAttribute(Constants.WebAttrsNames.href).Replace("//", "https://");

                        Console.WriteLine("Next page link: {0}", nextPageLink);
                    }
                    else
                    {
                        Console.WriteLine("Ссылки на следующую страницу нет. Закансиваем собирать ссылки этого региона, {0}.", regionLink);
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Ссылки на следующую страницу нет.Закансиваем собирать ссылки этого региона, вероятно, на странице ошибка, {0}.", regionLink);
                    break;
                }
                
                
                if (switchProxyRemindCounter > 10)
                {
                    Console.WriteLine("Refreshing proxy...");
                    UpdateInternalProxy();
                    switchProxyRemindCounter = 0;
                }
            }
            return offerLinks;

        }


    }
}


 
 
        