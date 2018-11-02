/*
This class will contain methods to load all the configurations.
*/

using System;
using System.Configuration;

namespace WebScraperModularized.helpers{

    public class MyConfigurationHelper
    {

        private static String DATABASE_CONNECTION_CONFIG = "DatabaseConnection";
        
        public String getDBConnectionConfig()
        {
            return ConfigurationManager.AppSettings.GetValues(DATABASE_CONNECTION_CONFIG)[0];
        }

        public string getDBConnectionString(){
            return ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        }

    }

}