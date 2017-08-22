using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using FTParser.DataProviders;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using AngleSharp;
using AngleSharp.Extensions;
using Jint.Native.Object;
using TruliaParser;
using OpenQA.Selenium.PhantomJS;
using FT.Components;
using NLog;


namespace FTParser.Components
{
    class Parser
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();
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

        internal static void FinalizeCity(City city)
        {
            try
            {
                SqlCommand finalCity = DataProvider.Instance.CreateSQLCommandForSP(Resources.SP_FinalizeCity);
                finalCity.Parameters.AddWithValue("@ID", city.ID);
                DataProvider.Instance.ExecureSP(finalCity);
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка финализирования города {2} {3} {0},{1}", ex.Message, ex.StackTrace, city.ID, city.CityName);
            }

        }

        /// <summary>
        /// Получает данные о штатах со страницы https://www.trulia.com/property-sitemap/ и записывает в БД
        /// </summary>
        public static void GetStateListToDb()
        {
            var driver = new PhantomJSDriver(ProxySolver.GetServiceForDriver());
            driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
            string statesLink = "https://www.trulia.com/property-sitemap/";
            driver.Navigate().GoToUrl(statesLink);
            var linksElemsList = driver.FindElementsByCssSelector(".content .cols12 li a");
            foreach (var linkElem in linksElemsList)
            {
                Console.WriteLine(linkElem.GetAttribute(Constants.WebAttrsNames.href) + ",   " + linkElem.Text.Remove(linkElem.Text.Length - 7, 6));
                SqlCommand insertState = DataProvider.Instance.CreateSQLCommandForSP(Resources.SP_AddNewState);
                insertState.Parameters.AddWithValue("@StateName", linkElem.Text.Remove(linkElem.Text.Length - 7, 6));
                insertState.Parameters.AddWithValue("@Link", linkElem.GetAttribute(Constants.WebAttrsNames.href));
                DataProvider.Instance.ExecureSP(insertState);
            }
            driver.Quit();
        }

