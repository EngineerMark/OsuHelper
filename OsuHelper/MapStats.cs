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
        private const double OD_0_MS = 79.5;
        private const double OD_10_MS = 19.5;
        private const double AR_0_MS = 1800;
        private const double AR_5_MS = 1200;
        private const double AR_10_MS = 450;
        private const double OD_MS_STEP = 6;
        private const double AR_MS_STEP1 = 120;
        private const double AR_MS_STEP2 = 150;


        /// <summary>
        /// Converted Beatmap Approach Rate
        /// </summary>
        public double AR;

        /// <summary>
        /// Converted Beatmap Overall Difficulty
        /// </summary>
        public double OD;

        /// <summary>
        /// Converted Beatmap Circle Size
        /// </summary>
        public double CS;

        /// <summary>
        /// Converted Beatmap Health
        /// </summary>
        public double HP;

        /// <summary>
        /// Converted Beatmap Speed
        /// </summary>
        public double Speed;

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
                CS *= 1.3;
                AR *= 1.4;
                OD *= 1.4;
                HP *= 1.4;
            }else if ((Mods & OsuMods.Easy) != 0)
            {
                CS *= 0.5;
                AR *= 0.5;
                OD *= 0.5;
                HP *= 0.5;
            }

            double ODMS = OD_0_MS - System.Math.Ceiling(OD_MS_STEP * OD);
            double ARMS = AR < 5 ? (AR_0_MS - AR_MS_STEP1 * AR) : (AR_5_MS - AR_MS_STEP2 * (AR - 5));

            ODMS = System.Math.Min(OD_0_MS, System.Math.Max(OD_10_MS, ODMS));
            ARMS = System.Math.Min(AR_0_MS, System.Math.Max(AR_10_MS, ARMS));

            Speed = 1;
            if ((Mods & OsuMods.DoubleTime) != 0)
                Speed *= 1.5;
            else if ((Mods & OsuMods.HalfTime) != 0)
                Speed *= 0.75;

            double invSpeed = 1 / Speed;

            ODMS *= invSpeed;
            ARMS *= invSpeed;

            OD = (OD_0_MS - ODMS) / OD_MS_STEP;
            AR = ARMS>AR_5_MS ? ((AR_0_MS - ARMS) / AR_MS_STEP1) : (5.0 + (AR_5_MS - ARMS) / AR_MS_STEP2);

            CS = System.Math.Max(0.0, System.Math.Min(10.0, CS));
        }
    }
}