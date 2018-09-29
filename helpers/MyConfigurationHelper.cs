/*
This class will contain methods to load all the configurations.
*/
using System.Configuration;

namespace WebScraperModularized.helpers{

    public class MyConfigurationHelper{

        public string getDBConnectionString(){
            return ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        }

    }

}