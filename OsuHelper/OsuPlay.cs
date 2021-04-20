using System;
using Newtonsoft.Json;

namespace OsuApiHelper
{
    /// <summary>
    /// This contains information of a submitted score
    /// </summary>
    public class OsuPlay
    {
        /// <summary>
        /// Gamemode the play is made in
        /// </summary>
        public OsuMode Mode { get; set; }

        /// <summary>
        /// Beatmap ID (For API)
        /// </summary>
        [JsonProperty("beatmap_id")] public string MapID { get; set; }

        /// <summary>
        /// Score ID (For API)
        /// </summary>
        [JsonProperty("score_id")] public string ScoreID { get; set; }

        /// <summary>
        /// Score achieved in this play
        /// </summary>
        [JsonProperty("score")] public int Score { get; set; }

        /// <summary>
        /// The rank of the play (SS, S, A etc)
        /// </summary>
        [JsonProperty("rank")] public string Rank { get; set; }

        /// <summary>
        /// If applicable, PP value of this play provided by API
        /// </summary>
        [JsonProperty("pp")] public float PP { get; set; } = -1;

        [JsonIgnore] private OsuPerformance _pp;

        /// <summary>
        /// Performance object to calculate different PP values on this map
        /// </summary>
        public OsuPerformance Performance
        {
            get
            {
                if (Beatmap == null)
                    return null;

                if (_pp == null)
                    _pp = new OsuPerformance(this, Beatmap);
                return _pp;
            }
        }

        /// <summary>
        /// Mods used in this play
        /// </summary>
        [JsonProperty("enabled_mods")] public OsuMods Mods { get; set; }

        /// <summary>
        /// Highest combo reached in this play
        /// </summary>
        [JsonProperty("maxcombo")] public float MaxCombo { get; set; } = 0;

        /// <summary>
        /// Boolean value if play is a full combo
        /// </summary>
        [JsonProperty("perfect")] public string IsFullcombo { get; set; }

        /// <summary>
        /// Date and time of when the play occured
        /// </summary>
        [JsonProperty("date")] public string DateAchieved { get; set; }

        /// <summary>
        /// Amount of 50s
        /// </summary>
        [JsonProperty("count50")] public float C50 { get; set; }

        /// <summary>
        /// Amount of 100s
        /// </summary>
        [JsonProperty("count100")] public float C100 { get; set; }

        /// <summary>
        /// Amount of 300s
        /// </summary>
        [JsonProperty("count300")] public float C300 { get; set; }

        /// <summary>
        /// Amount of katu hits
        /// </summary>
        [JsonProperty("countkatu")] public float CKatu { get; set; }

        /// <summary>
        /// Amount of geki hits
        /// </summary>
        [JsonProperty("countgeki")] public float CGeki { get; set; }

        /// <summary>
        /// Amount of misses
        /// </summary>
        [JsonProperty("countmiss")] public float CMiss { get; set; }

        /// <summary>
        /// Calculated accuracy of this play
        /// </summary>
        [JsonIgnore] public float Accuracy => OsuApi.CalculateAccuracy(Mode, CMiss, C50, C100, C300, CKatu, CGeki);

        /// <summary>
        /// Beatmap on which this play was made
        /// </summary>
        [JsonIgnore] public OsuBeatmap Beatmap { get; set; }
    }
}