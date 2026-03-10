namespace WebApi.Shared.Entities
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public Pagination Pagination { get; set; } = new Pagination();
    }
}