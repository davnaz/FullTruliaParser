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
    class PropertyCrime
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public long HomeId { get; set; }
        public void InsertToDb()
        {
            try
            {
                SqlCommand insertCrime = DataProvider.Instance.CreateSQLCommandForSP(Resources.SP_AddNewCrime);
                insertCrime.Parameters.AddWithValue("@Home_ID", HomeId);
                if (Date != null) { insertCrime.Parameters.AddWithValue("@Date", Date); }
                if (Type != null) { insertCrime.Parameters.AddWithValue("@Type", Type); }
                if (Description != null) { insertCrime.Parameters.AddWithValue("@Description", Description); }
                DataProvider.Instance.ExecureSP(insertCrime);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при добавлении записи в Crimes: {0}, {1}", ex.Message, ex.StackTrace);
                logger.Error("Ошибка при добавлении записи в Crimes: {0}, {1}", ex.Message, ex.StackTrace);
            }
        }
    }


}


 
 