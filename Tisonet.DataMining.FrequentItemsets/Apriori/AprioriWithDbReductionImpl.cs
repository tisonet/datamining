using System;
using System.Collections.Generic;
using System.Linq;
using Tisonet.DataMining.Domain.Itemsets;
using Tisonet.DataMining.FrequentItemsets.Extensions;

namespace Tisonet.DataMining.FrequentItemsets.Apriori
{
    public class AprioriWithDbReductionImpl : AprioriImpl
    {
        private readonly HashSet<Int32> _reductedTransactions = new HashSet<int>();
 
        public AprioriWithDbReductionImpl(ITransactionDatabase db) : base(db){}

        public override string Name
        {
            get { return "Apriori with DB reduction"; }
        }

        protected override Itemset[] ScanDbAndCountCandidatesSupport(Itemset[] candidates)
        {
            foreach (var transaction in _db.Transactions)
            {
                if (IsReducted(transaction))
                {
                    continue;
                }

                bool containsCandidates = false;
                
                foreach (var candidate in candidates)
                {
                    if (ItemsetHelper.IsSubset(transaction.Items, candidate.Items))
                    {
                        candidate.Support++;
                        containsCandidates = true;
                    }
                }

                if (containsCandidates) continue;
                
                ReduceTransaction(transaction);
            }

            return candidates;
        }

        private void ReduceTransaction(Transaction transaction)
        {
            _reductedTransactions.Add(transaction.TransactionId);
        }

        private bool IsReducted(Transaction transaction)
        {
            return _reductedTransactions.Contains(transaction.TransactionId);
        }
    }
}
