/*
This class will help us in reading/writing data from/to the Database.
*/

using System.Collections.Generic;
using WebScraperModularized.data;
using Dapper;
using Z.Dapper.Plus;
using System.Data;
using System;
using WebScraperModularized.wrappers;

namespace WebScraperModularized.helpers{
    public class DBHelper
    { 
        /*
        Method to return n URLs from DB.
        */
        public static IEnumerable<URL> getURLSFromDB(int n, bool initialLoad){
            IEnumerable<URL> myUrlEnumerable = null;

            String dbConfig = new MyConfigurationHelper().getDBConnectionConfig();

            using(IDbConnection db = DBConnectionHelper.getConnection(dbConfig)){//get connection
                if(db!=null){
                    if(!initialLoad){
                        //if not initial load, we need to get new urls in status INITIAL
                        myUrlEnumerable = 
                                    db.Query<URL>("Select Id, Url, Urltype, Property from URL where status = @status limit @k",
                                    new {status = URL.URLStatus.INITIAL, k = n});
                    }
                    else {
                        //if initial load, we need to get URLs in RUNNING status as well as they were not parseds last time
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
        public static void insertParsedProperties(PropertyData propData){
            
            if(propData==null) return;
            List<PropertyType> propertyTypeList = propData.urlList;
            if(propertyTypeList!=null && propertyTypeList.Count>0){
                String dbConfig = new MyConfigurationHelper().getDBConnectionConfig();
                using(IDbConnection db = DBConnectionHelper.getConnection(dbConfig)){//get connection
                    db.BulkMerge(propertyTypeList)//insert the list of property types
                        .ThenForEach(x => x.properties
                                            .ForEach(y => y.propertytype = x.id))//set property type id for properties
                        .ThenBulkMerge(x => x.properties)//insert properties
                        .ThenForEach(x => x.url.property = x.id)//set property id for urls
                        .ThenBulkMerge(x => x.url);//insert urls
                }
            }
        }


        /*
        This method simply merges whatever data is passed to it into DB
        */
        public static void updateURLs(Queue<URL> myUrlQueue){
            String dbConfig = new MyConfigurationHelper().getDBConnectionConfig();
            using(IDbConnection db = DBConnectionHelper.getConnection(dbConfig)){
                if(db!=null) db.BulkMerge(myUrlQueue);
            }
        }

        /*
        This method updates the status of url passed to it to DONE.
        */
        public static void markURLDone(URL url){
            String dbConfig = new MyConfigurationHelper().getDBConnectionConfig();
            using(IDbConnection db = DBConnectionHelper.getConnection(dbConfig)){
                if(db!=null) db.Execute("update url set status = @status where id=@id", new {status = (int)URL.URLStatus.DONE, id = url.id});
            }
        }

        public static void insertParsedApartment(ApartmentData apartmentData){
            //TODO
        }

    }
}