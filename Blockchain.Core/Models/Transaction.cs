using System;

namespace Blockchain.Core.Models
{
    public class Transaction
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public int Amount { get; set; }
    }
}