using Beauty4u.Models.Common;

namespace Beauty4u.Interfaces.DataAccess.B4u
{
    public interface IConnectionRepository
    {
        Task<INameValue<bool>> CheckConnection(string storeCode, string serverName);
    }
}