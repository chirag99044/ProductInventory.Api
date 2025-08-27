namespace ProductInventory.Api.Dtos
{
    public record ProductDto
    (
        int Id,
        string Name,
        decimal Price,
        int Quantity,
        int CategoryId,
        string CategoryName,
        DateTime Created
    );
}
