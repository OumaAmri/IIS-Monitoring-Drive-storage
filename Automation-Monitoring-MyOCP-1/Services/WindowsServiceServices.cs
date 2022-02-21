using Automation_Monitoring_MyOCP_1.Services.Interfaces;
using Serilog;
using System;
using System.ServiceProcess;

namespace Automation_Monitoring_MyOCP_1.Services
{
    public class WindowsServiceServices : IWindowsServiceServices
    {
        public bool IsWindowsServiceRunning(string winServiceName)
        {
            try
            {
                ServiceController service = new ServiceController(winServiceName, Environment.MachineName);
                if (service != null)
                {
                    Log.Verbose("Check if the windows service : {0} is running ?", winServiceName);
                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        Log.Information("Verification completed ! The windows service is {0}...", "running");
                        return true;
                    }
                    else
                    {
                        Log.Warning("The windows service is not running !", winServiceName);
                        try
                        {
                            Log.Verbose("Try to start the windows service...", winServiceName);
                            service.Start();
                            Log.Information("The windows service {0} !", "start successfully");
                            return true;
                        }

                        catch (Exception ex)
                        {
                            Log.Error("Error while starting {0} windows service !", winServiceName);
                            Log.Verbose("Exception : " + ex.Message + ". " + ex.InnerException);
                            return false;
                        }
                    }
                }
                else
                {
                    Log.Warning("The {0} windows service is not found !", winServiceName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Something went wrong --> WindowsServiceServices class : " + ex.Message + "\nInnerException : " + ex.InnerException);
                return false;
            }
        }
    }
}
