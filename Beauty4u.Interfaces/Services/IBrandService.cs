namespace Beauty4u.Interfaces.Services
{
    public interface IBrandService
    {
        Task<List<string>> GetBrandsAsync();
    }
}