/*
This class will help us in reading/writing data from/to the Database.
*/

using System.Collections.Generic;
using WebScraperModularized.data;
using Dapper;
using Z.Dapper.Plus;
using System.Data;

namespace WebScraperModularized.helpers{
    public class DBHelper{
        
        /*
        Method to return n URLs from DB.
        */
        public static IEnumerable<URL> getURLSFromDB(int n, bool initialLoad){
            IEnumerable<URL> myUrlEnumerable = null;

            using(IDbConnection db = DBConnectionHelper.getConnection()){
                if(db!=null){
                    if(!initialLoad){
                        myUrlEnumerable = 
                                    db.Query<URL>("Select Id, Url, Urltype, Property from URL where status = @status limit @k",
                                    new {status = URL.URLStatus.INITIAL, k = n});
                    }
                    else {
                        myUrlEnumerable = 
                                db.Query<URL>("Select Id, Url, Urltype, Property from URL where status = ANY(@status) limit @k",
                                new {status = new []{(int)URL.URLStatus.INITIAL, (int)URL.URLStatus.RUNNING}, k = n});
                    }
                }
            }
            return myUrlEnumerable;
        }

        /*
        Method to insert parsed properties into DB
        */
        public void insertParsedProperties(List<URL> propertyList){
            DapperPlusManager.Entity<URL>().Table("url").Identity(x => x.id);
            DapperPlusManager.Entity<Property>().Table("property").Identity(x => x.id);
            DapperPlusManager.Entity<PropertyType>().Table("propertytype").Identity(x => x.id);

            if(propertyList!=null && propertyList.Count>0){
                using(IDbConnection db = DBConnectionHelper.getConnection()){
                    db.BulkMerge(propertyList);
                }
            }
        }

        private void insertProperty(Property property){

        }

        private void insertURL(URL url){

        }



    }
}