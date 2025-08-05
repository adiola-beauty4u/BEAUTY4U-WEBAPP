using Beauty4u.Models.Dto;

namespace Beauty4u.DataAccess.B4u
{
    public interface IVendorRepository
    {
        Task<List<IVendorDto>> GetVendorsAsync();
    }
}