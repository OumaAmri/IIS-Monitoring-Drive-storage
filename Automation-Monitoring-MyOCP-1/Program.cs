using Automation_Monitoring_MyOCP_1.Services;
using Automation_Monitoring_MyOCP_1.Services.Emails;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Automation_Monitoring_MyOCP_1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                #region Configure logging

                #region Variables
                var date = DateTime.Now;
                string desktopFilepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string logFilepath = desktopFilepath + "/Logs/Log " + date.Day + "-" + date.Month + "-" + date.Year + ".txt";
                var customThemeStyles =
                new Dictionary<ConsoleThemeStyle, SystemConsoleThemeStyle>  
                {
                   {
                            ConsoleThemeStyle.LevelWarning, new SystemConsoleThemeStyle
                            {
                                Foreground = ConsoleColor.Black,
                                Background = ConsoleColor.Yellow,
                            }
                        },
                   {
                            ConsoleThemeStyle.LevelError, new SystemConsoleThemeStyle
                            {
                                Foreground = ConsoleColor.Black,
                                Background = ConsoleColor.Red,
                            }
                        },
                   {
                            ConsoleThemeStyle.String, new SystemConsoleThemeStyle
                            {
                                Foreground = ConsoleColor.Cyan,

                            }
                        },
                   {
                            ConsoleThemeStyle.LevelInformation, new SystemConsoleThemeStyle
                            {
                                Foreground = ConsoleColor.Black,
                                Background = ConsoleColor.DarkGreen
                            }
                        },
                   {
                            ConsoleThemeStyle.LevelDebug, new SystemConsoleThemeStyle
                            {
                                Foreground = ConsoleColor.Black,
                                Background = ConsoleColor.Green
                            }
                        },
                   {
                            ConsoleThemeStyle.Number, new SystemConsoleThemeStyle
                            {
                                Foreground = ConsoleColor.Red,

                            }
                        },
                };
                var customTheme = new SystemConsoleTheme(customThemeStyles);
                #endregion

                Log.Logger = new LoggerConfiguration()
                   .MinimumLevel.Verbose()
                   .WriteTo.Console(theme: customTheme)
                   .WriteTo.File(logFilepath)
                   .CreateLogger();
                #endregion

                #region Log application starts... My OCP
                Log.Information("Application Starts...");
                Console.WriteLine();
                Console.WriteLine("  *   *  *   *      * * *     * * *   * * * ");
                Console.WriteLine("  * * *  *   *    *       *  *        *     *");
                Console.WriteLine("  *   *  *   *    *       *  *        *     *");
                Console.WriteLine("  *   *  * * *      * * *     * * *   * * * ");
                Console.WriteLine("             *                        *");
                Console.WriteLine("             *                        *");
                Console.WriteLine("         * *                          *");
                #endregion

                #region Variables
                string iis = ConfigurationManager.AppSettings["W3SVC"].ToString();
                string applicationPool = ConfigurationManager.AppSettings["appPool"].ToString();
                string site = ConfigurationManager.AppSettings["site"].ToString();
                string windowsService = ConfigurationManager.AppSettings["windowsService"].ToString();
                string drive = ConfigurationManager.AppSettings["drive"].ToString();
                long minGB = Convert.ToInt64(ConfigurationManager.AppSettings["minGBBeforeNotif"]);
                #endregion

                #region Initialize objects
                IISServices iisServices = new IISServices();
                AppPoolServices appPoolServices = new AppPoolServices();
                SiteServices siteServices = new SiteServices();
                WindowsServiceServices windowsServiceServices = new WindowsServiceServices();
                CheckStorageDisk checkStorageDisk = new CheckStorageDisk();
                EmailServices emailServices = new EmailServices();
                #endregion

                #region Processing...
                Log.Verbose("----------------------- IIS verification :----------------------  ");
                if (iisServices.IISIsRunning(iis))
                {
                    Log.Verbose("----------------------- Application Pool verification :----------------------  ");
                    if (appPoolServices.AppPoolIsRunning(applicationPool))
                    {
                        Log.Verbose("----------------------- Site verification :----------------------  ");
                        if (siteServices.SiteIsRunning(site))
                        {
                            Log.Verbose("----------------------- Windows Service verification :----------------------  ");
                            if (windowsServiceServices.IsWindowsServiceRunning(windowsService))
                            {
                                Log.Verbose("----------------------- Drive Storage part :----------------------  ");
                                string result = checkStorageDisk.GetSize(drive, minGB);
                                if (!String.IsNullOrEmpty(result) && result != "-1")
                                {
                                    Log.Information("Success !");

                                    #region Log END
                                    Console.WriteLine();
                                    Console.WriteLine("  * * *  *       *  * * *   ");
                                    Console.WriteLine("  *      * *     *  *     * ");
                                    Console.WriteLine("  * * *  *   *   *  *     * ");
                                    Console.WriteLine("  *      *     * *  *     * ");
                                    Console.WriteLine("  * * *  *       *  * * *  ");
                                    #endregion

                                    Log.Verbose("{0}","Sending OK Email to Middleware Team...");
                                    emailServices.SendEmail("MyOCP Monitoring : OK", "All is Ok for 10.21.159.45 server.");                 
                                }
                                else if (result == "-1")
                                {
                                    Log.Verbose("{0}", "Sending Email to Middleware Team...");
                                    emailServices.SendEmail("MyOCP Monitoring : disk storage", "Insufficient disk storage for 10.21.159.45 server.");
                                }
                                else
                                {
                                    Log.Verbose("Check disk storage please !");
                                    emailServices.SendEmail("MyOCP Monitoring : disk storage", "Check disk storage for 10.21.159.45 server please !");
                                }
                            }
                            else
                            {
                                Log.Verbose("Check the windows service please !");
                                emailServices.SendEmail("MyOCP Monitoring : windows service", "Check windows service for 10.21.159.45 server please !");
                            }
                        }
                        else
                        {
                            Log.Verbose("Check the site please !");
                            emailServices.SendEmail("MyOCP Monitoring : site", "Check site for 10.21.159.45 server please !");

                        }
                    }
                    else
                    {
                        Log.Verbose("Check the application pool please !");
                        emailServices.SendEmail("MyOCP Monitoring : application pool", "Check application pool for 10.21.159.45 server please !");
                    }
                }
                else
                {
                    Log.Verbose("Check IIS please !");
                    emailServices.SendEmail("MyOCP Monitoring : IIS", "Check IIS for 10.21.159.45 server please !");
                }
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error("Exception in the main program !");
                Log.Verbose("Exception : " + ex.Message + "\nInnerException : " + ex.InnerException);
                EmailServices emailServices = new EmailServices();
                emailServices.SendEmail("MyOCP Monitoring : PROBLEM", "The program failed for 10.21.159.45 server !");
            }

            #region Message program has completed                    
            Console.Write("Press any key to exit...");
            Console.ReadKey(true);
            #endregion
        }
    }
}
