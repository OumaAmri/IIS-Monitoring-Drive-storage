using Automation_Monitoring_MyOCP_1.Services.Interfaces;
using Serilog;
using System;
using System.IO;
using System.Linq;

namespace Automation_Monitoring_MyOCP_1.Services
{
    public class CheckStorageDisk : ICheckStorageDisk
    {
        public string GetSize(string driveName, long minGB)
        {
            try
            {
                DriveInfo Drive = DriveInfo.GetDrives().Where(x => x.Name == driveName && x.IsReady).FirstOrDefault();
                if (Drive != null)
                {
                    try
                    {
                        long TotalSize = Drive.TotalSize / (1024 * 1024 * 1024);//En GB
                        long TotalFreeSpace = Drive.TotalFreeSpace / (1024 * 1024 * 1024);
                        if (TotalFreeSpace >= minGB)
                        {
                            Log.Verbose("{0} GB free of {1} GB for {2} drive", TotalFreeSpace, TotalSize, driveName);
                            return (TotalFreeSpace + " GB free of " + TotalSize + " GB for " + driveName + " drive");
                        }
                        else
                        {
                            Log.Warning("{0} GB free of {1} GB for {2} drive !", TotalFreeSpace, TotalSize, driveName);
                            return "-1";
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Somthing went wrong while calculating the drive storage !");
                        Log.Verbose("Exception : " + ex.Message + ", " + ex.InnerException);
                        return "";
                    }
                }
                else
                {
                    Log.Warning("The {0} drive not found !", driveName);
                    return "";
                }
            }
            catch (Exception ex)
            {
                Log.Error("Something went wrong --> CheckStorageDisk class : " + ex.Message + "\nInnerException : " + ex.InnerException);
                return "";
            }

        }
    }
}
