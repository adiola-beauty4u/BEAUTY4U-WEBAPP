using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Dto
{
    public class VendorDto : IVendorDto
    {
        public int VendorId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public int? BrandId { get; set; }

        public string? Address { get; set; }

        public string? Address2 { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zip { get; set; }

        public string? Phone { get; set; }

        public string? Fax { get; set; }

        public string? Taxein { get; set; }

        public string? ContactPerson { get; set; }

        public string? ContactPhone { get; set; }

        public string? ContactEmail { get; set; }

        public string? Description { get; set; }

        public bool Status { get; set; }

        public DateTime DateCreated { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? DateModified { get; set; }

        public int? ModifiedBy { get; set; }
        public string? BrandName { get; set; }
    }
}
