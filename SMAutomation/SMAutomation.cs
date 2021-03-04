using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml;
using OfficeOpenXml;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace SMAutomation
{
	// Token: 0x02000003 RID: 3
	internal class SMAutomation
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000029DC File Offset: 0x00000BDC
		private static void Main(string[] args)
		{
			bool flag = args.Length == 1;
			if (flag)
			{
				bool flag2 = args[0] == "smmonitor_ping";
				if (flag2)
				{
					Console.WriteLine("SM Automation running");
				}
			}
			else
			{
				AppDomain.CurrentDomain.UnhandledException += delegate(object sender, UnhandledExceptionEventArgs args1)
				{
					Console.Error.WriteLine("Unhandled exception: " + args1.ExceptionObject);
					Environment.Exit(1);
				};
				using (SingleProgramInstance singleProgramInstance = new SingleProgramInstance("x5k6yz"))
				{
					bool isSingleInstance = singleProgramInstance.IsSingleInstance;
					if (isSingleInstance)
					{
						SMAutomation smautomation = new SMAutomation();
						smautomation.writeToLog("Starting SM Automation");
						try
						{
							SMAutomation.readConfigFile();
						}
						catch (Exception ex)
						{
							smautomation.writeToLog("Exiting Main");
							smautomation.writeToLog(ex.StackTrace);
						}
						finally
						{
							smautomation.writeToLog("Exiting Finally Main");
						}
						SMAutomation.stDt = DateTime.Now;
						smautomation.checkLandingZoneForFiles();
					}
					else
					{
						singleProgramInstance.RaiseOtherProcess();
					}
				}
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002B04 File Offset: 0x00000D04
		private static void readConfigFile()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "\\SMAutomation.config");
			XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/SMAutomation");
			XmlNode xmlNode = xmlNodeList[0];
			SMAutomation.pwd = xmlNode.SelectSingleNode("pwd").InnerText.Trim();
			SMAutomation.sm9URL = xmlNode.SelectSingleNode("sm9URL").InnerText.Trim();
			SMAutomation.hubURL = xmlNode.SelectSingleNode("hubURL").InnerText.Trim();
			SMAutomation.webServiceURL = xmlNode.SelectSingleNode("webServiceURL").InnerText.Trim();
			SMAutomation.sm9User = xmlNode.SelectSingleNode("sm9User").InnerText.Trim();
			SMAutomation.sm9Template = xmlNode.SelectSingleNode("sm9Template").InnerText.Trim();
			SMAutomation.sm9UserHPEmail = xmlNode.SelectSingleNode("sm9EmpHPEmail").InnerText.Trim();
			SMAutomation.sm9SourceOfInteraction = xmlNode.SelectSingleNode("sm9SourceOfInteraction").InnerText.Trim();
			SMAutomation.sm9Company = xmlNode.SelectSingleNode("sm9Company").InnerText.Trim();
			SMAutomation.sm9OwnershipWorkgroup = xmlNode.SelectSingleNode("sm9OwnershipWorkgroup").InnerText.Trim();
			SMAutomation.sm9EmpId = xmlNode.SelectSingleNode("sm9EmpId").InnerText.Trim();
			SMAutomation.sm9ServiceLine = xmlNode.SelectSingleNode("sm9ServiceLine").InnerText.Trim();
			SMAutomation.sm9ServiceArea = xmlNode.SelectSingleNode("sm9ServiceArea").InnerText.Trim();
			SMAutomation.sm9Category = xmlNode.SelectSingleNode("sm9Category").InnerText.Trim();
			SMAutomation.sm9Area = xmlNode.SelectSingleNode("sm9Area").InnerText.Trim();
			SMAutomation.sm9SubArea = xmlNode.SelectSingleNode("sm9SubArea").InnerText.Trim();
			SMAutomation.uploadFilePath = xmlNode.SelectSingleNode("tmpUploadFilePath").InnerText.Trim();
			SMAutomation.userTimeZone = xmlNode.SelectSingleNode("UserTimeZone").InnerText.Trim();
			int.TryParse(xmlNode.SelectSingleNode("addTimeToTimeZoneInSec").InnerText.Trim(), out SMAutomation.addTimeToTimeZoneInSec);
			SMAutomation.logFilePath = xmlNode.SelectSingleNode("logFilePath").InnerText.Trim();
			SMAutomation.strLpath = xmlNode.SelectSingleNode("landingPath").InnerText.Trim();
			SMAutomation.ticketsFilePath = xmlNode.SelectSingleNode("ticketsFilePath").InnerText.Trim();
			SMAutomation.flgMultipleWorkGroupsAssignedToSM9User = xmlNode.SelectSingleNode("flgMultipleWorkGroupsAssignedToSM9User").InnerText.Trim();
			SMAutomation.encToolPath = xmlNode.SelectSingleNode("encToolPath").InnerText.Trim();
			SMAutomation.autoItProgPath = xmlNode.SelectSingleNode("autoItProgPath").InnerText.Trim();
			int.TryParse(xmlNode.SelectSingleNode("SleepTimeForNextRun").InnerText.Trim(), out SMAutomation.sleepTimeForNextRun);
			SMAutomation.sleepTimeForNextRun *= 1000;
			int.TryParse(xmlNode.SelectSingleNode("WaitTimeForAttachmentUpload").InnerText.Trim(), out SMAutomation.waitTimeForAttachmentUpload);
			double.TryParse(xmlNode.SelectSingleNode("waitTimeInHrsToDownloadStatusReport").InnerText.Trim(), out SMAutomation.waitTimeInHrsToDownloadStatusReport);
			int.TryParse(xmlNode.SelectSingleNode("NoOfRetriesBeforeMarkingAsHold").InnerText.Trim(), out SMAutomation.noRetriesBeforeMarkingHold);
			int.TryParse(xmlNode.SelectSingleNode("pickNthTicket").InnerText.Trim(), out SMAutomation.pickNthTicket);
			int.TryParse(xmlNode.SelectSingleNode("pickNthTicketFromNumber").InnerText.Trim(), out SMAutomation.pickNthTicketFromNumber);
			SMAutomation.onHoldTicketsFilePath = xmlNode.SelectSingleNode("onHoldTicketsLog").InnerText.Trim();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002EC8 File Offset: 0x000010C8
		private void makeEventEntry(Exception ex)
		{
			EventLog eventLog = new EventLog();
			eventLog.Source = ".NET Runtime";
			string message = string.Format("Application: SMAutomation.exe\n{0}\n{1}", ex.Message, ex.StackTrace);
			eventLog.WriteEntry(message, EventLogEntryType.Error, 1026);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002F10 File Offset: 0x00001110
		private void logout(IWebDriver drv)
		{
			try
			{
				drv.SwitchTo().DefaultContent();
				drv.FindElement(By.XPath("//button[contains(@aria-label, 'User Information')]")).Click();
				drv.FindElement(By.XPath("//button[contains(text(), 'Logout')]")).Click();
			}
			catch (Exception ex)
			{
				this.writeToLog(ex.Message);
				this.writeToLog(ex.StackTrace);
				this.makeEventEntry(ex);
				this.writeToLog("Error: Exception in logout 1");
			}
			for (;;)
			{
				try
				{
					IAlert alert = drv.SwitchTo().Alert();
					break;
				}
				catch (NoAlertPresentException ex2)
				{
					drv.FindElement(By.XPath("//button[contains(text(), 'Logout')]")).Click();
					Thread.Sleep(500);
				}
			}
			try
			{
				SMAutomation.acceptNextAlert = true;
				this.CloseAlertAndGetItsText();
				drv.Close();
				drv.Quit();
				drv.Dispose();
				drv = null;
			}
			catch (Exception ex3)
			{
				this.writeToLog(ex3.Message);
				this.writeToLog(ex3.StackTrace);
				this.makeEventEntry(ex3);
				this.writeToLog("Error: Exception in logout 2");
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00003054 File Offset: 0x00001254
		private void checkLandingZoneForFiles()
		{
			this.writeToLog("Inside checkLandingZoneForFiles");
			DirectoryInfo directoryInfo = new DirectoryInfo(SMAutomation.strLpath);
			DirectoryInfo directoryInfo2 = (from d in directoryInfo.GetDirectories()
			where d.GetFiles("completed.txt").Length < 1 && d.GetFiles("*.xlsx").Length != 0 && (int.Parse(d.Name.Substring(2)) - SMAutomation.pickNthTicketFromNumber) % SMAutomation.pickNthTicket == 0
			orderby d.Name
			select d).FirstOrDefault<DirectoryInfo>();
			this.writeToLog("------------------------");
			this.writeToLog(directoryInfo2.FullName);
			this.writeToLog("------------------------");
			bool flag = directoryInfo2 == null;
			if (flag)
			{
				SMAutomation.noRetriesBeforeMarkingHoldCounter = 0;
				this.writeToLog("No files to create tickets. Going to sleep mode.");
				Thread.Sleep(SMAutomation.sleepTimeForNextRun);
				this.checkLandingZoneForFiles();
			}
			else
			{
				bool flag2 = !this.searchTicketNumberInFile(directoryInfo2.Name + "_");
				if (flag2)
				{
					SMAutomation.noRetriesBeforeMarkingHoldCounter = 0;
					this.writeToLog("in checkLandingZoneForFiles");
					this.createNewTicket(directoryInfo2.FullName);
					this.writeToLog("Ticket created for" + directoryInfo2.FullName + ". Going for the next one.");
					this.checkLandingZoneForFiles();
				}
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00003188 File Offset: 0x00001388
		private void testMyFlow()
		{
			bool flag = !SMAutomation.flgLoggedIn;
			if (flag)
			{
				this.loginIntoSM();
				SMAutomation.flgLoggedIn = true;
			}
			else
			{
				this.checkIfSessionTimeOut();
			}
			Thread.Sleep(30000);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000031C8 File Offset: 0x000013C8
		private void refreshLoginSession()
		{
			this.writeToLog("Start: Refresh Login Session");
			SMAutomation.drv.SwitchTo().DefaultContent();
			SMAutomation.drv.waitForAsyncContent(By.XPath("//button[contains(@style, 'tcancel.png')]"), 120);
			SMAutomation.drv.FindElement(By.XPath("//button[contains(@style, 'tcancel.png')]")).Click();
			SMAutomation.drv.SwitchTo().DefaultContent();
			Thread.Sleep(5000);
			while (!SMAutomation.drv.FindElement(By.XPath("//div[contains(@class, 'message-title-ext-mb-warning')]")).Displayed)
			{
			}
			IWebElement webElement = SMAutomation.drv.FindElement(By.XPath("//div[contains(@class, 'message-title-ext-mb-warning')]"));
			webElement.FindElement(By.XPath("//button[@id='y']")).Click();
			SMAutomation.drv.waitForAsyncContent(By.XPath("//div[@id='ROOT/Service Desk/Register New Interaction']"), 10);
			bool displayed = SMAutomation.drv.FindElement(By.XPath("//div[@id='ROOT/Service Desk/Register New Interaction']")).Displayed;
			if (displayed)
			{
				SMAutomation.drv.FindElement(By.XPath("//div[@id='ROOT/Service Desk/Register New Interaction']")).Click();
			}
			this.writeToLog("End: Refresh Login Session");
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000032EC File Offset: 0x000014EC
		private bool searchTicketNumberInFile(string ticketNo)
		{
			StreamReader streamReader = File.OpenText(SMAutomation.ticketsFilePath);
			string text = streamReader.ReadToEnd();
			streamReader.Close();
			return text.Contains("###" + ticketNo);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00003328 File Offset: 0x00001528
		private void writeToLog(string msg)
		{
			StreamWriter streamWriter = File.AppendText(string.Format("{0}{1:dd_MM_yyyy}.log", SMAutomation.logFilePath, DateTime.Today));
			streamWriter.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " : " + msg);
			streamWriter.Flush();
			streamWriter.Close();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00003388 File Offset: 0x00001588
		private void writeCompletedTxt(string strPath, string ticketNo)
		{
			StreamWriter streamWriter = File.AppendText(string.Format("{0}/completed.txt", strPath));
			streamWriter.WriteLine(string.Format("Ticket Created: {0} on {1:dd/MM/yyyy hh:mm:ss}", ticketNo, DateTime.Now));
			streamWriter.Flush();
			streamWriter.Close();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000033D4 File Offset: 0x000015D4
		private void writeTicketNumberToFile(string ticketNoStr)
		{
			StreamWriter streamWriter = File.AppendText(SMAutomation.ticketsFilePath);
			streamWriter.Write("###" + ticketNoStr + "###,");
			streamWriter.Flush();
			streamWriter.Close();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00003414 File Offset: 0x00001614
		private void markTicketAsHold(string ticketNoStr)
		{
			StreamWriter streamWriter = File.AppendText(SMAutomation.ticketsFilePath);
			StreamWriter streamWriter2 = File.AppendText(SMAutomation.onHoldTicketsFilePath);
			streamWriter.Write(string.Format("###{0}_OnHold_{1:yyyyMMddHHmm}###,", ticketNoStr, DateTime.Now));
			streamWriter2.Write(string.Format("###{0}_OnHold_{1:yyyyMMddHHmm}###,", ticketNoStr, DateTime.Now));
			streamWriter.Flush();
			streamWriter.Close();
			streamWriter2.Flush();
			streamWriter2.Close();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000348C File Offset: 0x0000168C
		private IAlert AlertIsPresent(IWebDriver drv)
		{
			IAlert result;
			try
			{
				result = drv.SwitchTo().Alert();
			}
			catch (NoAlertPresentException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000034C0 File Offset: 0x000016C0
		private string CloseAlertAndGetItsText()
		{
			string result;
			try
			{
				IAlert alert = SMAutomation.drv.SwitchTo().Alert();
				string text = alert.Text;
				bool flag = SMAutomation.acceptNextAlert;
				if (flag)
				{
					alert.Accept();
				}
				else
				{
					alert.Dismiss();
				}
				result = text;
			}
			finally
			{
				SMAutomation.acceptNextAlert = true;
			}
			return result;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00003524 File Offset: 0x00001724
		private void checkIfSessionTimeOut()
		{
			SMAutomation.drv.SwitchTo().DefaultContent();
			try
			{
				this.writeToLog("checkIfSessionTimeOut");
				SMAutomation.drv.WaitUntilElementIsPresent(By.Id("loginAgain"), 15);
				IWebElement webElement = SMAutomation.drv.FindElement(By.Id("loginAgain"));
				while (webElement.Displayed)
				{
					webElement.Click();
					Thread.Sleep(2000);
				}
			}
			catch (Exception ex)
			{
				this.writeToLog("checkIfSessionTimeOut " + ex.Message);
			}
			Thread.Sleep(2000);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000035D8 File Offset: 0x000017D8
		private void loginIntoSM()
		{
			Process process = new Process();
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.FileName = SMAutomation.encToolPath + "\\SMAutomationEncPWD.exe ";
			process.StartInfo.Arguments = SMAutomation.sm9User + " " + SMAutomation.pwd;
			process.Start();
			string text = process.StandardOutput.ReadToEnd();
			process.WaitForExit();
			DesiredCapabilities desiredCapabilities = DesiredCapabilities.InternetExplorer();
			Uri uri = new Uri(SMAutomation.hubURL);
			SMAutomation.drv = new RemoteWebDriver(uri, desiredCapabilities);
			try
			{
				this.writeToLog("Before SMAutomationAutoIt");
				new Process
				{
					StartInfo = 
					{
						UseShellExecute = false,
						RedirectStandardOutput = true,
						FileName = SMAutomation.autoItProgPath + "SMAutomationAutoIt.exe"
					}
				}.Start();
				this.writeToLog("After SMAutomationAutoIt");
				SMAutomation.drv.Navigate().GoToUrl(SMAutomation.sm9URL);
				Thread.Sleep(2000);
				this.writeToLog("After navigate");
				IAlert alert = this.AlertIsPresent(SMAutomation.drv);
				bool flag = alert != null;
				if (flag)
				{
					alert.Dismiss();
				}
				SMAutomation.drv.SwitchTo().DefaultContent();
				IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)SMAutomation.drv;
				this.writeToLog("executor");
				SMAutomation.drv.WaitUntilElementIsPresent(By.Name("username"), 25);
				IWebElement webElement = SMAutomation.drv.FindElement(By.Name("username"));
				this.writeToLog("user");
				webElement.SendKeys(sm9User);
				this.writeToLog("After username");
				//Thread.Sleep(2000);
				IWebElement webElement2 = SMAutomation.drv.FindElement(By.Name("password"));
				webElement2.SendKeys(text);
				this.writeToLog("After pwd");
				//Thread.Sleep(2000);

				IWebElement webElement3 = SMAutomation.drv.FindElement(By.Id("okta-signin-submit"));
				webElement3.SendKeys(Keys.Space);
				this.writeToLog("After click");

				//Thread.Sleep(5000);
				SMAutomation.drv.WaitUntilElementIsPresent(By.XPath("//label[text()='Do not challenge me on this device for the next 24 hours']"), 15);
				IWebElement chkBx = SMAutomation.drv.FindElement(By.XPath("//label[text()='Do not challenge me on this device for the next 24 hours']"));
                var chkStatus = chkBx.Selected;
                if (chkStatus == false)
                {
                    javaScriptExecutor.ExecuteScript("arguments[0].click();", chkBx);
                }

                IWebElement push = SMAutomation.drv.FindElement(By.CssSelector(".button-primary"));
				push.SendKeys(Keys.Space);

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/{1}/recordlogin", SMAutomation.webServiceURL.Replace("/incident", "/sm"), SMAutomation.sm9User));
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                string text2 = streamReader.ReadToEnd();
                streamReader.Close();
                httpWebResponse.Close();
            }
            catch (Exception ex)
			{
				this.writeErrorToLog(ex);
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000038B4 File Offset: 0x00001AB4
		private void writeErrorToLog(Exception ex)
		{
			this.writeToLog(ex.Message);
			this.writeToLog(ex.StackTrace);
			while (ex.InnerException != null)
			{
				this.writeErrorToLog(ex.InnerException);
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000038FC File Offset: 0x00001AFC
		private void createNewTicket(string strPath)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(strPath);
			string text = "";
			try
			{
				bool flag = !SMAutomation.flgLoggedIn;
				if (flag)
				{
					this.loginIntoSM();
					SMAutomation.flgLoggedIn = true;
				}
				else
				{
					this.checkIfSessionTimeOut();
				}
				this.writeToLog("Beginning create ticket for: " + strPath);
				this.writeToLog("********************************************************************************************");
				this.writeToLog("********************************************************************************************");
				IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)SMAutomation.drv;
				string fullName = directoryInfo.GetFiles()[0].FullName;
				DirectoryInfo directoryInfo2 = new DirectoryInfo(strPath + "/Attachments");
				this.writeToLog("Start: Collect info from Excel file");
				FileInfo fileInfo = new FileInfo(fullName);
				ExcelPackage excelPackage = new ExcelPackage(fileInfo);
				ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.First<ExcelWorksheet>();
				bool flag2 = excelWorksheet.Cells[1, 2].Value.ToString().ToLower().Contains("e-enablement");
				string text2 = flag2 ? excelWorksheet.Cells[13, 2].Value.ToString() : excelWorksheet.Cells[16, 2].Value.ToString();
				string text3 = flag2 ? excelWorksheet.Cells[19, 2].Value.ToString() : excelWorksheet.Cells[32, 2].Value.ToString();
				string text4 = "Single User";
				bool flag3 = flag2;
				if (flag3)
				{
					bool flag4 = excelWorksheet.Cells[15, 2].Value != null;
					if (flag4)
					{
						text4 = excelWorksheet.Cells[15, 2].Value.ToString();
					}
				}
				else
				{
					bool flag5 = excelWorksheet.Cells[18, 2].Value != null;
					if (flag5)
					{
						text4 = excelWorksheet.Cells[18, 2].Value.ToString();
					}
				}
				string text5 = excelWorksheet.Cells[19, 2].Value.ToString();
				this.writeToLog("End: Collect info from Excel file");
				excelPackage.Dispose();
				this.writeToLog("Dispose: Excel file");
				SMAutomation.drv.SwitchTo().DefaultContent();
				Thread.Sleep(3000);
				bool flag6 = SMAutomation.drv.WaitUntilElementIsPresent(By.XPath("//div[@id='ROOT/Service Desk']"), 15);
				bool flag7 = false;
				this.writeToLog("Trying to open Register New Interaction form.");
				while (!SMAutomation.drv.FindElement(By.XPath("//div[@id='ROOT/Service Desk/Register New Interaction']")).Displayed)
				{
					SMAutomation.drv.FindElement(By.XPath("//div[@id='ROOT/Service Desk']")).Click();
					bool displayed = SMAutomation.drv.FindElement(By.XPath("//div[@id='ROOT/Service Desk/Register New Interaction']")).Displayed;
					if (displayed)
					{
						SMAutomation.drv.FindElement(By.XPath("//div[@id='ROOT/Service Desk/Register New Interaction']")).Click();
						flag7 = true;
					}
				}
				bool flag8 = flag7;
				if (flag8)
				{
					flag6 = SMAutomation.drv.WaitUntilElementIsPresent(By.XPath("//iframe[contains(@src, 'nav.menu')]"), 15);
				}
				IWebElement webElement = SMAutomation.drv.FindElement(By.XPath("//iframe[contains(@src, 'nav.menu')]"));
				SMAutomation.drv.SwitchTo().Frame(webElement);
				SMAutomation.drv.waitForAsyncContent(By.Id("topaz"), 120);
				this.writeToLog("Start: Source of Interaction");
				IWebElement webElement2 = SMAutomation.drv.FindElements(By.CssSelector("#topaz > div"))[3];
				IWebElement webElement3 = SMAutomation.drv.FindElements(By.CssSelector("#" + webElement2.GetAttribute("id") + " > div > div"))[0].FindElement(By.TagName("input"));
				webElement3.SendKeys(SMAutomation.sm9SourceOfInteraction + "\t");
				this.writeToLog("End: Source of Interaction");
				string text6 = webElement2.GetAttribute("id").Replace("Border", "") + "Popup";
				flag6 = SMAutomation.drv.WaitUntilElementIsPresent(By.Id(text6), 5);
				this.writeToLog("Start: Receipt time");
				DateTime dateTime = DateTime.Now;
				bool flag9 = TimeZoneInfo.Local.DisplayName.ToLower() != SMAutomation.userTimeZone.ToLower();
				if (flag9)
				{
					AllTimeZones allTimeZones = new AllTimeZones();
					TimeZoneInfo destinationTimeZone = TimeZoneInfo.FindSystemTimeZoneById(allTimeZones.getTimeZoneByName(SMAutomation.userTimeZone));
					dateTime = TimeZoneInfo.ConvertTime(DateTimeOffset.Now, destinationTimeZone).DateTime;
					bool flag10 = SMAutomation.addTimeToTimeZoneInSec != 0;
					if (flag10)
					{
						dateTime = dateTime.AddSeconds((double)SMAutomation.addTimeToTimeZoneInSec);
					}
				}
				flag6 = SMAutomation.drv.WaitUntilElementIsPresent(By.XPath("//img[contains(@src, 'calendar.png')]"), 15);
				SMAutomation.drv.waitForAsyncContent(By.XPath("//img[contains(@src, 'calendar.png')]"), 120);
				IWebElement webElement4 = SMAutomation.drv.FindElements(By.CssSelector("#topaz > div"))[5];
				IWebElement webElement5 = SMAutomation.drv.FindElements(By.CssSelector("#" + webElement4.GetAttribute("id") + " > div > div"))[0].FindElement(By.TagName("input"));
				webElement5.SendKeys(dateTime.ToString("dd/MM/yyyy H:mm:ss"));
				this.writeToLog("End: Receipt Time");
				this.writeToLog("Start: Company");
				IWebElement webElement6 = SMAutomation.drv.FindElements(By.CssSelector("#topaz > div"))[6];
				string text7 = webElement6.GetAttribute("id").Replace("Border", "") + "Popup";
				SMAutomation.drv.FindElements(By.CssSelector("#" + webElement6.GetAttribute("id") + " > div > div"))[0].FindElement(By.TagName("input")).SendKeys(SMAutomation.sm9Company + "\t");
				this.writeToLog("End: Company");
				this.writeToLog("Does user has multiple groups: " + SMAutomation.flgMultipleWorkGroupsAssignedToSM9User);
				bool flag11 = SMAutomation.flgMultipleWorkGroupsAssignedToSM9User == "true";
				if (flag11)
				{
					SMAutomation.drv.SwitchTo().DefaultContent();
					this.writeToLog("Start: workgroup selection");
					flag6 = SMAutomation.drv.WaitUntilElementIsPresent(By.XPath("//iframe[contains(@src, 'tpz_container.jsp')]"), 15);
					webElement = SMAutomation.drv.FindElement(By.XPath("//iframe[contains(@src, 'tpz_container.jsp')]"));
					SMAutomation.drv.SwitchTo().Frame(webElement);
					flag6 = SMAutomation.drv.WaitUntilElementIsPresent(By.LinkText(SMAutomation.sm9OwnershipWorkgroup), 15);
					this.writeToLog("Multiple work group link found");
					SMAutomation.drv.waitForAsyncContent(By.LinkText(SMAutomation.sm9OwnershipWorkgroup), 120);
					SMAutomation.drv.FindElement(By.LinkText(SMAutomation.sm9OwnershipWorkgroup)).Click();
					this.writeToLog("End: workgroup selection");
				}
				SMAutomation.drv.SwitchTo().DefaultContent();
				flag6 = SMAutomation.drv.WaitUntilElementIsPresent(By.XPath("//iframe[contains(@src, 'nav.menu')]"), 5);
				SMAutomation.drv.waitForAsyncContent(By.XPath("//iframe[contains(@src, 'nav.menu')]"), 120);
				webElement = SMAutomation.drv.FindElement(By.XPath("//iframe[contains(@src, 'nav.menu')]"));
				SMAutomation.drv.SwitchTo().Frame(webElement);
				SMAutomation.drv.waitForAsyncContent(By.Id("topaz"), 120);
				this.writeToLog("Start: Contact for this interaction");
				Thread.Sleep(2000);
				IWebElement webElement7 = SMAutomation.drv.FindElements(By.CssSelector("#topaz > div"))[7];
				SMAutomation.drv.FindElements(By.CssSelector("#" + webElement7.GetAttribute("id") + " > div > div"))[0].FindElements(By.TagName("div"))[0].FindElement(By.TagName("input")).SendKeys(SMAutomation.sm9UserHPEmail);
				SMAutomation.drv.SwitchTo().DefaultContent();
				this.writeToLog("End: Contact for this interaction");
				SMAutomation.drv.waitForAsyncContent(By.XPath("//iframe[contains(@src, 'nav.menu')]"), 120);
				webElement = SMAutomation.drv.FindElement(By.XPath("//iframe[contains(@src, 'nav.menu')]"));
				SMAutomation.drv.SwitchTo().Frame(webElement);
				flag6 = SMAutomation.drv.WaitUntilElementIsPresent(By.Id("topaz"), 15);
				this.writeToLog("Start: Title");
				IWebElement webElement8 = SMAutomation.drv.FindElements(By.CssSelector("#topaz > div"))[25];
				IWebElement webElement9 = SMAutomation.drv.FindElements(By.CssSelector("#" + webElement8.GetAttribute("id") + " > div "))[0].FindElement(By.TagName("input"));
				javaScriptExecutor.ExecuteScript("arguments[0].value = arguments[1];", new object[]
				{
					webElement9,
					text2
				});
				this.writeToLog("End: Title");
				this.writeToLog("Start: Description");
				SMAutomation.drv.FindElements(By.CssSelector("#topaz > div"))[27].FindElement(By.CssSelector("div > div")).Click();
				IWebElement webElement10 = SMAutomation.drv.FindElements(By.CssSelector("#topaz > div"))[27].FindElement(By.TagName("textarea"));
				javaScriptExecutor.ExecuteScript("arguments[0].value = arguments[1];", new object[]
				{
					webElement10,
					text3
				});
				this.writeToLog("End: Description");
				IWebElement webElement11 = SMAutomation.drv.FindElements(By.CssSelector("#topaz > div"))[2].FindElement(By.ClassName("notebookBackground")).FindElement(By.XPath("//a[contains(@title, 'Interaction')]"));
				this.writeToLog("Start: Apply Template");
				SMAutomation.drv.SwitchTo().DefaultContent();
				flag6 = SMAutomation.drv.WaitUntilElementIsPresent(By.XPath("//button[contains(text(), 'Apply Template')]"), 15);
				SMAutomation.drv.waitForAsyncContent(By.XPath("//button[contains(text(), 'Apply Template')]"), 120);
				SMAutomation.drv.FindElement(By.XPath("//button[contains(text(), 'Apply Template')]")).Click();
				flag6 = SMAutomation.drv.WaitUntilElementIsPresent(By.XPath("//iframe[contains(@src, 'nav.menu')]"), 15);
				SMAutomation.drv.waitForAsyncContent(By.XPath("//iframe[contains(@src, 'nav.menu')]"), 120);
				webElement = SMAutomation.drv.FindElement(By.XPath("//iframe[contains(@src, 'nav.menu')]"));
				SMAutomation.drv.SwitchTo().Frame(webElement);
				flag6 = SMAutomation.drv.WaitUntilElementIsPresent(By.LinkText(SMAutomation.sm9Template), 15);
				SMAutomation.drv.waitForAsyncContent(By.LinkText(SMAutomation.sm9Template), 120);
				SMAutomation.drv.FindElement(By.LinkText(SMAutomation.sm9Template)).Click();
				this.writeToLog("End: Apply Template");
				SMAutomation.drv.SwitchTo().DefaultContent();
				SMAutomation.drv.WaitUntilElementIsPresent(By.XPath("//iframe[contains(@src, 'nav.menu')]"), 15);
				SMAutomation.drv.waitForAsyncContent(By.XPath("//iframe[contains(@src, 'nav.menu')]"), 120);
				webElement = SMAutomation.drv.FindElement(By.XPath("//iframe[contains(@src, 'nav.menu')]"));
				SMAutomation.drv.SwitchTo().Frame(webElement);
				IWebElement webElement12 = SMAutomation.drv.FindElements(By.CssSelector("#topaz > div"))[2];
				IWebElement webElement13 = SMAutomation.drv.FindElements(By.CssSelector("#" + webElement12.GetAttribute("id") + " > div"))[1];
				string attribute = webElement13.GetAttribute("id");
				SMAutomation.drv.waitForAsyncContent(By.Id("topaz"), 120);
				SMAutomation.drv.SwitchTo().DefaultContent();
				SMAutomation.drv.WaitUntilElementIsPresent(By.XPath("//iframe[contains(@src, 'nav.menu')]"), 15);
				SMAutomation.drv.waitForAsyncContent(By.XPath("//iframe[contains(@src, 'nav.menu')]"), 120);
				webElement = SMAutomation.drv.FindElement(By.XPath("//iframe[contains(@src, 'nav.menu')]"));
				SMAutomation.drv.SwitchTo().Frame(webElement);
				this.writeToLog("Start: Impact");
				text4 = text4.ToLower();
				string text8 = (text4 == "entire site / enterprise") ? "1" : ((text4 == "entire account / team / department") ? "2" : ((text4 == "multiple user") ? "3" : "4"));
				bool flag12 = text8 != "4";
				if (flag12)
				{
					IWebElement webElement14 = SMAutomation.drv.FindElements(By.CssSelector("#" + attribute + " > div > div > div > div"))[3];
					IWebElement webElement15 = SMAutomation.drv.FindElements(By.CssSelector("#" + webElement14.GetAttribute("id") + " > div > div"))[1];
					webElement15.FindElement(By.TagName("img")).Click();
					string str = webElement14.GetAttribute("id").Replace("Border", "") + "Popup";
					string text9 = str + "_" + text8;
					SMAutomation.drv.waitForAsyncContent(By.Id(text9), 120);
					SMAutomation.drv.FindElement(By.Id(text9)).Click();
				}
				this.writeToLog("End: Impact");
				this.writeToLog("Start: urgency");
				string text10 = (text4 == "entire site / enterprise") ? "1" : ((text4 == "entire account / team / department") ? "2" : ((text4 == "multiple user") ? "3" : "4"));
				bool flag13 = text10 != "4";
				if (flag13)
				{
					IWebElement webElement16 = SMAutomation.drv.FindElements(By.CssSelector("#" + attribute + " > div > div > div > div"))[4];
					IWebElement webElement17 = SMAutomation.drv.FindElements(By.CssSelector("#" + webElement16.GetAttribute("id") + " > div > div"))[1];
					webElement17.FindElement(By.TagName("img")).Click();
					string str2 = webElement16.GetAttribute("id").Replace("Border", "") + "Popup";
					string text11 = str2 + "_" + text10;
					SMAutomation.drv.waitForAsyncContent(By.Id(text11), 120);
					SMAutomation.drv.FindElement(By.Id(text11)).Click();
				}
				this.writeToLog("End: Urgency");
				this.writeToLog("Start: Escalation");
				SMAutomation.drv.SwitchTo().DefaultContent();
				SMAutomation.drv.waitForAsyncContent(By.XPath("//button[contains(@style, 'tescalat.png')]"), 120);
				SMAutomation.drv.FindElement(By.XPath("//button[contains(@style, 'tescalat.png')]")).Click();
				this.writeToLog("End: Escalation");
				SMAutomation.drv.waitForAsyncContent(By.XPath("//iframe[contains(@src, 'nav.menu')]"), 120);
				webElement = SMAutomation.drv.FindElement(By.XPath("//iframe[contains(@src, 'nav.menu')]"));
				SMAutomation.drv.SwitchTo().Frame(webElement);
				flag6 = SMAutomation.drv.WaitUntilElementIsPresent(By.Id("topaz"), 15);
				SMAutomation.drv.waitForAsyncContent(By.Id("topaz"), 120);
				this.writeToLog("Start: Escalation assignment");
				IWebElement webElement18 = SMAutomation.drv.FindElements(By.CssSelector("#topaz > div > div"))[1].FindElement(By.TagName("fieldset"));
				IWebElement webElement19 = SMAutomation.drv.FindElements(By.CssSelector("#" + webElement18.GetAttribute("id") + " > div"))[1];
				IList<IWebElement> list = SMAutomation.drv.FindElements(By.CssSelector("#" + webElement19.GetAttribute("id") + " > div > div"));
				IWebElement webElement20 = webElement19.FindElements(By.CssSelector("." + list[0].GetAttribute("class") + " > div > input"))[0];
				this.writeToLog("Escalation assignment=" + webElement20.GetAttribute("id"));
				javaScriptExecutor.ExecuteScript("arguments[0].value = arguments[1];", new object[]
				{
					webElement20,
					SMAutomation.sm9OwnershipWorkgroup
				});
				webElement19.FindElement(By.CssSelector("." + list[1].GetAttribute("class") + " > div > img")).Click();
				this.writeToLog("End: Escalation assignment");
				SMAutomation.clickNextWaitForEscalateCntr = 0;
				this.clickNextAndWaitForEscalate(SMAutomation.drv);
				SMAutomation.SMTicketNumberWaitCntr = 0;
				text = this.getSMTicketNumber();
				this.writeTicketNumberToFile(string.Concat(new string[]
				{
					directoryInfo.Name,
					"_",
					text,
					"_",
					DateTime.Now.ToString("yyyyMMddHHmm")
				}));
				this.writeToLog("End: Collect SM ticket No " + text);
				this.writeCompletedTxt(strPath, text);
			}
			catch (Exception ex)
			{
				this.writeToLog("Failed to create ticket for: " + strPath);
				this.writeToLog(ex.StackTrace);
				this.writeToLog("Trying again for: " + strPath);
				SMAutomation.flgLoggedIn = false;
				this.logout(SMAutomation.drv);
				this.checkEligibleForHold(directoryInfo.Name, strPath);
			}
			try
			{
				bool flag14 = text != "";
				if (flag14)
				{
					HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/{1}_{2}/updateticketno", SMAutomation.webServiceURL, directoryInfo.Name, text));
					HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
					StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
					string text12 = streamReader.ReadToEnd();
					streamReader.Close();
					httpWebResponse.Close();
					this.writeToLog(string.Concat(new string[]
					{
						text12.ToLower().Contains("success") ? "Successfully updated" : "Failed to update",
						" Ticket details in 'BPS IT - My Support' for ",
						directoryInfo.Name,
						"_",
						text
					}));
				}
			}
			catch (Exception ex2)
			{
				this.writeToLog("logout failed");
				this.writeToLog(ex2.StackTrace);
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00004BBC File Offset: 0x00002DBC
		protected string getSMTicketNumber()
		{
			IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)SMAutomation.drv;
			SMAutomation.drv.SwitchTo().DefaultContent();
			this.writeToLog(string.Format("Start: Collect SM ticket no {0}", SMAutomation.SMTicketNumberWaitCntr++));
			IWebElement webElement = SMAutomation.drv.FindElement(By.XPath("//div[contains(@class, 'messageTrayElement infoMsg')]")).FindElements(By.TagName("p"))[1];
			string text = webElement.Text;
			bool flag = text.Trim().Length <= 1;
			if (flag)
			{
				text = javaScriptExecutor.ExecuteScript("var msgBx=document.getElementsByClassName('messageTrayElement infoMsg')[0]; return msgBx.getElementsByTagName('p')[1].textContent", new object[0]).ToString();
			}
			text = text.Replace(".", "").Trim();
			bool flag2 = SMAutomation.SMTicketNumberWaitCntr == 10 && (text.EndsWith("added") || text == "");
			if (flag2)
			{
				throw new Exception("Spent too much time to get ticket number");
			}
			bool flag3 = text.EndsWith("added");
			string result;
			if (flag3)
			{
				result = this.getSMTicketNumber();
			}
			else
			{
				string[] array = text.Split(new char[]
				{
					' '
				});
				result = array[array.Length - 1];
			}
			return result;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00004CF8 File Offset: 0x00002EF8
		protected bool clickNextAndWaitForEscalate(IWebDriver drv)
		{
			this.writeToLog(string.Format("In: clickNextAndWaitForEscalate {0}", SMAutomation.clickNextWaitForEscalateCntr));
			bool flag = false;
			try
			{
				SMAutomation.clickNextWaitForEscalateCntr++;
				drv.SwitchTo().DefaultContent();
				drv.waitForAsyncContent(By.XPath("//iframe[contains(@src, 'nav.menu')]"), 120);
				IWebElement webElement = drv.FindElement(By.XPath("//iframe[contains(@src, 'nav.menu')]"));
				drv.SwitchTo().Frame(webElement);
				IWebElement webElement2 = drv.FindElements(By.CssSelector("#topaz > div"))[2];
				IWebElement webElement3 = drv.FindElements(By.CssSelector("#" + webElement2.GetAttribute("id") + " > div > table > tbody > tr > td"))[1];
				IWebElement webElement4 = webElement3.FindElement(By.TagName("button"));
				webElement4.Click();
				Thread.Sleep(5000);
				drv.SwitchTo().DefaultContent();
				flag = drv.WaitUntilElementIsPresent(By.XPath("//button[contains(@style, 'tescalat.png')]"), 15);
			}
			catch (Exception ex)
			{
				flag = false;
			}
			bool flag2 = !flag;
			if (flag2)
			{
				bool flag3 = SMAutomation.clickNextWaitForEscalateCntr == 10;
				if (flag3)
				{
					throw new Exception("Spent too much time in escalate");
				}
				this.clickNextAndWaitForEscalate(drv);
			}
			return true;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00004E48 File Offset: 0x00003048
		protected void checkEligibleForHold(string ticketNoStr, string strPath)
		{
			bool flag = SMAutomation.prevTriedTicket == ticketNoStr;
			if (flag)
			{
				SMAutomation.noRetriesBeforeMarkingHoldCounter++;
			}
			else
			{
				SMAutomation.noRetriesBeforeMarkingHoldCounter = 1;
			}
			SMAutomation.prevTriedTicket = ticketNoStr;
			bool flag2 = SMAutomation.noRetriesBeforeMarkingHoldCounter > SMAutomation.noRetriesBeforeMarkingHold;
			if (flag2)
			{
				this.markTicketAsHold(ticketNoStr);
			}
			else
			{
				this.writeToLog("in checkEligibleForHold");
				this.createNewTicket(strPath);
			}
		}

		// Token: 0x04000002 RID: 2
		private static IWebDriver drv;

		// Token: 0x04000003 RID: 3
		private static bool acceptNextAlert;

		// Token: 0x04000004 RID: 4
		private static bool flgLoggedIn = false;

		// Token: 0x04000005 RID: 5
		private static string pwd = "";

		// Token: 0x04000006 RID: 6
		private static string sm9URL = "";

		// Token: 0x04000007 RID: 7
		private static string hubURL = "";

		// Token: 0x04000008 RID: 8
		private static string webServiceURL = "";

		// Token: 0x04000009 RID: 9
		private static string sm9Template = "";

		// Token: 0x0400000A RID: 10
		private static string sm9UserHPEmail = "";

		// Token: 0x0400000B RID: 11
		private static string sm9User = "";

		// Token: 0x0400000C RID: 12
		private static string sm9SourceOfInteraction = "";

		// Token: 0x0400000D RID: 13
		private static string sm9Company = "";

		// Token: 0x0400000E RID: 14
		private static string sm9OwnershipWorkgroup = "";

		// Token: 0x0400000F RID: 15
		private static string sm9EmpId = "";

		// Token: 0x04000010 RID: 16
		private static string sm9ServiceLine = "";

		// Token: 0x04000011 RID: 17
		private static string sm9ServiceArea = "";

		// Token: 0x04000012 RID: 18
		private static string sm9Category = "";

		// Token: 0x04000013 RID: 19
		private static string sm9Area = "";

		// Token: 0x04000014 RID: 20
		private static string sm9SubArea = "";

		// Token: 0x04000015 RID: 21
		private static string uploadFilePath = "";

		// Token: 0x04000016 RID: 22
		private static string userTimeZone = "";

		// Token: 0x04000017 RID: 23
		private static int addTimeToTimeZoneInSec = 0;

		// Token: 0x04000018 RID: 24
		private static string strLpath = "";

		// Token: 0x04000019 RID: 25
		private static string logFilePath = "";

		// Token: 0x0400001A RID: 26
		private static string ticketsFilePath = "";

		// Token: 0x0400001B RID: 27
		private static string onHoldTicketsFilePath = "";

		// Token: 0x0400001C RID: 28
		private static string flgMultipleWorkGroupsAssignedToSM9User = "";

		// Token: 0x0400001D RID: 29
		private static string encToolPath = "";

		// Token: 0x0400001E RID: 30
		private static string autoItProgPath = "";

		// Token: 0x0400001F RID: 31
		private static int waitTimeForAttachmentUpload = 2;

		// Token: 0x04000020 RID: 32
		private static int sleepTimeForNextRun = 1;

		// Token: 0x04000021 RID: 33
		private static int noRetriesBeforeMarkingHold = 10;

		// Token: 0x04000022 RID: 34
		private static int noRetriesBeforeMarkingHoldCounter = 0;

		// Token: 0x04000023 RID: 35
		private static int pickNthTicket = 1;

		// Token: 0x04000024 RID: 36
		private static int pickNthTicketFromNumber = 0;

		// Token: 0x04000025 RID: 37
		private static double waitTimeInHrsToDownloadStatusReport = 1.0;

		// Token: 0x04000026 RID: 38
		private static DateTime stDt;

		// Token: 0x04000027 RID: 39
		private static string prevTriedTicket = "";

		// Token: 0x04000028 RID: 40
		private static int clickNextWaitForEscalateCntr = 0;

		// Token: 0x04000029 RID: 41
		private static int SMTicketNumberWaitCntr = 0;
	}
}
