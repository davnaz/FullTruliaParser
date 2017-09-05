using FTParser.DataProviders;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruliaParser;

namespace FT.Components
{
    class PropertySchool
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();
        public long homeID { get; set; }
        public string Type { get; set; }
        public string SchoolName { get; set; }
        public string Address { get; set; }
        public string Grades { get; set; }
        public string Rank { get; set; }
        public double Distance { get; set; } = -1;

        public void InsertToDb()
        {
            try
            {
                SqlCommand insertSchool = DataProvider.Instance.CreateSQLCommandForSP(Resources.SP_AddNewSchool);
                insertSchool.Parameters.AddWithValue("@Home_ID", homeID);          
                if(Type != null){insertSchool.Parameters.AddWithValue("@Type", Type);               }
                if(SchoolName != null){insertSchool.Parameters.AddWithValue("@SchoolName", SchoolName);   }
                if(Address != null){insertSchool.Parameters.AddWithValue("@Address", Address);         }
                if(Grades != null){insertSchool.Parameters.AddWithValue("@Grades", Grades);           }
                if (Distance != -1) { insertSchool.Parameters.AddWithValue("@Distance", Distance); }
                if (Rank != null) { insertSchool.Parameters.AddWithValue("@Rank", Rank); }
                DataProvider.Instance.ExecureSP(insertSchool);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при добавлении записи в Schools: {0}, {1}", ex.Message, ex.StackTrace);
                logger.Error("Ошибка при добавлении записи в Schools: {0}, {1}", ex.Message, ex.StackTrace);
            }
        }
    }
}
