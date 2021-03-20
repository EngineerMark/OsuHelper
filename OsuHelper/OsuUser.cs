using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace OsuApiHelper
{
    public struct Userpage
    {
        public PlayHistory PlayHistory;
    }

    public struct PlayHistory
    {
        public PlayHistoryNode[] HistoryNodes;
    }

    public struct PlayHistoryNode
    {
        public string Month;
        public string Playtime;
    }
    
    public class OsuUser
    {
        [JsonProperty("user_id")]
        public int ID { get; set; }
        
        [JsonProperty("username")]
        public string Name { get; set; }
        
        [JsonProperty("pp_rank")]
        public int Globalrank { get; set; }
        
        [JsonProperty("pp_country_rank")]
        public int Countryrank { get; set; }
        
        [JsonProperty("country")]
        public string CountryCode { get; set; }
        
        [JsonProperty("pp_raw")]
        public float Performance { get; set; }
        
        [JsonProperty("level")]
        public float Level { get; set; }
        
        [JsonProperty("ranked_score")]
        public string RankedScore { get; set; }
        
        [JsonProperty("total_score")]
        public string TotalScore { get; set; }
        
        [JsonProperty("total_seconds_played")]
        public int Playtime { get; set; }
        
        [JsonProperty("join_date")]
        public string Joindate { get; set; }
        
        [JsonProperty("playcount")]
        public int Playcount { get; set; }

        [JsonProperty("count_rank_ss")] 
        public int CountRankSS { get; set; }
        
        [JsonProperty("count_rank_ssh")] 
        public int CountRankSSH { get; set; }
        
        [JsonProperty("count_rank_s")] 
        public int CountRankS { get; set; }
        
        [JsonProperty("count_rank_sh")] 
        public int CountRankSH { get; set; }
        
        [JsonProperty("count_rank_a")] 
        public int CountRankA { get; set; }

        public int GetCountRankSS() => CountRankSS + CountRankSSH;
        public int GetCountRankS() => CountRankS + CountRankSH;
        public int GetCountRankA() => CountRankA;
    }
}