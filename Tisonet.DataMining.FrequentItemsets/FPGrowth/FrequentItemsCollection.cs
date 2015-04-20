using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tisonet.DataMining.Domain.Itemsets;

namespace Tisonet.DataMining.FrequentItemsets.FPGrowth
{
    public class FrequentItemsCollection
    {
        private readonly IDictionary<UInt32, Itemset> _frequentItems = new Dictionary<uint, Itemset>();

        public FrequentItemsCollection(IEnumerable<KeyValuePair<UInt32, Itemset>> frequentItems)
        {
            foreach (var frequentItem in frequentItems)
            {
                _frequentItems.Add(frequentItem);
            }
        }

        public IEnumerable<UInt32> FrequentItems
        {
            get { return _frequentItems.Keys; }
        } 

        public IEnumerable<Itemset> FrequentItemset
        {
            get { return _frequentItems.Values; }
        }

        public bool IsFrequent(UInt32 item)
        {
            return _frequentItems.ContainsKey(item);
        }

        public int GetSupport(UInt32 item)
        {
            Itemset itemset;
            if (_frequentItems.TryGetValue(item, out itemset))
            {
                return itemset.Support;
            }

            return  0;
        }
    }
}
