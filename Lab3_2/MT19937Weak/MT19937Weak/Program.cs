using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace MT19937Weak
{
    class Program
    {
        private const int PLAYER_ID = 1853;

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
            CreateAccount();

            MT19937 MT;
            long number = 0;
            MakeBetResponse response = MakeBet(0);
            DateTimeOffset seed = response.Account.DeletionTime.ToUnixTimeSeconds();
            DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            do
            {
                MT = new MT19937();
                MT.init_genrand(seed.UtcNow.ToUnixTimeSeconds());
                number = (long)MT.genrand_int32();
                seed++;
            } while (response.RealNumber != number);

            Console.ForegroundColor = ConsoleColor.Green;

            while (response.Account.Money < 1000000)
            {
                number = (long)MT.genrand_int32();
                response = MakeBet(number);
            }
        }
    }
}
