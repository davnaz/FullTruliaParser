using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using Jint.Native.Object;
using TruliaParser.Components;

namespace TruliaParser
{
    internal class Offer
    {
        //Это те, которые берутся из JS-переменной
        public long postId { get; set; }     
        /// <summary>
        /// контактное лицо
        /// </summary>
        public string agentName { get; set; }     //контактное лицо
        public string addressForDisplay { get; set; }
        public string city { get; set; }                
        public string county { get; set; }              
        public string countyFIPS { get; set; }   
        /// <summary>
        /// 
        /// </summary>
        public string dataPhotos { get; set; }          //JSON с фотками
        public int feedId { get; set; }              
        public string formattedBedAndBath { get; set; } 
        public string formattedPrice { get; set; }      
        public string formattedSqft { get; set; }    
        /// <summary>
        /// наличие фоток в предложении
        /// </summary>
        public bool hasPhotos { get; set; }           //наличие фоток в предложении
        /// <summary>
        /// флаг сообщества
        /// </summary>
        public bool isRentalCommunity { get; set; }   //признак(флаг) сообщества
        /// <summary>
        /// широта
        /// </summary>
        public double latitude { get; set; }            //широта
        /// <summary>
        /// долгота
        /// </summary>
        public double longitude { get; set; }        //долгота   
        /// <summary>
        /// идентификатор локации
        /// </summary>
        public int locationId { get; set; }          
        public long listingId { get; set; }           
        public int numBathrooms { get; set; }        
        public int numBedrooms { get; set; }         
        public int numBeds { get; set; }       
        /// <summary>
        /// количество ванных комнат
        /// </summary>
        public int numFullBathrooms { get; set; }    //количество ванных комнат
        /// <summary>
        /// количетсво общих ванн. комнат
        /// </summary>
        public int numPartialBathrooms { get; set; }  //количетсво общих ванн. комнат
        ///// <summary>
        ///// ссылка до предложения (не включая главный домен)
        ///// </summary>
        //public string pdpURL { get; set; }              //ссылка до предложения (не включая главный домен)
        public double price { get; set; }               
        /// <summary>
        /// внутренняя оценка сервиса
        /// </summary>
        public int truliaRank { get; set; }      //внутренняя оценка сервиса
        /// <summary>
        /// тип жилья
        /// </summary>
        public string rentalType { get; set; }                //тип жилья
        /// <summary>
        /// индекс
        /// </summary>
        public string zipCode { get; set; }             //индекс
        /// <summary>
        /// номер улицы(дома)
        /// </summary>
        public int streetNumber { get; set; }      //номер улицы(дома)
        /// <summary>
        /// адрес иконки
        /// </summary>
        public string thumbnail { get; set; }           //адрес иконки
        /// <summary>
        /// площадь в  кв. футах
        /// </summary>
        public double sqft { get; set; }                //площадь в  кв. футах 
        /// <summary>
        /// код штата из двух букв
        /// </summary>
        public string stateCode { get; set; }           //код из двух букв
        /// <summary>
        /// название штата      
        /// </summary>
        public string stateName { get; set; }     //название штата      
        /// <summary>
        /// улица
        /// </summary>
        public string street { get; set; } //улица

        //добавим те, которые берутся из кода HTML

        /// <summary>
        /// телефон
        /// </summary>
        public string phone { get; set; }  //телефон
        /// <summary>
        /// идеальный уровень дохода в год, чтобы снять эту хату
        /// </summary>
        public double idealIncome { get; set; } = -1; //идеальный уровень дохода в год, чтобы снять эту хату
        /// <summary>
        /// описание
        /// </summary>
        public string description { get; set; } //описание
        /// <summary>
        /// тэги перед описанием
        /// </summary>
        public string metaInfo { get; set; }        //тэги перед описанием
        /// <summary>
        /// особенности
        /// </summary>
        public string features{ get; set; } //особенности
        /// <summary>
        /// если это сообщество, то в описании есть доп. поле с фичами
        /// </summary>
        public string communityOtherFeatures { get; set; } // если это сообщество, то в описании есть доп. поле с фичами
        /// <summary>
        /// подпредложения в JSON
        /// </summary>
        public string communityFloors { get; set; } //подпредложения в JSON
        /// <summary>
        /// прямая ссылка на предложение
        /// </summary>
        public string directLink { get; set; }


