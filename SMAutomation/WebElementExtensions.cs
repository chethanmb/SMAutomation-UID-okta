using System;
using System.Threading;
using OpenQA.Selenium;

namespace SMAutomation
{
	// Token: 0x02000004 RID: 4
	public static class WebElementExtensions
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00005018 File Offset: 0x00003218
		public static void waitForAsyncContent(this IWebDriver driver, By by, int timeout = 10)
		{
			IWebElement webElement = driver.FindElement(by);
			Thread.Sleep(2000);
			int num = 0;
			while (!webElement.Displayed && num < timeout)
			{
				num++;
				Thread.Sleep(1000);
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00005064 File Offset: 0x00003264
		public static bool WaitUntilElementIsPresent(this IWebDriver driver, By by, int timeout = 10)
		{
			Thread.Sleep(2000);
			int num = 0;
			while (!driver.ElementIsPresent(by) && num < timeout)
			{
				num++;
				Thread.Sleep(1000);
			}
			bool flag = driver.ElementIsPresent(by);
			return flag || num != timeout;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000050C4 File Offset: 0x000032C4
		public static bool ElementIsPresent(this IWebDriver driver, By by)
		{
			bool result = false;
			driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 0, 1));
			try
			{
				result = driver.FindElement(by).Displayed;
			}
			catch (NoSuchElementException)
			{
				return false;
			}
			return result;
		}
	}
}
