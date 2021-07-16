using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using Blockchain.Core.Models;
using Blockchain.Utilities;
using Newtonsoft.Json;

namespace Blockchain.Core
{
    public class Blockchain
    {
        private readonly object _lock = new object();

        public int NodeId { get; set; }
        private List<Block> _chain;
        private List<Transaction> _memPool;
        private List<Node> _nodes;
        private int _proofOfWork = 1;
        private Block _lastBlock => _chain[_chain.Count - 1];

        public Blockchain()
        {
            // Create the Genesis block & append it to the chain
            _chain.Add(new Block
            {
                HashOfBlock = "1",
                TimeStamp = DateTime.UtcNow,
            });
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
            lock (_lock)
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

        public void RegisterNodes(List<string> nodeAddresses)
        {
            foreach (var nodeAddress in nodeAddresses)
                AddNode(new Node { NodeAddress = new Uri($"http://{nodeAddress}") });
        }

        public void SetValidPOW()
        {
            if (_chain.Count % 10 == 0)
                _proofOfWork += 1;
        }

        public Block POW(Block block)
        {
            var hashOfThisBlock = Hash.CreateHash(block.ToString());

            // Set the correct proofOfWork before working with it
            SetValidPOW();
            var zerosInFrontOfHash = String.Concat(Enumerable.Repeat("0", _proofOfWork));

            while (!hashOfThisBlock.StartsWith(zerosInFrontOfHash))
            {
                block.Nonce += 1;
                hashOfThisBlock = Hash.CreateHash(block.ToString());
            }

            return block;
        }

        public void Consensus()
        {
            // Get the other nodes' chains
            foreach (var node in _nodes)
            {
                var urlOfNode = new Uri(node.NodeAddress, "/chain");
                var request = (HttpWebRequest)WebRequest.Create(urlOfNode);
                var response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var model = new
                    {
                        chain = new List<Block>(),
                        length = 0
                    };

                    var neighbourChain = JsonConvert.DeserializeAnonymousType(
                        new StreamReader(response.GetResponseStream()).ReadToEnd(),
                        model
                    );


                    // Check the length of chains
                    if (neighbourChain.chain.Count > _chain.Count)
                    {
                        // Set the chain of this node and every other nodes to the longest chain discovered
                        _chain = neighbourChain.chain;
                    }
                }
            }
        }

        public Block Mine()
        {
            var newBlock = new Block();

            // Add reward transaction
            var rewardTransaction = new Transaction
            {
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

            // Mine the correct block with proper nonce by using the POW algorithm
            var correctBlock = POW(newBlock);
            _chain.Add(correctBlock);

            return correctBlock;
        }
    }
}