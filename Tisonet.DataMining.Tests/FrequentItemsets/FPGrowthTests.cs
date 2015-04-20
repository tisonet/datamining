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
    public class FPGrowthTests
    {
        [Test]
        public void Can_Find_Frequent_Itemset_In_Small_DB_With_Support_2()
        {
            var miner = new FPGrowthImpl(SmallTransactionDatabase.CreateFPGrowthExample());

            miner.Run(new MiningOptions() {MinSupport = 2});

            Itemset[] result = miner.Result.ToArray();

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);

            Assert.AreEqual(19, result.Length);
        }

    }
}
