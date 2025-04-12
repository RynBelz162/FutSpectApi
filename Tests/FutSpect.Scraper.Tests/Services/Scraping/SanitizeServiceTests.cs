using FutSpect.Scraper.Services.Scraping;
using Shouldly;

namespace FutSpect.Scraper.Tests.Services.Scraping;

public class SanitizeServiceTests
{
    private readonly SanitizeService _sanitizeService;

    public SanitizeServiceTests()
    {
        _sanitizeService = new();
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("   ", "")]
    public void Sanitize_WhenNullOrEmpty_ReturnsEmptyString(string? input, string expected)
    {
        var result = _sanitizeService.Sanitize(input!);
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData("hello\nworld", "hello world")]
    [InlineData("hello\r\nworld", "hello world")]
    [InlineData("hello\n\rworld", "hello world")]
    [InlineData("hello\n\n\nworld", "hello world")]
    public void Sanitize_WhenContainsNewLines_RemovesThem(string input, string expected)
    {
        var result = _sanitizeService.Sanitize(input);
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData("hello   world", "hello world")]
    [InlineData("   hello   world   ", "hello world")]
    [InlineData("hello\t\tworld", "hello world")]
    [InlineData("hello    \t    world", "hello world")]
    public void Sanitize_WhenContainsExtraWhitespace_CollapsesIt(string input, string expected)
    {
        var result = _sanitizeService.Sanitize(input);
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData("hello\n  world  \r\n  test", "hello world test")]
    [InlineData("\n\r  hello  \t world\n\r", "hello world")]
    [InlineData("  hello  \n\r  world  ", "hello world")]
    public void Sanitize_WhenContainsMixedWhitespaceAndNewlines_CleansAll(string input, string expected)
    {
        var result = _sanitizeService.Sanitize(input);
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData("Hello, World!", "Hello, World!")]
    [InlineData("Hello,   World!", "Hello, World!")]
    [InlineData("Hello,\nWorld!", "Hello, World!")]
    [InlineData("Hello...   World!", "Hello... World!")]
    [InlineData("Questions? Answers.", "Questions? Answers.")]
    [InlineData("First; second, third.", "First; second, third.")]
    [InlineData("(Hello)\n[World]", "(Hello) [World]")]
    [InlineData("Oh,\n  what a day!!!", "Oh, what a day!!!")]
    public void Sanitize_WhenContainsPunctuation_PreservesIt(string input, string expected)
    {
        var result = _sanitizeService.Sanitize(input);
        result.ShouldBe(expected);
    }
}
