/*
This class will be used to parse apartment details.
Multiple threads of this parser will be created by the main method.
This class will run once for each zipcode.
*/
using WebScraperModularized.data;
using System.Collections.Generic;
using HtmlAgilityPack;
using System;
using WebScraperModularized.helpers;
using WebScraperModularized.wrappers;
using System.Text.RegularExpressions;

namespace WebScraperModularized.parsers{
    public class ApartmentParser{
        
        private string html;//html parsed from the URL.

        private URL myUrl;

        HtmlDocument htmlDoc;

        public ApartmentParser(string html, URL myUrl){//constructor
            this.html = html;
            this.myUrl = myUrl;
            htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
        }

        public ApartmentData parse(){
            ApartmentData apartmentData = new ApartmentData();
            apartmentData.apartmentsList = getApartments(getHtmlNodeApartments());
            apartmentData.expensesTypeList = getExpenses(getHtmlNodeExpenses());
        
            apartmentData.description = getDescription(getHtmlNodeDescription());
            apartmentData.soundScore = getSoundScore(getHtmlNodeSoundScore());
            
            apartmentData.reviewsList = getReviews(getHtmlNodeReviews());
            apartmentData.schoolsList = getSchools(getHtmlNodeSchools());
            apartmentData.NTPIList = getNTPI(getHtmlNodeNTPI());
            return apartmentData;
        }


        /*
        Helper methods for parsing the required data start
        */
        private List<Apartments> getApartments(HtmlNode row){
            List<Apartments> apartments = new List<Apartments>();
            try{
                HtmlNodeCollection trCollection = row.SelectNodes(".//tr[contains(@class, \"rentalGridRow\")]");
                if(trCollection!=null){
                    //loop across all trs
                    foreach(HtmlNode tr in trCollection){
                        if(tr.Name.Equals("tr")){
                            Apartments apartment = new Apartments();

                            //get num beds
                            apartment.beds = Util.parseDouble(tr.GetAttributeValue("data-beds","0"), 0);
                            
                            //get num baths
                            apartment.baths = Util.parseDouble(tr.GetAttributeValue("data-baths","0"), 0);
                            
                            //get min and max price
                            HtmlNode rentTd = tr.SelectSingleNode(".//td[contains(@class, \"rent\")]");
                            if(rentTd!=null){
                                string rentString = rentTd.InnerHtml;
                                double[] rents = Util.splitRentString(rentString);
                                apartment.minprice = rents[0];
                                apartment.maxprice = rents[1];
                            }
                            else{
                                apartment.minprice = 0;
                                apartment.maxprice = 0;
                            }

                            //get area
                            HtmlNode areaTd = tr.SelectSingleNode(".//td[contains(@class, \"sqft\")]");
                            if(areaTd!=null){
                                string areaString = areaTd.InnerHtml;
                                areaString = areaString.Trim();
                                areaString = Regex.Replace(areaString, "[^0-9.]", "");
                                apartment.area = Util.parseDouble(areaString, 0);
                            }
                            else{
                                apartment.area = 0;
                            }

                            //set property id
                            apartment.property = myUrl.property;

                            //get Availability
                            HtmlNode availabilityTd = tr.SelectSingleNode(".//td[contains(@class, \"available\")]");
                            if(availabilityTd!=null){
                                apartment.availability = availabilityTd.InnerHtml.Trim();
                            }

                            apartments.Add(apartment);
                        }
                    }
                }
            }
            catch(Exception e){
                ExceptionHelper.printException(e);
            }
            return apartments;
        }

