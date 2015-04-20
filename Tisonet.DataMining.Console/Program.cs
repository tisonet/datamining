using System;
using System.Collections.Generic;
using System.Linq;
using Tisonet.DataMining.Bechmark;
using Tisonet.DataMining.DataSets.Itemsets;
using Tisonet.DataMining.Domain.Itemsets;
using Tisonet.DataMining.FrequentItemsets;
using Tisonet.DataMining.FrequentItemsets.Apriori;
using Tisonet.DataMining.FrequentItemsets.FPGrowth;

namespace Tisonet.DataMining.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var testDatabasesWitTestSupport = new[]
            {
               //Tuple.Create<FrequentItemsetsMiningAlgorithm, Int32>( new AprioriImpl(new TextFileTransactionDatabase(@"..\..\..\DataSets\TextFile\T10I4D10K.dat")), 10 ),
               //Tuple.Create<FrequentItemsetsMiningAlgorithm, Int32>( new AprioriImpl(new TextFileTransactionDatabase(@"..\..\..\DataSets\TextFile\T40I10D10K.dat")), 100 ),

               //Tuple.Create<FrequentItemsetsMiningAlgorithm, Int32>( new AprioriWithDbReductionImpl(new TextFileTransactionDatabase(@"..\..\..\DataSets\TextFile\T10I4D10K.dat")), 10 ),
               //Tuple.Create<FrequentItemsetsMiningAlgorithm, Int32>( new AprioriWithDbReductionImpl(new TextFileTransactionDatabase(@"..\..\..\DataSets\TextFile\T40I10D10K.dat")), 100 ),

               //Tuple.Create<FrequentItemsetsMiningAlgorithm, Int32>( new AprioriParallelImpl(new TextFileTransactionDatabase(@"..\..\..\DataSets\TextFile\T10I4D10K.dat")), 10 ),
               //Tuple.Create<FrequentItemsetsMiningAlgorithm, Int32>( new AprioriParallelImpl(new TextFileTransactionDatabase(@"..\..\..\DataSets\TextFile\T40I10D10K.dat")), 100 ),

               //Tuple.Create<FrequentItemsetsMiningAlgorithm, Int32>( new AprioriWithHashingImpl(new TextFileTransactionDatabase(@"..\..\..\DataSets\TextFile\T10I4D10K.dat")), 10 ),
               //Tuple.Create<FrequentItemsetsMiningAlgorithm, Int32>( new AprioriWithHashingImpl(new TextFileTransactionDatabase(@"..\..\..\DataSets\TextFile\T40I10D10K.dat")), 100 ),


            
               Tuple.Create<FrequentItemsetsMiningAlgorithm, Int32>( new FPGrowthImpl(new TextFileTransactionDatabase(@"..\..\..\DataSets\TextFile\T10I4D10K.dat")), 10 ),
               //Tuple.Create<FrequentItemsetsMiningAlgorithm, Int32>( new FPGrowthImpl(new TextFileTransactionDatabase(@"..\..\..\DataSets\TextFile\T40I10D10K.dat")), 100 ),

            };


            foreach (var testInfo in testDatabasesWitTestSupport)
            {
                Microbenchmark test = new Microbenchmark();
                int patternsCount = 0;
                var elapsed = test.Run(() =>
                {
                    var apriori = testInfo.Item1;

                    apriori.Run(new MiningOptions { MinSupport = testInfo.Item2 });

                    var result = apriori.Result.ToArray();

                    patternsCount = result.Length;
                });

                Console.WriteLine("Algorithm: {0}", testInfo.Item1.Name);
                Console.WriteLine("Exec time: {0} s", elapsed.TotalSeconds);
                Console.WriteLine("Patterns: {0}", patternsCount);
                Console.WriteLine(string.Empty);

            }
            Console.ReadKey();
        }


    }
}
