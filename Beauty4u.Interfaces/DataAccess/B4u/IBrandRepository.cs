namespace Beauty4u.Interfaces.DataAccess.B4u
{
    public interface IBrandRepository
    {
        Task<List<string>> GetBrandsAsync();
    }
}