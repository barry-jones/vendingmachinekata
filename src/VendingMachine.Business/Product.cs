namespace Business;

/// <summary>
/// Describes the name and price of products available in the
/// <see cref="VendingMachine"/>
/// </summary>
internal record Product
{
    public string Name = "";
    public decimal Price;
}
