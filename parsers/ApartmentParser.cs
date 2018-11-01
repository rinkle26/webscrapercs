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
            apartmentData.amenityTypesList = getAmenities(getHtmlNodeAmeneties());
        
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

        private List<Amenitytype> getAmenities(HtmlNode row){
            List<Amenitytype> amenitytypes = new List<Amenitytype>();
            if(row!=null){
                foreach(HtmlNode specListDiv in row.SelectNodes(".//div[contains(@class, \"specList\")]")){
                    Amenitytype amenitytype = new Amenitytype();
                    List<Amenity> amenities = new List<Amenity>();

                    //get title
                    HtmlNode titleNode = specListDiv.SelectSingleNode(".//h3");
                    if(titleNode!=null) amenitytype.title = titleNode.InnerHtml;
                    else amenitytype.title = "";

                    HtmlNodeCollection specList = specListDiv.SelectNodes(".//li");
                    if(specList!=null){
                        foreach(HtmlNode liNode in specList){
                            Amenity amenity = new Amenity();

                            //get title
                            amenity.title = liNode.InnerHtml;

                            //set property
                            amenity.property = myUrl.property;

                            amenities.Add(amenity);
                        }
                    }

                    amenitytype.amenityList = amenities;

                    amenitytypes.Add(amenitytype);
                }
            }
            return amenitytypes;

        }

        private List<Expensetype> getExpenses(HtmlNode row){
            List<Expensetype> expensetypes = new List<Expensetype>();
            foreach(HtmlNode divRow in row.ChildNodes){
                if(divRow.Name.Equals("div")){

                    //initialize new expensetype
                    Expensetype expensetype = new Expensetype();
                    List<Expenses> expenseList = new List<Expenses>();
                    //get the title
                    HtmlNode h3Node = divRow.SelectSingleNode(".//h3");
                    if(h3Node!=null){
                        expensetype.title = h3Node.InnerHtml;
                    }

                    foreach(HtmlNode descriptionWrapperNode in divRow.ChildNodes){
                        if(descriptionWrapperNode.Name.Equals("div")){
                            //initialize new expense
                            Expenses expense = new Expenses();
                            
                            //get title
                            HtmlNode titleNode = descriptionWrapperNode.SelectSingleNode(".//span[1]");
                            if(titleNode!=null) expense.title = titleNode.InnerHtml;

                            //get cost
                            HtmlNode costNode = descriptionWrapperNode.SelectSingleNode(".//span[2]");
                            if(costNode!=null) {
                                double[] costs = Util.splitRentString(costNode.InnerHtml);
                                expense.mincost = costs[0];
                                expense.maxcost = costs[1];
                            }

                            //set property id
                            expense.property = myUrl.property;

                            //add to the list
                            expenseList.Add(expense);
                        }
                    }

                    expensetype.expensesList = expenseList;

                    //add to the list
                    expensetypes.Add(expensetype);
                }
            }
            return expensetypes;
        }

        
        private string getDescription(HtmlNode row)
        {
            string description = "";
            HtmlNode pNode = row.SelectSingleNode(".//p");
            if(pNode!=null) description = pNode.InnerHtml;
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

        private HtmlNode getHtmlNodeAmeneties(){
            HtmlNode htmlNode = null;
            if(htmlDoc!=null && htmlDoc.DocumentNode!=null){
                htmlNode = htmlDoc.DocumentNode.SelectSingleNode(".//section[contains(@class, \"specGroup\")]");
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