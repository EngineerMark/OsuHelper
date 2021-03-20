using System;
using System.Collections.Generic;
using OsuApiHelper.Math;

namespace OsuApiHelper
{
    public static class OsuApi
    {
        private static string apiUrl = "https://osu.ppy.sh/api/";

        public static OsuUser GetUser(string username, OsuMode mode = OsuMode.Standard)
        {
            List<OsuUser> users =
                APIHelper<List<OsuUser>>.GetData(
                    apiUrl + "get_user?k=" + OsuApiKey.Key + "&u=" + username + "&m=" + (int) mode);
            if (users.Count > 0)
            {
                //users[0].SetUserpage();
                return users[0];
            }

            return null;
        }

        public static List<OsuPlay> GetUserBest(OsuUser user, OsuMode mode = OsuMode.Standard, int limit = 5,
            bool generateBeatmaps = false)
        {
            return GetUserBest(user.Name, mode, limit, generateBeatmaps);
        }

        public static List<OsuPlay> GetUserBest(string username, OsuMode mode = OsuMode.Standard, int limit = 5,
            bool generateBeatmaps = false)
        {
            List<OsuPlay> plays = APIHelper<List<OsuPlay>>.GetData(apiUrl + "get_user_best?k=" + OsuApiKey.Key + "&u=" +
                                                                   username + "&m=" + (int) mode + "&limit=" + limit);
            plays.ForEach(play =>
            {
                play.Mode = mode;
                if (generateBeatmaps)
                    play.Beatmap = GetBeatmap(play.MapID, play.Mods, mode);
            });
            return plays.Count > 0 ? plays : null;
        }

        public static List<OsuPlay> GetUserRecent(string username, OsuMode mode = OsuMode.Standard, int limit = 1,
            bool generateBeatmaps = false)
        {
            List<OsuPlay> plays = APIHelper<List<OsuPlay>>.GetData(apiUrl + "get_user_recent?k=" + OsuApiKey.Key + "&u=" +
                                                                   username + "&m=" + (int) mode + "&limit=" + limit);
            plays.ForEach(play =>
            {
                play.Mode = mode;
                if (generateBeatmaps)
                    play.Beatmap = GetBeatmap(play.MapID, play.Mods, mode);
            });
            return plays.Count > 0 ? plays : null;
        }

        public static OsuBeatmap GetBeatmap(string id, OsuMods mods = OsuMods.None, OsuMode mode = OsuMode.Standard)
        {
            string url = apiUrl + "get_beatmaps?k=" + OsuApiKey.Key + "&b=" + id + "&m=" + (int) mode + "&a=1&mods=" +
                         (int) mods.ModParser(true);
            List<OsuBeatmap> maps = APIHelper<List<OsuBeatmap>>.GetData(url, true);
            if (maps.Count > 0)
            {
                maps[0].Mode = mode;
                maps[0].Mods = mods;
                maps[0].MapStats = new MapStats(maps[0], mods);
            }

            return maps.Count > 0 ? maps[0] : null;
        }

        public static OsuMods ModParser(this OsuMods mods, bool forApi = false)
        {
            OsuMods _mods = mods;
            //Stuff like NC has DT aswell, we gotta filter stuff like that
            if (!forApi)
            {
                if ((mods & OsuMods.Nightcore) == OsuMods.Nightcore) _mods &= ~OsuMods.DoubleTime;
            }
            else
            {
                //If target is API usage, we can only have several mods available
                _mods &= OsuMods.APIMods;
            }

            return _mods;
        }

        public static OsuModsShort ModParser(this OsuModsShort mods)
        {
            return (OsuModsShort) ((OsuMods) mods).ModParser();
        }

        public static float CalculateDifficultyLimit(OsuMods mods)
        {
            float limit = 10;

            if ((mods & OsuMods.HardRock) != 0 && ((mods & OsuMods.DoubleTime) != 0 || (mods & OsuMods.Nightcore) != 0))
                limit = 11;
            return limit;
        }

        public static float CalculateDifficulty(OsuMods mods, float baseValue)
        {
            float value = baseValue;
            float limit = CalculateDifficultyLimit(mods);
            
            if ((mods & OsuMods.HardRock) != 0)
                value *= 1.4f;

            if ((mods & OsuMods.Easy) != 0)
                value *= 0.5f;

            return Mathf.Max(Mathf.Min(value, limit), 0);
        }

        public static float CalculateAccuracy(OsuMode mode, float cMiss, float c50, float c100, float c300,
            float cKatu = 0f, float cGeki = 0f)
        {
            switch (mode)
            {
                case OsuMode.Standard:
                    return (c50 * 50f + c100 * 100f + c300 * 300f) / ((c50 + c100 + c300 + cMiss) * 300) * 100;
                case OsuMode.Mania:
                    return ((c50 * 50 + c100 * 100 + cKatu * 200 + c300 * 300 + cGeki * 300) /
                            ((cMiss + c50 + c100 + cKatu + c300 + cGeki) * 300)) * 100;
                case OsuMode.Catch:
                    return (((c50 + c100 + c300) / (cMiss + c50 + c100 + c300 + cKatu)) * 100);
                case OsuMode.Taiko:
                    return (((c100 * 0.5f + c300 * 1) * 300) / ((cMiss + c100 + c300) * 300)) * 100;
                default:
                    return (c50 * 50f + c100 * 100f + c300 * 300f) / ((c50 + c100 + c300 + cMiss) * 300) * 100;
            }
        }
    }
}