using NUnit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;



namespace TestProject1
{
    public class Tests
    {
        IWebDriver driver;
        string url = "https://admlucid.com/Home/WebElements";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void Test1()
        {
            driver.Navigate().GoToUrl(url);
            Assert.That(driver.Url, Is.EqualTo(url));
        }
        [Test]
        public void TextBox()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.Id("Text1")).Clear();
            driver.FindElement(By.Id("Text1")).SendKeys("Ramesh");

        }
        [Test]
        public void TextArea()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.Name("TextArea2")).Clear();
            driver.FindElement(By.Name("TextArea2")).SendKeys("TextArea Test");
            Console.WriteLine("Test Successfully Executed");
        }

        [Test]
        public void Button()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.Id("Button1")).Click();
            Thread.Sleep(1000);
            driver.SwitchTo().Alert().Accept();
        }

        [TearDown]
        public void TearDown()
        {
           // driver.Quit();  
        }



    }
}