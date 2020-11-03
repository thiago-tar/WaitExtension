namespace WaitExtension
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;

    public static class DriverExtension
    {
        public static IWebElement FindElementSafe(this RemoteWebDriver driver, By by)
        {
            try
            {
                return driver.FindElement(by);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static ReadOnlyCollection<IWebElement> FindElementsSafe(this RemoteWebDriver driver, By by)
        {
            try
            {
                return driver.FindElements(by);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IWebElement WaitElementVisible(this RemoteWebDriver driver, By by, int time = 15000)
        {
            for (int i = 0; i < (time / 100); i++)
            {
                var elemento = FindElementSafe(driver, by);
                if (elemento != null)
                {
                    if (elemento.Displayed)
                    {
                        return elemento;
                    }
                }
                Thread.Sleep(100);
            }

            throw new NoSuchElementException();
        }
        public static IWebElement WaitElementVisibleSafe(this RemoteWebDriver driver, By by, int time = 15000)
        {
            IWebElement elemento = null;

            for (int i = 0; i < (time / 100); i++)
            {
                try
                {
                    elemento = FindElementSafe(driver, by);
                    if (elemento != null)
                    {
                        if (elemento.Displayed)
                        {
                            return elemento;
                        }
                    }
                    Thread.Sleep(100);
                }

                catch (Exception)
                {
                }

            }

            return elemento;
        }

        public static IWebElement WaitElementEnable(this RemoteWebDriver driver, By by, int time = 15000)
        {
            for (int i = 0; i < (time / 100); i++)
            {
                var elemento = FindElementSafe(driver, by);
                try
                {
                    if (elemento != null)
                    {
                        if (elemento.Enabled)
                        {
                            return elemento;
                        }
                    }
                    Thread.Sleep(100);
                }
                catch (StaleElementReferenceException)
                {
                    continue;
                }
            }

            throw new NoSuchElementException();
        }



        public static bool Visible(this RemoteWebDriver driver, By by)
        {
            var elemento = FindElementSafe(driver, by);
            if (elemento != null)
            {
                if (elemento.Displayed)
                    return true;
                else
                    return false;
            }
            throw new NoSuchElementException(by.ToString());
        }
        public static ReadOnlyCollection<IWebElement> WaitElements(this RemoteWebDriver driver, By by, int time = 20000)
        {
            for (int i = 0; i < (time / 100); i++)
            {
                var elemento = FindElementsSafe(driver, by);
                if (elemento != null) return elemento;
                Thread.Sleep(100);
            }

            throw new NoSuchElementException(by.ToString());
        }

        public static IWebElement WaitElement(this RemoteWebDriver driver, By by, int time = 20000)
        {
            for (int i = 0; i < (time / 100); i++)
            {
                var elemento = FindElementSafe(driver, by);
                if (elemento != null) return elemento;
                Thread.Sleep(100);
            }

            throw new NoSuchElementException(by.ToString());
        }

        public static ReadOnlyCollection<IWebElement> WaitElementsCount(this RemoteWebDriver driver, By by, int count, int time)
        {
            for (int i = 0; i < (time / 100); i++)
            {
                var elementos = driver.FindElements(by);

                if (elementos.Count() == count)
                    return elementos;

                Thread.Sleep(100);
            }

            throw new NoSuchElementException(by.ToString());
        }
        public static bool WaitText(this RemoteWebDriver driver, By by, string[] textos, int time)
        {
            for (int i = 0; i < (time / 100); i++)
            {
                try
                {
                    var elemento = driver.FindElementSafe(by);

                    if (elemento != null)
                    {
                        var textoColetado = elemento.Text.Trim();

                        if (textoColetado != "")
                        {
                            foreach (var texto in textos)
                            {
                                if (textoColetado.Contains(texto))
                                    return true;
                            }
                        }
                    }

                    Thread.Sleep(100);
                }
                catch (StaleElementReferenceException)
                {
                    continue;
                }
            }

            return false;
        }

        public static bool CheckText(this RemoteWebDriver driver, By by, string texto, int Segundos = 5)
        {
            for (int i = 0; i < Segundos * 10; i++)
            {
                var elemento = driver.FindElementSafe(by);

                if (elemento != null)
                {
                    var textoColetado = elemento.Text.Trim();

                    if (!string.IsNullOrEmpty(textoColetado))
                    {
                        if (textoColetado.Contains(texto))
                        {
                            return true;
                        }
                    }
                }
                Thread.Sleep(100);
            }


            return false;
        }


        public static bool Exist(this RemoteWebDriver driver, By by)
        {
            var retorno = FindElementSafe(driver, by);

            if (retorno != null) return true;
            else return false;
        }

        public static bool Exist(this RemoteWebDriver driver, By by, int time)
        {
            var existe = false;

            for (int i = 0; i < (time / 100); i++)
            {
                existe = Exist(driver, by);
                if (existe) break;
                Thread.Sleep(100);
            }

            return existe;
        }

        public static bool CheckLoadSpan(this RemoteWebDriver drive, By by, int time = 15)
        {
            int qtdLinhas, qtdLinhasCarregadas;

            while (true)
            {
                qtdLinhas = drive.WaitElementVisibleSafe(by, 10000).FindElements(By.TagName("span")).Count;
                Thread.Sleep(2000);
                qtdLinhasCarregadas = drive.WaitElementVisibleSafe(by, 10000).FindElements(By.TagName("span")).Count;
                time--;
                if (qtdLinhas == qtdLinhasCarregadas && time > 0)
                    return true;
                else if (time <= 0)
                    return false;
            }

        }

        public static bool CheckTitle(this RemoteWebDriver driver, By by, string titulo, int time = 30000)
        {
            var text = driver.WaitElement(by, time).GetAttribute("title");
            if (text.Equals(titulo))
                return true;
            return false;
        }

        public static void Fill(this RemoteWebDriver driver, By by, string text, int segundos = 5)
        {
            for (int i = 0; i < segundos * 10; i++)
            {
                try
                {
                    var elemento = driver.FindElementSafe(by);
                    if (elemento != null)
                    {
                        elemento.Clear();
                        elemento.SendKeys(text);
                        Thread.Sleep(300);
                        return;
                    }
                }
                catch (Exception)
                {
                    continue;
                }
                Thread.Sleep(100);
            }
            throw new FillException(by.ToString());
        }

        public static void Click(this RemoteWebDriver driver, By by, int tempoSegundos = 10)
        {
            for (int i = 0; i < tempoSegundos * 10; i++)
            {
                try
                {
                    Thread.Sleep(100);
                    driver.FindElementSafe(by).Click();
                    return;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            throw new ClickExcetion(by.ToString());
        }

        public static void Click(this RemoteWebDriver driver, By by, string msgError, int tempoSegundos = 10)
        {
            for (int i = 0; i < tempoSegundos * 10; i++)
            {
                try
                {
                    Thread.Sleep(100);
                    driver.FindElementSafe(by).Click();
                    return;
                }
                catch (InvalidOperationException)
                {
                    continue;
                }
                catch (StaleElementReferenceException)
                {
                    continue;
                }
                catch (NoSuchElementException)
                {
                    continue;
                }
                catch (ElementNotVisibleException)
                {
                    continue;
                }
                catch (NullReferenceException)
                {
                    continue;
                }
            }
            throw new ClickExcetion(msgError);
        }

        public static IWebElement GetShadowRoot(this RemoteWebDriver driver, IWebElement elemento)
        {
            return (IWebElement)driver.ExecuteScript($"return arguments[0].shadowRoot", elemento);
        }
    }

    public class ClickExcetion : Exception
    {
        public ClickExcetion(string msg)
            : base(msg) { }
        public ClickExcetion()
           : base("It was not possible to click.") { }
    }

    public class FillException : Exception
    {
        public FillException(string msg)
            : base(msg) { }
        public FillException()
            : base("Couldn't fill") { }
    }
}
