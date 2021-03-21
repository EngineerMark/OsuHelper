using System;
using System.Collections.Generic;
using OsuApiHelper.Math;

namespace OsuApiHelper
{
    /// <summary>
    /// Beatmap difficulty data
    /// </summary>
    public class MapStats
    {
        private const float OD_0_MS = 79.5f;
        private const float OD_10_MS = 19.5f;
        private const float AR_0_MS = 1800f;
        private const float AR_5_MS = 1200f;
        private const float AR_10_MS = 450f;
        private const float OD_MS_STEP = 6f;
        private const float AR_MS_STEP1 = 120f;
        private const float AR_MS_STEP2 = 150f;


        /// <summary>
        /// Converted Beatmap Approach Rate
        /// </summary>
        public float AR;

        /// <summary>
        /// Converted Beatmap Overall Difficulty
        /// </summary>
        public float OD;

        /// <summary>
        /// Converted Beatmap Circle Size
        /// </summary>
        public float CS;

        /// <summary>
        /// Converted Beatmap Health
        /// </summary>
        public float HP;

        /// <summary>
        /// Converted Beatmap Speed
        /// </summary>
        public float Speed;

        /// <summary>
        /// Beatmap that owns this MapStats instance
        /// </summary>
        public OsuBeatmap Beatmap;

        /// <summary>
        /// Beatmap used mods in case of scores
        /// </summary>
        public OsuMods Mods;

        /// <summary>
        /// Create and convert beatmap map stats
        /// </summary>
        public MapStats(OsuBeatmap beatmap, OsuMods mods)
        {
            Beatmap = beatmap;
            Mods = mods;

            Calculate();
        }

        private void Calculate()
        {
            AR = Beatmap.ApproachRate??0;
            OD = Beatmap.OverallDifficulty??0;
            CS = Beatmap.CircleSize??0;
            HP = Beatmap.Drain??0;

            // HR / EZ multiplier
            if ((Mods & OsuMods.HardRock) != 0)
            {
                CS *= 1.3f;
                AR *= 1.4f;
                OD *= 1.4f;
                HP *= 1.4f;
            }else if ((Mods & OsuMods.Easy) != 0)
            {
                CS *= 0.5f;
                AR *= 0.5f;
                OD *= 0.5f;
                HP *= 0.5f;
            }

            float ODMS = OD_0_MS - Mathf.Ceil(OD_MS_STEP * OD);
            float ARMS = AR < 5 ? (AR_0_MS - AR_MS_STEP1 * AR) : (AR_5_MS - AR_MS_STEP2 * (AR - 5f));

            ODMS = Mathf.Min(OD_0_MS, Mathf.Max(OD_10_MS, ODMS));
            ARMS = Mathf.Min(AR_0_MS, Mathf.Max(AR_10_MS, ARMS));

            Speed = 1f;
            if ((Mods & OsuMods.DoubleTime) != 0)
                Speed *= 1.5f;
            else if ((Mods & OsuMods.HalfTime) != 0)
                Speed *= 0.75f;

            float invSpeed = 1 / Speed;

            ODMS *= invSpeed;
            ARMS *= invSpeed;

            OD = (OD_0_MS - ODMS) / OD_MS_STEP;
            AR = ARMS>AR_5_MS ? ((AR_0_MS - ARMS) / AR_MS_STEP1) : (5.0f + (AR_5_MS - ARMS) / AR_MS_STEP2);

            CS = Mathf.Max(0.0f, Mathf.Min(10.0f, CS));
        }
    }
}