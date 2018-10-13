/*
This class will be used to return Apartments data from the parser.
It will also be used to insert values into DB.
*/
namespace WebScraperModularized.data{

    public class Apartments{

        public int id {get; set;}

        public double beds {get; set;}

        public double baths {get; set;}
        
        public double minprice {get; set;}
        
        public double maxprice {get; set;}

        public double area {get; set;}

        public int property {get; set;}

        public string availability {get; set;}
    }
}