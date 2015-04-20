using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tisonet.DataMining.FrequentItemsets.FPGrowth
{
    public enum FPTreeNodeType
    {
        Root,
        Node
    }

    public class FPTreeNode
    {
        private readonly IDictionary<UInt32, FPTreeNode> _subNodes = new Dictionary<UInt32, FPTreeNode>(); 

        public FPTreeNode(FPTreeNodeType type)
        {
            Type = type;
        }

        private FPTreeNode()
        {
            
        }

        public FPTreeNodeType Type { get; private set; }
        public int Support { get; set; }
        public UInt32 Item { get; internal set; }
        public FPTreeNode ParentNode { get; internal set; }
        public IEnumerable<FPTreeNode> SubNodes { get { return _subNodes.Values; } }
        public Boolean Ignored { get; internal set; }
        public Boolean IsLeafNode { get { return _subNodes.Count == 0; } }


        public bool TryGetSubNode(UInt32 item, out FPTreeNode subNode)
        {
            return _subNodes.TryGetValue(item, out subNode);
        }


        internal FPTreeNode AddSubNode(UInt32 item)
        {
            FPTreeNode subNode = new FPTreeNode
            {
                Support = 1,
                Item = item,
                Type = FPTreeNodeType.Node,
                ParentNode = this
            };

            _subNodes.Add(item, subNode);

            return subNode;
        }

        internal FPTreeNode GetSubNode(UInt32 item)
        {
            return _subNodes[item];
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}, childs: {2}", Type, Item, _subNodes.Count);
        }
    }
}
