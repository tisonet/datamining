using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Tisonet.DataMining.Domain;
using Tisonet.DataMining.Domain.Itemsets;
using Tisonet.DataMining.FrequentItemsets;
using Tisonet.DataMining.FrequentItemsets.Apriori;

namespace Tisonet.DataMining.Tests.FrequentItemsets
{
    [TestFixture]
    public class AprioriTests
    {
        [Test]
        public void Can_Pruned_3_Length_Itemset_According_To_Apriori()
        {
            Itemset[] frequentItemset = new[]
            {
                Itemset.Create(1, 2 ), 
                Itemset.Create(1, 3 ), 
                Itemset.Create(1, 5 ), 
                Itemset.Create(2, 3 ), 
                Itemset.Create(2, 4 ), 
                Itemset.Create(2, 5 )
            };

            Itemset[] allcandidates = new[]
            {
                Itemset.Create(1, 2, 3 ), 
                Itemset.Create(1, 2, 5 ), 
                Itemset.Create(1, 3, 5 ), 
                Itemset.Create(2, 3, 4 ), 
                Itemset.Create(2, 3, 5 ), 
                Itemset.Create(2, 4, 5 )
            };


            Itemset[] result = AprioriImpl.PruneCandidatesByApriori(allcandidates, frequentItemset);

            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(Itemset.Create(1, 2, 3), result[0]);
            Assert.AreEqual(Itemset.Create(1, 2, 5), result[1]);
        }

        [Test]
        public void Can_Pruned_4_Length_Itemset_According_To_Apriori()
        {
            Itemset[] frequentItemset = new[]
            {
                Itemset.Create(1,2,3),
                Itemset.Create(1,2,5)
            };

            Itemset[] allcandidates = new[]
            {
                 Itemset.Create(1, 2, 3, 5 )
            };

            Itemset[] result = AprioriImpl.PruneCandidatesByApriori(allcandidates, frequentItemset);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void Can_Generate_Candidates_From_1_Length_Itemset()
        {
            Itemset[] itemsets = new[]
            {
                 Itemset.Create(1),
                 Itemset.Create(2),
                 Itemset.Create(3),
                 Itemset.Create(4),
                 Itemset.Create(5)
            };

            var result = AprioriImpl.GenerateCandidates(itemsets);

            Assert.AreEqual(10, result.Length);

            Assert.Contains(Itemset.Create(1, 2), result);
            Assert.Contains(Itemset.Create(4, 5), result);
        }

        [Test]
        public void Can_Generate_Candidates_From_2_Length_Itemset()
        {
            Itemset[] itemsets = new[]
            {
                 Itemset.Create(1, 2),
                 Itemset.Create(1, 3),
                 Itemset.Create(1, 5),
                 Itemset.Create(2, 3),
                 Itemset.Create(2, 4),
                 Itemset.Create(2, 5)
            };

            var result = AprioriImpl.GenerateCandidates(itemsets);

            Assert.AreEqual(6, result.Length);
        }

        [Test]
        public void Can_Find_Frequent_Itemset_In_Small_DB_With_Support_2()
        {
            AprioriImpl miner = new AprioriImpl(SmallTransactionDatabase.CreateAprioriExample());

            miner.Run(new MiningOptions() { MinSupport = 2 });

            Itemset[] result = miner.Result.ToArray();
           
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);

            Assert.AreEqual(13, result.Length);

            Assert.Contains(Itemset.Create(1), result);
            Assert.Contains(Itemset.Create(2), result);
            Assert.Contains(Itemset.Create(3), result);
            Assert.Contains(Itemset.Create(4), result);
            Assert.Contains(Itemset.Create(5), result);

            Assert.Contains(Itemset.Create(1, 2), result);
            Assert.Contains(Itemset.Create(1, 3), result);
            Assert.Contains(Itemset.Create(1, 5), result);
            Assert.Contains(Itemset.Create(2, 3), result);
            Assert.Contains(Itemset.Create(2, 4), result);
            Assert.Contains(Itemset.Create(2, 5), result);

            Assert.Contains(Itemset.Create(1, 2, 3), result);
            Assert.Contains(Itemset.Create(1, 2, 5), result);
        }

        [Test]
        public void Can_Find_Frequent_Itemset_In_Small_DB_With_Support_3()
        {
            AprioriImpl miner = new AprioriImpl(SmallTransactionDatabase.CreateAprioriExample());

            miner.Run(new MiningOptions() { MinSupport = 3 });

            Itemset[] result = miner.Result.ToArray();

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);

            Assert.AreEqual(6, result.Length);

            Assert.Contains(Itemset.Create(1), result);
            Assert.Contains(Itemset.Create(2), result);
            Assert.Contains(Itemset.Create(3), result);
          

            Assert.Contains(Itemset.Create(1, 2), result);
            Assert.Contains(Itemset.Create(1, 3), result);
            Assert.Contains(Itemset.Create(2, 3), result);
        }


        [Test]
        public void Can_Find_Frequent_Itemset_In_Small_DB_With_Transaction_Reduction_ANd_Support_2()
        {
            AprioriWithDbReductionImpl miner = new AprioriWithDbReductionImpl(SmallTransactionDatabase.CreateAprioriExample());

            miner.Run(new MiningOptions() { MinSupport = 2 });

            Itemset[] result = miner.Result.ToArray();

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);

            Assert.AreEqual(13, result.Length);

            Assert.Contains(Itemset.Create(1), result);
            Assert.Contains(Itemset.Create(2), result);
            Assert.Contains(Itemset.Create(3), result);
            Assert.Contains(Itemset.Create(4), result);
            Assert.Contains(Itemset.Create(5), result);

            Assert.Contains(Itemset.Create(1, 2), result);
            Assert.Contains(Itemset.Create(1, 3), result);
            Assert.Contains(Itemset.Create(1, 5), result);
            Assert.Contains(Itemset.Create(2, 3), result);
            Assert.Contains(Itemset.Create(2, 4), result);
            Assert.Contains(Itemset.Create(2, 5), result);

            Assert.Contains(Itemset.Create(1, 2, 3), result);
            Assert.Contains(Itemset.Create(1, 2, 5), result);
        }

    
    }


}
