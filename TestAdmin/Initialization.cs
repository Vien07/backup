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
    public class Initialization
    {
        public IWebDriver _driver;
        WebDriverWait _wait; // Added WebDriverWait

        [OneTimeSetUp]
        public void Setup()
        {
            // Get the drivers folder path dynamically
            //string path = AppDomain.CurrentDomain.BaseDirectory;
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            ////Creates the ChomeDriver object, Executes tests on Google Chrome

            _driver = new ChromeDriver(path + @"\drivers\");

            // Initialize WebDriverWait with a timeout of 10 seconds
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // If you want to execute tests on Firefox, uncomment the below code and specify the correct location of geckodriver.exe
            // _driver = new FirefoxDriver(driversPath);
        }



        [OneTimeTearDown]
        public void TearDown()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose(); // Ensure resources are released
            }
        }
    }
}
