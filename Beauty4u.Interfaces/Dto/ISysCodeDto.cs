namespace Beauty4u.Interfaces.Dto
{
    public interface ISysCodeDto
    {
        string Class { get; set; }
        string Code { get; set; }
        string Description { get; set; }
        string Name { get; set; }
        int Orders { get; set; }
        bool Status { get; set; }
    }
}