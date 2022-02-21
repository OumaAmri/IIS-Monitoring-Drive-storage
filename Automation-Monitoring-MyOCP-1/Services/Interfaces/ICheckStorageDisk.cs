
namespace Automation_Monitoring_MyOCP_1.Services.Interfaces
{
    public interface ICheckStorageDisk
    {
        #region Methods 
        string GetSize(string driveName, long minGB);
        #endregion
    }
}
