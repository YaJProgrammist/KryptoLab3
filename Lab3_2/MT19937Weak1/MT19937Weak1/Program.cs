using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace MT19937Weak1
{
    class Program
    {
        private const int PLAYER_ID = 18573;

        private static string GetHttp(string request)
        {
            string str;
            using (StreamReader strr = new StreamReader(WebRequest.Create(request).GetResponse().GetResponseStream()))
                str = strr.ReadToEnd();
            return str;
        }

        private static AccountResponse CreateAccount()
        {
            string response = GetHttp($"http://95.217.177.249/casino/createacc?id={PLAYER_ID}");
            return JsonConvert.DeserializeObject<AccountResponse>(response);
        }

        private static MakeBetResponse MakeBet(long betNumber)
        {
            string response = GetHttp($"http://95.217.177.249/casino/playMt?id={PLAYER_ID}&bet=1&number={betNumber}");

            Console.WriteLine(response);
            return JsonConvert.DeserializeObject<MakeBetResponse>(response);
        }

        static void Main(string[] args)
        {
            ulong seed = (ulong)DateTimeOffset.UtcNow.AddHours(-1).ToUnixTimeSeconds();

            CreateAccount();

            MT19937 MTGenerator = new MT19937();
            long next = 0;
            long realNext = MakeBet(0).RealNumber;

            while (realNext != next)
            {
                MTGenerator.init_genrand(seed);
                next = (long)MTGenerator.genrand_int32();
                Console.WriteLine(next);
                seed++;
            } 

            Console.ForegroundColor = ConsoleColor.Green;

            for (int i = 0; i <= 1500; i++)
            {
                next = (long)MTGenerator.genrand_int32();
                MakeBet(next);
            }

            Console.ReadLine();
        }
    }
}
