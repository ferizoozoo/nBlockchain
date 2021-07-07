using System;
using System.Collections.Generic;
using Blockchain.Core.Models;

namespace Blockchain.Core
{
    public class Blockchain
    {
        public List<Block> Chain { get; set; }
        public List<Transaction> MemPool { get; set; }

        public Blockchain()
        {

        }

        public bool ValidateChain()
        {
            for (int index = 0; index < Chain.Count - 1; index++)
            {
                var currentBlock = Chain[index];
                var nextBlock = Chain[index + 1];

                if (currentBlock.HashOfBlock != nextBlock.PreviousHash)
                    return false;
            }

            return true;
        }
    }
}