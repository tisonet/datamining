using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Tisonet.DataMining.Domain.Itemsets;
using Tisonet.DataMining.FrequentItemsets.FPGrowth;

namespace Tisonet.DataMining.Tests.FrequentItemsets
{
    [TestFixture]
    public class FPTreeTests
    {
        [Test]
        public void New_ZeroTransactionsInTree()
        {
            FPTree tree = new FPTree();

            Assert.AreEqual(0, tree.TransactionsCount);
        }

        [Test]
        public void Add_FirstTransaction_ReturnOneTransactionsCount()
        {
            FPTree tree = new FPTree();
            
            tree.AddTransaction(Transaction.Create(1,2,3));

            Assert.AreEqual(1, tree.TransactionsCount);
        }

        [Test]
        public void GetHeaderItemPath_TwoTransactionWithTheSameItemOnDifferentPositionAdded_ReturnsPathWithTwoItems()
        {
            FPTree tree = new FPTree();
            tree.AddTransaction(Transaction.Create(1, 2, 3));
            tree.AddTransaction(Transaction.Create(2, 3, 4));

            FPTreePrefixPathItem[] path = tree.GetItemPrefixPath(2).ToArray();

            Assert.AreEqual(2, path.Length);
            Assert.AreEqual(1, path[0].FPTreeNode.Support);
            Assert.AreEqual(1, path[1].FPTreeNode.Support);
        }

       

        [Test]
        public void GetHeaderItemPath_TwoSameTransactionAdded_AllPathHaveOneItem()
        {
            FPTree tree = new FPTree();
            UInt32[] items = {1, 2, 3, 4};

            tree.AddTransaction(Transaction.Create(items));
            tree.AddTransaction(Transaction.Create(items));

            foreach (var item in items)
            {
                FPTreePrefixPathItem[] itemPath = tree.GetItemPrefixPath(item).ToArray();

                Assert.AreEqual(1, itemPath.Length);
                Assert.AreEqual(2, itemPath[0].FPTreeNode.Support);
            }
        }

        [Test]
        public void GetRootNode_OneTransactionAdded_RootNodeContainsItemWithHigherSupport()
        {

            FrequentItemsCollection frequentItems = new FrequentItemsCollection(new[]
            {
                new KeyValuePair<uint, Itemset>(1 , Itemset.CreateWithSupport(1, 40)),
                new KeyValuePair<uint, Itemset>(2 , Itemset.CreateWithSupport(2, 100)),
                new KeyValuePair<uint, Itemset>(3 , Itemset.CreateWithSupport(3, 30)),
                new KeyValuePair<uint, Itemset>(4 , Itemset.CreateWithSupport(4, 30))
 
            });

            FPTree tree = new FPTree(frequentItems);
            tree.AddTransaction(Transaction.Create(1 , 2, 3 , 4));

            FPTreeNode rootNode = tree.GetRootNode();
           
            Assert.IsNotNull(rootNode.GetSubNode(2));
        }

        [Test]
        public void GetItemPrefixPath_TwoTransactionWithTheSamePrefixAdded_ReturnsPathWithOneItem()
        {
            FPTree tree = new FPTree();
            tree.AddTransaction(Transaction.Create(2, 3));
            tree.AddTransaction(Transaction.Create(2, 3, 4));

            FPTreePrefixPathItem[] path = tree.GetItemPrefixPath(2).ToArray();

            Assert.AreEqual(1, path.Length);
            Assert.AreEqual(2, path[0].FPTreeNode.Support);
        }

        [Test]
        public void GetItemPrefixPath_OneTransactionAdded_CanTraverseTransaction()
        {
            FPTree tree = new FPTree();
            UInt32[] items = { 1, 2, 3, 4, 5 };
            tree.AddTransaction(Transaction.Create(items));

            FPTreeNode leafNode = tree.GetItemPrefixPath(5).ToArray()[0].FPTreeNode;

            Assert.AreEqual(5, leafNode.Item);
            Assert.AreEqual(4, leafNode.ParentNode.Item);
            Assert.AreEqual(3, leafNode.ParentNode.ParentNode.Item);
            Assert.AreEqual(2, leafNode.ParentNode.ParentNode.ParentNode.Item);
            Assert.AreEqual(1, leafNode.ParentNode.ParentNode.ParentNode.ParentNode.Item);
            Assert.AreEqual(FPTreeNodeType.Root, leafNode.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode.Type);
        }



    }
}
