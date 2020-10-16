using System;
using System.Collections.Generic;

namespace Explorer.Service.Contract.Config
{
    public sealed class NgkConfig
    {
        public string ChainId { get; set; }
        public List<NodeConfig> Nodes { get; set; }
        public List<ProducerNodeConfig> ProducerNodes { get; set; }
    }

    [Serializable]
    public class NodeConfig
    {
        public string HttpAddress { get; set; }
        public int TimeOut { get; set; }
    }

    [Serializable]
    public sealed class ProducerNodeConfig : NodeConfig
    {
        public string Owner { get; set; }
    }

    public sealed class ContractConfig
    {
        public string Name { get; set; }
        public string Actions { get; set; }
    }
}