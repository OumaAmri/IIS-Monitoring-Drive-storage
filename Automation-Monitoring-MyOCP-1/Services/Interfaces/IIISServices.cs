
namespace Automation_Monitoring_MyOCP_1.Services.Interfaces
{
    public interface IIISServices
    {
        #region Methods
        bool IISIsRunning(string W3SVC);
        #endregion
    }
}
