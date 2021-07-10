using System;
using System.Collections.Generic;
using Blockchain.Core.Models;
using Blockchain.Utilities;

namespace Blockchain.Core
{
    public class Blockchain
    {
        private readonly object _lock = new object();

        public int NodeId { get; set; }
        private List<Block> _chain;
        private List<Transaction> _memPool;
        private List<Node> _nodes;
        private int nonce;
        private Block _lastBlock => _chain[_chain.Count - 1];

        public Blockchain()
        {
            // Create the Genesis block & append it to the chain
        }

        public bool ValidateChain()
        {
            for (int index = 0; index < _chain.Count - 1; index++)
            {
                var currentBlock = _chain[index];
                var nextBlock = _chain[index + 1];

                if (currentBlock.HashOfBlock != nextBlock.PreviousHash)
                    return false;
            }

            return true;
        }

        public void AddTransaction(Transaction trx)
        {
            lock(_lock)
            {
                _memPool.Add(trx);
            } 
        }

        public void AddBlock(Block block)
        {
            _chain.Add(block);
        }

        public void AddNode(Node node)
        {
            _nodes.Add(node);
        }

        public string POW(string data)
        {
            var hashOfThisBlock = Hash.CreateHash(data);

            while (!hashOfThisBlock.StartsWith("0"))
            {
                
            }

            return null;
        }

        public void Consensus()
        {
            // Get the other nodes' chains

            // Check the length of chains

            // Set the chain of this node and every other nodes to the longest chain discovered
        }

        public void Mine()
        {
            var newBlock = new Block();

            // Add reward transaction
            var rewardTransaction = new Transaction {
                Sender = NodeId.ToString(),
                Recipient = null,
                Amount = 1
            };
            AddTransaction(rewardTransaction);

            // Create the new block
            newBlock.Transactions = _memPool;
            newBlock.TimeStamp = DateTime.UtcNow;
            newBlock.PreviousHash = _lastBlock.HashOfBlock;
            newBlock.Id = _lastBlock.Id + 1;
            newBlock.HashOfBlock = Hash.CreateHash(newBlock.ToString());

            // Clear the MemPool
            _memPool = new List<Transaction>();

        }
    }
}