        public Offer(ObjectInstance data)
        {
            this.addressForDisplay = StringGetFromJintObject(data, Constants.OfferJSObjectKeys.addressForDisplay);
            this.agentName = StringGetFromJintObject(data, Constants.OfferJSObjectKeys.agentName);
            this.city = StringGetFromJintObject(data, Constants.OfferJSObjectKeys.city);
            this.county = StringGetFromJintObject(data, Constants.OfferJSObjectKeys.county);
            this.countyFIPS = StringGetFromJintObject(data, Constants.OfferJSObjectKeys.countyFIPS);
            this.dataPhotos = StringGetFromJintObject(data, Constants.OfferJSObjectKeys.dataPhotos);
            this.feedId = IntGetFromJintObject(data, Constants.OfferJSObjectKeys.feedId);
            this.formattedBedAndBath = StringGetFromJintObject(data, Constants.OfferJSObjectKeys.formattedBedAndBath);
            this.formattedPrice = StringGetFromJintObject(data, Constants.OfferJSObjectKeys.formattedPrice);
            this.formattedSqft = StringGetFromJintObject(data, Constants.OfferJSObjectKeys.formattedSqft);
            this.hasPhotos = BoolGetFromJintObject(data, Constants.OfferJSObjectKeys.hasPhotos);
            this.postId = LongGetFromJintObject(data, Constants.OfferJSObjectKeys.id);
            this.isRentalCommunity = BoolGetFromJintObject(data, Constants.OfferJSObjectKeys.isRentalCommunity);
            this.latitude = DoubleGetFromJintObject(data, Constants.OfferJSObjectKeys.latitude);
            this.longitude = DoubleGetFromJintObject(data, Constants.OfferJSObjectKeys.longitude);
            this.locationId = IntGetFromJintObject(data, Constants.OfferJSObjectKeys.locationId);
            this.listingId = LongGetFromJintObject(data, Constants.OfferJSObjectKeys.listingId);
            this.numBathrooms = IntGetFromJintObject(data, Constants.OfferJSObjectKeys.numBathrooms);
            this.numBedrooms = IntGetFromJintObject(data, Constants.OfferJSObjectKeys.numBedrooms);
            this.numBeds = IntGetFromJintObject(data, Constants.OfferJSObjectKeys.numBeds);
            this.numFullBathrooms = IntGetFromJintObject(data, Constants.OfferJSObjectKeys.numFullBathrooms);
            this.numPartialBathrooms = IntGetFromJintObject(data, Constants.OfferJSObjectKeys.numPartialBathrooms);
            //this.pdpURL = Resources.BaseLink + StringGetFromJintObject(data, Constants.OfferJSObjectKeys.pdpURL);
            this.price = DoubleGetFromJintObject(data, Constants.OfferJSObjectKeys.price);
            this.sqft = DoubleGetFromJintObject(data, Constants.OfferJSObjectKeys.sqft);
            this.stateCode = StringGetFromJintObject(data, Constants.OfferJSObjectKeys.stateCode);
            this.stateName = StringGetFromJintObject(data, Constants.OfferJSObjectKeys.stateName);
            this.street = StringGetFromJintObject(data, Constants.OfferJSObjectKeys.street);
            this.streetNumber = IntGetFromJintObject(data, Constants.OfferJSObjectKeys.streetNumber);
            this.thumbnail = StringGetFromJintObject(data, Constants.OfferJSObjectKeys.thumbnail).Replace("//", "https://");
            this.truliaRank = IntGetFromJintObject(data, Constants.OfferJSObjectKeys.truliaRank);
            this.rentalType = StringGetFromJintObject(data, Constants.OfferJSObjectKeys.type);
            this.zipCode = StringGetFromJintObject(data, Constants.OfferJSObjectKeys.zipCode);

        }

        internal void FillFromHtmlDocument(IDocument offerDom)
        {
            //добываем телефон
            try
            {
                IElement phoneDom = offerDom.QuerySelector(Constants.OfferPageSelectors.phone);
                if (phoneDom != null)
                {
                    this.phone = phoneDom.TextContent;
                }
                else
                {
                    phoneDom = offerDom.QuerySelector(Constants.OfferPageSelectors.phoneAlt);
                    if (phoneDom != null)
                    {
                        this.phone = phoneDom.TextContent;
                    }
                    else this.phone = String.Empty;
                }
            }
            catch(Exception ex)
            {                
                //this.phone = String.Empty;
                Console.WriteLine("Ошибка при получении номера телефона, {0},{1}", this.directLink, ex.Message);
            }

            //добываем описание
            try
            {
                IElement descriptionDom = offerDom.QuerySelector(Constants.OfferPageSelectors.description);
                this.description = descriptionDom != null ? descriptionDom.TextContent : String.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при получении описания, {0},{1}", this.directLink, ex.Message);
            }
            //добываем уровень дохода
            try
            {
                IElement idealIncomeDom = offerDom.QuerySelector(Constants.OfferPageSelectors.idealIncome);
                this.idealIncome = idealIncomeDom != null ? Double.Parse(idealIncomeDom.TextContent.TrimStart('$').Replace(",","")) : -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при получении уровня дохода, {0},{1}", this.directLink, ex.Message);
            }
            //добываем метатеги
            try
            {
                var metaInfoDom = offerDom.QuerySelectorAll(Constants.OfferPageSelectors.metaInfo);
                string meta = String.Empty;
                foreach(IElement i in metaInfoDom)
                {
                    meta += i.TextContent + "\n";
                }
                this.metaInfo = meta;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при получении метатегов, {0},{1}", this.directLink, ex.Message);
            }

            //теперь парсим те параметры, которые различаются
            if (this.isRentalCommunity)
            {
                //сообщество
                //парсим feratures
                try
                {
                    var featuresDom = offerDom.QuerySelectorAll(Constants.OfferPageSelectors.featuresCommunity);
                    string features = String.Empty;
                    foreach (IElement i in featuresDom)
                    {
                        features += i.TextContent + "\n";
                    }
                    this.features = features;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при получении features, {0},{1}", this.directLink, ex.Message);
                }
                //парсим communityOtherFeatures
                try
                {
                    IElement featuresDom = offerDom.QuerySelector(Constants.OfferPageSelectors.communityOtherFeatures).ParentElement;                    
                    this.features += featuresDom!= null ? featuresDom.TextContent : String.Empty;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при получении additional features, {0},{1}", this.directLink, ex.Message);
                }
                //парсим  communityFloors
                try
                {
                    var communityFloorsDom = offerDom.QuerySelectorAll(Constants.OfferPageSelectors.communityFloors);
                    string communityFloors = String.Empty;
                    foreach (IElement i in communityFloorsDom)
                    {
                        communityFloors += i.GetAttribute("data-floorplan") + " }|{ ";
                    }
                    this.communityFloors = communityFloors;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при получении communityFloors, {0},{1}", this.directLink, ex.Message);
                }

            }
            else
            {
                //одиночное предложение
                try
                {
                    var featuresDom = offerDom.QuerySelectorAll(Constants.OfferPageSelectors.featuresSingle);
                    string features = String.Empty;
                    foreach (IElement i in featuresDom)
                    {
                        features += i.TextContent + "\n";
                    }
                    this.features = features;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при получении features, {0},{1}", this.directLink, ex.Message);
                }
                this.communityFloors = "No need";
                this.communityOtherFeatures = "No need;";
            }
        }






