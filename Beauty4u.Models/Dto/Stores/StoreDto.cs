using Beauty4u.Interfaces.Dto.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Dto.Stores
{
    public class StoreDto : IStoreDto
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string StoreAbbr { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string RemoteIp { get; set; }
        public string Remote2ndIp { get; set; }
        public string Db { get; set; }
        public string Id { get; set; }
        public string Pwd { get; set; }
        public string Port { get; set; }
        public bool Status { get; set; }
        public int Orders { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteUser { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUser { get; set; }
        public string PayrollCompanyCode { get; set; }
        public string ApiUrl { get; set; }
    }
}
