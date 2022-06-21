using Xunit;
using Business;

namespace Tests;

public class VendingMachineTests
{
    [Theory]
    [InlineData(Coins.FivePence, "£0.05")]
    [InlineData(Coins.TenPence, "£0.10")]
    [InlineData(Coins.TwentyPence, "£0.20")]
    [InlineData(Coins.FiftyPence, "£0.50")]
    [InlineData(Coins.OnePound, "£1.00")]
    [InlineData(Coins.TwoPound, "£2.00")]
    public void When_ValidCoinAdded_DisplayShowsCoinAmount(Coins coin, string expected)
    {
        var m = new VendingMachine();
        m.InsertCoin(coin);

        string result = m.ReadDisplay();

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(new Coins[] { Coins.TenPence, Coins.FiftyPence }, "£0.60")]
    [InlineData(new Coins[] { Coins.OnePound, Coins.FiftyPence }, "£1.50")]
    [InlineData(new Coins[] { Coins.TwoPound, Coins.TwentyPence }, "£2.20")]
    [InlineData(new Coins[] { Coins.TwoPound, Coins.TwentyPence, Coins.OnePound }, "£3.20")]
    public void When_MultipleCoinsAdded_DisplayShowsTotal(Coins[] coins, string expected) {
        var m = new VendingMachine();
        foreach(Coins current in coins)
            m.InsertCoin(current);

        string result = m.ReadDisplay();

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(Coins.OnePence, new Coins[] { Coins.OnePence })]
    [InlineData(Coins.TwoPence, new Coins[] { Coins.TwoPence })]
    public void When_InvalidCoinInserted_CoinReturned(Coins coin, Coins[] expected) {
        var m = new VendingMachine();
        m.InsertCoin(coin);

        Coins[] result = m.EmptyCoinReturn();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void When_InvalidCoinInserted_TotalNotUpdated() {
        const string EXPECTED = "£0.10";
        var m = new VendingMachine();
        m.InsertCoin(Coins.TenPence);
        m.InsertCoin(Coins.OnePence);

        string result = m.ReadDisplay();

        Assert.Equal(EXPECTED, result);
    }

    [Fact]
    public void When_NoCoinsInserted_DisplayInsertCoin() {
        const string EXPECTED = "INSERT COIN";
        var m = new VendingMachine();

        string result = m.ReadDisplay();

        Assert.Equal(EXPECTED, result);
    }

    [Theory]
    [InlineData(new Coins[] { Coins.FiftyPence }, "Crisps", true )]
    [InlineData(new Coins[] { Coins.OnePound }, "Cola", true )]
    public void When_ProductRequestedAndCorrectMoney_ProductIsDispensed(Coins[] coins, string product, bool expected) {
        var m = new VendingMachine();
        foreach (Coins current in coins)
            m.InsertCoin(current);

        bool result = m.DispenseProduct(product);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void When_ProductRequestedAndCorrectMoney_DisplayThankYouMessage() {
        const string EXPECTED = "THANK YOU";
        var m = new VendingMachine();
        m.InsertCoin(Coins.FiftyPence);
        m.DispenseProduct("Crisps");

        string result = m.ReadDisplay();

        Assert.Equal(EXPECTED, result);
    }

    [Fact]
    public void When_AfterThankYouMessage_DisplayDefaultMessage() {
        const string EXPECTED = "INSERT COIN";
        var m = new VendingMachine();
        m.InsertCoin(Coins.FiftyPence);
        m.DispenseProduct("Crisps");
        m.ReadDisplay();

        string result = m.ReadDisplay();

        Assert.Equal(EXPECTED, result);
    }

    [Fact]
    public void When_ProductRequestedAndNotEnoughMoney_DisplayProductPrice() {
        const string EXPECTED = "£1.00";
        var m = new VendingMachine();
        m.DispenseProduct("Cola");

        string result = m.ReadDisplay();

        Assert.Equal(EXPECTED, result);
    }

    [Fact]
    public void When_AfterIncorrectPriceMessageAndCoinsInserted_DisplayCurrentValue() {
        const string EXPECTED = "£0.10";
        var m = new VendingMachine();
        m.InsertCoin(Coins.TenPence);
        m.DispenseProduct("Cola");
        m.ReadDisplay();
		
        string result = m.ReadDisplay();

        Assert.Equal(EXPECTED, result);
    }

	[Fact]
    public void When_AfterIncorrectPriceMessageAndNoCoinsInserted_DisplayDefaultMessage() {
        const string EXPECTED = "INSERT COIN";
        var m = new VendingMachine();
        m.DispenseProduct("Cola");
		m.ReadDisplay();

        string result = m.ReadDisplay();

        Assert.Equal(EXPECTED, result);
    }

    [Theory]
    [InlineData(new Coins[] { Coins.OnePound }, 0.50)]
    [InlineData(new Coins[] { Coins.OnePound, Coins.TwentyPence }, 0.70)]
    [InlineData(new Coins[] { Coins.TwoPound, Coins.TenPence }, 1.60)]
    [InlineData(new Coins[] { Coins.TwoPound, Coins.FivePence }, 1.55)]
    [InlineData(new Coins[] { Coins.FiftyPence, Coins.FivePence }, .05)]
    public void When_TooMuchMoneyAdded_ChangeIsReturned(Coins[] insertedCoins, decimal totalChange) {
        var m = new VendingMachine();
		foreach(Coins current in insertedCoins)
        	m.InsertCoin(current);
        m.DispenseProduct("Crisps");

        Coins[] change = m.EmptyCoinReturn();

        decimal result = this.CalculateTotalCoins(change);
        Assert.Equal(totalChange, result);
    }

    private decimal CalculateTotalCoins(Coins[] coins) {
		decimal total = 0;

		foreach(Coins current in coins)
		{
			total += current switch
			{
				Coins.FivePence => 0.05M,
				Coins.TenPence => 0.10M,
				Coins.TwentyPence => 0.20M,
				Coins.FiftyPence => 0.50M,
				Coins.OnePound => 1.0M,
				Coins.TwoPound => 2.00M,
				_ => 0M
			};
		}

		return total;
    }
}