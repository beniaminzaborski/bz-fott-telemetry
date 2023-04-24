using MassTransit;
using System;

namespace Bz.Fott.Telemetry.IntegrationEvents;

[EntityName("competitor-time-calculated")]
public sealed record CompetitorTimeCalculatedIntegrationEvent(
    Guid CompetitorId,
    TimeSpan NetTime)
{ }
