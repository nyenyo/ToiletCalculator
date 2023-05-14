using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ToiletCalculator.StepDefinitions
{
    [Binding]
    public sealed class CalculatorStepDefinitions
    {
        private IWebDriver driver;

        [Given(@"user navigates to the toilet calculator")]
        public void GivenUserNavigatesToTheToiletCalculator()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddUserProfilePreference("download.default_directory", @"C:\Users\saran1\source\repos\ToiletCalculator\Downloaded");
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
            driver = new ChromeDriver(options);
            driver.Url = "https://www.building.govt.nz/building-code-compliance/g-services-and-facilities/g1-personal-hygiene/calculator-for-toilet-pan/toilet-calculator/";
            driver.Manage().Window.Maximize();
        }

        [Given(@"user starts form with number of people unknown")]
        public void GivenUserStartsFormWithNumberOfPeopleUnknown()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            IWebElement calcframe = driver.FindElement(By.XPath("//iframe[@src='https://msg-tc-spa-as-prd.azurewebsites.net/']"));
            driver.SwitchTo().Frame(calcframe);
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("mat-radio-button#countKnownNo")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("mat-select#buildingUse")));
        }

        [Given(@"user starts form with known number of people")]
        public void GivenUserStartsFormWithKnownNumberOfPeople()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            IWebElement calcframe = driver.FindElement(By.XPath("//iframe[@src='https://msg-tc-spa-as-prd.azurewebsites.net/']"));
            driver.SwitchTo().Frame(calcframe);
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("mat-radio-button#countKnownYes")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("mat-select#buildingUse")));
        }

        [Given(@"user inputs data for building type (.*)")]
        public void GivenUserInputsDataForBuildingType(String buildingtype)
        {
            driver.FindElement(By.CssSelector("mat-select#buildingUse")).SendKeys(buildingtype);
        }

        [Given(@"user inputs data with metrics of (.*)")]
        public void GivenUserInputsDataWithMetricsNumofPeople(String numofpeople)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input#occupantCount"))).SendKeys(numofpeople);
        }

        [Given(@"user inputs data with the metrics of (.*), (.*), (.*), (.*), (.*), (.*), (.*), (.*), (.*), (.*)")]
        public void GivenUserInputsDataForForBuildingTypeHospitalWithMetricsOf(string dining_area, string interview_area, string kitchen_area, string laundry_area, string lobbies_area, string offices_area, string facilities_area, string reception_area, string subordinates_area, string number_of_beds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div[formarrayname = 'occupantDensities'] div.ng-valid:nth-child(2) input"))).SendKeys(dining_area);
            driver.FindElement(By.CssSelector("div[formarrayname = 'occupantDensities'] div.ng-valid:nth-child(4) input")).SendKeys(interview_area);
            driver.FindElement(By.CssSelector("div[formarrayname = 'occupantDensities'] div.ng-valid:nth-child(5) input")).SendKeys(kitchen_area);
            driver.FindElement(By.CssSelector("div[formarrayname = 'occupantDensities'] div.ng-valid:nth-child(6) input")).SendKeys(laundry_area);
            driver.FindElement(By.CssSelector("div[formarrayname = 'occupantDensities'] div.ng-valid:nth-child(7) input")).SendKeys(lobbies_area);
            driver.FindElement(By.CssSelector("div[formarrayname = 'occupantDensities'] div.ng-valid:nth-child(8) input")).SendKeys(offices_area);
            driver.FindElement(By.CssSelector("div[formarrayname = 'occupantDensities'] div.ng-valid:nth-child(9) input")).SendKeys(facilities_area);
            driver.FindElement(By.CssSelector("div[formarrayname = 'occupantDensities'] div.ng-valid:nth-child(10) input")).SendKeys(reception_area);
            driver.FindElement(By.CssSelector("div[formarrayname = 'occupantDensities'] div.ng-valid:nth-child(11) input")).SendKeys(subordinates_area);
            driver.FindElement(By.CssSelector("div[formarrayname = 'occupantDensities'] div.ng-valid:nth-child(12) input")).SendKeys(number_of_beds);
        }

        [When(@"user submits the form")]
        public void WhenUserSubmitsTheForm()
        {
            driver.FindElement(By.CssSelector("button#submit")).Click();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button[mattooltip='Print calculation result']")));
        }

        [When(@"user prints the results")]
        public void WhenUserPrintsTheResults()
        {
            driver.FindElement(By.CssSelector("button[mattooltip='Print calculation result']")).Click();
            Thread.Sleep(3000);
            driver.SwitchTo().Window(driver.WindowHandles.Last());
        }

        [Then(@"user is able to save the results to PDF")]
        public void ThenUserIsAbleToSaveTheResultsToPDF()
        {
            Thread.Sleep(3000);
            driver.Navigate().GoToUrl("chrome-untrusted://print/1/0/print.pdf");
            Thread.Sleep(2000);
            Assert.True(CheckFileDownloadSuccessful("print.pdf"), "File not found in Downloads folder");
            driver.Quit();
        }
        private static bool CheckFileDownloadSuccessful(string filename)
        {
            bool exist = false;
            string Path = "C:/Users/saran1/source/repos/ToiletCalculator/Downloaded";
            string[] filePath = Directory.GetFiles(Path);
            foreach (string p in filePath)
            {
                if (p.Contains(filename))
                {
                    FileInfo thisFile = new FileInfo(p);
                    //Check if a file with the filename is downloaded within the last 3 mins
                    if (thisFile.LastWriteTime.ToShortTimeString() == DateTime.Now.ToShortTimeString() ||
                    thisFile.LastWriteTime.AddMinutes(1).ToShortTimeString() == DateTime.Now.ToShortTimeString() ||
                    thisFile.LastWriteTime.AddMinutes(2).ToShortTimeString() == DateTime.Now.ToShortTimeString() ||
                    thisFile.LastWriteTime.AddMinutes(3).ToShortTimeString() == DateTime.Now.ToShortTimeString())
                        exist = true;
                    File.Delete(p);
                    break;
                }
            }
            return exist;
        }
    }
}