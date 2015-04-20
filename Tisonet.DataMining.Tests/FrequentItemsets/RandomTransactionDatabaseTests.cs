using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Tisonet.DataMining.DataSets.Itemsets;
using Tisonet.DataMining.Domain.Itemsets;

namespace Tisonet.DataMining.Tests.FrequentItemsets
{
    [TestFixture]
    public class RandomTransactionDatabaseTests
    {
        [Test]
        public void Can_Generate_Random_DB_With_Corrent_Size()
        {
            const int DB_SIZE = 24;

            RandomTransactionDatabase db = new RandomTransactionDatabase(20, 1, 10, DB_SIZE);

            Transaction[] result = db.Transactions.ToArray();

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(DB_SIZE, result.Length);
        }

        [Test]
        public void Can_Generate_Random_DB_With_Corrent_Items()
        {
            const int ITEMS = 100;

            RandomTransactionDatabase db = new RandomTransactionDatabase(ITEMS, 1, 10, 100);

            Transaction[] trans = db.Transactions.ToArray();
            int result = trans.SelectMany(t => t.Items).Distinct().Count();

          
            Assert.IsNotNull(result);
            Assert.IsTrue(ITEMS >= result);
        }
    }
}
