using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace BookingCom.PageObjects
{
    /// <summary>
    /// This class models the Page Header and its objects
    /// </summary>
    public class PageHeader
    {
        [FindsBy(How = How.ClassName, Using = "long_currency_text")]
        public IWebElement Currency { get; set; }

        [FindsBy(How = How.ClassName, Using = "currency_EUR")]
        public IWebElement CurrencyType { get; set; }

        [FindsBy(How = How.ClassName, Using = "uc_language")]
        public IWebElement Language { get; set; }

        [FindsBy(How = How.ClassName, Using = "lang_en-us")]
        public IWebElement LanguageType { get; set; }
    }
}