using System;
using Newtonsoft.Json;

namespace OsuApiHelper
{
    public class OsuPlay
    {
        public OsuMode Mode { get; set; }

        public enum ResultMode
        {
            Short,
            Expansive
        }

        [JsonProperty("beatmap_id")] public string MapID { get; set; }

        [JsonProperty("score_id")] public string ScoreID { get; set; }

        [JsonProperty("score")] public int Score { get; set; }

        [JsonProperty("rank")] public string Rank { get; set; }

        [JsonProperty("pp")] public float PP { get; set; } = -1;

        [JsonIgnore] private OsuPerformance _pp;

        public OsuPerformance Performance
        {
            get
            {
                if (_pp == null)
                    _pp = new OsuPerformance(this, Beatmap);
                return _pp;
            }
        }

        [JsonProperty("enabled_mods")] public OsuMods Mods { get; set; }

        [JsonProperty("maxcombo")] public float MaxCombo { get; set; } = 0;

        [JsonProperty("perfect")] public string IsFullcombo { get; set; }

        [JsonProperty("date")] public string DateAchieved { get; set; }

        [JsonProperty("count50")] public float C50 { get; set; }

        [JsonProperty("count100")] public float C100 { get; set; }

        [JsonProperty("count300")] public float C300 { get; set; }

        [JsonProperty("countkatu")] public float CKatu { get; set; }

        [JsonProperty("countgeki")] public float CGeki { get; set; }

        [JsonProperty("countmiss")] public float CMiss { get; set; }

        [JsonIgnore] public float Accuracy => OsuApi.CalculateAccuracy(Mode, CMiss, C50, C100, C300, CKatu, CGeki);

        [JsonIgnore] public OsuBeatmap Beatmap { get; set; }
    }
}