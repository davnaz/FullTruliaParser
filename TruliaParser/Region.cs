using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruliaParser
{
    class Region
    {
        public int ID { get; set; }
        public string State { get; set; }
        public string RegionName { get; set; }
        public string Link { get; set; }
        public bool Done { get; set; }
        public int  OffersCount { get; set; }

        public Region()
        {
            ID = -1;
            State = String.Empty;
            RegionName = String.Empty;
            Link = String.Empty;
            OffersCount = -1;
        }
        public Region(DataRow row)
        {
            ID = Convert.ToInt32(row[0]);
            State = row[1].ToString();
            RegionName = row[2].ToString();
            Link = row[3].ToString();
            Done = Convert.ToBoolean(row[4]);
            OffersCount = Convert.ToInt32(row[5]);
        }
        /// <summary>
        /// Возвращает экземпляр класса, созданный вручную из названий штата, региона и ссылки.
        /// </summary>
        /// <param name="id">Identificator of Region in DB</param>
        /// <param name="state">Название штата.</param>
        /// <param name="regionName">Название региона.</param>
        /// <param name="link">Ссылка.</param>
        public Region(string state, string regionName, string link, int id = -1, int offersCount = -1)
        {
            ID = id;
            State = state;
            RegionName = regionName;
            Link = link;
            Done = false;
            OffersCount = offersCount;
        }

        public override string ToString()
        {
            return String.Format("{0,-5}: {1,-10},{2,-15},{3,-40},{4,-10},{5,-13}", ID,State,RegionName,Link,Done?"Parsed":"Not parsed",OffersCount);
        }
    }
   
}
