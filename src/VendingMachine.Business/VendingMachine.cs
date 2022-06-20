namespace Business;

public record Product {
  public string Name;
  public decimal Price;
}

public class VendingMachine
{
  private const string DEFAULT_DISPLAY = "INSERT COIN";
  private List<Product> products = new List<Product>() {
    new Product { Name = "Cola", Price = 1M },
    new Product { Name = "Crisps", Price = 0.5M },
    new Product { Name = "Chocolate", Price = 0.65M },
  };
  private List<Coins> coinReturn = new List<Coins>();
  private decimal totalCoinsInserted = 0;

  public void AddCoin(Coins coin) {
    if(coin == Coins.OnePence || coin == Coins.TwoPence)
      this.coinReturn.Add(coin);
    else {
      this.totalCoinsInserted += coin switch
      {
        Coins.FivePence => 0.05M,
        Coins.TenPence => 0.10M,
        Coins.TwentyPence => 0.20M,
        Coins.FiftyPence => 0.50M,
        Coins.OnePound => 1.0M,
        Coins.TwoPound => 2.00M
      };
    }
  }

  public string GetDisplay() {
    return totalCoinsInserted == 0 
      ? DEFAULT_DISPLAY 
      : string.Format("{0:C}", totalCoinsInserted);
  }

  public Coins[] EmptyCoinReturn() {
    Coins[] allCoins = this.coinReturn.ToArray();
    this.coinReturn.Clear();
    return allCoins;
  }

  public bool DispenseProduct(string productName) {
    var foundProduct = this.products.Find(p => p.Name == productName);
    if(foundProduct == null)
      throw new ArgumentException("Product does not exist");
    
    return (this.totalCoinsInserted == foundProduct.Price);
  }
}