        /// <summary>
        /// Возвращает объект типа int из объекта ObjectInstance по заданному строковому параметру 
        /// </summary>
        /// <param name="data">Исходный объект</param>
        /// <param name="propertyName">Параметр(propertyName), который нужно получить</param>
        /// <returns></returns>
        internal int IntGetFromJintObject(ObjectInstance data,string propertyName)
        {
            try
            {


                if (data.HasProperty(propertyName))
                {
                    if (!data.Get(propertyName).IsNull())
                    {
                        return Convert.ToInt32(data.Get(propertyName).ToString());
                    }
                    else
                    {
                        return -1;
                    }

                }
                else
                {
                    return -1;
                }
            }
            catch
            {
                return -1;
            }
        }
        internal double DoubleGetFromJintObject(ObjectInstance data, string propertyName)
        {
            try
            {
                if (data.HasProperty(propertyName))
                {
                    if (!data.Get(propertyName).IsNull())
                    {
                        return Convert.ToDouble(data.Get(propertyName).ToString());
                    }
                    else
                    {
                        return -1;
                    }

                }
                else
                {
                    return -1;
                }
            }
            catch
            {
                return -1;
            }
        }
        internal string StringGetFromJintObject(ObjectInstance data, string propertyName)
        {
            try
            {
                if (data.HasProperty(propertyName))
                {
                    if (!data.Get(propertyName).IsNull())
                    {
                        return data.Get(propertyName).ToString();
                    }
                    else
                    {
                        return String.Empty;
                    }

                }
                else
                {
                    return String.Empty;
                }
            }
            catch
            {
                return String.Empty;
            }            
        }
        internal long LongGetFromJintObject(ObjectInstance data, string propertyName)
        {
            try
            {


                if (data.HasProperty(propertyName))
                {
                    if (!data.Get(propertyName).IsNull())
                    {
                        return Convert.ToInt64(data.Get(propertyName).ToString());
                    }
                    else
                    {
                        return -1;
                    }

                }
                else
                {
                    return -1;
                }
            }
            catch
            {
                return -1;
            }
        }
        internal bool BoolGetFromJintObject(ObjectInstance data, string propertyName)
        {
            try
            {


                if (data.HasProperty(propertyName))
                {
                    if (!data.Get(propertyName).IsNull())
                    {
                        return Convert.ToBoolean(data.Get(propertyName).ToString());
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        //public static void Print(Offer o)
        //{
        //    Console.WriteLine("City: " + o.City);
        //    Console.WriteLine("ID); " + o.PostID);
        //    Console.WriteLine("Name); " + o.Name);
        //    Console.WriteLine("Price); " + o.Price);
        //    Console.WriteLine("PlaceName); " + o.PlaceName);
        //    Console.WriteLine("PlaceMapsLink); " + o.PlaceMapsLink);
        //    Console.WriteLine("Description); " + o.Description);
        //    Console.WriteLine("BathRooms); " + o.BathRooms);
        //    Console.WriteLine("BedRooms); " + o.BedRooms);
        //    Console.WriteLine("Square); " + o.Square);
        //    Console.WriteLine("Availability); " + o.Availability);
        //    Console.WriteLine("Additional); " + o.Additional);
        //    Console.WriteLine("Images); " + o.Images);
        //    Console.WriteLine("Posted); " + o.Posted);
        //    Console.WriteLine("Updated); " + o.Updated);
        //}

    }

   

}

