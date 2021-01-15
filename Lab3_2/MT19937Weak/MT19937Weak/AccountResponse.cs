using System;
using System.Collections.Generic;
using System.Text;

namespace MT19937Weak
{
    public class AccountResponse
    {
        public string Id { get; }
        public int Money { get; }
        public DateTimeOffset DeletionTime { get; }
    }
}
