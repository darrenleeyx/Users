namespace Users.Application.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
