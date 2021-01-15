using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BetterMT19937
{
    class Program
    {
        private const int PLAYER_ID = 19591;

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
            string response = GetHttp($"http://95.217.177.249/casino/playBetterMt?id={PLAYER_ID}&bet=1&number={betNumber}");

            Console.WriteLine(response);
            return JsonConvert.DeserializeObject<MakeBetResponse>(response);
        }

        private static ulong Untemper(ulong state)
        {
            state ^= (state >> 18);
            state ^= (state << 15) & 0xefc60000UL;
            state ^= ((state << 7) & 0x9d2c5680UL) ^
                ((state << 14) & 0x94284000UL) ^
                ((state << 21) & 0x14200000UL) ^
                ((state << 28) & 0x10000000UL);
            state ^= (state >> 11) ^ (state >> 22);

            return state;
        }

        static void Main(string[] args)
        {
            CreateAccount();

            List<ulong> numbers = new List<ulong>();

            for (int i = 0; i < 624; i++)
            {
                numbers.Add(Untemper((ulong)MakeBet(0).RealNumber));
            }

            MT19937 MTGenerator = new MT19937();
            MTGenerator.init_genrand(numbers.ToArray());

            Console.ForegroundColor = ConsoleColor.Green;

            for (int i = 0; i <= 1000; i++)
            {
                long next = (long)MTGenerator.genrand_int32();
                MakeBet(next);
            }

            Console.ReadLine();
        }
    }
}
