using Cronos;

namespace FutSpect.Scraper.Utility;

public class CronTimer
{
    private readonly CronExpression _cron;
    private readonly TimeProvider _timeProvider;

    public CronTimer(string cronExpression, TimeProvider timeProvider)
    {
        _cron = CronExpression.Parse(cronExpression);
        _timeProvider = timeProvider;
    }

    public async Task<bool> WaitForNextTickAsync(CancellationToken cancellationToken)
    {
        var now = _timeProvider.GetUtcNow().UtcDateTime;

        var nextOccurrence = _cron.GetNextOccurrence(now)
            ?? throw new InvalidOperationException("Invalid cron expression.");

        var nextInterval = DetermineNextInterval(now, nextOccurrence);
        var timer = new PeriodicTimer(nextInterval);

        return await timer.WaitForNextTickAsync(cancellationToken);
    }

    private static TimeSpan DetermineNextInterval(DateTime now, DateTime nextOccurrence)
    {
        var interval = nextOccurrence - now;

        if (interval < TimeSpan.Zero)
        {
            throw new InvalidOperationException("The next occurrence is in the past.");
        }

        return interval;
    }
}