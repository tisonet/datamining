using System;
using System.Collections.Generic;
using System.Linq;
using Tisonet.DataMining.Domain.Itemsets;
using Tisonet.DataMining.Extensions;

namespace Tisonet.DataMining.FrequentItemsets.Extensions
{
    public static class ItemsetHelper
    {
        public static List<UInt32[]> GenerateOneLevelSubItemsets(UInt32[] sourceItemset)
        {
            List<UInt32[]> subItemset = new List<UInt32[]>();

            int subItemsetsLength = sourceItemset.Length - 1;

            int skipItemIndex = subItemsetsLength;

            while (skipItemIndex != -1)
            {
                UInt32[] subItems = new UInt32[subItemsetsLength];
                int currentIndex = 0;

                for (int i = 0; i < sourceItemset.Length; i++)
                {
                    if (i != skipItemIndex)
                    {
                        subItems[currentIndex++] = sourceItemset[i];
                    }
                }

                --skipItemIndex;

                subItemset.Add(subItems);
            }

            return subItemset;
        }

        public static IEnumerable<Itemset> GenerateOneLevelSubItemsets(Itemset sourceItemset)
        {
            foreach (UInt32[] itemset in  GenerateOneLevelSubItemsets(sourceItemset.Items))
            {
                yield return new Itemset(itemset);
            }
        }

        public static bool IsSubset(UInt32[] sourceItemset, UInt32[] subItemset)
        {
            unchecked
            {
                if (sourceItemset.Length < subItemset.Length)
                {
                    return false;
                }

                int actualItemIndex = 0;
                
                for (int i = 0; i < sourceItemset.Length; i++)
                {
                    if (sourceItemset[i] > subItemset[actualItemIndex])
                    {
                        return false;
                    }

                    if (sourceItemset[i] < subItemset[actualItemIndex])
                    {
                        continue;
                    }

                    actualItemIndex++;

                    if (actualItemIndex == subItemset.Length)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public static bool IsSubset(Itemset sourceItemset, Itemset subItemset)
        {
            return IsSubset(sourceItemset.Items, subItemset.Items);
        }

        public static IEnumerable<UInt32[]> EnumerateSubItemsets(UInt32[] sourceItemset, int subItemsetLength)
        {
            for (UInt32 i = 0; i <= sourceItemset.Length - subItemsetLength; i++)
            {
                UInt32[] newPrefix = new[] { sourceItemset[i] };
                
                foreach (var subItemset in EnumerateSubItemsetsRecursive(newPrefix, sourceItemset, i + 1, subItemsetLength))
                {
                    yield return subItemset;
                }
            }
        }

        private static IEnumerable<UInt32[]> EnumerateSubItemsetsRecursive(UInt32[] prefix, UInt32[] sourceItemset, UInt32 startIndexInSourceItemset, int subItemsetLength)
        {
            if (prefix.Length == subItemsetLength)
            {
                yield return prefix;
            }
            else
            {
                for (UInt32 i = startIndexInSourceItemset; i < sourceItemset.Length; i++)
                {
                    UInt32[] newPrefix = prefix.Add(sourceItemset[i]);
                   
                    foreach (var subItemset in EnumerateSubItemsetsRecursive(newPrefix, sourceItemset, i + 1, subItemsetLength))
                    {
                        yield return subItemset;
                    }
                }
            }
        }



        public static List<UInt32[]> GenerateSubItemsets(UInt32[] sourceItemset, int subItemsetLength)
        {
            List<UInt32[]> subItemsets = new List<UInt32[]>();

            for (UInt32 i = 0; i <= sourceItemset.Length - subItemsetLength; i++)
            {
                UInt32[] newPrefix = new[] { sourceItemset[i] };

                GenerateSubItemsetsRecursive(newPrefix, sourceItemset, i + 1, subItemsetLength, subItemsets);
            }

            return subItemsets;
        }

        private static void GenerateSubItemsetsRecursive(UInt32[] prefix, UInt32[] sourceItemset, UInt32 startIndexInSourceItemset, int subItemsetLength, List<UInt32[]> subItemsets)
        {
            if (prefix.Length == subItemsetLength)
            {
               subItemsets.Add((UInt32[]) prefix.Clone());
            }
            else
            {
                UInt32[] newPrefix = new UInt32[prefix.Length + 1];
               
                Array.Copy(prefix, newPrefix, prefix.Length);

                for (UInt32 i = startIndexInSourceItemset; i < sourceItemset.Length; i++)
                {
                    newPrefix[prefix.Length] = sourceItemset[i];
                    
                    GenerateSubItemsetsRecursive(newPrefix, sourceItemset, i + 1, subItemsetLength, subItemsets);
                }
            }
        }
    }
}
