using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using OsuApiHelper.Math;

namespace OsuApiHelper
{
    /// <summary>
    /// Base class for Performance Points handling
    /// </summary>
    public class OsuPerformance
    {
        /// <summary>
        /// Connected beatmap
        /// </summary>
        [JsonIgnore] // beatmap references to this already, noone has a PP object without beatmap object first, so its pointless
        [IgnoreDataMember] // its fine to exist, but serializing would cause self referencing loop so we ignore it for that
        public OsuBeatmap Beatmap;

        /// <summary>
        /// Connected score
        /// </summary>
        public OsuPlay Play;

        /// <summary>
        /// PP value based on current or given score
        /// </summary>
        public double CurrentValue { get; set; }

        /// <summary>
        /// PP value if full combo with current accuracy
        /// </summary>
        public double CurrentValueIfFC { get; set; }

        /// <summary>
        /// Aim influence on performance
        /// </summary>
        public double AimPP = 0;

        /// <summary>
        /// Speed influence on performance
        /// </summary>
        public double SpeedPP = 0;

        /// <summary>
        /// Accuracy influence on performance
        /// </summary>
        public double AccPP = 0;

        /// <summary>
        /// Create Performance object for a score
        /// </summary>
        public OsuPerformance(OsuPlay play, OsuBeatmap beatmap)
        {
            Beatmap = beatmap;
            Play = play;

            CalculateCurrentPerformance();
        }

        /// <summary>
        /// Calculate the performance for a score
        /// </summary>
        public void CalculateCurrentPerformance()
        {
            if (Play.PP == -1)
                CurrentValue = CalculatePerformance(Play.MaxCombo, Play.C50, Play.C100, Play.C300, Play.CMiss, Play.CKatu, Play.CGeki);
            else
                CurrentValue = Play.PP;

            CurrentValueIfFC =
                CalculatePerformance(Beatmap.MaxCombo??0, Play.C50, Play.C100, Play.C300 + Play.CMiss, 0, Play.CKatu, Play.CGeki);
        }

        /// <summary>
        /// Calculate performance with manual input
        /// </summary>
        public double CalculatePerformance(double combo, double c50, double c100, double c300, double cMiss, double cKatu = 0, double cGeki = 0)
        {
            if ((Play.Mods & OsuMods.ScoreV2) != 0)
                return 0;
            
            switch (Play.Mode)
            {
                case OsuMode.Catch:
                    return CalculateCatchPP(combo, c50, c100, c300, cMiss, cKatu, cGeki);
                case OsuMode.Mania:
                    return CalculateManiaPP(combo, c50, c100, c300, cMiss, cKatu, cGeki);
                case OsuMode.Taiko:
                    return CalculateTaikoPP(combo, c50, c100, c300, cMiss, cKatu, cGeki);
                case OsuMode.Standard:
                    return CalculateStandardPP(combo, c50, c100, c300, cMiss, cKatu, cGeki);
            }

            return 0f;
        }

        private double CalculateTaikoPP(double combo, double c50, double c100, double c300, double cMiss, double cKatu = 0, double cGeki = 0)
        {
            if ((Play.Mods & OsuMods.Relax) != 0 || (Play.Mods & OsuMods.Relax2) != 0 ||
                (Play.Mods & OsuMods.Autoplay) != 0)
                return 0f;

            double cTotalHits = combo;

            double real_acc = OsuApi.CalculateAccuracy(Play.Mode, cMiss, c50, c100, c300, cKatu, cGeki) * 0.01;
            //float real_acc = Mathf.Min(1.0f, Mathf.Max(0.0f, (c100 * 150 + c300 * 300)/(cTotalHits*300)));

            double strain = System.Math.Pow(5.0*System.Math.Max(1.0, (double)Beatmap.Starrating/0.0075)-4.0, 2.0)/100000.0;

            double bonusLength = 1 + 0.1 * System.Math.Min(1.0, cTotalHits / 1500);
            strain *= bonusLength;

            strain *= System.Math.Pow(0.985, cMiss);

            strain *= System.Math.Min(System.Math.Pow(Play.MaxCombo, 0.5) / System.Math.Pow(cTotalHits, 0.5), 1);

            if ((Play.Mods & OsuMods.Hidden) != 0)
                strain *= 1.025;
            
            if ((Play.Mods & OsuMods.Flashlight) != 0)
                strain *= 1.05*bonusLength;

            strain *= real_acc;

            double OD300 = 0;

            double hwMax = 20;
            double hwMin = 50;

            double hwResult = hwMin + (hwMax - hwMin) * Beatmap.MapStats.OD / 10;
            hwResult = System.Math.Floor(hwResult) - 0.5;
            
            if ((Play.Mods & OsuMods.HalfTime) != 0) hwResult *= 1.5;
            if ((Play.Mods & OsuMods.DoubleTime) != 0) hwResult *= 0.75;

            OD300 = System.Math.Round(hwResult * 100) / 100;

            double acc = 0;
            if (OD300 > 0)
            {
                acc = System.Math.Pow(150.0 / OD300, 1.1) * System.Math.Pow(real_acc, 15) * 22.0;
                acc *= System.Math.Min(1.15, System.Math.Pow(cTotalHits / 1500.0, 0.3));
            }

            double total = 1.1;
            
            if ((Play.Mods & OsuMods.NoFail) != 0)
                total *= 0.9;
            
            if ((Play.Mods & OsuMods.Hidden) != 0)
                total *= 1.1;

            return System.Math.Pow(
                    System.Math.Pow(strain, 1.1)+ System.Math.Pow(acc, 1.1),
                    1.0/1.1
                ) * total;
        }

