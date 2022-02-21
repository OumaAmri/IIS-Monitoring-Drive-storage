using Automation_Monitoring_MyOCP_1.Services.Interfaces;
using Microsoft.Web.Administration;
using Serilog;
using System;
using System.Linq;

namespace Automation_Monitoring_MyOCP_1.Services
{
    public class AppPoolServices : IAppPoolServices
    {
        #region Readonlys

        private readonly ServerManager _server;

        #endregion

        #region Constructor        
        public AppPoolServices() => _server = new ServerManager();

        #endregion

        #region Methods
        public bool AppPoolIsRunning(string appPoolName)
        {
            try
            {
                ApplicationPoolCollection applicationPoolCollection = _server.ApplicationPools;
                ApplicationPool applicationPool = applicationPoolCollection.Where(x => x.Name == appPoolName).FirstOrDefault();
                if (applicationPool != null)
                {
                    Log.Verbose("Check if the application pool : {0} is running ?", appPoolName);
                    if (applicationPool.State == ObjectState.Started)
                    {
                        Log.Information("Verification completed ! The application pool is {0}...", "running");
                        return true;
                    }
                    else
                    {
                        Log.Warning("The application pool is not running !", appPoolName);
                        try
                        {
                            Log.Verbose("Try to start the application pool...", appPoolName);
                            applicationPool.Start();
                            Log.Information("The application pool {0} !", "start successfully");
                            return true;
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Error while starting {0} application pool !", appPoolName);
                            Log.Verbose("Exception : " + ex.Message + ". " + ex.InnerException);
                            return false;
                        }
                    }
                }
                else
                {
                    Log.Warning("The {0} application pool is not found !", appPoolName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Something went wrong --> AppPoolServices class : " + ex.Message + "\nInnerException : " + ex.InnerException);
                return false;
            }
        }
        #endregion
    }
}
