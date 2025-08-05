
namespace Beauty4u.Models.Dto
{
    public interface IVendorDto
    {
        string? Address { get; set; }
        string? Address2 { get; set; }
        int? BrandId { get; set; }
        string? BrandName { get; set; }
        string? City { get; set; }
        string Code { get; set; }
        string? ContactEmail { get; set; }
        string? ContactPerson { get; set; }
        string? ContactPhone { get; set; }
        int CreatedBy { get; set; }
        DateTime DateCreated { get; set; }
        DateTime? DateModified { get; set; }
        string? Description { get; set; }
        string? Fax { get; set; }
        int? ModifiedBy { get; set; }
        string Name { get; set; }
        string? Phone { get; set; }
        string? State { get; set; }
        bool Status { get; set; }
        string? Taxein { get; set; }
        string Type { get; set; }
        int VendorId { get; set; }
        string? Zip { get; set; }
    }
}