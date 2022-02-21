
namespace Automation_Monitoring_MyOCP_1.Services.Interfaces
{
    public interface IAppPoolServices
    {
        #region Methods        
        bool AppPoolIsRunning(string appPoolName);
        #endregion
    }
}
