using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace Linear
{
    class Program
    {
        private const long M = 4294967296; // m is 2^32
        private const int A = 1664525;
        private const int C = 1013904223;
        private const int PLAYER_ID = 1836;
        private static int _last = 0;

        private static int Next()
        {
            _last = (int)((A * _last + C) % M);
            return _last;
        }

        private static string GetHttp(string request)
        {
            string str;
            using (StreamReader strr = new StreamReader(HttpWebRequest.Create(request).GetResponse().GetResponseStream()))
                str = strr.ReadToEnd();
            return str;
        }

        private static AccountResponse CreateAccount()
        {
            string response = GetHttp($"http://95.217.177.249/casino/createacc?id={PLAYER_ID}");
            return JsonConvert.DeserializeObject<AccountResponse>(response);
        }

        private static MakeBetResponse MakeBet(int betNumber)
        {
            string response = GetHttp($"http://95.217.177.249/casino/playLcg?id={PLAYER_ID}&bet=1&number={betNumber}");

            Console.WriteLine(response);
            return JsonConvert.DeserializeObject<MakeBetResponse>(response);
        }

        static void Main(string[] args)
        {
            //CreateAccount();

            MakeBetResponse makeBetResponse1 = MakeBet(0);
            MakeBetResponse makeBetResponse2 = MakeBet(0);
            MakeBetResponse makeBetResponse3 = MakeBet(0);

            Console.WriteLine(makeBetResponse1.RealNumber);
            Console.WriteLine(makeBetResponse2.RealNumber);
            Console.WriteLine(makeBetResponse3.RealNumber);
        }
    }
}
