using System;

namespace Tisonet.DataMining.Extensions
{
    public static class UInt32Extensions
    {
        public static int CalculateHashCode(this UInt32[] array)
        {
            unchecked
            {
                const int prime = 16777619;
                int hash = (int)2166136261;

                for (int i = 0; i < array.Length; i++)
                {
                    hash = (hash ^ (int)array[i]) * prime;
                }

                hash += hash << 13;
                hash ^= hash >> 7;
                hash += hash << 3;
                hash ^= hash >> 17;
                hash += hash << 5;

                return hash;
            }
        }
    
        public static bool IsEqualsTo(this UInt32[] array, UInt32[] other)
        {
            if (other == null) return false;

            if (array.Length != other.Length) return false;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != other[i]) return false;
            }

            return true;
        }
    
        /// <summary>
        /// Create a new array by adding an item to the end.
        /// </summary>
        /// <returns></returns>
        public static UInt32[] Add(this UInt32[] array, UInt32 item)
        {
            UInt32[] newArray = new UInt32[array.Length + 1];

            Array.Copy(array, newArray, array.Length);

            newArray[array.Length] = item;

            return newArray;
        }

    }
}
