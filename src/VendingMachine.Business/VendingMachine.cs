namespace Business;

public record Product {
  public string Name;
  public decimal Price;
}

public class VendingMachine
{
  private Dictionary<VendindMachineState, string> stateMessages = new Dictionary<VendindMachineState, string> {
    { VendindMachineState.Ready, "INSERT COIN" },
    { VendindMachineState.ProductDispensed, "THANK YOU" },
    { VendindMachineState.IncorrectMoney, "" }
  };
  private List<Product> products = new List<Product>() {
    new Product { Name = "Cola", Price = 1M },
    new Product { Name = "Crisps", Price = 0.5M },
    new Product { Name = "Chocolate", Price = 0.65M },
  };
  private List<Coins> coinReturn = new List<Coins>();
  private decimal totalCoinsInserted = 0;
  private VendindMachineState currentState = VendindMachineState.Ready;

  public void InsertCoin(Coins coin) {
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

  public string ReadDisplay() {
    string message = totalCoinsInserted == 0 
      ? this.stateMessages[this.currentState]
      : formatCurrency(totalCoinsInserted);

    if(this.currentState == VendindMachineState.ProductDispensed 
      || this.currentState == VendindMachineState.IncorrectMoney)
      this.currentState = VendindMachineState.Ready;
    
    return message;
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
    
    var productVended = this.totalCoinsInserted == foundProduct.Price;
    if(productVended) {
      this.currentState = VendindMachineState.ProductDispensed;
      this.totalCoinsInserted -= foundProduct.Price;
    }
    else {
      this.currentState = VendindMachineState.IncorrectMoney;
      this.stateMessages[this.currentState] = formatCurrency(foundProduct.Price);
    }

    return productVended;
  }

  private string formatCurrency(decimal value) {
    return string.Format("{0:C}", value);
  }
}
