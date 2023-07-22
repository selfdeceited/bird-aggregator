using System;
using System.Collections.Generic;

namespace BirdAggregator.Application.Photos
{
    public record PhotoDto
    (
        string Src,
        string Caption,
        string Id,
        DateTime DateTaken,
        int Height,
        double Width,
        string Original,
        string Text,
        IEnumerable<string> BirdIds,
        string HostingLink
    );
}