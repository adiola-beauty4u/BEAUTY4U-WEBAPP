namespace Beauty4u.Interfaces.Dto.Stores
{
    public interface IStoreDto
    {
        string Address { get; set; }
        string Address2 { get; set; }
        string ApiUrl { get; set; }
        string City { get; set; }
        string Code { get; set; }
        string Id { get; set; }
        DateTime LastUpdate { get; set; }
        string LastUser { get; set; }
        string Name { get; set; }
        int Orders { get; set; }
        string PayrollCompanyCode { get; set; }
        string Port { get; set; }
        string Pwd { get; set; }
        string Remote2ndIp { get; set; }
        string RemoteIp { get; set; }
        string State { get; set; }
        bool Status { get; set; }
        string StoreAbbr { get; set; }
        string Db { get; set; }
        string Type { get; set; }
        DateTime WriteDate { get; set; }
        string WriteUser { get; set; }
        string Zip { get; set; }
    }
}