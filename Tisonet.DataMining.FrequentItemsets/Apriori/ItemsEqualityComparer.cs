using System;
using System.Collections.Generic;
using Tisonet.DataMining.Extensions;

namespace Tisonet.DataMining.FrequentItemsets.Apriori
{
    internal class ItemsEqualityComparer : IEqualityComparer<UInt32[]>
    {
        public bool Equals(UInt32[] x, UInt32[] y)
        {
            return x.IsEqualsTo(y);
        }

        public int GetHashCode(UInt32[] obj)
        {
            return obj.CalculateHashCode();
        }
    }
}
