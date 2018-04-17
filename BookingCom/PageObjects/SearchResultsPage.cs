using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace BookingCom.PageObjects
{
    /// <summary>
    /// This class models the Search Results Page and its objects
    /// </summary>
    public class SearchResultsPage
    {
        [FindsBy(How = How.Id, Using = "hotellist_inner")]
        public IWebElement Properties { get; set; }
    }
}