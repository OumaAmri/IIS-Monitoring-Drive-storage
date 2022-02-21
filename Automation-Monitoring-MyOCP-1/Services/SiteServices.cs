using Automation_Monitoring_MyOCP_1.Services.Interfaces;
using Microsoft.Web.Administration;
using Serilog;
using System;
using System.Linq;

namespace Automation_Monitoring_MyOCP_1.Services
{
    public class SiteServices : ISiteServices
    {
        #region Readonlys

        private readonly ServerManager _server;

        #endregion

        #region Constructor

        public SiteServices() => _server = new ServerManager();

        #endregion

        #region Methods  
        public bool SiteIsRunning(string siteName)
        {
            try
            {
                Site site = _server.Sites.FirstOrDefault(s => s.Name.Equals(siteName, StringComparison.OrdinalIgnoreCase));
                if (site != null)
                {
                    Log.Verbose("Check if the site : {0} is running ?", siteName);
                    if (site.State == ObjectState.Started)
                    {
                        Log.Information("Verification completed ! The site is {0}...", "running");
                        return true;
                    }
                    else
                    {
                        Log.Warning("The site is not running !", siteName);
                        try
                        {
                            Log.Verbose("Try to start the site ...", siteName);
                            site.Start();
                            Log.Information("The site {0} !", "start successfully");
                            return true;
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Error while starting {0} site !", siteName);
                            Log.Verbose("Exception : " + ex.Message + ". " + ex.InnerException);
                            return false;
                        }
                    }
                }
                else
                {
                    Log.Warning("The {0} site is not found !", siteName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Something went wrong --> SiteService class : " + ex.Message + "\nInnerException : " + ex.InnerException);
                return false;
            }
        }

        #region Implements IDisposable

        #region Private Dispose Fields

        private bool _disposed;

        #endregion

        /// <summary>
        /// Cleans up any resources being used.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // Take this object off the finalization queue to prevent 
            // finalization code for this object from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Cleans up any resources being used.
        /// </summary>
        /// <param name="disposing">Whether or not we are disposing</param>
        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    _server?.Dispose();
                }

                // Dispose any unmanaged resources here...

                _disposed = true;
            }
        }

        #endregion

        #endregion
    }
}
