namespace Beauty4u.Interfaces.Dto.Products
{
    public interface IProductFullDto: IProductDto
    {
        string Closed { get; set; }
        bool Inventory { get; set; }
        DateTime LastUpdate { get; set; }
        string LastUser { get; set; }
        bool Status { get; set; }
        string TaxType { get; set; }
        DateTime WriteDate { get; set; }
        string WriteUser { get; set; }
    }
}