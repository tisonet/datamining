using System;
using System.Collections.Generic;
using System.Linq;
using Tisonet.DataMining.Domain.Itemsets;

namespace Tisonet.DataMining.Tests.FrequentItemsets
{
    public class SmallTransactionDatabase : ITransactionDatabase
    {
        private Int32 _tid = 0;
        private List<Transaction> _transactions = new List<Transaction>();

        public IEnumerable<Transaction> Transactions { get { return _transactions; } }

        private void Add(params UInt32[] items)
        {
            _transactions.Add(Transaction(items));
        }

        private Transaction Transaction(params UInt32[] items)
        {
            return new Transaction(_tid++, items);
        }

        public static ITransactionDatabase CreateAprioriExample()
        {
            SmallTransactionDatabase db = new SmallTransactionDatabase();
            
            db.Add(1, 2, 5);
            db.Add(2, 4);
            db.Add(2, 3);
            db.Add(1, 2, 4);
            db.Add(1, 3);
            db.Add(2, 3);
            db.Add(1, 3);
            db.Add(1, 2, 3, 5);
            db.Add(1, 2, 3);

            return db;
        }


        public static ITransactionDatabase CreateFPGrowthExample()
        {
            SmallTransactionDatabase db = new SmallTransactionDatabase();

            db.Add(1, 2);
            db.Add(2, 3, 4);
            db.Add(1, 3, 4, 5);
            db.Add(1, 4, 5);
            db.Add(1, 2, 3);
            db.Add(1, 2, 3, 4);
            db.Add(1);
            db.Add(1, 2, 3);
            db.Add(1, 2, 4);
            db.Add(2, 3, 5);
            return db;
        }
    }
}
