using System;

namespace Blockchain.Core.Models
{
    public class Block
    {
        public int Id { get; set; }
        public string PreviousHash { get; set; }
        public string HashOfBlock { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}