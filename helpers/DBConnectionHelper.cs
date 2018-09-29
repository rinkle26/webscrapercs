/*
This class will be used as a helper for DB connections.
*/

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Npgsql;

namespace WebScraperModularized.helpers{

    public class DBConnectionHelper{

        public static IDbConnection getConnection(){
            MyConfigurationHelper myConfigurationManager = new MyConfigurationHelper();
            return new NpgsqlConnection(myConfigurationManager.getDBConnectionString());
        }
    }
}