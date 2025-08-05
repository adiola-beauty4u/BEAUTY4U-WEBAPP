namespace Beauty4u.Models.Common
{
    public class NameValue<T> : INameValue<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }
    }
}
