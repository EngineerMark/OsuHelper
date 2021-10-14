using System;
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
        public OsuBeatmap Beatmap;

        /// <summary>
        /// Connected score
        /// </summary>
        public OsuPlay Play;

        /// <summary>
        /// PP value based on current or given score
        /// </summary>
        public float CurrentValue { get; set; }

        /// <summary>
        /// PP value if full combo with current accuracy
        /// </summary>
        public float CurrentValueIfFC { get; set; }

        /// <summary>
        /// Aim influence on performance
        /// </summary>
        public float AimPP = 0;

        /// <summary>
        /// Speed influence on performance
        /// </summary>
        public float SpeedPP = 0;

        /// <summary>
        /// Accuracy influence on performance
        /// </summary>
        public float AccPP = 0;

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
        public float CalculatePerformance(float combo, float c50, float c100, float c300, float cMiss, float cKatu = 0, float cGeki = 0)
        {
            if ((Play.Mods & OsuMods.ScoreV2) != 0)
                return 0f;
            
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

        private float CalculateTaikoPP(float combo, float c50, float c100, float c300, float cMiss, float cKatu = 0, float cGeki = 0)
        {
            if ((Play.Mods & OsuMods.Relax) != 0 || (Play.Mods & OsuMods.Relax2) != 0 ||
                (Play.Mods & OsuMods.Autoplay) != 0)
                return 0f;
            
            float cTotalHits = combo;
            
            float real_acc = OsuApi.CalculateAccuracy(Play.Mode, cMiss, c50, c100, c300, cKatu, cGeki) * 0.01f;
            //float real_acc = Mathf.Min(1.0f, Mathf.Max(0.0f, (c100 * 150 + c300 * 300)/(cTotalHits*300)));
            
            float strain = Mathf.Pow(5.0f*Mathf.Max(1.0f, (float)Beatmap.Starrating/0.0075f)-4.0f, 2.0f)/100000.0f;

            float bonusLength = 1f + 0.1f * Mathf.Min(1.0f, cTotalHits / 1500f);
            strain *= bonusLength;

            strain *= Mathf.Pow(0.985f, cMiss);

            strain *= Mathf.Min(Mathf.Pow(Play.MaxCombo, 0.5f) / Mathf.Pow(cTotalHits, 0.5f), 1);

            if ((Play.Mods & OsuMods.Hidden) != 0)
                strain *= 1.025f;
            
            if ((Play.Mods & OsuMods.Flashlight) != 0)
                strain *= 1.05f*bonusLength;

            strain *= real_acc;

            float OD300 = 0;

            float hwMax = 20f;
            float hwMin = 50f;

            float hwResult = hwMin + (hwMax - hwMin) * Beatmap.MapStats.OD / 10f;
            hwResult = Mathf.Floor(hwResult) - 0.5f;
            
            if ((Play.Mods & OsuMods.HalfTime) != 0) hwResult *= 1.5f;
            if ((Play.Mods & OsuMods.DoubleTime) != 0) hwResult *= 0.75f;

            OD300 = Mathf.Round(hwResult * 100f) / 100f;

            float acc = 0;
            if (OD300 > 0)
            {
                acc = Mathf.Pow(150.0f / OD300, 1.1f) * Mathf.Pow(real_acc, 15f) * 22.0f;
                acc *= Mathf.Min(1.15f, Mathf.Pow(cTotalHits / 1500.0f, 0.3f));
            }

            float total = 1.1f;
            
            if ((Play.Mods & OsuMods.NoFail) != 0)
                total *= 0.9f;
            
            if ((Play.Mods & OsuMods.Hidden) != 0)
                total *= 1.1f;

            return Mathf.Pow(
                    Mathf.Pow(strain, 1.1f)+Mathf.Pow(acc, 1.1f),
                    1.0f/1.1f
                ) * total;
        }

        private float CalculateManiaPP(float combo, float c50, float c100, float c300, float cMiss, float cKatu = 0, float cGeki = 0)
        {
            if ((Play.Mods & OsuMods.Relax) != 0 || (Play.Mods & OsuMods.Relax2) != 0 ||
                (Play.Mods & OsuMods.Autoplay) != 0)
                return 0f;
            
            float cTotalHits = c50 + c100 + c300 + cMiss + cGeki + cKatu;

            float real_acc = OsuApi.CalculateAccuracy(Play.Mode, cMiss, c50, c100, c300, cKatu, cGeki);

            float strainbase = Mathf.Pow(5.0f * Mathf.Max(1f, (float) Beatmap.Starrating / 0.2f) - 4f, 2.2f) / 135.0f;
            strainbase *= 1f + 0.1f * Mathf.Min(1f, cTotalHits / 1500f);

            float strain = strainbase;
            float scoreMultiplier = 0;
            if (Play.Score < 500000)
                scoreMultiplier = Play.Score / 500000f * 0.1f;
            else if (Play.Score < 600000)
                scoreMultiplier = (Play.Score - 500000f) / 100000f * 0.3f;
            else if (Play.Score < 700000)
                scoreMultiplier = (Play.Score - 600000f) / 100000f * 0.25f+0.3f;
            else if (Play.Score < 800000)
                scoreMultiplier = (Play.Score - 700000f) / 100000f * 0.2f+0.55f;
            else if (Play.Score < 900000)
                scoreMultiplier = (Play.Score - 800000f) / 100000f * 0.15f+0.75f;
            else
                scoreMultiplier = (Play.Score - 900000f) / 100000f * 0.1f+0.9f;
            

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

            float acc = 1.0f;
            float nerfod = ((Play.Mods & OsuMods.Easy) != 0) ? 0.5f : 1.0f;
            if (Play.Score >= 960000)
                acc = Beatmap.MapStats.OD * nerfod * 0.02f * strainbase *
                      Mathf.Pow((Play.Score - 960000f) / 40000f, 1.1f);
            else
                acc = 0;

            float total = 0.8f;

            if ((Play.Mods & OsuMods.NoFail) != 0)
                total *= 0.9f;
            
            if ((Play.Mods & OsuMods.SpunOut) != 0)
                total *= 0.95f;
            
            if ((Play.Mods & OsuMods.Easy) != 0)
                total *= 0.5f;
            
            return total*Mathf.Pow(
                Mathf.Pow(strain*scoreMultiplier, 1.1f)+
                Mathf.Pow(acc, 1.1f),
                1.0f/1.1f
                );
        }

        private float CalculateCatchPP(float combo, float c50, float c100, float c300, float cMiss, float cKatu = 0, float cGeki = 0)
        {
            if ((Play.Mods & OsuMods.Relax) != 0 || (Play.Mods & OsuMods.Relax2) != 0 ||
                (Play.Mods & OsuMods.Autoplay) != 0)
                return 0f;

            //float real_acc = OsuApi.CalculateAccuracy(Play.Mode, cMiss, c50, c100, c300, cKatu, cGeki) * 0.01f;
            float cTotalHits = cMiss + c100 + c300;
            float real_acc = Mathf.Min(1f, Mathf.Max((c50 + c100 + c300) / (cTotalHits+c50+cKatu), 0f));
            
            float value = Mathf.Pow(5.0f*Mathf.Max(1.0f, (float)Beatmap.Starrating/0.0049f)-4.0f, 2.0f)/100000.0f;
            float bonusLength = 0.95f+0.3f*Mathf.Min(1.0f, cTotalHits/2500f)+
                                (cTotalHits>2500?Mathf.Log10(cTotalHits/2500f)*0.475f:0.0f);
            value *= bonusLength;

            //Miss penalty
            value *= Mathf.Pow(0.97f, cMiss);
            
            // Combo scaling
            if (Beatmap.MaxCombo > 0)
                value *= Mathf.Min(Mathf.Pow(Play.MaxCombo, 0.8f)/Mathf.Pow((float)Beatmap.MaxCombo, 0.8f), 1.0f);
            
            //AR bonus
            float bonusAR = 1.0f;
            if (Beatmap.MapStats.AR > 9.0f)
                bonusAR += 0.1f * (Beatmap.MapStats.AR - 9.0f);
            if (Beatmap.MapStats.AR > 10.0f)
                bonusAR += 0.1f * (Beatmap.MapStats.AR - 10.0f);
            else if (Beatmap.MapStats.AR < 8.0f)
                bonusAR += 0.025f * (8.0f - Beatmap.MapStats.AR);
            value *= bonusAR;
            
            //HD Bonus
            if ((Play.Mods & OsuMods.Hidden) != 0)
            {
                if (Beatmap.MapStats.AR <= 10.0f)
                    value *= 1.05f + 0.075f * (10.0f - Beatmap.MapStats.AR);
                else if (Beatmap.MapStats.AR > 10f)
                    value *= 1.01f + 0.04f * (11.0f - Mathf.Min(11.0f, Beatmap.MapStats.AR));
            }

            if ((Play.Mods & OsuMods.Flashlight) != 0)
                value *= 1.35f * bonusLength;

            value *= Mathf.Pow(real_acc, 5.5f);

            if ((Play.Mods & OsuMods.NoFail) != 0)
                value *= 0.9f;
            if ((Play.Mods & OsuMods.SpunOut) != 0)
                value *= 0.95f;

            return value;
        }

        private float CalculateStandardPP(float combo, float c50, float c100, float c300, float cMiss, float cKatu = 0, float cGeki = 0)
        {
            if ((Play.Mods & OsuMods.Relax) != 0 || (Play.Mods & OsuMods.Relax2) != 0 ||
                (Play.Mods & OsuMods.Autoplay) != 0)
                return 0f;

            float Accuracy = OsuApi.CalculateAccuracy(Play.Mode, cMiss, c50, c100, c300, cKatu, cGeki) * 0.01f;
            
#region Standard MUL
            float cTotalHits = c50 + c100 + c300 + cMiss;
            
            float BonusLength = 0.95f + 0.4f * Mathf.Min(1.0f, cTotalHits/2000.0f)+
                                (cTotalHits>2000?Mathf.Log10(cTotalHits/2000.0f)*0.5f:0.0f);
            
            float BonusMiss = (cMiss>0?0.97f*Mathf.Pow(1.0f-Mathf.Pow(cMiss/cTotalHits, 0.775f), cMiss):1.0f);
            
            float BonusCombo = (Beatmap.MaxCombo > 0
                ? Mathf.Min((Mathf.Pow(combo, 0.8f) / Mathf.Pow((float) Beatmap.MaxCombo, 0.8f)), 1.0f)
                : 1.0f);

            float BonusApproachRateFactor = 0.0f;
            if (Beatmap.MapStats.AR > 10.33f)
                BonusApproachRateFactor = (Beatmap.MapStats.AR - 10.33f);
            else if (Beatmap.MapStats.AR < 8.0f)
                BonusApproachRateFactor = 0.025f * (8.0f - Beatmap.MapStats.AR);
            //float BonusApproachRate =
            //    1.0f + Mathf.Min(BonusApproachRateFactor, BonusApproachRateFactor * (cTotalHits / 1000.0f));
            float BonusApproachRateHitFactor = 1.0f / (1.0f+Mathf.Exp(-(0.007f*(cTotalHits-400))));
            float BonusApproachRate = 1.0f+(0.03f + 0.37f * BonusApproachRateHitFactor) * BonusApproachRateFactor;


            float BonusHidden = ((Play.Mods&OsuMods.Hidden)!=0)?1.0f+0.04f*(12.0f-Beatmap.MapStats.AR):1.0f;
            
            float BonusFlashlight = ((Play.Mods&OsuMods.Flashlight)!=0)
                    ?1.0f+0.35f*Mathf.Min(1.0f, cTotalHits/200.0f)+(cTotalHits>200?0.3f*Mathf.Min(1.0f,(cTotalHits-200f)/300f)+(cTotalHits>500? (
                        cTotalHits - 500)/1200.0f:0.0f):0.0f):1.0f;

#endregion
            
#region Standard AIM
            float AimValue = GetPPBase((float)Beatmap.StarratingAim);

            AimValue *= BonusLength;
            AimValue *= BonusMiss;
            AimValue *= BonusCombo;
            AimValue *= BonusHidden;
            AimValue *= BonusApproachRate;
            AimValue *= BonusFlashlight;

            AimValue *= (0.5f + Accuracy / 2.0f);

            AimValue *= (0.98f + (Mathf.Pow(Beatmap.MapStats.OD, 2f) / 2500f));
#endregion

#region Standard SPEED
            float SpeedValue = GetPPBase((float)Beatmap.StarratingSpeed);

            SpeedValue *= BonusLength;
            SpeedValue *= BonusMiss;
            SpeedValue *= BonusCombo;
            SpeedValue *= BonusApproachRate;
            SpeedValue *= BonusHidden;

            SpeedValue *= (0.95f + Mathf.Pow(Beatmap.MapStats.OD, 2f) / 750f) *
                          Mathf.Pow(Accuracy, (14.5f - Mathf.Max(Beatmap.MapStats.OD, 8.0f)) / 2f);
            SpeedValue *= Mathf.Pow(0.98f, (c50 < cTotalHits / 500f) ? (0.0f) : (c50 - cTotalHits / 500f));
#endregion

#region Standard ACC

            float BetterAccuracyPercentage = 0;
            float cHitObjectsWithAccuracy = (float)Beatmap.CircleCount;
            if (cHitObjectsWithAccuracy > 0)
                BetterAccuracyPercentage = ((c300 - (cTotalHits - cHitObjectsWithAccuracy)) * 6 + c100 * 2 + c50)/(cHitObjectsWithAccuracy*6);

            if (BetterAccuracyPercentage < 0)
                BetterAccuracyPercentage = 0;

            float AccValue = Mathf.Pow(1.52163f, Beatmap.MapStats.OD)*Mathf.Pow(BetterAccuracyPercentage, 24)*2.83f;

            AccValue *= Mathf.Min(1.15f, Mathf.Pow(cHitObjectsWithAccuracy / 1000f, 0.3f));

            if ((Play.Mods & OsuMods.Hidden) != 0)
                AccValue *= 1.08f;
            
            if ((Play.Mods & OsuMods.Flashlight) != 0)
                AccValue *= 1.02f;
#endregion

            float TotalMultiplier = 1.12f;

            if ((Play.Mods & OsuMods.NoFail) != 0)
                TotalMultiplier *= Mathf.Max(0.9f, 1.0f - 0.02f * cMiss);

            if ((Play.Mods & OsuMods.SpunOut) != 0)
                TotalMultiplier *= 1.0f - Mathf.Pow((float)Beatmap.SpinnerCount / cTotalHits, 0.85f);

            float TotalValue = Mathf.Pow(
                Mathf.Pow(AimValue, 1.1f) +
                Mathf.Pow(SpeedValue, 1.1f) +
                Mathf.Pow(AccValue, 1.1f),
                1.0f / 1.1f
            ) * TotalMultiplier;
            
            return TotalValue;
        }

        private static float GetPPBase(float stars) =>
            Mathf.Pow(5.0f * Mathf.Max(1.0f, stars / 0.0675f) - 4.0f, 3.0f) / 100000.0f;
    }
}