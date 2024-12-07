// Inside SeleniumTest.cs

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI; // Added for WebDriverWait
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace SeleniumCsharp
{
    public class TestsLogin: Initialization
    {

        [Test]
        public void VerifyLogo()
        {
            //_driver.Navigate().GoToUrl("https://www.browserstack.com/");
            //var logo = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("logo")));
            _driver.Navigate().GoToUrl("https://www.browserstack.com/");

            Assert.IsTrue(_driver.FindElement(By.ClassName("bstack-mm-logo")).Displayed);
            //Assert.IsTrue(logo.Displayed, "Logo is not displayed on the homepage.");
        }

        [Test]
        public void VerifyMenuItemCount()
        {
            //_driver.Navigate().GoToUrl("https://www.browserstack.com/");
            //ReadOnlyCollection<IWebElement> menuItem = _wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath("//ul[contains(@class,'horizontal-list product-menu')]/li")));
            //Assert.AreEqual(4, menuItem.Count, "The number of menu items is not as expected.");
        }

        [Test]
        public void VerifyPricingPage()
        {
            //_driver.Navigate().GoToUrl("https://browserstack.com/pricing");
            //IWebElement contactUsPageHeader = _wait.Until(ExpectedConditions.ElementIsVisible(By.TagName("h1")));
            //Assert.IsTrue(contactUsPageHeader.Text.Contains("Replace your device lab and VMs with any of these plans"), "The pricing page header text is incorrect.");
        }

    }
}
