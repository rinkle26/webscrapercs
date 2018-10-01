/*
This class will be used to parse property listings.
Multiple threads of this parser will be created by the main method.
This will populate the data in Property table and url table.
This class will run once for each zipcode.
*/
using WebScraperModularized.data;
using System.Collections.Generic;
using HtmlAgilityPack;
using System;
using WebScraperModularized.helpers;

namespace WebScraperModularized.parsers{
    public class PropertyParser{

        private string html;//url this parser will parse in this instance
        public PropertyParser(string html){//constructor
            this.html = html;
        }

        public List<URL> parse(){
            List<URL> myList = new List<URL>();
            try{
                if(html!=null && html.Length!=0){
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);

                    HtmlNode apartmentsContainer = htmlDoc.GetElementbyId("placardContainer");
                    if(apartmentsContainer!=null){
                        HtmlNode listOfApartments = apartmentsContainer.SelectSingleNode(".//ul");
                        if(listOfApartments!=null){
                            foreach(HtmlNode row in listOfApartments.ChildNodes){
                                if(row!=null && row.Name == "li"){
                                    URL url = new URL();
                                    HtmlNode paginationDiv = row.SelectSingleNode(".//div[@id=\"paging\"]");
                                    if(paginationDiv!=null){
                                        url.url = getNextUrl(row);
                                        url.urltype = (int)URL.URLType.PROPERTY_URL;
                                        url.status = (int)URL.URLStatus.INITIAL;
                                    }
                                    else{
                                        url.property = getProperty(row);
                                        url.url = getUrl(row);
                                        url.urltype = (int)URL.URLType.APARTMENT_URL;
                                        url.status = (int)URL.URLStatus.INITIAL;
                                    }
                                    if(url.url!=null && url.url.Length!=0){
                                        myList.Add(url);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception e){
                ExceptionHelper.printException(e);
            }
            return myList;
        }
		
		private string getTitle(HtmlNode row){
			var isAttr = false;
            string title = "";
            try{
                if(row!=null){
                    HtmlNode titleNode = row.SelectSingleNode(".//a[contains(@class, \"placardTitle\")]");//try selecting using placardTitle node for basic and gold placards
                    if(titleNode == null) {//if no element found, means we have placard of reinforcement type
                        isAttr = true;
                        titleNode = row.SelectSingleNode(".//div[@class=\"item\"]");
                    }
                    if(titleNode!=null) {
                        if(!isAttr){
                            if(titleNode.InnerHtml!=null && titleNode.InnerHtml.Length!=0){
                                title = titleNode.InnerHtml.Trim();
                            }
                        }
                        else{
                            title = titleNode.GetAttributeValue("title","");
                        }
                    }
                }
            }
            catch(Exception e){
                ExceptionHelper.printException(e);
            }
            return title;
		}

        private string getAddress(HtmlNode row){
            string address = "";
            try{
                if(row!=null){
                    HtmlNode addressNode = row.SelectSingleNode(".//div[@class=\"location\"]");
                    if(addressNode!=null) address = addressNode.InnerHtml;
                }
            }
            catch(Exception e){
                ExceptionHelper.printException(e);
            }
            return address;
        }

        private string getUrl(HtmlNode row){
            string url = "";
            try{
                if(row!=null){
                    HtmlNode linkNode = row.SelectSingleNode(".//a[contains(@class, \"placardTitle\")]");
                    if(linkNode==null){//means we have a placard of reinforcement type
                        linkNode = row.SelectSingleNode(".//a");
                    }
                    if(linkNode!=null){
                        url = linkNode.GetAttributeValue("href","");
                    }
                }
            }
            catch(Exception e){
                ExceptionHelper.printException(e);
            }
            return url;
        }

        private string getContactemail(HtmlNode row){
            string email = "";//we don\"t have email information on this page. To be implemented in the future
            return email;
        }

        private string getContactno(HtmlNode row){
            string contactno = "";
            try{
                if(row!=null){
                    HtmlNode phoneNode = row.SelectSingleNode(".//div[@class=\"phone\"]");
                    if(phoneNode!=null){
                        HtmlNode phoneSpan = phoneNode.SelectSingleNode(".//span");
                        if(phoneSpan!=null){
                            contactno = phoneSpan.InnerHtml;
                        }
                    }
                }
            }
            catch(Exception e){
                ExceptionHelper.printException(e);
            }
            return contactno;
        }

        private int getMaxPrice(HtmlNode row){
            int maxPrice = 0;
            try{
                if(row!=null){
                    HtmlNode rentSpan = row.SelectSingleNode(".//span[@class=\"altRentDisplay\"]");
                    if(rentSpan!=null){
                        string rentString = rentSpan.InnerHtml;
                        if(rentString!=null && rentString.Length!=0){
                            rentString = rentString.Trim();
                            rentString = rentString.Replace("$", "");
                            rentString = rentString.Replace(",", "");

                            if(rentString.Contains(" - ")){
                                maxPrice = Int32.Parse(rentString.Split(" - ")[1]);
                            }
                            else maxPrice = Int32.Parse(rentString.Trim());
                        }
                    }
                }
            }
            catch(Exception e){
                ExceptionHelper.printException(e);
            }
            return maxPrice;
        }

        private int getMinPrice(HtmlNode row){
            int minPrice = 0;
            try{
                if(row!=null){
                    HtmlNode rentSpan = row.SelectSingleNode(".//span[@class=\"altRentDisplay\"]");
                    if(rentSpan!=null){
                        string rentString = rentSpan.InnerHtml;
                        if(rentString!=null && rentString.Length!=0){
                            rentString = rentString.Trim();
                            rentString = rentString.Replace("$", "");
                            rentString = rentString.Replace(",", "");
                            
                            if(rentString.Contains(" - ")){
                                minPrice = Int32.Parse(rentString.Split(" - ")[0]);
                            }
                            else minPrice = Int32.Parse(rentString.Trim());
                        }
                    }
                }
            }
            catch(Exception e){
                ExceptionHelper.printException(e);
            }
            return minPrice;
        }

        private PropertyType getPropertyType(HtmlNode row){
            PropertyType propertyType = new PropertyType();
            try{
                if(row!=null){
                    HtmlNode proptypeSpan = row.SelectSingleNode(".//span[contains(@class, \"unitLabel\")]");
                    if(proptypeSpan!=null){
                        propertyType.propertytype = proptypeSpan.InnerHtml;
                    }
                }
            }
            catch(Exception e){
                ExceptionHelper.printException(e);
            }
            return propertyType;
        }

        private bool getReinforcement(HtmlNode row){
            bool reinforcement = false;
            try{
                if(row!=null){
                    HtmlNode articleNode = row.SelectSingleNode(".//article[contains(@class, \"reinforcement\")]");
                    if(articleNode!=null) reinforcement = true;
                }
            }
            catch(Exception e){
                ExceptionHelper.printException(e);
            }
            return reinforcement;
        }

        private string getNextUrl(HtmlNode row){
            string url = "";
            try{
                HtmlNode nextNode = row.SelectSingleNode(".//a[contains(@class, \"next\")]");
                if(nextNode!=null){
                    url = nextNode.GetAttributeValue("href","");
                }
            }
            catch(Exception e){
                ExceptionHelper.printException(e);
            }
            return url;
        }

        private Property getProperty(HtmlNode row){
            Property Property = new Property();

            Property.name = getTitle(row);
            Property.address = getAddress(row);
            Property.contactemail = getContactemail(row);
            Property.contactno = getContactno(row);
            Property.maxprice = getMaxPrice(row);
            Property.minprice = getMinPrice(row);
            Property.propertytype = getPropertyType(row);
            Property.reinforcement = getReinforcement(row);
            
            return Property;
        }
    }
}