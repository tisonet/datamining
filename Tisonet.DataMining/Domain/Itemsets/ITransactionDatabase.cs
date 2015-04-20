using System.Collections.Generic;

namespace Tisonet.DataMining.Domain.Itemsets
{
    public interface ITransactionDatabase
    {
        IEnumerable<Transaction> Transactions { get;  } 
    }
}
