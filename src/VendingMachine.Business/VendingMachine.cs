namespace Business;

public record Product
{
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
	private Dictionary<Coins, decimal> coinValues = new Dictionary<Coins, decimal> {
		{ Coins.FivePence, 0.05M },
		{ Coins.TenPence, 0.1M },
		{ Coins.TwentyPence, 0.2M },
		{ Coins.FiftyPence, 0.5M },
		{ Coins.OnePound, 1M },
		{ Coins.TwoPound, 2M },
	};
	private List<Coins> coinReturn = new List<Coins>();
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
		string message = totalCoinsInserted == 0 || this.currentState == VendindMachineState.IncorrectMoney
			? this.stateMessages[this.currentState]
			: formatCurrency(totalCoinsInserted);

		if (this.currentState == VendindMachineState.ProductDispensed
			|| this.currentState == VendindMachineState.IncorrectMoney)
			this.currentState = VendindMachineState.Ready;

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
		var foundProduct = this.products.Find(p => p.Name == productName);
		if (foundProduct == null)
			throw new ArgumentException("Product does not exist");

		var productVended = this.totalCoinsInserted >= foundProduct.Price;
		if (productVended)
		{
			this.currentState = VendindMachineState.ProductDispensed;
			this.totalCoinsInserted -= foundProduct.Price;
			this.DispenseChange();
		}
		else
		{
			this.currentState = VendindMachineState.IncorrectMoney;
			this.stateMessages[this.currentState] = formatCurrency(foundProduct.Price);
		}

		return productVended;
	}

	private string formatCurrency(decimal value)
	{
		return string.Format("{0:C}", value);
	}

	private void DispenseChange()
	{
		while (this.totalCoinsInserted > 0)
		{
			foreach(KeyValuePair<Coins, decimal> coinValue in this.coinValues) 
			{
				while(this.totalCoinsInserted >= coinValue.Value)
				{
					this.coinReturn.Add(coinValue.Key);
					this.totalCoinsInserted -= coinValue.Value;
				}
			}
		}
	}
}
