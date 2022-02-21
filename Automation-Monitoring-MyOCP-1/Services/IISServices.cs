using Automation_Monitoring_MyOCP_1.Services.Interfaces;
using Serilog;
using System;
using System.ServiceProcess;

namespace Automation_Monitoring_MyOCP_1.Services
{
    public class IISServices : IIISServices
    {
        public bool IISIsRunning(string W3SVC)
        {
            try
            {
                ServiceController serviceController = new ServiceController(W3SVC, Environment.MachineName); 
                if (serviceController != null)
                {
                    Log.Verbose("Check if {0} is running ?", "IIS");
                    if (serviceController.Status == ServiceControllerStatus.Running)
                    {
                        Log.Information("Verification completed ! IIS is {0}...", "running");
                        return true;
                    }
                    else
                    {
                        Log.Warning("IIS is not running...");
                        Log.Verbose("Trying to start IIS...");
                        try
                        {
                            serviceController.Start();
                            Log.Information("IIS {0} !", "start succesfully");
                            return true;
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Problem while starting W3SVC : " + ex.Message + " " + ex.InnerException);
                            return false;
                        }
                    }
                }
                else
                {
                    Log.Error("World Wide Web Publishing Service not found !");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Something went wrong --> IISServices class : " + ex.Message + "\nInnerException : " + ex.InnerException);
                return false;
            }
        }
    }
}
