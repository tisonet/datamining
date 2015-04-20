using System;
using System.Collections.Generic;
using System.Linq;
using Tisonet.DataMining.Domain.Itemsets;
using Tisonet.DataMining.Extensions;

namespace Tisonet.DataMining.FrequentItemsets.FPGrowth
{
    internal class ConditionalFPTree : IFPTree
    {
        private readonly Dictionary<FPTreeNode, FPTreeNode> _visitedNodesMapping = new Dictionary<FPTreeNode, FPTreeNode>(); 
        private readonly FPTreeHeader _header = new FPTreeHeader();

        public ConditionalFPTree(UInt32 item)
        {
            Itemset = new Itemset(item);
        } 

        public ConditionalFPTree(UInt32 item, UInt32[] suffix)
        {
            Itemset = new Itemset(suffix.Add(item), true);
        }

        public Int32 Support
        {
            get { return Itemset.Support; } 
            
            internal set { Itemset.Support = value; }
        }

        #region IFPTree members

        public Itemset Itemset { get; private set; }
        
        public IEnumerable<uint> Items 
        {
            get { return _header.Items; }
        } 

        public IEnumerable<FPTreePrefixPathItem> GetItemPrefixPath(uint item)
        {
            return _header.GetItemPath(item);
        }

        #endregion

        internal void AddTransaction(FPTreeNode fPTreeNode, Int32 support)
        {
            if (fPTreeNode.Type == FPTreeNodeType.Root) return;

            FPTreeNode curretNodeInConditioanlTree  = null;
            FPTreeNode currentNodeInInputFpTree = fPTreeNode;

            // We are navigatin from botton to up in origin transaction and creating a new one.
            while (currentNodeInInputFpTree.Type != FPTreeNodeType.Root)
            {
                bool existingNode;
                FPTreeNode node = FindOrCreateNode(currentNodeInInputFpTree, out existingNode);
                node.Support += support;

                if (curretNodeInConditioanlTree != null)
                {
                    curretNodeInConditioanlTree.ParentNode = node;
                }
               
                if (!existingNode)
                {
                    _visitedNodesMapping.Add(currentNodeInInputFpTree, node);
                    _header.Add(node);
                }



                // TODO: When we have found already visited node, we can traverse it to up and just increment support.

                // Move upward in tree traversing;
                curretNodeInConditioanlTree = node;                
                currentNodeInInputFpTree = currentNodeInInputFpTree.ParentNode;
            }


            curretNodeInConditioanlTree.ParentNode = currentNodeInInputFpTree;

        }

        internal Int32 RemoveInfrequentItems(Int32 minSupport)
        {
            List<UInt32> itemsToRemoved = new List<UInt32>();

            foreach (var item in _header.Items)
            {
                var itemSupport = _header.GetItemPath(item).Sum(itemNode => itemNode.FPTreeNode.Support);

                if (itemSupport < minSupport)
                {
                    itemsToRemoved.Add(item);
                }
            }

            foreach (var itemToRemoved in itemsToRemoved)
            {
                _header.Remove(itemToRemoved);   
            }

            return itemsToRemoved.Count;
        }

        private FPTreeNode FindOrCreateNode(FPTreeNode actualTreeNode, out bool found)
        {
            FPTreeNode existingNode;
            found = _visitedNodesMapping.TryGetValue(actualTreeNode, out existingNode);

            return found ? existingNode : new FPTreeNode(FPTreeNodeType.Node) {Item  = actualTreeNode.Item};
        }
    }
}
