using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Tisonet.DataMining.FrequentItemsets.FPGrowth;
using Moq;

namespace Tisonet.DataMining.Tests.FrequentItemsets
{
    [TestFixture]
    public class ConditionalFPTreeTests
    {
        [Test]
        public void AddTransactione_OneTransactionAdded_ReturnPathWithOneItem()
        {
            ConditionalFPTree cTree = new ConditionalFPTree(5);
            FPTreeNode transaction = CreateTransaction(1, 3, 4);

            cTree.AddTransaction(transaction, 1);
            var result = cTree.GetItemPrefixPath(4);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void AddTransaction_OneTransactionAdded_ContainsAddedTransaction()
        {
            ConditionalFPTree cTree = new ConditionalFPTree(5);
            FPTreeNode transaction = CreateTransaction(1, 3, 4);

            cTree.AddTransaction(transaction, 1);
            var result = cTree.GetItemPrefixPath(4).ToArray();

           Assert.AreEqual(4, result[0].FPTreeNode.Item);
           Assert.AreEqual(3, result[0].FPTreeNode.ParentNode.Item);
           Assert.AreEqual(1, result[0].FPTreeNode.ParentNode.ParentNode.Item);
           Assert.AreEqual(FPTreeNodeType.Root, result[0].FPTreeNode.ParentNode.ParentNode.ParentNode.Type);
        }

        [Test]
        public void AddTransaction_TwoTransactionAddedWithCommonPath_CommonPathHasCorrectSupport()
        {
            ConditionalFPTree cTree = new ConditionalFPTree(0);
            FPTreeNode[] transactions = CreateTransactionWithCommonPath( new uint[] { 1 }, new uint[] { 3, 4, 5}, new uint[] { 4, 5} );

            cTree.AddTransaction(transactions[0], 1);
            cTree.AddTransaction(transactions[1], 1);
            
            var result = cTree.GetItemPrefixPath(1).ToArray();

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(1, result[0].FPTreeNode.Item);
            Assert.AreEqual(2, result[0].FPTreeNode.Support);
        }

        [Test]
        public void LeafItems_NoTransaction_RetunrsEmptyCollection()
        {
            ConditionalFPTree cTree = new ConditionalFPTree(5);

            var result = cTree.Items.ToArray();

            Assert.AreEqual(0, result.Length);
        }

      

        [Test]
        public void RemoveUnfrequentItems_NoUnfrequentItemsAdded_AllItemsPreserve()
        {
            ConditionalFPTree cTree = new ConditionalFPTree(5);
            FPTreeNode transaction = CreateTransaction(1, 3, 4);
            cTree.AddTransaction(transaction, 1);

            var result = cTree.RemoveInfrequentItems(1);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void RemoveUnfrequentItems_OneFrequentItem_PreserveOneItem()
        {
            ConditionalFPTree cTree = new ConditionalFPTree(0);
            FPTreeNode[] transactions = CreateTransactionWithCommonPath(new uint[] { 1 }, new uint[] { 3, 4, 5 }, new uint[] { 6, 7 });

            cTree.AddTransaction(transactions[0], 1);
            cTree.AddTransaction(transactions[1], 1);
            
            var result = cTree.RemoveInfrequentItems(2);

            Assert.AreEqual(5, result);
        }


       


        private static FPTreeNode CreateTransaction(params UInt32[] items)
        {
            FPTreeNode parent = new FPTreeNode(FPTreeNodeType.Root);
            FPTreeNode sub = parent;
            foreach (var item in items)
            {
                sub = sub.AddSubNode(item);
            }
            
            return sub;
        }

        private static FPTreeNode[] CreateTransactionWithCommonPath(UInt32[] commonItemsPrefix, UInt32[] suffix1, UInt32[] suffix2)
        {
            FPTreeNode parent = new FPTreeNode(FPTreeNodeType.Root);
            FPTreeNode sub = parent;

            foreach (var item in commonItemsPrefix)
            {
                sub = sub.AddSubNode(item);
            }

            FPTreeNode nodes1 = sub;

            foreach (var item in suffix1)
            {
                nodes1 = nodes1.AddSubNode(item);
            }

            FPTreeNode nodes2= sub;

            foreach (var item in suffix2)
            {
               nodes2 = nodes2.AddSubNode(item);
            }

            return new FPTreeNode[] { nodes1, nodes2 };
        }
    }
}
