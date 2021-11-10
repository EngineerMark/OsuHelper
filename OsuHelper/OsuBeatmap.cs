using System;
using Newtonsoft.Json;

namespace OsuApiHelper
{
    /// <summary>
    /// Class containing beatmap information
    /// </summary>
    public class OsuBeatmap
    {
        /// <summary>
        /// Gamemode for this beatmap
        /// </summary>
        public OsuMode Mode { get; set; }
        
        /// <summary>
        /// Supplied mods for conversions
        /// </summary>
        public OsuMods Mods { get; set; }
        
        /// <summary>
        /// In case the beatmap is a Convert, this value contains the original gamemode
        /// </summary>
        [JsonProperty("mode")]
        public OsuMode OriginalMode { get; set; }
        
        /// <summary>
        /// ID of the set this map is part of
        /// </summary>
        [JsonProperty("beatmapset_id")]
        public string BeatmapSetID { get; set; }
        
        /// <summary>
        /// ID of this beatmap
        /// </summary>
        [JsonProperty("beatmap_id")]
        public string BeatmapID { get; set; }
        
        /// <summary>
        /// Beatmap status influencing scoring and performance
        /// </summary>
        [JsonProperty("approved")]
        public BeatmapStatus Status { get; set; }
        
        /// <summary>
        /// Title of the beatmap song
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        
        /// <summary>
        /// Artist of the beatmap song
        /// </summary>
        [JsonProperty("artist")]
        public string Artist { get; set; }
        
        /// <summary>
        /// Difficulty name of the beatmap
        /// </summary>
        [JsonProperty("version")]
        public string DifficultyName { get; set; }

        /// <summary>
        /// Aim star rating
        /// </summary>
        [JsonProperty("diff_aim")] public double? StarratingAim { get; set; } = 0;

        /// <summary>
        /// Speed star rating
        /// </summary>
        [JsonProperty("diff_speed")] public double? StarratingSpeed { get; set; } = 0;
        
        /// <summary>
        /// Beatmap star rating
        /// </summary>
        [JsonProperty("difficultyrating")]
        public double? Starrating { get; set; }

        /// <summary>
        /// Highest combo achievable
        /// </summary>
        [JsonProperty("max_combo")] public double? MaxCombo { get; set; } = 0;

        /// <summary>
        /// Incase of Mania, it returns 1500
        /// </summary>
        public double TryMaxCombo { get{
                return MaxCombo ?? 1500;
            } 
        }
        
        /// <summary>
        /// Base circle size (CS)
        /// </summary>
        [JsonProperty("diff_size")]
        public double? CircleSize { get; set; }

        /// <summary>
        /// Base overall difficulty (OD)
        /// </summary>
        [JsonProperty("diff_overall")]
        public double? OverallDifficulty { get; set; }

        /// <summary>
        /// Base approach rate (AR)
        /// </summary>
        [JsonProperty("diff_approach")]
        public double? ApproachRate { get; set; }

        /// <summary>
        /// Base health (HP)
        /// </summary>
        [JsonProperty("diff_drain")]
        public double? Drain { get; set; }
        
        /// <summary>
        /// User who made this beatmap
        /// </summary>
        [JsonProperty("creator")]
        public string Mapper { get; set; }

        /// <summary>
        /// ID of user who made this beatmap
        /// </summary>
        [JsonProperty("creator_id")]
        public string MapperID { get; set; }

        /// <summary>
        /// Amount of circles in the map
        /// </summary>
        [JsonProperty("count_normal")]
        public double? CircleCount { get; set; }

        /// <summary>
        /// Amount of sliders in the map
        /// </summary>
        [JsonProperty("count_slider")]
        public double? SliderCount { get; set; }

        /// <summary>
        /// Amount of spinners in the map
        /// </summary>
        [JsonProperty("count_spinner")]
        public double? SpinnerCount { get; set; }

        /// <summary>
        /// Song length in seconds
        /// </summary>
        [JsonProperty("total_length")]
        public double? MapLength { get; set; }

        /// <summary>
        /// Converted song length in seconds
        /// </summary>
        public double GetLength()
        {
            return (((Mods & OsuMods.DoubleTime) != 0 || (Mods & OsuMods.Nightcore) != 0))?MapLength??0*1.5:MapLength??0;
        }

        /// <summary>
        /// Length of playable map in seconds
        /// </summary>
        [JsonProperty("hit_length")]
        public double MapDrainLength { get; set; }

        /// <summary>
        /// Converted length of playable map in seconds
        /// </summary>
        public double GetDrainLength()
        {
            return (((Mods & OsuMods.DoubleTime) != 0 || (Mods & OsuMods.Nightcore) != 0))?MapDrainLength*1.5:MapDrainLength;
        }

        /// <summary>
        /// Total amount of hit objects in the map
        /// </summary>
        public double ObjectCount => (CircleCount??0) + (SliderCount??0) + (SpinnerCount??0);

        /// <summary>
        /// Difficulty data for conversions and calculations
        /// </summary>
        public MapStats MapStats { get; set; }
    }
}