using BookingCom.BaseFolder;
using BookingCom.PageObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System;
using System.Diagnostics;
using System.Reflection;
using Xunit;

namespace BookingCom.Tests
{
    /// <summary>
    /// This class holds tests that test http://booking.com search for properties
    /// </summary>
    public class SearchTest : BaseTest
    {
        private static Double sScore;
        private static string sPriceString;
        private static int sPrice;
        private static readonly By sTotalPrice = By.ClassName("totalPrice");
        private static readonly string sDataScore = "data-score";
        private static readonly By sHotelName = By.ClassName("sr-hotel__name");

        // Path to csv file with search data and delimiter used
        private static readonly string sCsvPath = "../../Data/SearchTestData.csv";
        private static readonly string sDelimiter = ",";

        [Fact]
        public void SearchforProperty()
        {
            // Initialize PageHeader page and its objects
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

            // Loop through the list of properties and find the first property that matches the desired conditions
            foreach(IWebElement property in listOfProperties)
            {
                // Find the first hotel with a review mark of higher than ‘8.0’ and	a price under ‘200’ EUR.
                // If any hotel is sold out catch the NoSuchElementException and log the hotel's name and the message that it is sold out.
                try
                {
                    // Find the price and the score of the hotel
                    sPriceString = property.FindElement(sTotalPrice).Text;
                    sPrice = int.Parse(sPriceString.Substring(sPriceString.Length - 3));
                    sScore = Double.Parse(property.GetAttribute(sDataScore));
                }
                catch (NoSuchElementException)
                {
                    // If the hotel is sold out, log its name and message
                    Debug.WriteLine(property.FindElement(sHotelName).Text + "'s last room sold out a few days ago.");
                }

                // 5. Assert that there is a property with both
                //  review mark of higher than ‘8.0’ and
                //  price under ‘200’ EUR
                if ((sScore > 8.0) && (sPrice < 200))
                {
                    // 6.Use Console.log to report the name of the first property found
                    Debug.WriteLine("Matched hotel's name is: " + property.FindElement(sHotelName).Text);
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

            // Since "Number of rooms" element is missing on the new booking.com home page layout,
            // possible exception is caught and message logged
            try
            {
                var numberOfRooms = new SelectElement(homePage.Rooms);
            numberOfRooms.SelectByValue(row[1]);
            }
            catch (NoSuchElementException)
            {
                // If the booking.com home page is displayed using the new layout, log the message
                Debug.WriteLine("New Home Page layout displayed without the rooms element.");
            }

            // If the booking.com home page is displayed using the new layout,
            // first click on the adults-child element to open dropdown
            try
            {
                homePage.AdultsChildren.Click();
            }
            catch (NoSuchElementException)
            {
                // If the booking.com home page is displayed using the old layout, log the message
                Debug.WriteLine("Old Home Page layout displayed.");
            }

            var numberOfAdults = new SelectElement(homePage.Adults);
            numberOfAdults.SelectByValue(row[2]);
            var numberOfChildren = new SelectElement(homePage.Children);
            numberOfChildren.SelectByValue(row[2]);
            var ageNumber = new SelectElement(homePage.Age);
            ageNumber.SelectByValue(row[3]);

            // Since it is currently not possible to find work element by the same locator on different booking.com home page layouts,
            // first try the old home page locator and then the new home page locator
            try
            {
                homePage.Work.Click();
            }
            catch (TargetInvocationException)
            {
                driver.FindElement(By.ClassName("bui-checkbox__label")).Click();
            }

            // 4.Click on ‘Search’ button
            homePage.Search.Click();
        }
    }
}