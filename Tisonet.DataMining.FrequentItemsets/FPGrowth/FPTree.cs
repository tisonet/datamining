using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tisonet.DataMining.Domain.Itemsets;

namespace Tisonet.DataMining.FrequentItemsets.FPGrowth
{
    internal class FPTree : IFPTree
    {
        private readonly FrequentItemsCollection _frequentItems;
        private int _transactionsCount = 0;

        private readonly FPTreeHeader _header = new FPTreeHeader();
        private readonly FPTreeNode _rootNode = new FPTreeNode(FPTreeNodeType.Root);
        private readonly ItemsComparerBySupport _itemsComparer;
        private readonly List<FPTreeNode> _leafNodes = new List<FPTreeNode>();

        public FPTree()
        {

        }

        public FPTree(FrequentItemsCollection frequentItems)
        {
            _frequentItems = frequentItems;
            _itemsComparer = new ItemsComparerBySupport(_frequentItems);
        }

        public int TransactionsCount
        {
            get { return _transactionsCount; }
        }

        public void AddTransaction(Transaction transaction)
        {
            _transactionsCount++;

            AddTransactionNodes(transaction);
        }

        public FPTreeNode GetRootNode()
        {
            return _rootNode;
        }

        private void AddTransactionNodes(Transaction transaction)
        {
            FPTreeNode actualNode = _rootNode;

            var transactionItems = GetTransactionItems(transaction);

            for (int i = 0; i < transactionItems.Length; i++)
            {
                // Because we order transaction's items by support, 
                // we can break loop when we find the first non-frequent item in transaction.
                if (!IsFrequent(transactionItems[i]))
                {
                    break;
                }

                FPTreeNode subNode;

                if (actualNode.TryGetSubNode(transactionItems[i], out subNode))
                {
                    subNode.Support++;
                    actualNode = subNode;
                }
                else
                {
                    AddSubNodesForTransactionItems(actualNode, transactionItems, i);
                    break;
                }
            }
        }

        private uint[] GetTransactionItems(Transaction transaction)
        {
            if (_itemsComparer == null)
            {
                return transaction.Items;
            }
            else
            {
                // Order by support (descending)
                var transactionItems = transaction.Items.OrderByDescending(item => item, _itemsComparer).ToArray();

                return transactionItems;
            }


        }

        private bool IsFrequent(UInt32 item)
        {
            return _frequentItems == null || _frequentItems.IsFrequent(item);
        }

        private void AddSubNodesForTransactionItems(FPTreeNode parentNode, UInt32[] transaction, int transactionStartIndex)
        {
            FPTreeNode actualNode = parentNode;

            for (int i = transactionStartIndex; i < transaction.Length; i++)
            {
                actualNode = actualNode.AddSubNode(transaction[i]);

                _header.Add(actualNode);
            }

            // Last item is on leaf node, save it.
            _leafNodes.Add(actualNode);

        }

        #region IFPTree members

        public Itemset Itemset
        {
            get { return Itemset.Empty;  }
        }

        public IEnumerable<UInt32> Items
        {
            get { return _header.Items; }
        }

        public IEnumerable<FPTreePrefixPathItem> GetItemPrefixPath(UInt32 item)
        {
            return _header.GetItemPath(item);
        }

        #endregion


    }
}
