using System;
using System.Collections.Generic;

namespace Tisonet.DataMining.FrequentItemsets.FPGrowth
{
    internal class ItemsComparerBySupport : IComparer<UInt32>
    {
        private readonly FrequentItemsCollection _frequentItems;

        public ItemsComparerBySupport(FrequentItemsCollection frequentItems)
        {
            _frequentItems = frequentItems;
        }

        public int Compare(UInt32 x, UInt32 y)
        {
            var xSupport = _frequentItems.GetSupport(x);
            var ySupport = _frequentItems.GetSupport(y);

            return xSupport.CompareTo(ySupport);
        }
    }
}
