using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace BookingCom.PageObjects
{
    /// <summary>
    /// This class models the Home Page and its objects
    /// </summary>
    public class HomePage
    {
        [FindsBy(How = How.Id, Using = "ss")]
        public IWebElement Destination { get; set; }

        [FindsBy(How = How.CssSelector, Using = "[name = 'checkin_month']")]
        public IWebElement CheckIn { get; set; }

        [FindsBy(How = How.CssSelector, Using = "[name = 'checkout_month']")]
        public IWebElement CheckOut { get; set; }

        [FindsBy(How = How.Id, Using = "group_adults")]
        public IWebElement Adults { get; set; }

        [FindsBy(How = How.Id, Using = "group_children")]
        public IWebElement Children { get; set; }

        [FindsBy(How = How.Id, Using = "no_rooms")]
        public IWebElement Rooms { get; set; }

        [FindsBy(How = How.Name, Using = "age")]
        public IWebElement Age { get; set; }

        [FindsBy(How = How.Name, Using = "sb_travel_purpose")]
        public IWebElement OldWork { get; set; }

        [FindsBy(How = How.ClassName, Using = "bui-checkbox__label")]
        public IWebElement NewWork { get; set; }

        [FindsBy(How = How.ClassName, Using = "sb-searchbox__button  ")]
        public IWebElement Search { get; set; }

        [FindsBy(How = How.CssSelector, Using = "[data-visible = 'accommodation']")]
        public IWebElement AdultsChildren { get; set; }
    }
}