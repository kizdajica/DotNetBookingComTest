using BookingCom.PageObjects;
using Microsoft.VisualBasic.FileIO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BookingCom.BaseFolder
{
    /// <summary>
    /// This is the Base Test class that initializes the driver and holds common methods
    /// </summary>
    public class BaseTest : IDisposable
    {
        protected IWebDriver driver;

        // 1. Open https://www.booking.com/
        public BaseTest()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://www.booking.com/";
        }

        public static List<string[]> ParseCsv(string file, string delimiter)
        {
            var parsedData = new List<string[]>();
            string[] fields;

            using(var parser = new TextFieldParser(file))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(delimiter);
                parser.TrimWhiteSpace = true;

                while (!parser.EndOfData)
                {
                    fields = parser.ReadFields();
                    parsedData.Add(fields);
                }
            }

            return parsedData;
        }

        // 2. Choose:
        //  Currency: ‘Euro’
        //  Language: ‘English(US)’
        public void SetCurrencyAndLanguage(PageHeader pageHeader)
        {
            pageHeader.Currency.Click();
            Thread.Sleep(500);
            pageHeader.CurrencyType.Click();
            pageHeader.Language.Click();
            pageHeader.LanguageType.Click();
        }

        // Return the last day of the current month in needed form
        public string GetLastDayOfCurrentMonth()
        {
            var today = DateTime.Now;
            var firstOfNextMonth = new DateTime(today.Year, today.Month, 1).AddMonths(1);
            var lastDay = firstOfNextMonth.AddDays(-1);
            return lastDay.ToString("MMddyyyy");
        }

        // Return the first day of the next month in needed form
        public string GetFirstDayOfNextMonth()
        {
            var today = DateTime.Now;
            var firstOfNextMonth = new DateTime(today.Year, today.Month, 1).AddMonths(1);
            return firstOfNextMonth.ToString("MMddyyyy");
        }

        public void Dispose()
        {
            driver.Dispose();
        }
    }
}