        private double CalculateManiaPP(double combo, double c50, double c100, double c300, double cMiss, double cKatu = 0, double cGeki = 0)
        {
            if ((Play.Mods & OsuMods.Relax) != 0 || (Play.Mods & OsuMods.Relax2) != 0 ||
                (Play.Mods & OsuMods.Autoplay) != 0)
                return 0f;

            double cTotalHits = c50 + c100 + c300 + cMiss + cGeki + cKatu;

            double real_acc = OsuApi.CalculateAccuracy(Play.Mode, cMiss, c50, c100, c300, cKatu, cGeki);

            double strainbase = System.Math.Pow(5.0 * System.Math.Max(1, (double) Beatmap.Starrating / 0.2) - 4, 2.2) / 135.0;
            strainbase *= 1 + 0.1 * System.Math.Min(1, cTotalHits / 1500);

            double strain = strainbase;
            double scoreMultiplier = 0;
            if (Play.Score < 500000)
                scoreMultiplier = Play.Score / 500000 * 0.1;
            else if (Play.Score < 600000)
                scoreMultiplier = (Play.Score - 500000) / 100000 * 0.3;
            else if (Play.Score < 700000)
                scoreMultiplier = (Play.Score - 600000) / 100000 * 0.25+0.3;
            else if (Play.Score < 800000)
                scoreMultiplier = (Play.Score - 700000) / 100000 * 0.2+0.55;
            else if (Play.Score < 900000)
                scoreMultiplier = (Play.Score - 800000) / 100000 * 0.15+0.75;
            else
                scoreMultiplier = (Play.Score - 900000) / 100000 * 0.1+0.9;


            // float[][] odconvertwindow = new float[2][]
            // {
            //     new float[2] {47, 34},
            //     new float[2] {65, 47}
            // };
            //
            // float odwindow = ((Beatmap.OriginalMode == OsuMode.Standard)) ? odconvertwindow[((Play.Mods&OsuMods.Easy)!=0)?1:0][(Beatmap.MapStats.OD>=5)?1:0]:((64f-3f*Beatmap.MapStats.OD)* (((Play.Mods & OsuMods.Easy) != 0) ? 1.4f : 1f));
            //
            // float acc = Mathf.Max(0.0f, 0.2f - ((odwindow - 34f) * 0.006667f)) * strain;
            // acc *= Mathf.Pow(Mathf.Max(0.0f, (Play.Score - 960000f)) / 40000.0f, 1.1f);

            double acc = 1.0;
            double nerfod = ((Play.Mods & OsuMods.Easy) != 0) ? 0.5 : 1.0;
            if (Play.Score >= 960000)
                acc = Beatmap.MapStats.OD * nerfod * 0.02 * strainbase *
                      System.Math.Pow((Play.Score - 960000) / 40000, 1.1);
            else
                acc = 0;

            double total = 0.8;

            if ((Play.Mods & OsuMods.NoFail) != 0)
                total *= 0.9;
            
            if ((Play.Mods & OsuMods.SpunOut) != 0)
                total *= 0.95;
            
            if ((Play.Mods & OsuMods.Easy) != 0)
                total *= 0.5;
            
            return total*System.Math.Pow(
                System.Math.Pow(strain*scoreMultiplier, 1.1)+
                System.Math.Pow(acc, 1.1),
                1.0/1.1
                );
        }

