namespace Bz.Fott.Telemetry;

internal record LapTimeEvent(
    Guid Id,
    Guid CheckPointId,
    Guid CompetitorId,
    DateTime Timestamp);
