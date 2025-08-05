namespace Beauty4u.Interfaces.Dto.Products
{
    public interface IUPCValidateResult
    {
        bool ExistsInB4u { get; set; }
        bool ExistsInMIS { get; set; }
        string UPC { get; set; }
    }
}