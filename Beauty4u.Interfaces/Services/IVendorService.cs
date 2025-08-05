using Beauty4u.Models.Dto;

namespace Beauty4u.Interfaces.Services
{
    public interface IVendorService
    {
        Task<List<IVendorDto>> GetVendorsAsync();
    }
}