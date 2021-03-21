namespace OsuApiHelper
{
    /// <summary>
    /// Static class that holds the api key
    /// </summary>
    public static partial class OsuApiKey
    {
        /// <summary>
        /// Important: supply an valid osu API key here in order to use all the functions
        /// </summary>
        public static string Key = "";
        
        //INFO: Create an seperate cs file which is also an partial APIKeys class, store keys in there and ignore it from git.
        //This way u keep api keys in the project but safe from the repository.
    }
}