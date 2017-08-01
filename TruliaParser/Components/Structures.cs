using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT.Components
{
    class State
    {
        public long ID { get; set; }
        public string StateName { get; set; }
        public string Link { get; set; }
        public bool Parsed { get; set; }

        public State(DataRow row)
        {
            ID = Convert.ToInt32(row[0]);
            StateName = row[1].ToString();
            Link = row[2].ToString();
            Parsed = Boolean.Parse(row[3].ToString());
        }

    }

    class City
    {
        public long ID { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string Link { get; set; }
        public int Count { get; set; }
        public bool Parsed { get; set; }

        public City(DataRow row)
        {
            ID = Convert.ToInt32(row[0]);
            CityName = row[1].ToString();
            StateName = row[2].ToString();
            Link = row[3].ToString();
            Count = Convert.ToInt32(row[4]);
            Parsed = Boolean.Parse(row[5].ToString());
        }
    }
    
    class Street
    {
        public long ID { get; set; }
        public string CityName { get; set; }
        public string StreetName { get; set; }
        public string StateName { get; set; }
        public string Link { get; set; }
        public string ZIP { get; set; }
        public bool Parsed { get; set; }

        public Street(DataRow row)
        {
            ID = Convert.ToInt32(row[0]);
            StreetName = row[1].ToString();
            CityName =   row[2].ToString();
            StateName =  row[3].ToString();
            Link = row[4].ToString();
            ZIP = row[5].ToString();
            Parsed =    Boolean.Parse(row[6].ToString());
        }
    }
}
