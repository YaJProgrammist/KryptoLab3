using System;
using System.Collections.Generic;
using System.Text;

namespace BetterMT19937
{
    public class MakeBetResponse
    {
        public string Message { get; set; }
        public AccountResponse Account { get; set; }
        public long RealNumber { get; set; }
    }
}