        private List<Expensetype> getExpenses(HtmlNode row){
            List<Expensetype> expensetypes = new List<Expensetype>();
           try{
               HtmlNodeCollection fees = row.ChildNodes;
               if(fees != null)
               {
                   foreach(HtmlNode typefees in fees)
                   {
                       Expensetype newExpenseType = new Expensetype();
                        if(typefees= fees.SelectSingleNode(".//div[contains(@class, \"monthlyFees\")]"))
                        {
                            newExpenseType.id = 0;
                            newExpenseType.title = "Recurring Fees";
                        }
                        else if(typefees= fees.SelectSingleNode(".//div[contains(@class, \"oneTimeFees\")]"))
                        {
                            newExpenseType.id = 1;
                            newExpenseType.title = "One-Time Fees";
                        }                      
                        else
                        {
                            newExpenseType.id = -1;
                            newExpenseType.title = "Unknown";
                        }
                       foreach(HtmlNode descrip in typefees)
                       {
                           Expenses expenses = new Expenses();

                            if(descrip != null)
                            {
                                //List<HtmlNode> spans = descrip.childNodes;
                                var spans = descrip.GetElementsOfType("span");

                                string title1 = spans[0].InnerHtml;
                                int cost1 = Convert.ToInt32(spans[1].InnerHtml);

                                expenses.title = title1;
                                expenses.cost = cost1;

                                if(title1 == "Assigned Cover Parking")
                                {
                                    expenses.id = 0;
                                }
                                else if (title1 == "Storage Fee")
                                {
                                    expenses.id = 1;
                                }
                                else if (title1 == "Cat Fee")
                                {
                                    expenses.id = 2;
                                }
                                else if (title1 == "Dog Fee")
                                {
                                    expenses.id = 3;
                                }
                                else
                                {
                                    expenses.id = -1;
                                    expenses.title = "Unknown";
                                }
                            }
                            //newExpenseType.expenses.add(expenses);
                            newExpenseType.expensesList.add(expenses);
                       }
                       foreach(HtmlNode descripio in typefees)
                       {
                           Expenses expensesad = new Expenses();

                            if(descripio != null)
                            {
                                List<HtmlNode> spans = descripio.childNodes;

                                /*if(descripio.SelectSingleNode(".//span[contains(text(),'Admin Fee']")!= null)
                                {
                                    expensesadd.id = 0;
                                    expensesadd.title = newExpense.SelectSingleNode(".//span[contains(text(),'Admin Fee']");
                                    expensesadd.cost = newExpense.SelectSingleNode(".//span[contains(text(), ");
                                    // Cost could be Expense.cost = Util.Double... (Check Apartments code above)

                                }
                                else if(descripio.SelectSingleNode(".//span[contains(text(),'Application Fee']")!= null)
                                {
                                    expensesadd.id = 1;
                                    expensesadd.title = newExpense.SelectSingleNode(".//span[contains(text(),'Application Fee']");
                                }
                                else if(descripio.SelectSingleNode(".//span[contains(text(),'Cat Deposit']")!= null)
                                {
                                    expensesadd.id = 2;
                                    expenses.title = newExpense.SelectSingleNode(".//span[contains(text(),'Cat Deposit']");
                                }
                                else if(descripio.SelectSingleNode(".//span[contains(text(),'Dog Deposit']")!= null)
                                {
                                    expensesadd.id = 3;
                                    expensesadd.title = newExpense.SelectSingleNode(".//span[contains(text(),'Dog Deposit']");
                                }*/

                            }
                       }
                       //expensetypes.add(newExpsense);
                        newExpenseType.expensesList.add(expensesad);
                   }
               }

           }
           catch(Exception e)
           {
               ExceptionHelper.printException(e);
           }
            return expensetypes;
        }

        private string getDescription(HtmlNode row){
            string description = "";
            return description;
        }

        private SoundScore getSoundScore(HtmlNode row){
            SoundScore soundScore = new SoundScore();

            return soundScore;
        }

        private List<Review> getReviews(HtmlNode row){
            List<Review> reviews = new List<Review>();
            return reviews;
        }

        private List<School> getSchools(HtmlNode row){
            List<School> schools = new List<School>();
            return schools;
        }

        private List<NTPI> getNTPI(HtmlNode row){
            List<NTPI> nTPIs = new List<NTPI>();
            return nTPIs;
        }
        /*
        Helper methods for parsing the required data end
        */

        /*
        Helper methods for getting HtmlNodes from htmlDoc start
        */
        private HtmlNode getHtmlNodeApartments(){
            HtmlNode htmlNode = null;
            if(htmlDoc!=null && htmlDoc.DocumentNode!=null){
                htmlNode = htmlDoc.DocumentNode
                            .SelectSingleNode(".//table[contains(@class, \"availabilityTable\")]/tbody");
            }
            return htmlNode;
        }

        private HtmlNode getHtmlNodeExpenses(){
            HtmlNode htmlNode = null;
            if(htmlDoc!=null && htmlDoc.DocumentNode!=null){
                htmlNode = htmlDoc.GetElementbyId("feesWrapper");
            }
            return htmlNode;
        }

        private HtmlNode getHtmlNodeDescription(){
            HtmlNode htmlNode = null;
            if(htmlDoc!=null && htmlDoc.DocumentNode!=null){
                htmlNode = htmlDoc.GetElementbyId("descriptionSection");
            }
            return htmlNode;
        }

        private HtmlNode getHtmlNodeReviews(){
            HtmlNode htmlNode = null;
            if(htmlDoc!=null && htmlDoc.DocumentNode!=null){
                htmlNode = htmlDoc.DocumentNode
                            .SelectSingleNode(".//div[contains(@class, \"reviewsContainer\")]");
            }
            return htmlNode;
        }

        private HtmlNode getHtmlNodeSchools(){
            HtmlNode htmlNode = null;
            if(htmlDoc!=null && htmlDoc.DocumentNode!=null){
                htmlNode = htmlDoc.DocumentNode
                            .SelectSingleNode(".//div[contains(@class, \"schoolsModule\")]");
            }
            return htmlNode;
        }

        private HtmlNode getHtmlNodeNTPI(){
            HtmlNode htmlNode = null;
            if(htmlDoc!=null && htmlDoc.DocumentNode!=null){
                htmlNode = htmlDoc.DocumentNode
                            .SelectSingleNode(".//section[contains(@class, \"nearbyAmenitiesSection\")]");
            }
            return htmlNode;
        }

        private HtmlNode getHtmlNodeSoundScore(){
            HtmlNode htmlNode = null;
            if(htmlDoc!=null && htmlDoc.DocumentNode!=null){
                htmlNode = htmlDoc.DocumentNode
                            .SelectSingleNode(".//div[contains(@class, \"soundScoreWrapper\")]");
            }
            return htmlNode;
        }

        /*
        Helper methods for getting HtmlNodes from htmlDoc end
        */

    }
}
