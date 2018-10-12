/*
This class will be used to return Apartments data from the parser.
It will also be used to insert values into DB.
*/
namespace WebScraperModularized.data{

    public class Apartments{

        public int id {get; set;}

        public int beds {get; set;}

        public int baths {get; set;}
        
        public int minprice {get; set;}
        
        public int maxprice {get; set;}

        public int area {get; set;}

        public int property {get; set;}

        public string availability {get; set;}
    }
}