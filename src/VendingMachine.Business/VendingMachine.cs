namespace Business;

public class VendingMachine
{
  private decimal totalCoinsInserted = 0;

  public void AddCoin(Coins coin) {
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

  public string GetDisplay() {
    return string.Format("{0:C}", totalCoinsInserted);
  }
}
