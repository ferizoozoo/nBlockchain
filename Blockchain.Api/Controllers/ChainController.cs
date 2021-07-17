using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Blockchain.Core.Models;


namespace Blockchain.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChainController : ControllerBase
    {
        private readonly Blockchain.Core.Blockchain _blockchain;

        public ChainController(Blockchain.Core.Blockchain blockchain)
        {
            _blockchain = blockchain;
        }

        [HttpGet]
        public List<Block> Get()
        {
            return _blockchain.Chain;
        }
    }
}
