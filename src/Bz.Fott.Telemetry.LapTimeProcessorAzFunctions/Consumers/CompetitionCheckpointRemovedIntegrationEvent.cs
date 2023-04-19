using System;

namespace Bz.Fott.Administration.Application.Competitions;

public sealed record CompetitionCheckpointRemovedIntegrationEvent(
    Guid CompetitionId,
    Guid CheckpointId,
    decimal TrackPointDistance,
    string TrackPointUnit)
{ }
