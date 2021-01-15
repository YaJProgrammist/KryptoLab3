using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace Linear
{
    class Program
    {
        private const long M = 4294967296; // m is 2^32
        private const int PLAYER_ID = 1837;
        private static long a = 0;
        private static long c = 0;
        private static long _last = 0;

        private static long Next(long last)
        {
            last = (a * last + c) % M;
            if (last > int.MaxValue || last < int.MinValue)
                last = last > 0 ? last - M : last + M;
            return last;
        }

        public static long ModularInverse(long a)
        {
            long m = M;
            (long x, long y) = (1, 0);

            while (a > 1)
            {
                long q = a / m;
                (a, m) = (m, a % m);
                (x, y) = (y, x - q * y);
            }
            return x < 0 ? x + M : x;
        }

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
            string response = GetHttp($"http://95.217.177.249/casino/playLcg?id={PLAYER_ID}&bet=1&number={betNumber}");

            Console.WriteLine(response);
            return JsonConvert.DeserializeObject<MakeBetResponse>(response);
        }

        private static void CalcCoeffs(int number0, int number1, int number2)
        {
            a = (number2 - number1) * ModularInverse(number1 - number0) % M;
            c = (number1 - a * number0) % M;
        }

        static void Main(string[] args)
        {
            //CreateAccount();

            int number0 = MakeBet(0).RealNumber;
            int number1 = MakeBet(0).RealNumber;
            int number2 = MakeBet(0).RealNumber;

            while (true)
            {
                CalcCoeffs(number0, number1, number2);

                _last = Next(number2);
                int realNext = MakeBet(_last).RealNumber;
                Console.WriteLine("=============== {0} ==============", _last);
                if (realNext == _last)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                }

                number0 = number1;
                number1 = number2;
                number2 = realNext;
            }

            for (int i = 0; i <= 1000; i++)
            {
                _last = Next(_last);
                MakeBet(_last);
            }
        }
    }
}
