/*
This class will act as the URL manager.
It will get k urls from database and keep them in memory.

We need to make sure that the getNextURL code is thread safe.
If two threads call the getNextURL method at the same time,
it can lead to unexpected results!
*/
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using WebScraperModularized.data;
using System.Linq;

namespace WebScraperModularized.helpers{

    public static class URLHelper {

        private static readonly object nextURLLock = new object();//simple lock object
        private static int k = 100;
        private static Queue<URL> myURLQueue = new Queue<URL>();

        private static bool INITIALIZED = false;

        /*
        This method returns the next url in the queue to be parsed.
        Returns null if not URLs are left to be parsed.
        */
        public static URL getNextURL(){
            lock(nextURLLock){//make sure that this part of the code is thread safe.
                if(myURLQueue.Count==0){
                    int loadedURLCount = loadNextURLS(!INITIALIZED);
                    INITIALIZED = true;
                    if(loadedURLCount==0) return null;
                }
                
                return myURLQueue.Dequeue();
            }
        }

        /*
        This method gets the next k URLs from DB and returns the count of URLs loaded.
        */
        private static int loadNextURLS(bool initialLoad){
            IEnumerable<URL> myUrlEnumerable = DBHelper.getURLSFromDB(k, initialLoad);

            if(myUrlEnumerable!=null && myUrlEnumerable.Count()>0){
                foreach(URL url in myUrlEnumerable){
                    myURLQueue.Enqueue(url);
                }

                return myUrlEnumerable.Count();
            }

            return 0;
        }

    }
}