using System;

namespace BirdAggregator.Application.LifeList.GetLifeListQuery;

public record Occurrence
(
    string BirdId,
    string Name,
    DateTime DateMet,
    string Location,
    string PhotoId
);
