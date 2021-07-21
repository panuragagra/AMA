using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AMAUnitTests
{
    [TestFixture]
    public class SmokeTests
    {
        public object webdriver { get; private set; }
        // Home Page
        [Test]
        public void Homepage()
        {
            IWebDriver browser = new InternetExplorerDriver();
            browser.Navigate().GoToUrl("http://localhost:49370/Home/AcceptTermsAndConditions");
            IJavaScriptExecutor jse = (IJavaScriptExecutor)browser;   
            jse.ExecuteScript("document.getElementById('ButtonIaccept').click();"); 
            WebDriverWait waitForElement = new WebDriverWait(browser, TimeSpan.FromSeconds(5));
             waitForElement.Until(ExpectedConditions.ElementIsVisible(By.Id("hplDashboard")));
        }
        // Recertify Page
        [Test]
        public void Recertify()
        {
            IWebDriver browser = new InternetExplorerDriver();
            browser.Navigate().GoToUrl("http://localhost:49370/Home/AcceptTermsAndConditions");
            IJavaScriptExecutor jse = (IJavaScriptExecutor)browser;
            jse.ExecuteScript("document.getElementById('ButtonIaccept').click();");
            WebDriverWait waitForElement = new WebDriverWait(browser, TimeSpan.FromSeconds(5));
            waitForElement.Until(ExpectedConditions.ElementIsVisible(By.Id("hplDashboard")));
            //var x = jse.ExecuteScript("document.getElementById('SelectedApplicationID').length;");
            jse.ExecuteScript("document.getElementById('SelectedApplicationID').selectedIndex = 1;");
            jse.ExecuteScript("document.getElementById('LoadDetails').click();");
            //waitForElement.Until(ExpectedConditions.ElementIsVisible(By.ClassName("mvc-grid")));
            jse.ExecuteScript("document.getElementById('SaveDetails').click();");
            browser.SwitchTo().Alert().Accept();
        }
        // Change Log Page
        [Test]
        public void Changelog()
        {
            IWebDriver browser = new InternetExplorerDriver();
            browser.Navigate().GoToUrl("http://localhost:49370/Home/AcceptTermsAndConditions");
            IJavaScriptExecutor jse = (IJavaScriptExecutor)browser;
            jse.ExecuteScript("document.getElementById('ButtonIaccept').click();");
            WebDriverWait waitForElement = new WebDriverWait(browser, TimeSpan.FromSeconds(5));
            waitForElement.Until(ExpectedConditions.ElementIsVisible(By.Id("hplDashboard")));
            jse.ExecuteScript("document.getElementById('myLink').click();");
            //jse.ExecuteScript("document.getElementById('myLink').click();");
        }
        // Signout Page
        [Test]
        public void Signout()
        {
            IWebDriver browser = new InternetExplorerDriver();
            browser.Navigate().GoToUrl("http://localhost:49370/Home/AcceptTermsAndConditions");
            IJavaScriptExecutor jse = (IJavaScriptExecutor)browser;
            jse.ExecuteScript("document.getElementById('ButtonIaccept').click();");
            WebDriverWait waitForElement = new WebDriverWait(browser, TimeSpan.FromSeconds(5));
            waitForElement.Until(ExpectedConditions.ElementIsVisible(By.Id("hplLogout")));
            jse.ExecuteScript("document.getElementById('hplLogout').click();");
            browser.SwitchTo().Alert().Accept();
        }
    }
}
