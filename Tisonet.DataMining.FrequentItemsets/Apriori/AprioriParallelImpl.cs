using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tisonet.DataMining.Domain.Itemsets;
using Tisonet.DataMining.FrequentItemsets.Extensions;

namespace Tisonet.DataMining.FrequentItemsets.Apriori
{
    public class AprioriParallelImpl : AprioriImpl
    {
        private Transaction _actualTransaction;
        private Itemset[] _candidates; 

        public AprioriParallelImpl(ITransactionDatabase db) : base(db) {}

        public override string Name
        {
            get { return "Apriori parallel"; }
        }

        protected override Itemset[] ScanDbAndCountCandidatesSupport(Itemset[] candidates)
        {
            if (candidates.Length == 0)
            {
                return candidates;
            }

            _candidates = candidates;

            foreach (Transaction actualTransaction in _db.Transactions)
            {
                _actualTransaction = actualTransaction;

                Parallel.ForEach(CreatePartitioner(candidates.Length), CountSupportForCandidatesInRange);
            }

            return candidates;
        }

        private void CountSupportForCandidatesInRange(Tuple<int, int> range)
        {
            for (int i = range.Item1; i < range.Item2; i++)
            {
                if (ItemsetHelper.IsSubset(_actualTransaction.Items, _candidates[i].Items))
                {
                    _candidates[i].Support++;
                }
            }
        }

        private static OrderablePartitioner<Tuple<int, int>> CreatePartitioner(int items)
        {
            return Partitioner.Create(0, items, ((int)(items / Environment.ProcessorCount) + 1));
        }
    }
}
