using System;
using System.Collections.Generic;
using Tisonet.DataMining.Domain.Itemsets;
using Tisonet.DataMining.FrequentItemsets.Extensions;

namespace Tisonet.DataMining.FrequentItemsets.Apriori
{
    public class AprioriWithHashingImpl : AprioriImpl
    {
        public override string Name
        {
            get { return "Apriori with hashing"; }
        }

        public AprioriWithHashingImpl(ITransactionDatabase db) : base(db) { }

        protected override Itemset[] ScanDbAndCountCandidatesSupport(Itemset[] candidates)
        {
            if (candidates.Length == 0)
            {
                return candidates;
            }

            Dictionary<UInt32[], Itemset> candidatesDictionary = CreateCandidatesDictionary(candidates);

            foreach (var transaction in _db.Transactions)
            {
                List<UInt32[]> transactionsSubItemsets = ItemsetHelper.GenerateSubItemsets(transaction.Items, candidates[0].Length);

                for (int i = 0; i < transactionsSubItemsets.Count; i++)
                {
                    Itemset itemset;
                    
                    if (candidatesDictionary.TryGetValue(transactionsSubItemsets[i], out itemset))
                    {
                        itemset.Support++;
                    }
                }
            }

            return candidates;
        }

        private static Dictionary<UInt32[], Itemset> CreateCandidatesDictionary(Itemset[] candidates)
        {
            var dictionary = new Dictionary<UInt32[], Itemset>(candidates.Length, new ItemsEqualityComparer());

            for (int i = 0; i < candidates.Length; i++)
            {
                dictionary.Add(candidates[i].Items, candidates[i]);
            }

            return dictionary;
        }
    }

  
}
