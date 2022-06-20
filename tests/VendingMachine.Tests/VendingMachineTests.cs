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
}