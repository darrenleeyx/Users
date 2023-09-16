namespace Users.Application.Providers;

public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateTimeOffset UtcNow { get; }
}
