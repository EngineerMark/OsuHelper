using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuHelper.Tests
{
    public class Program
    {
        private static void Main(string[] args)
        {
            string t = OsuApiHelper.APIHelper<string>.GetDataFromWeb("google.com");
            //OsuApiHelper.OsuApiKey.Key = "Secret";
            //bool keyTest = OsuApiHelper.OsuApi.IsKeyValid();
            //if(keyTest){
            //    Console.WriteLine("Valid key");
            //    bool userTest = OsuApiHelper.OsuApi.IsUserValid("Amayakase");
            //    if(userTest){
            //        Console.WriteLine("Valid user");
            //    }else{
            //        Console.WriteLine("Invalid user");
            //    }
            //}
            //else{
            //    Console.WriteLine("Invalid key");
            //}
            Console.ReadLine();
        }
    }
}
