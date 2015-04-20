using System;
using System.Collections.Generic;
using System.Linq;
using Tisonet.DataMining.Domain.Itemsets;
using Tisonet.DataMining.FrequentItemsets.Extensions;

namespace Tisonet.DataMining.FrequentItemsets.Apriori
{
    public class AprioriImpl : FrequentItemsetsMiningAlgorithm
    {
        protected readonly ITransactionDatabase _db;
        protected MiningOptions _options;
        protected List<Itemset> _frequentItemset;

        private AprioriImpl()
        {
            
        }

        public AprioriImpl(ITransactionDatabase db)
        {
            _db = db;
        }

        public override string Name
        {
            get { return "Apriori classic"; }
        }

        public override IEnumerable<Itemset> Result
        {
            get 
            {
                if (_frequentItemset == null)
                {
                    throw new InvalidOperationException("First execute: 'Run' method to process mining.");
                }

                return _frequentItemset;
            }
        } 

        public override void Run(MiningOptions options)
        {
            _options = options;

            _frequentItemset = new List<Itemset>();

            // Finds frequent 1-itemset by scanning entire database.
            Itemset[] frequentItemsetFromPreviousStep = ScanDbForFrequentItems();

            while (frequentItemsetFromPreviousStep.Length > 0)
            {
                _frequentItemset.AddRange(frequentItemsetFromPreviousStep);

                // Generates candidates (k+1)-itemsets from frequent k-itemsets. 
                UInt32[][] candidates = GenerateCandidatesSlim(frequentItemsetFromPreviousStep);

                Itemset[] prunedCandidates = PruneCandidatesByAprioriSlim(candidates, frequentItemsetFromPreviousStep);

                Itemset[] candidatesWithSupport  = ScanDbAndCountCandidatesSupport(prunedCandidates);

                frequentItemsetFromPreviousStep = RemoveUnfrequentCandidates(candidatesWithSupport);
            }

        }

        protected Itemset[] ScanDbForFrequentItems()
        {

            var dbScanner = new TransactionDatabaseScanner(_db);

            var frequentItems =  dbScanner.FindFrequentItems(_options.MinSupport);

            return frequentItems.FrequentItemset.ToArray();
        }

        protected static UInt32[][] GenerateCandidatesSlim(Itemset[] itemsets)
        {
            var candidates = new List<UInt32[]>();

            for (int i = 0; i < itemsets.Length - 1; i++)
            {
                for (int j = i + 1; j < itemsets.Length; j++)
                {
                    Itemset itemset1 = itemsets[i];
                    Itemset itemset2 = itemsets[j];

                    int currentIndex = 0;
                    int lastIndex = itemset1.Length - 1;

                    // We can join two k-itemset if the first k-1 items are equals and
                    // last item in the first itemset is less than last item in the second itemset.   
                    for (; currentIndex < lastIndex; currentIndex++)
                    {
                        if (itemset1[currentIndex] != itemset2[currentIndex])
                        {
                            // We break early if we dont have the same items.
                            break;
                        }
                    }

                    if ((currentIndex == lastIndex) && (itemset1[lastIndex] < itemset2[lastIndex]))
                    {
                        var candidate = JoinItemsetSlim(itemset1, itemset2);

                        candidates.Add(candidate);
                    }
                }
            }

            return candidates.ToArray();
        }

        protected static UInt32[] JoinItemsetSlim(Itemset itemset1, Itemset itemset2)
        {
            // Creates a new itemset which contains all items from itemset1 and last item from itemset2.
            UInt32[] items = new UInt32[itemset1.Length + 1];

            Array.Copy(itemset1.Items, items, itemset1.Items.Length);

            items[itemset1.Length] = itemset2[itemset1.Length - 1];

            return items;
        }

        protected virtual Itemset[] PruneCandidatesByAprioriSlim(UInt32[][] candidatesItemset, Itemset[] frequentItemset)
        {
            var frequentCandidates = new List<Itemset>();
            var frequent = new HashSet<UInt32[]>(frequentItemset.Select(i => i.Items), new ItemsEqualityComparer());

            for (int i = 0; i < candidatesItemset.Length; i++)
            {
                bool hasAllSubItemsetFrequent = true;


                // TODO: Lazy generating, we can stop generating subitemset if we know that an one is not frequent. 
                List<UInt32[]> subItemsets = ItemsetHelper.GenerateOneLevelSubItemsets(candidatesItemset[i]);

                for (int j = 0; j < subItemsets.Count; j++)
                {
                    if (frequent.Contains(subItemsets[j]))
                    {
                        continue;
                    }

                    hasAllSubItemsetFrequent = false;
                    
                    break;
                }

                if (hasAllSubItemsetFrequent)
                {
                    frequentCandidates.Add(new Itemset(candidatesItemset[i]));
                }
            }

            return frequentCandidates.ToArray();
        }

        protected virtual Itemset[] ScanDbAndCountCandidatesSupport(Itemset[] candidates)
        {
            foreach (var transaction in _db.Transactions)
            {
                foreach (var candidate in candidates)
                {
                    if (ItemsetHelper.IsSubset(transaction.Items, candidate.Items))
                    {
                        candidate.Support++;
                    }
                }
            }

            return candidates;
        }

        protected Itemset[] RemoveUnfrequentCandidates(Itemset[] candidates)
        {
            List<Itemset> frequent = new List<Itemset>(candidates.Length);

            for (int i = 0; i < candidates.Length; i++)
            {
                if (candidates[i].Support >= _options.MinSupport)
                {
                    frequent.Add(candidates[i]);
                }
            }

            return frequent.ToArray();
        }

        internal static Itemset[] GenerateCandidates(Itemset[] itemsets)
        {
            return GenerateCandidatesSlim(itemsets).Select(i => new Itemset(i)).ToArray();
        }

        internal static Itemset JoinItemset(Itemset itemset1, Itemset itemset2)
        {
            return new Itemset(JoinItemsetSlim(itemset1, itemset2), false);
        }

        internal static Itemset[] PruneCandidatesByApriori(Itemset[] candidatesItemset, Itemset[] frequentItemset)
        {
            return new AprioriImpl().PruneCandidatesByAprioriSlim(candidatesItemset.Select(i => i.Items).ToArray(), frequentItemset);
        }
    }
}
