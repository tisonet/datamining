using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tisonet.DataMining.FrequentItemsets.FPGrowth
{
    internal class FPTreeHeader
    {
        private readonly IDictionary<UInt32, List<FPTreePrefixPathItem>> _items = new Dictionary<UInt32, List<FPTreePrefixPathItem>>(); 
    
        public FPTreePrefixPathItem Add(FPTreeNode node)
        {
            FPTreePrefixPathItem item = new FPTreePrefixPathItem
            {
                FPTreeNode = node
            };

            List<FPTreePrefixPathItem> itemList;
            
            if (!_items.TryGetValue(node.Item, out itemList))
            {
                itemList = new List<FPTreePrefixPathItem>();
                _items.Add(node.Item, itemList);
            }

            itemList.Add(item);

            return item;
        }

        internal IEnumerable<FPTreePrefixPathItem> GetItemPath(UInt32 item)
        {
            List<FPTreePrefixPathItem> list = _items[item];
            return list ?? Enumerable.Empty<FPTreePrefixPathItem>();
        }
    
        internal IEnumerable<UInt32> Items
        {
            get { return _items.Keys; }
        }

        public void Remove(UInt32 item)
        {
            _items.Remove(item);
        }
    }

    public class FPTreePrefixPathItem
    {
        public FPTreeNode FPTreeNode { get; set; }
    }
}
