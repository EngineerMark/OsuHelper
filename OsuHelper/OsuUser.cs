using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace OsuApiHelper
{
    /// <summary>
    /// Class containing user information and statistics
    /// </summary>
    public class OsuUser
    {
        /// <summary>
        /// User ID in osu database
        /// </summary>
        [JsonProperty("user_id")]
        public int ID { get; set; }
        
        /// <summary>
        /// Username
        /// </summary>
        [JsonProperty("username")]
        public string Name { get; set; }
        
        /// <summary>
        /// Ranking on world leaderboards
        /// </summary>
        [JsonProperty("pp_rank")]
        public int Globalrank { get; set; }
        
        /// <summary>
        /// Ranking on country leaderboards
        /// </summary>
        [JsonProperty("pp_country_rank")]
        public int Countryrank { get; set; }
        
        /// <summary>
        /// Country where account is made or where player lives
        /// </summary>
        [JsonProperty("country")]
        public string CountryCode { get; set; }
        
        /// <summary>
        /// Raw performance points stat
        /// </summary>
        [JsonProperty("pp_raw")]
        public double Performance { get; set; }
        
        /// <summary>
        /// Current level of user
        /// </summary>
        [JsonProperty("level")]
        public double Level { get; set; }
        
        /// <summary>
        /// Cumulative ranked score
        /// </summary>
        [JsonProperty("ranked_score")]
        public string RankedScore { get; set; }
        
        /// <summary>
        /// Total score of all submitted plays
        /// </summary>
        [JsonProperty("total_score")]
        public string TotalScore { get; set; }
        
        /// <summary>
        /// Amount of seconds this user has played (based on all submitted plays)
        /// </summary>
        [JsonProperty("total_seconds_played")]
        public int Playtime { get; set; }
        
        /// <summary>
        /// Date and time the user registered
        /// </summary>
        [JsonProperty("join_date")]
        public string Joindate { get; set; }
        
        /// <summary>
        /// Amount of submitted plays
        /// </summary>
        [JsonProperty("playcount")]
        public int Playcount { get; set; }

        /// <summary>
        /// Amount of SS ranks
        /// </summary>
        [JsonProperty("count_rank_ss")] 
        public int CountRankSS { get; set; }

        /// <summary>
        /// Amount of silver SS ranks
        /// </summary>
        [JsonProperty("count_rank_ssh")] 
        public int CountRankSSH { get; set; }

        /// <summary>
        /// Amount of S ranks
        /// </summary>
        [JsonProperty("count_rank_s")] 
        public int CountRankS { get; set; }

        /// <summary>
        /// Amount of silver S ranks
        /// </summary>
        [JsonProperty("count_rank_sh")] 
        public int CountRankSH { get; set; }

        /// <summary>
        /// Amount of A ranks
        /// </summary>
        [JsonProperty("count_rank_a")] 
        public int CountRankA { get; set; }

        /// <summary>
        /// Global accuracy
        /// </summary>
        [JsonProperty("accuracy")]
        public double Accuracy { get; set; }

        /// <summary>
        /// Amount of 300 hits
        /// </summary>
        [JsonProperty("count300")]
        public double Hits300 { get; set; }

        /// <summary>
        /// Amount of 100 hits
        /// </summary>
        [JsonProperty("count100")]
        public double Hits100 { get; set; }

        /// <summary>
        /// Amount of 50 hits
        /// </summary>
        [JsonProperty("count50")]
        public double Hits50 { get; set; }


        /// <summary>
        /// Total SS ranks
        /// </summary>
        public int GetCountRankSS() => CountRankSS + CountRankSSH;

        /// <summary>
        /// Total S ranks
        /// </summary>
        public int GetCountRankS() => CountRankS + CountRankSH;

        /// <summary>
        /// Total A ranks
        /// </summary>
        public int GetCountRankA() => CountRankA;
    }
}