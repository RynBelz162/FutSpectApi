using FutSpect.Scraper.Helpers;

namespace FutSpect.Scraper.Tests.Helpers;

public class ArgumentHelperTests
{
    [Theory]
    [InlineData(new string[] { "test", "yellow", "--run-now:class,league" }, "--run-now:", true, true)]
    [InlineData(new string[] { "--run-now:class" }, "--run-now:", true, false)]
    [InlineData(new string[] { "--run-now:league" }, "--run-now:", false, true)]
    [InlineData(new string[] { "scraper:" }, "--run-now:", false, false)]
    [InlineData(new string[] { "other:class,league" }, "--run-now:", false, false)]
    public void ParseArguments_ReturnsExpectedScraperArgs(string[] args, string name, bool expectedClass, bool expectedLeague)
    {
        var result = ArgumentHelper.ParseArguments(args, name);
        Assert.Equal(expectedClass, result.RunClubNow);
        Assert.Equal(expectedLeague, result.RunLeagueNow);
    }
}
