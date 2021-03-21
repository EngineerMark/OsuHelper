using System;

namespace OsuApiHelper
{
    /// <summary>
    /// Enum list with possible beatmap statuses
    /// </summary>
    public enum BeatmapStatus
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Graveyarded = -2,
        Unranked = -1,
        Pending = 0,
        Ranked = 1,
        Approved = 2,
        Qualified = 3,
        Loved = 4
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
