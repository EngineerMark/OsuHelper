using System;
using Newtonsoft.Json;

namespace OsuApiHelper
{
    public class OsuBeatmap
    {
        public OsuMode Mode { get; set; }
        
        public OsuMods Mods { get; set; }
        
        [JsonProperty("mode")]
        public OsuMode OriginalMode { get; set; }
        
        [JsonProperty("beatmapset_id")]
        public string BeatmapSetID { get; set; }
        
        [JsonProperty("beatmap_id")]
        public string BeatmapID { get; set; }
        
        [JsonProperty("approved")]
        public BeatmapStatus Status { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("artist")]
        public string Artist { get; set; }
        
        [JsonProperty("version")]
        public string DifficultyName { get; set; }

        [JsonProperty("diff_aim")] public float? StarratingAim { get; set; } = 0;

        [JsonProperty("diff_speed")] public float? StarratingSpeed { get; set; } = 0;
        
        [JsonProperty("difficultyrating")]
        public float? Starrating { get; set; }

        [JsonProperty("max_combo")] public float? MaxCombo { get; set; } = 0;
        
        [JsonProperty("diff_size")]
        public float? CircleSize { get; set; }
        
        [JsonProperty("diff_overall")]
        public float? OverallDifficulty { get; set; }
        
        [JsonProperty("diff_approach")]
        public float? ApproachRate { get; set; }
        
        [JsonProperty("diff_drain")]
        public float? Drain { get; set; }
        
        [JsonProperty("creator")]
        public string Mapper { get; set; }
        
        [JsonProperty("creator_id")]
        public string MapperID { get; set; }
        
        [JsonProperty("count_normal")]
        public float? CircleCount { get; set; }
        
        [JsonProperty("count_slider")]
        public float? SliderCount { get; set; }
        
        [JsonProperty("count_spinner")]
        public float? SpinnerCount { get; set; }
        
        [JsonProperty("total_length")]
        public float? MapLength { get; set; }

        public float GetLength()
        {
            return (((Mods & OsuMods.DoubleTime) != 0 || (Mods & OsuMods.Nightcore) != 0))?MapLength??0*1.5f:MapLength??0;
        }
        
        [JsonProperty("hit_length")]
        public float MapDrainLength { get; set; }
        
        public float GetDrainLength()
        {
            return (((Mods & OsuMods.DoubleTime) != 0 || (Mods & OsuMods.Nightcore) != 0))?MapDrainLength*1.5f:MapDrainLength;
        }
        
        public float ObjectCount => CircleCount??0 + SliderCount??0 + SpinnerCount??0;
        
        public MapStats MapStats { get; set; }
    }
}