using System;
using Tisonet.DataMining.Extensions;

namespace Tisonet.DataMining.Domain.Itemsets
{
    public class Itemset : IEquatable<Itemset>
    {
        private readonly UInt32[] _items;

        public Itemset(UInt32 item)
        {
            _items = new [] { item };
        }

        public Itemset(UInt32[] items)
        {
            _items = items;
        }

        public Itemset(UInt32[] items, bool sort)
        {
            if (sort)
            {
                Array.Sort(items);
            }

            _items = items;
        }

        public int Support { get; set; }

        public int Length
        {
            get { return _items.Length; }
        }

        public UInt32 this[int index]
        {
            get { return _items[index]; }   
        }

        public UInt32[] Items
        {
            get { return _items; }
        }

  
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return Equals(obj as Itemset);
        }

        public bool Equals(Itemset other)
        {
            return _items.IsEqualsTo(other.Items);
        }

        public override int GetHashCode()
        {
            return _items.CalculateHashCode();
        }

        public override string ToString()
        {
            return String.Join(" ", _items) + " : " + Support;
        }

        public static Itemset Empty = Itemset.Create();

        public static Itemset Create(params UInt32[] items)
        {
            return new Itemset(items);
        }

        public static Itemset CreateWithSupport(UInt32 items, Int32 support)
        {
            var itemset = new Itemset(items)
            {
                Support = support
            };

            return itemset;
        }
    }
}
