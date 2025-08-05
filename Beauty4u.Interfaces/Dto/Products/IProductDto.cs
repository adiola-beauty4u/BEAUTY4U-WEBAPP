namespace Beauty4u.Interfaces.Dto
{
    public interface IProductDto
    {
        string Brand { get; set; }
        string Color { get; set; }
        decimal Cost { get; set; }
        string ItmGroup { get; set; }
        decimal Retail { get; set; }
        string Size { get; set; }
        string StyleCode { get; set; }
        string StyleDesc { get; set; }
        string UPC { get; set; }
    }
}