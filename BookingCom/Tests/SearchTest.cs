using BookingCom.BaseFolder;
using BookingCom.PageObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System;
using System.Diagnostics;
using Xunit;

namespace BookingCom.Tests
{
    /// <summary>
    /// This class holds tests that test http://booking.com search for properties
    /// </summary>
    public class SearchTest : BaseTest
    {
        private static Double score;
        private static string priceString;
        private static int price;

        // Path to csv file with search data and delimiter used
        private static readonly string sCsvPath = "../../Data/SearchTestData.csv";
        private static readonly string sDelimiter = ",";

        [Fact]
        public void SearchforProperty()
        {
            // Initialize PageHeader page and it's objects
            var pageHeader = new PageHeader();
            PageFactory.InitElements(driver, pageHeader);

            // Set the currency to 'Euro' and the language to 'English (US)' in page header
            SetCurrencyAndLanguage(pageHeader);

            var homePage = new HomePage();
            PageFactory.InitElements(driver, homePage);

            // Parse data from CSV file located in the project
            var searchData = ParseCsv(sCsvPath, sDelimiter);

            // Loop through rows in csv file and fill the search form
            foreach(string[] row in searchData)
            {
                FindDealsForAnySeason(homePage, row);
            }

            // Assert we are on the correct search results page
            Assert.Contains("Booking.com: Hotels in Malaga. Book your hotel now!", driver.Title);

            var searchResultsPage = new SearchResultsPage();
            PageFactory.InitElements(driver, searchResultsPage);

            // Create a list of properties returned by search
            var listOfProperties = searchResultsPage.Properties.FindElements(By.ClassName("sr_property_block"));

            // 
            foreach(IWebElement property in listOfProperties)
            {
                // Find the first hotel with a review mark of higher than ‘8.0’ and	a price under ‘200’ EUR.
                // If any hotel is sold out catch the NoSuchElementException and log the hotel's name and the message that it is sold out.
                try
                {
                    // Find the price and score of the hotel
                    priceString = property.FindElement(By.ClassName("totalPrice")).Text;
                    price = int.Parse(priceString.Substring(priceString.Length - 3));
                    score = Double.Parse(property.GetAttribute("data-score"));
                }
                catch (NoSuchElementException)
                {
                    // If hotel is sold out log its name and message
                    Debug.WriteLine(property.FindElement(By.ClassName("sr-hotel__name")).Text + "'s last room sold out a few days ago.");
                }

                // 5. Assert that there is a property with both
                //  review mark of higher than ‘8.0’ and
                //  price under ‘200’ EUR
                if ((score > 9.0) && (price < 200))
                {
                    // 6.Use Console.log to report the name of the first property found
                    Debug.WriteLine(property.FindElement(By.ClassName("sr-hotel__name")).Text);
                    break;
                }
            }
        }

        internal void FindDealsForAnySeason(HomePage homePage, string[] row)
        {
            // 3. Complete property search details as follows:
            //  Destination: Málaga, Andalucía, Spain
            //  Check -in: last day of current month
            //  Check -out: first day of next month
            //  1 adult
            //  1 child 5 years old
            //  2 rooms
            //  I'm traveling for work: check

            homePage.Destination.SendKeys(row[0]);
            var lastDay = GetLastDayOfCurrentMonth();
            homePage.CheckIn.SendKeys(lastDay);
            var firstDay = GetFirstDayOfNextMonth();
            homePage.CheckOut.SendKeys(firstDay);
            var numberOfRooms = new SelectElement(homePage.Rooms);
            numberOfRooms.SelectByValue(row[1]);
            var numberOfAdults = new SelectElement(homePage.Adults);
            numberOfAdults.SelectByValue(row[2]);
            var numberOfChildren = new SelectElement(homePage.Children);
            numberOfChildren.SelectByValue(row[2]);
            var ageNumber = new SelectElement(homePage.Age);
            ageNumber.SelectByValue(row[3]);

            // "I'm traveling for work" chekcbox disabled due to the fact
            // that the alternative Booking.com page style doesn't have this checkbox
            //homePage.Work.Click();

            // 4.Click on ‘Search’ button
            homePage.Search.Click();
        }
    }
}