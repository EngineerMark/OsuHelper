using System;

namespace OsuApiHelper
{
    /// <summary>
    /// Gamemode enum
    /// </summary>
    [Flags]
    public enum OsuMode
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Standard = 0,
        Taiko = 1,
        Catch = 2,
        Mania = 3
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
