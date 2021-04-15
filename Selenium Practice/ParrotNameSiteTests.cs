using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace SeleniumPractice
{
    public class ParrotNameSiteTests
    {
        ChromeDriver driver;
        [SetUp]
        public void SetUp()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            driver = new ChromeDriver(options);
        }

        private By boyRadioButtonLocator = By.Id("boy");
        private By girlRadioButtonLocator = By.Id("girl");
        private By emailInputLocator = By.Name("email");
        private By submitButtonLocator = By.Id("sendMe");
        private By emailResultLocator = By.ClassName("your-email");
        private By genderResultLocator = By.ClassName("result-text");
        private By anotherEmailLocator = By.Id("anotherEmail");
        private By incorrectEmailTextLocator = By.ClassName("form-error");
        private By boyLabelLocator = By.XPath("//*[@id=\"form\"]/div[3]/label[1]");
        private By girlLabelLocator = By.XPath("//*[@id=\"form\"]/div[3]/label[2]");
        private By resultTextBlockLocator = By.Id("resultTextBlock");
        private By screenLocator = By.TagName("body");

        private string expectedEmail = "test@mail.ru";

        [Test]
        public void ParrotNameSite_FillFormWithGenderBoyAndEmail_Success()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(boyRadioButtonLocator).Click();
            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(submitButtonLocator).Click();

            var emailText = driver.FindElement(emailResultLocator).Text;
            var genderText = driver.FindElement(genderResultLocator).Text.Split(' ')[6];

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedEmail, emailText, "Отображаемый email не совпадает с указанным в заявке");
                Assert.AreEqual("мальчика", genderText, "Отображаемый пол не совпадает с указанным в заявке");
            });
        }

        [Test]
        public void ParrotNameSite_FillFormWithGenderGirlAndEmail_Success()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(girlRadioButtonLocator).Click();
            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(submitButtonLocator).Click();

            var emailText = driver.FindElement(emailResultLocator).Text;
            var genderText = driver.FindElement(genderResultLocator).Text.Split(' ')[6];

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedEmail, emailText, "Отображаемый email не совпадает с указанным в заявке");
                Assert.AreEqual("девочки", genderText, "Отображаемый пол не совпадает с указанным в заявке");
            });
        }

        [Test]
        public void ParrotNameSite_ClickAnotherEmail_EmailInputIsEmpty()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(submitButtonLocator).Click();
            driver.FindElement(anotherEmailLocator).Click();

            //проверка email
            Assert.Multiple(() =>
            {
                Assert.AreEqual(string.Empty, driver.FindElement(emailInputLocator).Text, "Поле email не очистилось");
                Assert.IsFalse(driver.FindElement(anotherEmailLocator).Displayed, "Ссылка \"указать другой e-mail\" не исчезла");
            });
        }

        [Test]
        public void ParrotNameSite_EnterValidEmails_RequestSent()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");
            string[] emails = new string[]
            {
                "CAPS@MAIL.RU",
                "123456789@mail.ru",
                "test@m12345.ru",
                "test-defis@mail.ru",
                "test@test-defis.ru",
                "test__t@mail.ru",
                "test@ma_il.ru",
                "test.dot@mail.ru",
                "test@ma.il.ru",
                "теСТ@мэил.рф",
                "a@b.c",
                "spec.!#$%&'*+-/=?^_`{|}~chars@mail.ru",
                "\"double..dot..inquotes\"@mail.ru",
                "\" \"@mail.ru",
                "spec\"(),:;<>@[\\]\"chars@mail.ru",
            };

            Assert.Multiple(() =>
            {
                for (int i = 0; i < emails.Length; i++)
                {
                    if (driver.FindElement(incorrectEmailTextLocator).Displayed)
                        driver.FindElement(emailInputLocator).Clear();
                    if (!driver.FindElement(submitButtonLocator).Displayed)
                        driver.FindElement(anotherEmailLocator).Click();
                    driver.FindElement(emailInputLocator).SendKeys(emails[i]);
                    driver.FindElement(submitButtonLocator).Click();
                    Assert.IsFalse(driver.FindElement(incorrectEmailTextLocator).Displayed, "Ошибка \"Некорректный email\" при вводе почты " + emails[i]);

                }
            });
        }

        [Test]
        public void ParrotNameSite_EnterInvalidEmails_ShowError()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");
            string[] emails = new string[]
            {
                "test.mail.ru",
                "test@example@mail.ru",
                " ",
                "spec\"(),:;<>@[\\]chars@mail.ru",
                "abc",
                "test@unde_r_line.com",
                "@.",
                "test space@mail.ru",
                "test@domain space.ru",
                "test..dot@mail.ru",
                "test@",
                "test@-defis.ru",
                "test@defis-.ru",
                "test@123456.78",
                "1234567890123456789012345678901234567890123456789012345678901234+x@example.com",
                "255charsemailaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@mail.ru",
                ".firstdottest@mail.ru",
                "lastdottest.@mail.ru",
                "test@мэил.ru",
            };

            Assert.Multiple(() =>
            {
                for (int i = 0; i < emails.Length; i++)
                {
                    if (driver.FindElement(incorrectEmailTextLocator).Displayed)
                        driver.FindElement(emailInputLocator).Clear();
                    if (!driver.FindElement(submitButtonLocator).Displayed)
                        driver.FindElement(anotherEmailLocator).Click();
                    driver.FindElement(emailInputLocator).SendKeys(emails[i]);
                    driver.FindElement(submitButtonLocator).Click();
                    Assert.IsTrue(driver.FindElement(incorrectEmailTextLocator).Displayed, "Форма отправилась с невалидным эмейлом " + emails[i]);
                }
            });
        }

        [Test]
        public void ParrotNameSite_EmptyEmailInput_ShowError()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");
            driver.FindElement(submitButtonLocator).Click();
            Assert.IsTrue(driver.FindElement(incorrectEmailTextLocator).Displayed, "Форма отправилась с пустым полем email");
        }

        [Test]
        public void ParrotNameSite_SelectGenderRadioButtonsByLabel_RadioButtonSwitched()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(girlLabelLocator).Click();
            var girlLabelSelected = driver.FindElement(girlLabelLocator).Selected;

            driver.FindElement(boyLabelLocator).Click();
            var boyLabelSelected = driver.FindElement(boyLabelLocator).Selected;

            Assert.Multiple(() =>
            {
                Assert.IsTrue(girlLabelSelected, "Радиокнопка \"девочка\" не выбралась");
                Assert.IsTrue(boyLabelSelected, "Радиокнопка \"мальчик\" не выбралась");
            });
        }

        [Test]
        public void ParrotNameSite_PressTabKey_ElementFocusSwitched()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(screenLocator).SendKeys(Keys.Tab);
            var radioButtonFocus = driver.SwitchTo().ActiveElement();

            driver.FindElement(screenLocator).SendKeys(Keys.Tab);
            var emailInputFocus = driver.SwitchTo().ActiveElement();

            driver.FindElement(screenLocator).SendKeys(Keys.Tab);
            var submitButtonFocus = driver.SwitchTo().ActiveElement();
            Assert.Multiple(() =>
            {
                Assert.AreEqual(radioButtonFocus, driver.FindElement(boyRadioButtonLocator), "Фокус не переключился на радиокнопку");
                Assert.AreEqual(emailInputFocus, driver.FindElement(emailInputLocator), "Фокус не переключился на поле email");
                Assert.AreEqual(submitButtonFocus, driver.FindElement(submitButtonLocator), "Фокус не переключился на кнопку \"Подобрать имя\"");
            });
        }

        [Test]
        public void ParrotNameSite_FillFormEmailAndPressEnter_RequestSent()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(emailInputLocator).SendKeys(Keys.Enter);

            Assert.IsTrue(driver.FindElement(resultTextBlockLocator).Displayed, "Заявка не отправилась по нажатию Enter");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
