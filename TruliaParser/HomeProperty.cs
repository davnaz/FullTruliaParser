using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using Jint.Native.Object;
using FTParser.Components;
using System.Data.SqlClient;
using TruliaParser;
using NLog;
using FT.Components;

namespace FTParser
{
    internal class HomeProperty
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();
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
        public int yearBuilt                   {get; set;} 
        public string HomeDetails                 {get; set;} 
        public string PublicRecords               {get; set;} 
        public string PetsAllowed                 {get; set;}   
        public string ComparablesJSON {get; set;}

        /// <summary>
        /// Записывает Property в БД и возвращает ID записанной строки
        /// </summary>
        /// <returns>ID записанной строки</returns>
        public long InsertToDb()
        {
            try
            {

            
            SqlCommand insertHP = DataProviders.DataProvider.Instance.CreateSQLCommandForSP(Resources.SP_AddNewHome);
            if(this.addressForDisplay != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.addressForDisplay,   this.addressForDisplay                                   );}
            if(this.addressForLeadForm != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.addressForLeadForm,                    this.addressForLeadForm              );}
            if(this.agentName != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.agentName                           ,this.agentName                                  );}
            if(this.apartmentNumber != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.apartmentNumber                       ,this.apartmentNumber                    );}
            if(this.builderCommunityId != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.builderCommunityId                    ,this.builderCommunityId              );}
            if(this.builderName != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.builderName                           ,this.builderName                            );}
            if(this.city != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.city                                  ,this.city                                          );}
            if(this.communityFloors != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.communityFloors                       ,this.communityFloors                    );}
            if(this.communityOtherFeatures != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.communityOtherFeatures                ,this.communityOtherFeatures      );}
            if(this.ComparablesJSON != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.ComparablesJSON                       ,this.ComparablesJSON                    );}
            if(this.county != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.county                                ,this.county                                      );}
            if(this.countyFIPS != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.countyFIPS                            ,this.countyFIPS                              );}
            if(this.dataPhotos != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.dataPhotos                            ,this.dataPhotos                              );}
            if(this.description != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.Description                        ,this.description                               );}
            if(this.directLink != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.Link                       ,this.directLink                                         );}
            if(this.features != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.Features                              ,this.features                                  );}
            if(this.formattedBedAndBath != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.formattedBedAndBath                   ,this.formattedBedAndBath            );}
            if(this.formattedLotSize != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.formattedLotSize                      ,this.formattedLotSize                  );}
            if(this.formattedPrice != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.formattedPrice                        ,this.formattedPrice                      );}
            if(this.formattedSqft != null){ insertHP.Parameters.AddWithValue(Constants.HomeCellNames.formattedSqft, this.formattedSqft); }
 
             insertHP.Parameters.AddWithValue(Constants.HomeCellNames.hasOpenHouse, this.hasOpenHouse);
            insertHP.Parameters.AddWithValue(Constants.HomeCellNames.hasPhotos,this.hasPhotos);
            if(this.HomeDetails != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.HomeDetails, this.HomeDetails     );}
            if(this.idealIncome != -1){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.idealIncome, Convert.ToInt32( this.idealIncome )     );}
            if (this.indexSource != null) { insertHP.Parameters.AddWithValue(Constants.HomeCellNames.indexSource, this.indexSource); }
            insertHP.Parameters.AddWithValue(Constants.HomeCellNames.isBuilder                               ,this.isBuilder            );
            insertHP.Parameters.AddWithValue(Constants.HomeCellNames.isBuilderCommunity                      ,this.isBuilderCommunity   );
            insertHP.Parameters.AddWithValue(Constants.HomeCellNames.isForeclosure                           ,this.isForeclosure        );
            insertHP.Parameters.AddWithValue(Constants.HomeCellNames.isForSale                               ,this.isForSale            );
            insertHP.Parameters.AddWithValue(Constants.HomeCellNames.isPlan                                  ,this.isPlan               );
            insertHP.Parameters.AddWithValue(Constants.HomeCellNames.isPromotedCommunity                     ,this.isPromotedCommunity  );
            insertHP.Parameters.AddWithValue(Constants.HomeCellNames.isRealogy                               ,this.isRealogy            );
            insertHP.Parameters.AddWithValue(Constants.HomeCellNames.isRental                                ,this.isRental             );
            insertHP.Parameters.AddWithValue(Constants.HomeCellNames.isRentalCommunity                       ,this.isRentalCommunity    );
            insertHP.Parameters.AddWithValue(Constants.HomeCellNames.isSpec                                  ,this.isSpec               );
            insertHP.Parameters.AddWithValue(Constants.HomeCellNames.isSrpFeatured                           ,this.isSrpFeatured        );
            insertHP.Parameters.AddWithValue(Constants.HomeCellNames.isStudio                                ,this.isStudio             );
                        insertHP.Parameters.AddWithValue(Constants.HomeCellNames.isSubsidized, this.isSubsidized);
                        if (this.lastSaleDate != null) { insertHP.Parameters.AddWithValue(Constants.HomeCellNames.lastSaleDate, this.lastSaleDate); }
 
             insertHP.Parameters.AddWithValue(Constants.HomeCellNames.latitude, this.latitude);
            if(this.listingId != 0){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.listingId          ,this.listingId      );         }    
            if(this.listingType != null){ insertHP.Parameters.AddWithValue(Constants.HomeCellNames.listingType, this.listingType);           }
                            if (this.locationId != null) { insertHP.Parameters.AddWithValue(Constants.HomeCellNames.locationId, this.locationId); }
 
             insertHP.Parameters.AddWithValue(Constants.HomeCellNames.longitude, this.longitude);
            if (this.metaInfo != null) { insertHP.Parameters.AddWithValue(Constants.HomeCellNames.metaInfo, this.metaInfo); }
            if(this.numBathrooms != -1){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.numBathrooms                             ,this.numBathrooms              );}
            if(this.numBedrooms != -1){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.numBedrooms                               ,this.numBedrooms               );}
            if(this.numBeds != -1){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.numBeds                                       ,this.numBeds                   );}
            if(this.numFullBathrooms != -1){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.numFullBathrooms                     ,this.numFullBathrooms          );}
            if(this.numPartialBathrooms != -1){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.numPartialBathrooms               ,this.numPartialBathrooms       );}
            if(this.pdpURL != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.pdpURL                                       ,this.pdpURL                    );}
            if(this.PetsAllowed != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.PetsAllowed                             ,this.PetsAllowed               );}
            if(this.phone != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.phone                                         ,this.phone                     );}
            if(this.postId != -1){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.postId                                         ,this.postId                    );}
            if(this.price != 0){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.price                                            ,Convert.ToInt32(this.price )               );}
            if(this.pricePerSqft != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.pricePerSqft                           ,this.pricePerSqft              );}
            if(this.PublicRecords != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.PublicRecords                         ,this.PublicRecords             );}
            if(this.rentalPartnerDisplayText != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.rentalPartnerDisplayText   ,this.rentalPartnerDisplayText  );}
            if(this.shortDescription != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.shortDescription                   ,this.shortDescription          );}
            if(this.sqft != 0)           {insertHP.Parameters.AddWithValue(Constants.HomeCellNames.sqft                                   ,this.sqft                      );}
            if(this.stateCode != null)   {insertHP.Parameters.AddWithValue(Constants.HomeCellNames.stateCode                              ,this.stateCode                 );}
            if(this.stateName != null)   {insertHP.Parameters.AddWithValue(Constants.HomeCellNames.stateName                              ,this.stateName                 );}
            if(this.status != null)      {insertHP.Parameters.AddWithValue(Constants.HomeCellNames.status                                 ,this.status                    );}
            if(this.street != null)      {insertHP.Parameters.AddWithValue(Constants.HomeCellNames.street                                 ,this.street                    );}
            if(this.streetNumber != null){insertHP.Parameters.AddWithValue(Constants.HomeCellNames.streetNumber                           ,this.streetNumber              );}
            if(this.type != null)        {insertHP.Parameters.AddWithValue(Constants.HomeCellNames.type                                   ,this.type                      );}
            if(this.typeDisplay != null) {insertHP.Parameters.AddWithValue(Constants.HomeCellNames.typeDisplay                            ,this.typeDisplay               );}
            if(this.yearBuilt != 0)      {insertHP.Parameters.AddWithValue(Constants.HomeCellNames.yearBuilt                              ,this.yearBuilt                 );}
            if (this.zipCode != null)    {insertHP.Parameters.AddWithValue(Constants.HomeCellNames.zipCode                                ,this.zipCode                   );}
                long homeId = -1;
                homeId = DataProviders.DataProvider.Instance.ExecureSPWithRetVal(insertHP);
                return homeId;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ошибка при добавлении записи в Properties: {0}, {1}",ex.Message, ex.StackTrace);
                logger.Error("Ошибка при добавлении записи в Properties: {0}, {1}", ex.Message, ex.StackTrace);
                return -1;
            }
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

              


    }

   

}

