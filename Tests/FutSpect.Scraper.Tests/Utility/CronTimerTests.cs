using FutSpect.Scraper.Utility;
using Microsoft.Extensions.Time.Testing;
using Shouldly;

namespace FutSpect.Scraper.Tests.Utility;

public class CronTimerTests
{
    private readonly FakeTimeProvider _timeProvider = new();

    [Fact]
    public async Task WaitForNextTickAsync_ShouldReturnTrue_WhenNextOccurrenceIsInFuture()
    {
        // Set the time provider to just before the next occurrence
        _timeProvider.SetUtcNow(new DateTimeOffset(2050, 1, 31, 23, 59, 59, TimeSpan.Zero));

        var timer = new CronTimer("@monthly", _timeProvider);

        var result = await timer.WaitForNextTickAsync(CancellationToken.None);

        result.ShouldBeTrue();
    }
}