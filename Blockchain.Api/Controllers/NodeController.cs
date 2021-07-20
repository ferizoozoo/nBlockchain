using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Blockchain.Core.Models;
using System.Net;
using Microsoft.AspNetCore.Http.Features;

namespace Blockchain.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NodeController : ControllerBase
    {
        private readonly Blockchain.Core.Blockchain _blockchain;

        public NodeController(Blockchain.Core.Blockchain blockchain)
        {
            _blockchain = blockchain;
        }

        [HttpGet]
        public Node Get()
        {
            return _blockchain.Nodes[0];
        }

        [HttpGet("register")]
        public Uri Register()
        {
            var urlString = HttpContext.Features.Get<IHttpConnectionFeature>().LocalIpAddress?.MapToIPv4().ToString(); 
            var url = "http://" + urlString;
            var node = new Node { NodeAddress = new Uri(url) };
            _blockchain.AddNode(node);
            return node.NodeAddress;
        }
    }
}
