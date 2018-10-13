/*
This class will be used to return property data from the parser.
*/
namespace WebScraperModularized.data{
    
    public class Property{
        public int id{get; set;}
        
        public string name{get; set;}

        public string zipcode{get; set;}

        public double minprice{get; set;}

        public double maxprice{get; set;}

        public string description{get; set;}

        public string contactno{get; set;}

        public string contactemail{get; set;}

        public string address{get; set;}

        public int propertytype{get; set;}

        public int soundscore{get; set;}

        public URL url {get; set;}

        public string soundscoretext{get; set;}

        public bool reinforcement{get; set;}
    }
}