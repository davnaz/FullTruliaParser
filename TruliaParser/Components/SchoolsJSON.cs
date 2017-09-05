using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FT.Components
{
    class SchoolsJSON
    {
        public List<Elementary> elementary { get; set; }
        public List<Middle> middle { get; set; }
        public List<High> high { get; set; }
        public bool success { get; set; }
        public List<object> errors { get; set; }
        public class Elementary
        {
            public string name { get; set; }
            public string type { get; set; }
            public string city { get; set; }
            public string state_code { get; set; }
            public string street_address { get; set; }
            public string gradespan { get; set; }
            public int greatschools_rating { get; set; }
            public string url { get; set; }
            public double distance { get; set; }
            public bool is_assigned { get; set; }
        }

        public class Middle
        {
            public string name { get; set; }
            public string type { get; set; }
            public string city { get; set; }
            public string state_code { get; set; }
            public string street_address { get; set; }
            public string gradespan { get; set; }
            public int greatschools_rating { get; set; }
            public string url { get; set; }
            public double distance { get; set; }
            public bool is_assigned { get; set; }
        }

        public class High
        {
            public string name { get; set; }
            public string type { get; set; }
            public string city { get; set; }
            public string state_code { get; set; }
            public string street_address { get; set; }
            public string gradespan { get; set; }
            public int greatschools_rating { get; set; }
            public string url { get; set; }
            public double distance { get; set; }
            public bool is_assigned { get; set; }
        }

        public static SchoolsJSON GenerateFromString(string jsonString)
        {
            return (SchoolsJSON)JsonConvert.DeserializeObject(jsonString);
        }
    }
}
