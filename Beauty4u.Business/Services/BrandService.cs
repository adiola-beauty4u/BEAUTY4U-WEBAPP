using Beauty4u.Interfaces.DataAccess.B4u;
using Beauty4u.Interfaces.Services;

namespace Beauty4u.Business.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<List<string>> GetBrandsAsync()
        {
            return await _brandRepository.GetBrandsAsync();
        }
    }
}
