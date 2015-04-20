using System;
using System.Collections.Generic;
using Tisonet.DataMining.Domain.Itemsets;

namespace Tisonet.DataMining.DataSets.Itemsets
{
    public class RandomTransactionDatabase : ITransactionDatabase
    {
        private readonly int _items;
        private readonly int _minTransactionLength;
        private readonly int _maxTransactionLength;
        private readonly int _transactionsCount;
        private bool _genereted;
        private Transaction[] _transactions;
        private readonly Random _random = new Random();

        public IEnumerable<Transaction> Transactions
        {
            get
            {
                return _genereted ? _transactions : GenerateAndSavedTransactions();
            }
        }

        public RandomTransactionDatabase(int items, int minTransactionLength, int maxTransactionLength, int transactionsCount)
        {
            _items = items;
            _minTransactionLength = minTransactionLength;
            _maxTransactionLength = maxTransactionLength;
            _transactionsCount = transactionsCount;
        }

        private IEnumerable<Transaction> GenerateAndSavedTransactions()
        {
            _transactions = new Transaction[_transactionsCount];

            for (Int32 tid = 0; tid < _transactionsCount; tid++)
            {
                int transactionLength = RandomTransactionLength();
                UInt32[] transactionItems = new UInt32[transactionLength];
                
                for (int start = 0; start <  transactionLength; start++)
                {
                    transactionItems[start] = RandomItem();
                }

                _transactions[tid] = new Transaction(tid, transactionItems);

                yield return _transactions[tid];
            }

            _genereted = true;
        }

        private uint RandomItem()
        {
           return (UInt32)_random.Next(0, _items);
        }

        private int RandomTransactionLength()
        {
            return _random.Next(_minTransactionLength, _maxTransactionLength);
        }
    }
}
