using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tisonet.DataMining.Domain.Itemsets;

namespace Tisonet.DataMining.DataSets.Itemsets
{
    public class TextFileTransactionDatabase : ITransactionDatabase
    {
        private readonly string _databaseFilePath;
        private readonly bool _isDbCacheEnabled;
        private bool _loaded;
        private ICollection<Transaction> _transactions;

        public TextFileTransactionDatabase(string databaseFilePath, bool isDbCacheEnabled = false)
        {
            _databaseFilePath = databaseFilePath;
            _isDbCacheEnabled = isDbCacheEnabled;
        }

        public IEnumerable<Transaction> Transactions
        {
            get
            {
                return IsDbLoaded ? _transactions : LoadAndSavedTransactions();
            }
        }

        private IEnumerable<Transaction> LoadAndSavedTransactions()
        {
            _transactions = new List<Transaction>();


            using (var database = File.OpenText(_databaseFilePath))
            {
                string transactionLine = database.ReadLine();

                int tid = 0;
                while (transactionLine != null)
                {
                    List<UInt32> items = new List<UInt32>();

                    foreach (var item in transactionLine.Split(new char[] {' '} , StringSplitOptions.RemoveEmptyEntries))
                    {
                        items.Add(UInt32.Parse(item));
                    }

                    Transaction transaction = new Transaction(++tid, items.ToArray());

                    if (_isDbCacheEnabled)
                    {
                        _transactions.Add(transaction);
                    }

                    transactionLine = database.ReadLine();

                    yield return transaction;

                }
            }
            _loaded = true;
        }

        private bool IsDbLoaded
        {
            get { return _isDbCacheEnabled && _loaded; }
        }
    }
}
