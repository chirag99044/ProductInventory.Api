namespace ProductInventory.Api.Dtos
{
    public class PagedResult<T>
    {
        public required IReadOnlyList<T> Items { get; init; }
        public int TotalCount { get; init; }
    }
}
