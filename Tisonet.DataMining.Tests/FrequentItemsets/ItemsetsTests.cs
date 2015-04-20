using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Tisonet.DataMining.Domain;
using Tisonet.DataMining.Domain.Itemsets;
using Tisonet.DataMining.FrequentItemsets;
using Tisonet.DataMining.FrequentItemsets.Extensions;

namespace Tisonet.DataMining.Tests.FrequentItemsets
{
    [TestFixture]
    public class ItemsetsTests
    {
        [Test]
        public void Can_Generate_SubItemset_From_2_Length_Itemset()
        {
            Itemset itemset = new Itemset(new uint[] { 0, 1 });

            Itemset[] result = ItemsetHelper.GenerateOneLevelSubItemsets(itemset).ToArray();

            Assert.AreEqual(2, result.Length);

            Assert.AreEqual(1, result[0].Length);
            Assert.AreEqual(1, result[1].Length);

            Assert.AreEqual(result[0][0], 0);
            Assert.AreEqual(result[1][0], 1);
        }

        [Test]
        public void Can_Generate_SubItemset_From_3_Length_Itemset()
        {
            Itemset itemset = Itemset.Create(0, 1, 2 );

            Itemset[] result = ItemsetHelper.GenerateOneLevelSubItemsets(itemset).ToArray();

            Assert.AreEqual(3, result.Length);

            Assert.AreEqual(2, result[0].Length);
            Assert.AreEqual(2, result[1].Length);
            Assert.AreEqual(2, result[2].Length);

            Assert.AreEqual(result[0][0], 0);
            Assert.AreEqual(result[0][1], 1);
            
            Assert.AreEqual(result[1][0], 0);
            Assert.AreEqual(result[1][1], 2);

            Assert.AreEqual(result[2][0], 1);
            Assert.AreEqual(result[2][1], 2);
        }

     
        [Test]
        public void Can_Calculate_SubItemset()
        {
            bool result = ItemsetHelper.IsSubset(Itemset.Create(1), Itemset.Create(2));
            Assert.AreEqual(false, result);

            result = ItemsetHelper.IsSubset(Itemset.Create(1, 2), Itemset.Create(2));
            Assert.AreEqual(true, result);

            result = ItemsetHelper.IsSubset(Itemset.Create(1, 2, 3), Itemset.Create(2));
            Assert.AreEqual(true, result);

            result = ItemsetHelper.IsSubset(Itemset.Create(1, 2, 3), Itemset.Create(2, 3));
            Assert.AreEqual(true, result);

            result = ItemsetHelper.IsSubset(Itemset.Create(1, 2, 3), Itemset.Create(2, 3, 4));
            Assert.AreEqual(false, result);
          
        }

        [Test]
        public void Can_Enumerate_All_3_Length_SubItemsets()
        {
            Itemset itemset = new Itemset(new uint[] { 1, 2, 3, 5, 6 });

            UInt32[][] result = ItemsetHelper.EnumerateSubItemsets(itemset.Items, 3).ToArray();

            Assert.AreEqual(10, result.Length);

            // Length
            Assert.AreEqual(3, result[0].Length);
            Assert.AreEqual(3, result[9].Length);

            // SubItemsets
            Assert.AreEqual(result[0][0], 1);
            Assert.AreEqual(result[0][1], 2);
            Assert.AreEqual(result[0][2], 3);

            Assert.AreEqual(result[9][0], 3);
            Assert.AreEqual(result[9][1], 5);
            Assert.AreEqual(result[9][2], 6);
        }


        [Test]
        public void Can_Generate_All_3_Length_SubItemsets()
        {
            Itemset itemset = new Itemset(new uint[] { 1, 2, 3, 5, 6 });

            var result = ItemsetHelper.GenerateSubItemsets(itemset.Items, 3);

            Assert.AreEqual(10, result.Count);

            // Length
            Assert.AreEqual(3, result[0].Length);
            Assert.AreEqual(3, result[9].Length);

            // SubItemsets
            Assert.AreEqual(result[0][0], 1);
            Assert.AreEqual(result[0][1], 2);
            Assert.AreEqual(result[0][2], 3);

            Assert.AreEqual(result[9][0], 3);
            Assert.AreEqual(result[9][1], 5);
            Assert.AreEqual(result[9][2], 6);
        }
    }
}
