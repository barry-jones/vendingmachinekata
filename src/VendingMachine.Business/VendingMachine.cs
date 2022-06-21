namespace Business;

/// <summary>
/// A vending machine that can accept coins, dispense products, and give chamge.
/// </summary>
public class VendingMachine
{
	private readonly Dictionary<VendindMachineState, string> stateMessages = new()
	{
		{ VendindMachineState.Ready, "INSERT COIN" },
		{ VendindMachineState.ProductDispensed, "THANK YOU" },
		{ VendindMachineState.IncorrectMoney, "" }
	};
	private readonly List<Product> products = new()
	{
		new Product { Name = "Cola", Price = 1M },
		new Product { Name = "Crisps", Price = 0.5M },
		new Product { Name = "Chocolate", Price = 0.65M },
	};
	private readonly Dictionary<Coins, decimal> coinValues = new()
	{
		{ Coins.FivePence, 0.05M },
		{ Coins.TenPence, 0.1M },
		{ Coins.TwentyPence, 0.2M },
		{ Coins.FiftyPence, 0.5M },
		{ Coins.OnePound, 1M },
		{ Coins.TwoPound, 2M },
	};
	private readonly List<Coins> coinReturn = new List<Coins>();
	private decimal totalCoinsInserted = 0;
	private VendindMachineState currentState = VendindMachineState.Ready;

	public void InsertCoin(Coins coin)
	{
		if (coin == Coins.OnePence || coin == Coins.TwoPence)
			this.coinReturn.Add(coin);
		else
		{
			this.totalCoinsInserted += this.coinValues[coin];
		}
	}

	public string ReadDisplay()
	{
		// incorrect money state shows price then ready state
		string message = totalCoinsInserted == 0 || this.currentState == VendindMachineState.IncorrectMoney
			? this.stateMessages[this.currentState]
			: FormatCurrency(totalCoinsInserted);

		// after interim messages displayed revert back to default message
		if (
			this.currentState == VendindMachineState.ProductDispensed ||
			this.currentState == VendindMachineState.IncorrectMoney
			)
		{
			this.currentState = VendindMachineState.Ready;
		}

		return message;
	}

	public Coins[] EmptyCoinReturn()
	{
		Coins[] allCoins = this.coinReturn.ToArray();
		this.coinReturn.Clear();
		return allCoins;
	}

	public bool DispenseProduct(string productName)
	{
		Product? foundProduct = this.products.Find(p => p.Name == productName);
		if (foundProduct == null)
			throw new ArgumentException("Product does not exist");

		var canVendProduct = this.totalCoinsInserted >= foundProduct.Price;
		if (canVendProduct)
		{
			this.currentState = VendindMachineState.ProductDispensed;
			this.totalCoinsInserted -= foundProduct.Price;
			this.DispenseChange();
		}
		else
		{
			this.currentState = VendindMachineState.IncorrectMoney;
			this.stateMessages[this.currentState] = $"PRICE {this.FormatCurrency(foundProduct.Price)}";
		}

		return canVendProduct;
	}

	private string FormatCurrency(decimal value)
	{
		return string.Format("{0:C}", value);
	}

	private void DispenseChange()
	{
		// dispense large value coins first, will always reach zero unless change
		// required cannot be built from coins available
		foreach (KeyValuePair<Coins, decimal> coinValue in this.coinValues)
		{
			while (this.totalCoinsInserted >= coinValue.Value)
			{
				this.coinReturn.Add(coinValue.Key);
				this.totalCoinsInserted -= coinValue.Value;
			}
		}
	}
}