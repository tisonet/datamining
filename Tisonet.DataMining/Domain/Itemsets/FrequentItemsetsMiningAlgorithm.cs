using System.Collections.Generic;

namespace Tisonet.DataMining.Domain.Itemsets
{
    public abstract class FrequentItemsetsMiningAlgorithm : Algorithm
    {
        public abstract IEnumerable<Itemset> Result { get; }

        public abstract void Run(MiningOptions options);
    }
}
