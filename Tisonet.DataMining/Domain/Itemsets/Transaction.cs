using System;

namespace Tisonet.DataMining.Domain.Itemsets
{
    public class Transaction<T>
    {
        protected  T[] _items;
        protected  Int32 _transactionId;
    }


    public class Transaction : Transaction<UInt32>
    {
        //private readonly UInt32[] _items;
        //private readonly Int32 _transactionId;

        public Transaction(Int32 transactionId, UInt32[] items)
        {
            _transactionId = transactionId;
            _items = items;
        }  

        public Int32 TransactionId
        {
            get { return _transactionId; }
        }

        public UInt32[] Items
        {
            get { return _items; }
        }

        public UInt32 this[int index]
        {
            get { return _items[index]; }
        }

        public int Length
        {
            get { return _items.Length; }
        }

        public static Transaction Create(params UInt32[] items)
        {
            return new Transaction(0, items);
        }
    }
}
