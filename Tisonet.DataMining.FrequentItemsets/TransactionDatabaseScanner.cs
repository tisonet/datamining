using System;
using System.Collections.Generic;
using System.Linq;
using Tisonet.DataMining.Domain.Itemsets;
using Tisonet.DataMining.FrequentItemsets.FPGrowth;

namespace Tisonet.DataMining.FrequentItemsets
{
    internal class TransactionDatabaseScanner
    {
        private readonly ITransactionDatabase _db;

        public TransactionDatabaseScanner(ITransactionDatabase db)
        {
            _db = db;   
        }


        public FrequentItemsCollection FindFrequentItems(Int32 minSupport)
        {
            var items = new Dictionary<UInt32, Itemset>();

            foreach (var trans in _db.Transactions)
            {
                HashSet<UInt32> alreadyAddedItems = new HashSet<UInt32>();

                for (int i = 0; i < trans.Length; i++)
                {
                    UInt32 currentItem = trans[i];

                    if (alreadyAddedItems.Add(currentItem))
                    {
                        Itemset itemset;
                        if (!items.TryGetValue(currentItem, out itemset))
                        {
                            itemset = new Itemset(currentItem);
                            items.Add(currentItem, itemset);
                        }

                        itemset.Support++;
                    }
                }
            }

            return new FrequentItemsCollection(items.Where(item => item.Value.Support >= minSupport)); 
                
        } 
    }
}
