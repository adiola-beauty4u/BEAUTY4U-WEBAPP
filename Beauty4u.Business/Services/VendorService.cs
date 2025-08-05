using Beauty4u.DataAccess.B4u;
using Beauty4u.Models.Common;
using Beauty4u.Models.DataAccess.B4u;
using Beauty4u.Models.Dto;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Beauty4u.Interfaces.Services;

namespace Beauty4u.Business.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _vendorRepository;
        public VendorService(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        public async Task<List<IVendorDto>> GetVendorsAsync()
        {
            return await _vendorRepository.GetVendorsAsync();
        }
    }
}
