using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruliaParser.Components
{
    public class Constants
    {
        public class OfferPageSelectors
        {
            /// <summary>
            /// просто селектор
            /// </summary>
            public const string phone = ".property_contact_field.h7.man";
            /// <summary>
            /// document.querySelector('span.typeEmphasize.h2').innerText"; // format $0-9
            /// </summary>
            public const string idealIncome = "span.typeEmphasize.h2";//"document.querySelector('span.typeEmphasize.h2').innerText"; // format $0-9
            /// <summary>
            /// описание - document.querySelector('span[itemprop=description]').innerText";
            /// </summary>
            public const string description = "span[itemprop=description]";//"document.querySelector('span[itemprop=description]').innerText";
            /// <summary>
            /// "document.querySelectorAll('span.tag')";
            /// </summary>
            public const string metaInfo = "span.tag";//"document.querySelectorAll('span.tag')";
            /// <summary>
            /// посмотри код в коментарии этого свойства в классе
            /// </summary>
            public const string featuresSingle = ".listBulleted.mbn li";//"for (i = 0;i < document.querySelectorAll('.listBulleted.mbn li').length;i++) { console.log(document.querySelectorAll('.listBulleted.mbn li')[i].innerText); }";
            /// <summary>
            /// console.log(document.querySelectorAll('.listBulleted.mll li')[i].innerText);
            /// </summary>
            public const string featuresCommunity = ".listBulleted.mll li"; //console.log(document.querySelectorAll('.listBulleted.mll li')[i].innerText);
            /// <summary>
            /// document.querySelectorAll("#listingHomeDetailsContainer div.mtm > span.typeEmphasize")[0].parentElement.textContent
            /// </summary>
            public const string communityOtherFeatures = "#listingHomeDetailsContainer div.mtm > span.typeEmphasize"; //document.querySelectorAll("#listingHomeDetailsContainer div.mtm > span.typeEmphasize")[0].parentElement.textContent
            /// <summary>
            /// document.querySelectorAll('[data-floorplan]')
            /// </summary>
            public const string communityFloors = "[data-floorplan]"; //document.querySelectorAll('[data-floorplan]')
            /// <summary>
            /// Если первый селектор не прокатил
            /// </summary>
            public const string phoneAlt = "#contactAside > div > div > span";
        }

        public class WebAttrsNames
        {
            public const string href = "href";
            public const string NotFound = "no";
        }
        public class OfferJSObjectKeys
        {
            public const string id = "id";
            public const string agentName = "agentName";
            public const string city = "city";
            public const string county = "county";
            public const string countyFIPS = "countyFIPS";
            public const string dataPhotos = "dataPhotos";
            public const string feedId = "feedId";
            public const string formattedBedAndBath = "formattedBedAndBath";
            public const string formattedPrice = "formattedPrice";
            public const string formattedSqft = "formattedSqft";
            public const string hasPhotos = "hasPhotos";
            public const string isRentalCommunity = "isRentalCommunity";
            public const string latitude = "latitude";
            public const string longitude = "longitude";
            public const string locationId = "locationId";
            public const string listingId = "listingId";
            public const string numBathrooms = "numBathrooms";
            public const string numBedrooms = "numBedrooms";
            public const string numBeds = "numBeds";
            public const string numFullBathrooms = "numFullBathrooms";
            public const string numPartialBathrooms = "numPartialBathrooms";
            public const string pdpURL = "pdpURL";
            public const string price = "price";
            public const string truliaRank = "truliaRank";
            public const string type = "type";
            public const string zipCode = "zipCode";
            public const string streetNumber = "streetNumber";
            public const string thumbnail = "thumbnail";
            public const string sqft = "sqft";
            public const string stateCode = "stateCode";
            public const string stateName = "stateName";
            public const string street = "street";
            public const string addressForDisplay = "addressForDisplay";
        }

        public class OfferListSelectors
        {
            public const string NextPage = ".paginationContainer .mrs.bas.pvs.phm";
            public const string OfferLinks = "a.tileLink.phm";
            /// <summary>
            /// document.querySelector(".pls.typeLowlight").innerText
            /// </summary>
            public const string OffersCount = ".pls.typeLowlight"; //"document.querySelector(".pls.typeLowlight").innerText"
        }


        public class OfferCellNames
        {
            public const string postId = "@postId";
            public const string agentName = "@agentName";
            public const string addressForDisplay = "@addressForDisplay";
            public const string city = "@city";
            public const string county = "@county";
            public const string countyFIPS = "@countyFIPS";
            public const string dataPhotos = "@dataPhotos";
            public const string feedId = "@feedId";
            public const string formattedBedAndBath = "@formattedBedAndBath";
            public const string formattedPrice = "@formattedPrice";
            public const string formattedSqft = "@formattedSqft";
            public const string hasPhotos = "@hasPhotos";
            public const string isRentalCommunity = "@isRentalCommunity";
            public const string latitude = "@latitude";
            public const string longitude = "@longitude";
            public const string locationId = "@locationId";
            public const string listingId = "@listingId";
            public const string numBathrooms = "@numBathrooms";
            public const string numBedrooms = "@numBedrooms";
            public const string numBeds = "@numBeds";
            public const string numFullBathrooms = "@numFullBathrooms";
            public const string numPartialBathrooms = "@numPartialBathrooms";
            public const string price = "@price";
            public const string truliaRank = "@truliaRank";
            public const string rentalType = "@rentalType";
            public const string zipCode = "@zipCode";
            public const string streetNumber = "@streetNumber";
            public const string thumbnail = "@thumbnail";
            public const string sqft = "@sqft";
            public const string stateCode = "@stateCode";
            public const string stateName = "@stateName";
            public const string street = "@street";
            public const string phone = "@phone";
            public const string idealIncome = "@idealIncome";
            public const string metaInfo = "@metaInfo";
            public const string features = "@features";
            public const string communityOtherFeatures = "@communityOtherFeatures";
            public const string communityFloors = "@communityFloors";
            public const string directLink = "@directLink";



        }

        public class RegionLinkDbParams
        {
            public const string State = "@State";
            public const string RegionName = "@RegionName";
            public const string Link = "@Link";
            public const string OffersCount = "@OffersCount";
        }

    }
}
