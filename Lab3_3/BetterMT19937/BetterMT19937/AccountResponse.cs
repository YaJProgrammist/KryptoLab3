using System;
using System.Collections.Generic;
using System.Text;

namespace BetterMT19937
{
    public class AccountResponse
    {
        public string Id { get; }
        public int Money { get; }
        public DateTimeOffset DeletionTime { get; }
    }
}
