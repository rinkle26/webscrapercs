/*
This class will be used to return data for expenses from the parser.
This will also be used to save data into DB.
*/

namespace WebScraperModularized.data{
    
    public class Expenses{

        public int id {get; set;}

        public string title {get; set;}

        public int expensetype {get; set;}

        public int cost {get; set;}

        public int property {get; set;}

    }
}