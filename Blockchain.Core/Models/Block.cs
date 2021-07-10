using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System;
using System.Collections.Generic;

namespace Blockchain.Core.Models
{
    public class Block
    {
        public int Id { get; set; }
        public string PreviousHash { get; set; }
        public string HashOfBlock { get; set; }
        public DateTime TimeStamp { get; set; }
        public List<Transaction> Transactions { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(Id);
            builder.Append(PreviousHash);
            builder.Append(TimeStamp.ToString());

            Transactions.ForEach((trx) => builder.Append(trx));

            return builder.ToString();
        }
    }
}