        // public static List<string> ParseHomes(Street street)
        public static List<string> ParseHomes(string Link)
        { 
          PhantomJSDriver driver = CreateDriver();

            try
            {
                if (driver.SessionId == null)
                {
                    driver = CreateDriver();
    }

                //переход по стартовой ссылке города
                while (true)
                {
                    try
                    {
                        driver.Navigate().GoToUrl(Link);
                        break;
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex, "Ошибка получения страницы, время ожидания истекло. {0},{1}", ex.Message, ex.StackTrace);
                        driver.Quit();
                        driver = CreateDriver();
                    }
                }

                //забор элементов со страниц
                List<string> homeHrefs = new List<string>(); //создаем список для ссылок на дома(property) 
                //сначала в цикле while соберем ссылки на дома в список
                while (true)
                {
                    var homelinks = driver.FindElementsByCssSelector(".mvm a"); //выбираем элементы, в которых есть нужные нам ссылки
                    if (homelinks.Count == 0) //т.е. улица пуста
                    {
                        return homeHrefs;
                    }

                    foreach (var a in homelinks)
                    {
                        homeHrefs.Add(a.GetAttribute(Constants.WebAttrsNames.href));
                    }

                    var nextButtons = driver.FindElementsByCssSelector("*[rel=next]");
                    if (nextButtons.Count > 0)
                    {
                        while (true)
                        {
                            try
                            {

                                nextButtons[0].Click();
                                break;
                            }
                            catch (OpenQA.Selenium.WebDriverTimeoutException ex)
                            {
                                //throw new Exception();
                                logger.Trace(ex, "Ошибка получения страницы, время ожидания истекло.");
                                logger.Error(ex, "Ошибка получения страницы, время ожидания истекло.");
                            }
                            catch (OpenQA.Selenium.StaleElementReferenceException ex)
                            {
                                //throw new Exception();
                                logger.Trace(ex, "Отсутствует данный Элемент на странице(кнопка Next)");
                                logger.Error("Отсутствует данный Элемент на странице(кнопка Next)");
                                break;
                            }
                            catch (Exception ex)
                            {

                                logger.Error(ex, "Неизвестная ошибка: {1}, {0}", ex.StackTrace, ex.Message);
                                logger.Trace(ex, "Неизвестная ошибка: {1}, {0}", ex.StackTrace, ex.Message);
                                throw;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                foreach(string link in homeHrefs) //а теперь сам процесс обработки домов по ссылкам
                {

                    Console.WriteLine(link);
                }
                return homeHrefs; //все прошло успешно
            }
            catch (OpenQA.Selenium.WebDriverException ex)
            {
                logger.Error(ex, "Возникло исключение web-драйвера: {1}, {0}", ex.StackTrace, ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Возникло неизвестное исключение: {1}, {0}", ex.StackTrace, ex.Message);
                return null;
            }
            finally
            {
                driver.Quit();
            }
        }

        public static bool ParseProperty(string link)
        {
            PhantomJSDriver driver = CreateDriver();

            try
            {
                if (driver.SessionId == null)
                {
                    driver = CreateDriver();
                }

                //переход по стартовой ссылке города
                while (true)
                {
                    try
                    {
                        driver.Navigate().GoToUrl(link);
                        break;
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex, "Ошибка получения страницы, время ожидания истекло. {0},{1}", ex.Message, ex.StackTrace);
                        driver.Quit();
                        driver = CreateDriver();
                    }
                }
                
                if (!link.Contains("sold"))
                {
                    Console.WriteLine("Property isn't sold."+ link);
                    logger.Info("Property isn't sold. {0}", link);
                    return false;
                }

                Dictionary<string, object> mainProperties = driver.ExecuteScript("return trulia.pdp.propertyJSON") as Dictionary<string, object>;
                HomeProperty hp = new HomeProperty();
                hp.addressForDisplay = (string)mainProperties[Constants.HomePropertyJSObjectKeys.addressForDisplay];
                hp.addressForLeadForm = (string)mainProperties[Constants.HomePropertyJSObjectKeys.addressForLeadForm];
                hp.agentName = (string)mainProperties[Constants.HomePropertyJSObjectKeys.agentName];
                hp.apartmentNumber = (string)mainProperties[Constants.HomePropertyJSObjectKeys.apartmentNumber];
                hp.builderCommunityId = (string)mainProperties[Constants.HomePropertyJSObjectKeys.builderCommunityId];
                hp.builderName = (string)mainProperties[Constants.HomePropertyJSObjectKeys.builderName];
                hp.city = (string)mainProperties[Constants.HomePropertyJSObjectKeys.city];
                hp.communityFloors = null; //!!!
                hp.communityOtherFeatures = null;   //!!!
                hp.county = (string)mainProperties[Constants.HomePropertyJSObjectKeys.county];
                hp.countyFIPS = (string)mainProperties[Constants.HomePropertyJSObjectKeys.countyFIPS];
                hp.dataPhotos = (string)mainProperties[Constants.HomePropertyJSObjectKeys.dataPhotos];
                hp.description = null; //!!!!!!!!!!!!!!!!
                hp.directLink = link;
                hp.features = null; //!!!!!!!!!!
                hp.formattedBedAndBath = (string)mainProperties[Constants.HomePropertyJSObjectKeys.formattedBedAndBath];
                hp.formattedLotSize = (string)mainProperties[Constants.HomePropertyJSObjectKeys.formattedLotSize];
                hp.formattedPrice = (string)mainProperties[Constants.HomePropertyJSObjectKeys.formattedPrice];
                hp.formattedSqft = (string)mainProperties[Constants.HomePropertyJSObjectKeys.formattedSqft];
                hp.hasOpenHouse = (bool)mainProperties[Constants.HomePropertyJSObjectKeys.hasOpenHouse];
                hp.hasPhotos = (bool)mainProperties[Constants.HomePropertyJSObjectKeys.hasPhotos];
                hp.HomeDetails = null; //!!!!
                hp.idealIncome = -1; //!!!!
                hp.indexSource = (string)mainProperties[Constants.HomePropertyJSObjectKeys.indexSource];
                hp.isBuilder = (bool)mainProperties[Constants.HomePropertyJSObjectKeys.isBuilder];
                hp.isBuilderCommunity = (bool)mainProperties[Constants.HomePropertyJSObjectKeys.isBuilderCommunity];
                hp.isForeclosure = (bool)mainProperties[Constants.HomePropertyJSObjectKeys.isForeclosure];
                hp.isForSale = (bool)mainProperties[Constants.HomePropertyJSObjectKeys.isForSale];
                hp.isPlan = (bool)mainProperties[Constants.HomePropertyJSObjectKeys.isPlan];
                hp.isPromotedCommunity = (bool)mainProperties[Constants.HomePropertyJSObjectKeys.isPromotedCommunity];
                hp.isRealogy = (bool)mainProperties[Constants.HomePropertyJSObjectKeys.isRealogy];
                hp.isRental = (bool)mainProperties[Constants.HomePropertyJSObjectKeys.isRental];
                hp.isRentalCommunity = (bool)mainProperties[Constants.HomePropertyJSObjectKeys.isRentalCommunity];
                hp.isSpec = (bool)mainProperties[Constants.HomePropertyJSObjectKeys.isSpec];
                hp.isSrpFeatured = (bool)mainProperties[Constants.HomePropertyJSObjectKeys.isSrpFeatured];
                hp.isStudio = (bool)mainProperties[Constants.HomePropertyJSObjectKeys.isStudio];
                hp.isSubsidized = (bool)mainProperties[Constants.HomePropertyJSObjectKeys.isSubsidized];
                hp.lastSaleDate = (string)mainProperties[Constants.HomePropertyJSObjectKeys.lastSaleDate];
                hp.latitude = (double)mainProperties[Constants.HomePropertyJSObjectKeys.latitude];
                hp.listingId = (long)mainProperties[Constants.HomePropertyJSObjectKeys.listingId];
                hp.listingType = (string)mainProperties[Constants.HomePropertyJSObjectKeys.listingType];
                hp.locationId = (string)mainProperties[Constants.HomePropertyJSObjectKeys.locationId];
                hp.longitude = (double)mainProperties[Constants.HomePropertyJSObjectKeys.longitude];
                hp.metaInfo = null;//!!!!
                hp.numBathrooms = (int)mainProperties[Constants.HomePropertyJSObjectKeys.numBathrooms];
                hp.numBedrooms = (int)mainProperties[Constants.HomePropertyJSObjectKeys.numBedrooms];
                hp.numBeds = (int)mainProperties[Constants.HomePropertyJSObjectKeys.numBeds];
                hp.numFullBathrooms = (int)mainProperties[Constants.HomePropertyJSObjectKeys.numFullBathrooms];
                hp.numPartialBathrooms = (int)mainProperties[Constants.HomePropertyJSObjectKeys.numPartialBathrooms];
                hp.pdpURL = (string)mainProperties[Constants.HomePropertyJSObjectKeys.pdpURL];
                hp.PetsAllowed = null;// !!!!!!!!!!!
                hp.phone = null;//!!!!!!!
                hp.postId = (long)mainProperties[Constants.HomePropertyJSObjectKeys.postId];
                hp.pricePerSqft = (string)mainProperties[Constants.HomePropertyJSObjectKeys.pricePerSqft];
                hp.PublicRecords = null; //!!!!!!
                hp.rentalPartnerDisplayText = (string)mainProperties[Constants.HomePropertyJSObjectKeys.rentalPartnerDisplayText];
                hp.type = (string)mainProperties[Constants.HomePropertyJSObjectKeys.type];
                hp.typeDisplay = (string)mainProperties[Constants.HomePropertyJSObjectKeys.typeDisplay];
                hp.shortDescription = (string)mainProperties[Constants.HomePropertyJSObjectKeys.shortDescription];
                hp.sqft = (double)mainProperties[Constants.HomePropertyJSObjectKeys.sqft];
                hp.stateCode = (string)mainProperties[Constants.HomePropertyJSObjectKeys.stateCode];
                hp.stateName = (string)mainProperties[Constants.HomePropertyJSObjectKeys.stateName];
                hp.status = (string)mainProperties[Constants.HomePropertyJSObjectKeys.status];
                hp.street = (string)mainProperties[Constants.HomePropertyJSObjectKeys.street];
                hp.streetNumber = (string)mainProperties[Constants.HomePropertyJSObjectKeys.streetNumber];
                hp.yearBuilt = (string)mainProperties[Constants.HomePropertyJSObjectKeys.yearBuilt];
                hp.zipCode = (string)mainProperties[Constants.HomePropertyJSObjectKeys.zipCode];


                return true; //все прошло успешно
            }
            catch (OpenQA.Selenium.WebDriverException ex)
            {
                logger.Error(ex, "Возникло исключение web-драйвера: {1}, {0}", ex.StackTrace, ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Возникло неизвестное исключение: {1}, {0}", ex.StackTrace, ex.Message);
                return false;
            }
            finally
            {
                driver.Quit();
            }
        }

        public static void GetCityListToDb(State state)
        {
            while (true)
            {
                PhantomJSDriver driver;
                while (true)
                {
                    try
                    {
                        driver = new PhantomJSDriver(ProxySolver.GetServiceForDriver());
                        break;
                    }
                    catch(Exception ex)
                    {
                        logger.Error(ex, "Ошибка создания драйвера: {0},{1}", ex.Message, ex.StackTrace);
                    }
                }
                
                try
                {
                    driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
                    driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
                    while (true)
                    {
                        try
                        {
                            driver.Navigate().GoToUrl(state.Link);
                            break;
                        }
                        catch (Exception ex)
                        {
                            logger.Trace(ex, "Ошибка получения страницы, время ожидания истекло. {0},{1}", ex.Message, ex.StackTrace);
                            driver.Quit();
                            driver = new PhantomJSDriver(ProxySolver.GetServiceForDriver());
                            driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
                            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
                        }
                    }

                    while (true)
                    {
                        var citylinks = driver.FindElementsByCssSelector("dl a");


                        foreach (var a in citylinks)
                        {
                            string Text = a.Text;
                            Text = Text.Remove(Text.Length - 7, 6);
                            string href = a.GetAttribute(Constants.WebAttrsNames.href);
                            //Console.WriteLine("City: {0}, Link: {1}, Count: {2}", Text.Replace(Text.Split(' ')[0], String.Empty), href, Int32.Parse(Text.Split(' ')[0].Replace(",", String.Empty)));
                            SqlCommand insertCity = DataProvider.Instance.CreateSQLCommandForSP(Resources.SP_AddNewCity);
                            insertCity.Parameters.AddWithValue("@StateName", state.StateName);
                            insertCity.Parameters.AddWithValue("@Link", href);
                            insertCity.Parameters.AddWithValue("@CityName", Text.Replace(Text.Split(' ')[0], String.Empty));
                            insertCity.Parameters.AddWithValue("@Count", Int32.Parse(Text.Split(' ')[0].Replace(",", String.Empty)));
                            DataProvider.Instance.ExecureSP(insertCity);
                        }
                        var nextButtons = driver.FindElementsByCssSelector("*[rel=next]");
                        if (nextButtons.Count > 0)
                        {
                            while (true)
                            {
                                try
                                {

                                    nextButtons[0].Click();
                                    break;
                                }
                                catch (OpenQA.Selenium.WebDriverTimeoutException ex)
                                {
                                    //throw new Exception();
                                    logger.Trace(ex, "Ошибка получения страницы, время ожидания истекло.");
                                    logger.Error(ex, "Ошибка получения страницы, время ожидания истекло.");
                                }
                                catch (OpenQA.Selenium.StaleElementReferenceException ex)
                                {
                                    //throw new Exception();
                                    logger.Trace(ex, "Отсутствует данный Элемент на странице(кнопка Next)");
                                    logger.Error("Отсутствует данный Элемент на странице(кнопка Next)");
                                    break;
                                }
                                catch (Exception ex)
                                {

                                    logger.Error(ex, "Неизвестная ошибка: {1}, {0}", ex.StackTrace, ex.Message);
                                    logger.Trace(ex, "Неизвестная ошибка: {1}, {0}", ex.StackTrace, ex.Message);
                                    throw;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    driver.Quit();
                    break;
                }
                catch (OpenQA.Selenium.WebDriverException ex)
                {
                    driver.Quit();
                    logger.Error(ex, "Возникло исключение web-драйвера: {1}, {0}", ex.StackTrace, ex.Message);
                }
            }
            //foreach (var linkElem in linksElemsList)
            //{
            //    Console.WriteLine(linkElem.GetAttribute(Constants.WebAttrsNames.href) + ",   " + linkElem.Text.Replace(" homes", String.Empty));
            //    SqlCommand insertState = DataProvider.Instance.CreateSQLCommandForSP(Resources.SP_AddNewState);
            //    insertState.Parameters.AddWithValue("@StateName", linkElem.Text.Replace(" homes", String.Empty));
            //    insertState.Parameters.AddWithValue("@Link", linkElem.GetAttribute(Constants.WebAttrsNames.href));
            //    DataProvider.Instance.ExecureSP(insertState);
            //}

        }

        public static bool GetSteetsToDb(City city)
        {
            PhantomJSDriver driver = CreateDriver();

            try
            {
                if (driver.SessionId == null)
                {
                    driver = CreateDriver();
                }

                //переход по стартовой ссылке города
                while (true)
                {
                    try
                    {
                        driver.Navigate().GoToUrl(city.Link);
                        break;
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex, "Ошибка получения страницы, время ожидания истекло. {0},{1}", ex.Message, ex.StackTrace);
                        driver.Quit();
                        driver = CreateDriver();
                    }
                }

                //забор элементов со страниц
                while (true)
                {
                    var streetlinks = driver.FindElementsByCssSelector(".mvm a");


                    foreach (var a in streetlinks)
                    {
                        string Text = a.Text;
                        Text = Text.Replace("homes in ", String.Empty);
                        string href = a.GetAttribute(Constants.WebAttrsNames.href);
                        Console.WriteLine("State: {2}, City: {3} Street: {0}, Link: {1}", Text, href, city.StateName, city.CityName);
                        SqlCommand insertStreet = DataProvider.Instance.CreateSQLCommandForSP(Resources.SP_AddNewStreet);
                        insertStreet.Parameters.AddWithValue("@StateName", city.StateName);
                        insertStreet.Parameters.AddWithValue("@Link", href);
                        insertStreet.Parameters.AddWithValue("@CityName", city.CityName);
                        insertStreet.Parameters.AddWithValue("@StreetName", Text);
                        insertStreet.Parameters.AddWithValue("@ZIP", Text.Split(' ')[Text.Split(' ').Length - 1]);
                        DataProvider.Instance.ExecureSP(insertStreet);
                    }
                    var nextButtons = driver.FindElementsByCssSelector("*[rel=next]");
                    if (nextButtons.Count > 0)
                    {
                        while (true)
                        {
                            try
                            {

                                nextButtons[0].Click();
                                break;
                            }
                            catch (OpenQA.Selenium.WebDriverTimeoutException ex)
                            {
                                //throw new Exception();
                                logger.Trace(ex, "Ошибка получения страницы, время ожидания истекло.");
                                logger.Error(ex, "Ошибка получения страницы, время ожидания истекло.");
                            }
                            catch (OpenQA.Selenium.StaleElementReferenceException ex)
                            {
                                //throw new Exception();
                                logger.Trace(ex, "Отсутствует данный Элемент на странице(кнопка Next)");
                                logger.Error("Отсутствует данный Элемент на странице(кнопка Next)");
                                break;
                            }
                            catch (Exception ex)
                            {

                                logger.Error(ex, "Неизвестная ошибка: {1}, {0}", ex.StackTrace, ex.Message);
                                logger.Trace(ex, "Неизвестная ошибка: {1}, {0}", ex.StackTrace, ex.Message);
                                throw;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                return true; //все прошло успешно
            }
            catch (OpenQA.Selenium.WebDriverException ex)
            {
                logger.Error(ex, "Возникло исключение web-драйвера: {1}, {0}", ex.StackTrace, ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Возникло неизвестное исключение: {1}, {0}", ex.StackTrace, ex.Message);
                return false;
            }
            finally
            {
                driver.Quit();
            }
        }

        private static PhantomJSDriver CreateDriver()
        {
            PhantomJSDriver driver;
            while (true)
            {
                try
                {
                    driver = new PhantomJSDriver(ProxySolver.GetServiceForDriver());
                    driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
                    driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
                    break;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Ошибка создания драйвера: {0},{1}", ex.Message, ex.StackTrace);
                }
            }

            return driver;
        }

        private void ParseRegionsToDb()
        {
            DataProvider.Instance.ExecureSP(DataProvider.Instance.CreateSQLCommandForSP(Resources.ClearRegionsSPName)); //чистим таблицу с ссылками на регионы
            SqlCommand insertLink = DataProvider.Instance.CreateSQLCommandForSP(Resources.InsertRegionSP); //подготовка процедуры для занесения ссылок в БД
            string rentMapLink = "https://www.FT.com/rent-sitemap/";
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
        /// Парсит регион сайта FT  
        /// </summary>
        /// <param name="regionLink">Полный адрес сайта региона для выборки</param>
        public void StartParsing(Region region) //
        {
            string regionLink = region.Link;
            Console.WriteLine("Beginning the parsing new region:\nState: {0}, County: {1}, Link: {2}", region.State, region.RegionName, region.Link);
            List<string> offerLinks = new List<string>(GetOffersLinks(regionLink));
            offerLinks.ForEach(i => ParseOffer(i));
            if (offerLinks.Count >= region.OffersCount * 0.9 || (offerLinks.Count - region.OffersCount < 20))
            {
                DataProvider.Instance.FinalizeRegion(region);
            }
            else
            {
                Console.WriteLine("Кажется, регион неправильно спарсился(забрал ссылки)собрано/ожидалось: {0},{1}", offerLinks.Count, region.OffersCount);
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
                    int offersCount = Convert.ToInt32(regionPageDom.QuerySelector(Constants.OfferListSelectors.OffersCount).TextContent.Trim('(', ')'));
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
            while (true)
            {
                offerHtml = WebHelpers.GetHtmlThrowProxy(offerLink, currentProxy);
                if (offerHtml == Constants.WebAttrsNames.NotFound)
                {
                    UpdateInternalProxy();
                    Console.WriteLine("Хреновый прокси! {0}", offerLink);
                }
                else
                {
                    break;
                }
            }

            if (offerHtml != Constants.WebAttrsNames.NotFound)
            {
                string offerPageHtmlReplacedOurData = offerHtml.Replace("FT.propertyData.set", "var ourdata = ");
                offerDom = parser.Parse(offerPageHtmlReplacedOurData); //костыль, призванный решить проблему с не работающими методами сайта в голом HTML(без внешних JS)
                System.Threading.Thread.Sleep(500);
                try
                {
                    ObjectInstance basicData = offerDom.ExecuteScript("ourdata") as ObjectInstance; //получаем JS-переменную, и теперь по ключам вытаскиваем данные
                    Console.WriteLine("Получена страница: {0}", offerLink);
                    //Offer o = new Offer(basicData);
                    //o.directLink = offerLink;
                    //o.FillFromHtmlDocument(offerDom);
                    //DataProvider.Instance.InsertOfferToDb(o);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка,{0},{1}", ex.Message, offerHtml);
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
                    if (searchResultPageHtml != Constants.WebAttrsNames.NotFound)
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
                    offerLinks.AddRange(offerLinksDom.ToList().Select(i => Resources.BaseLink + i.GetAttribute(Constants.WebAttrsNames.href)));
                    Console.WriteLine("Добавлено предложений в список/всего: {0}/{1}, {2}", offerLinksDom.Length, offerLinks.Count, nextPageLink);
                }
                else
                {
                    Console.WriteLine("На этой странице нет ссылок: {0}", nextPageLink);
                    break;
                }
                var findNextPageButton = searchResultPageDom.QuerySelectorAll(Constants.OfferListSelectors.NextPage); //ищем кнопку на след страницу
                nextPageLinkDom = null;
                for (int o = 0; o < findNextPageButton.Length; o++)
                {
                    if (findNextPageButton[o].TextContent == ">>")
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




