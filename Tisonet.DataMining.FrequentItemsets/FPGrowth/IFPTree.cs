using System;
using System.Collections.Generic;
using Tisonet.DataMining.Domain.Itemsets;

namespace Tisonet.DataMining.FrequentItemsets.FPGrowth
{
    public interface IFPTree
    {
        Itemset Itemset { get; }

        IEnumerable<UInt32> Items { get; }
                    
        IEnumerable<FPTreePrefixPathItem> GetItemPrefixPath(UInt32 item);
    }
}