        private double CalculateCatchPP(double combo, double c50, double c100, double c300, double cMiss, double cKatu = 0, double cGeki = 0)
        {
            if ((Play.Mods & OsuMods.Relax) != 0 || (Play.Mods & OsuMods.Relax2) != 0 ||
                (Play.Mods & OsuMods.Autoplay) != 0)
                return 0;

            //float real_acc = OsuApi.CalculateAccuracy(Play.Mode, cMiss, c50, c100, c300, cKatu, cGeki) * 0.01f;
            double cTotalHits = cMiss + c100 + c300;
            double real_acc = System.Math.Min(1, System.Math.Max((c50 + c100 + c300) / (cTotalHits+c50+cKatu), 0));

            double value = System.Math.Pow(5.0*System.Math.Max(1.0, (double)Beatmap.Starrating/0.0049)-4.0, 2.0)/100000.0;
            double bonusLength = 0.95+0.3*System.Math.Min(1.0, cTotalHits/2500)+
                                (cTotalHits>2500?System.Math.Log10(cTotalHits/2500)*0.475:0.0);
            value *= bonusLength;

            //Miss penalty
            value *= System.Math.Pow(0.97, cMiss);
            
            // Combo scaling
            if (Beatmap.MaxCombo > 0)
                value *= System.Math.Min(System.Math.Pow(Play.MaxCombo, 0.8)/System.Math.Pow((double)Beatmap.MaxCombo, 0.8), 1.0);

            //AR bonus
            double bonusAR = 1.0;
            if (Beatmap.MapStats.AR > 9.0)
                bonusAR += 0.1 * (Beatmap.MapStats.AR - 9.0);
            if (Beatmap.MapStats.AR > 10.0)
                bonusAR += 0.1 * (Beatmap.MapStats.AR - 10.0);
            else if (Beatmap.MapStats.AR < 8.0)
                bonusAR += 0.025 * (8.0 - Beatmap.MapStats.AR);
            value *= bonusAR;
            
            //HD Bonus
            if ((Play.Mods & OsuMods.Hidden) != 0)
            {
                if (Beatmap.MapStats.AR <= 10.0)
                    value *= 1.05 + 0.075 * (10.0 - Beatmap.MapStats.AR);
                else if (Beatmap.MapStats.AR > 10)
                    value *= 1.01 + 0.04 * (11.0 - System.Math.Min(11.0, Beatmap.MapStats.AR));
            }

            if ((Play.Mods & OsuMods.Flashlight) != 0)
                value *= 1.35 * bonusLength;

            value *= System.Math.Pow(real_acc, 5.5);

            if ((Play.Mods & OsuMods.NoFail) != 0)
                value *= 0.9;
            if ((Play.Mods & OsuMods.SpunOut) != 0)
                value *= 0.95;

            return value;
        }

