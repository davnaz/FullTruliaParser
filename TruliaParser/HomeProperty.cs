using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using Jint.Native.Object;
using FTParser.Components;

namespace FTParser
{
    internal class HomeProperty
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
        public string locationId { get; set; }          
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
        /// индекс
        /// </summary>
        public string zipCode { get; set; }             //индекс
        /// <summary>
        /// номер улицы(дома)
        /// </summary>
        public string streetNumber { get; set; }      //номер улицы(дома)
       
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

        public string addressForLeadForm          {get; set;} 
        public string apartmentNumber             {get; set;} 
        public string builderCommunityId          {get; set;} 
        public string builderName                 {get; set;} 
        public string formattedLotSize            {get; set;} 
        public bool hasOpenHouse                {get; set;} 
        public string indexSource                 {get; set;} 
        public bool isBuilder                   {get; set;} 
        public bool isBuilderCommunity          {get; set;} 
        public bool isForSale                   {get; set;} 
        public bool isForeclosure               {get; set;} 
        public bool isPlan                      {get; set;} 
        public bool isPromotedCommunity         {get; set;} 
        public bool isRealogy                   {get; set;} 
        public bool isRental                    {get; set;} 
        public bool isSpec                      {get; set;} 
        public bool isSrpFeatured               {get; set;} 
        public bool isStudio                    {get; set;} 
        public bool isSubsidized                {get; set;} 
        public string lastSaleDate                {get; set;} 
        public string listingType                 {get; set;} 
        public string pdpURL                      {get; set;} 
        public string pricePerSqft                {get; set;} 
        public string rentalPartnerDisplayText    {get; set;} 
        public string shortDescription            {get; set;} 
        public string status                      {get; set;} 
        public string type                        {get; set;} 
        public string typeDisplay                 {get; set;} 
        public string yearBuilt                   {get; set;} 
        public string HomeDetails                 {get; set;} 
        public string PublicRecords               {get; set;} 
        public string PetsAllowed                 {get; set;}                                      




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

              


    }

   

}

