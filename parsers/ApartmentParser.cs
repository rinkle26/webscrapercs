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
            return apartments;
        }

        private List<Expensetype> getExpenses(HtmlNode row){
            List<Expensetype> expensetypes = new List<Expensetype>();
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