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
        m.AddCoin(coin);

        string result = m.GetDisplay();

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
            m.AddCoin(current);

        string result = m.GetDisplay();

        Assert.Equal(expected, result);
    }
}