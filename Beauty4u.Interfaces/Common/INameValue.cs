namespace Beauty4u.Models.Common
{
    public interface INameValue<T>
    {
        string Name { get; set; }
        T Value { get; set; }
    }
}