        private double CalculateStandardPP(double combo, double c50, double c100, double c300, double cMiss, double cKatu = 0, double cGeki = 0)
        {
            if ((Play.Mods & OsuMods.Relax) != 0 || (Play.Mods & OsuMods.Relax2) != 0 ||
                (Play.Mods & OsuMods.Autoplay) != 0)
                return 0d;

            double Accuracy = OsuApi.CalculateAccuracy(Play.Mode, cMiss, c50, c100, c300, cKatu, cGeki) * 0.01;
            
#region Standard MUL
            double cTotalHits = c50 + c100 + c300 + cMiss;

            double BonusLength = 0.95d + 0.4d * System.Math.Min(1.0d, cTotalHits/2000.0d)+
                                (cTotalHits>2000d?System.Math.Log10(cTotalHits/2000.0d)*0.5d:0.0d);

            double BonusMiss = (cMiss>0d?0.97d*System.Math.Pow(1.0d-System.Math.Pow(cMiss/cTotalHits, 0.775d), cMiss):1.0d);

            double BonusCombo = (Beatmap.MaxCombo > 0d
                ? System.Math.Min((System.Math.Pow(combo, 0.8d) / System.Math.Pow((double)Beatmap.MaxCombo, 0.8d)), 1.0d)
                : 1.0d);

            double BonusApproachRateFactor = 0.0d;
            if (Beatmap.MapStats.AR > 10.33)
                BonusApproachRateFactor = (Beatmap.MapStats.AR - 10.33d);
            else if (Beatmap.MapStats.AR < 8.0)
                BonusApproachRateFactor = 0.025d * (8.0d - Beatmap.MapStats.AR);
            //float BonusApproachRate =
            //    1.0f + Mathf.Min(BonusApproachRateFactor, BonusApproachRateFactor * (cTotalHits / 1000.0f));
            double BonusApproachRateHitFactor = 1.0d / (1.0d+System.Math.Exp(-(0.007d*(cTotalHits-400d))));
            double BonusApproachRate = 1.0d+(0.03d + 0.37d * BonusApproachRateHitFactor) * BonusApproachRateFactor;


            double BonusHidden = ((Play.Mods&OsuMods.Hidden)!=0)?1.0d+0.04d*(12.0-Beatmap.MapStats.AR):1.0d;

            double BonusFlashlight = ((Play.Mods&OsuMods.Flashlight)!=0)
                    ?1.0d+0.35d*System.Math.Min(1.0d, cTotalHits/200.0d)+(cTotalHits>200d?0.3d*System.Math.Min(1.0d,(cTotalHits-200d)/300d)+(cTotalHits>500d? (
                        cTotalHits - 500d)/1200.0d:0.0d):0.0d):1.0d;

#endregion

 #region Standard AIM
            double AimValue = GetPPBase((double)Beatmap.StarratingAim);

            AimValue *= BonusLength;
            AimValue *= BonusMiss;
            AimValue *= BonusCombo;
            AimValue *= BonusHidden;
            AimValue *= BonusApproachRate;
            AimValue *= BonusFlashlight;

            AimValue *= (0.5d + Accuracy / 2.0d);

            AimValue *= (0.98d + (System.Math.Pow(Beatmap.MapStats.OD, 2d) / 2500d));
#endregion

#region Standard SPEED
            double SpeedValue = GetPPBase((double)Beatmap.StarratingSpeed);

            SpeedValue *= BonusLength;
            SpeedValue *= BonusMiss;
            SpeedValue *= BonusCombo;
            SpeedValue *= BonusApproachRate;
            SpeedValue *= BonusHidden;

            SpeedValue *= (0.95d + System.Math.Pow(Beatmap.MapStats.OD, 2d) / 750d) *
                          System.Math.Pow(Accuracy, (14.5d - System.Math.Max(Beatmap.MapStats.OD, 8.0d)) / 2d);
            SpeedValue *= System.Math.Pow(0.98d, (c50 < cTotalHits / 500) ? (0.0) : (c50 - cTotalHits / 500d));
#endregion

#region Standard ACC

            double BetterAccuracyPercentage = 0d;
            double cHitObjectsWithAccuracy = (double)Beatmap.CircleCount;
            if (cHitObjectsWithAccuracy > 0)
                BetterAccuracyPercentage = ((c300 - (cTotalHits - cHitObjectsWithAccuracy)) * 6d + c100 * 2d + c50)/(cHitObjectsWithAccuracy*6d);

            if (BetterAccuracyPercentage < 0)
                BetterAccuracyPercentage = 0d;

            double AccValue = System.Math.Pow(1.52163d, Beatmap.MapStats.OD)*System.Math.Pow(BetterAccuracyPercentage, 24d)*2.83d;

            AccValue *= System.Math.Min(1.15d, System.Math.Pow(cHitObjectsWithAccuracy / 1000d, 0.3d));

            if ((Play.Mods & OsuMods.Hidden) != 0)
                AccValue *= 1.08d;
            
            if ((Play.Mods & OsuMods.Flashlight) != 0)
                AccValue *= 1.02d;
#endregion

            double TotalMultiplier = 1.12d;

            if ((Play.Mods & OsuMods.NoFail) != 0)
                TotalMultiplier *= System.Math.Max(0.9d, 1.0d - 0.02d * cMiss);

            if ((Play.Mods & OsuMods.SpunOut) != 0)
                TotalMultiplier *= 1.0d - System.Math.Pow((double)Beatmap.SpinnerCount / cTotalHits, 0.85d);

            double TotalValue = System.Math.Pow(
                System.Math.Pow(AimValue, 1.1d) +
                System.Math.Pow(SpeedValue, 1.1d) +
                System.Math.Pow(AccValue, 1.1d),
                1.0d / 1.1d
            ) * TotalMultiplier;
            
            return TotalValue;
        }


        private static double GetPPBase(double stars) =>
            System.Math.Pow(5.0d * System.Math.Max(1.0d, stars / 0.0675d) - 4.0d, 3.0d) / 100000.0d;
    }
}