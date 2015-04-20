using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tisonet.DataMining.Domain.Itemsets;

namespace Tisonet.DataMining.FrequentItemsets.FPGrowth
{
    public class FPGrowthImpl : FrequentItemsetsMiningAlgorithm
    {
        private readonly ITransactionDatabase _db;
        protected MiningOptions _options;
        protected List<Itemset> _frequentItemset;
        internal FPTree _fpTree;
        protected FrequentItemsCollection _frequentItems;

        public override string Name
        {
            get { return "FP-Growth classic";  }
        }


        public FPGrowthImpl(ITransactionDatabase db)
        {
            _db = db;
        }

        public override IEnumerable<Itemset> Result
        {
            get
            {
                if (_frequentItemset == null)
                {
                    throw new InvalidOperationException("First execute: 'Run' method to process mining.");
                }

                return _frequentItemset;
            }
        }

        public override void Run(MiningOptions options)
        {
            _options = options;
            _frequentItemset = new List<Itemset>();

            ScanDbForFrequentItems();
            ScanDbForConstructingFpTree();

            MinePatternsFromFpTreeRecursive(_fpTree);


        }

        private void MinePatternsFromFpTreeRecursive(IFPTree fpTree)
        {
            foreach (var item in fpTree.Items.OrderByDescending(item => item))
            {
                ConditionalFPTree cTree = CreateConditionalFPTree(item, fpTree);

                if (cTree.Support >= _options.MinSupport)
                {
                    _frequentItemset.Add(cTree.Itemset);

                    // Recursion
                    MinePatternsFromFpTreeRecursive(cTree);
                }

            }
        }

        private ConditionalFPTree CreateConditionalFPTree(uint item, IFPTree fpTree)
        {
            ConditionalFPTree cTree = new ConditionalFPTree(item, fpTree.Itemset.Items);

            foreach (FPTreePrefixPathItem pathItem in fpTree.GetItemPrefixPath(item))
            {
                cTree.Support += pathItem.FPTreeNode.Support;
                cTree.AddTransaction(pathItem.FPTreeNode.ParentNode, pathItem.FPTreeNode.Support);
            }

           // cTree.RemoveInfrequentItems(_options.MinSupport);

            return cTree;
        }

        private void ScanDbForFrequentItems()
        {

            var dbScanner = new TransactionDatabaseScanner(_db);

            _frequentItems = dbScanner.FindFrequentItems(_options.MinSupport);
        }

        private void ScanDbForConstructingFpTree()
        {
            _fpTree = new FPTree(_frequentItems);

            foreach (var trans in _db.Transactions)
            {
                _fpTree.AddTransaction(trans);
            }
        }
    }
}
