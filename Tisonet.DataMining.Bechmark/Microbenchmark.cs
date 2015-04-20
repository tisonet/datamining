using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tisonet.DataMining.Bechmark
{
    public class Microbenchmark
    {
        private readonly int _innerIterations;
        private readonly int _outerIterations;

        public Microbenchmark()
            : this(1, 1)
        {

        }

        public Microbenchmark(int innerIterations): 
            this(innerIterations, 1)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerIterations">The inner measurement loop is repeated many times to make sure we are measuring an operation of significant duration.</param>
        /// <param name="outerIterations">The outer measurement loop is repeated many times to make sure we get reliable results.</param>
        public Microbenchmark(int innerIterations, int outerIterations)
        {
            _innerIterations = innerIterations;
            _outerIterations = outerIterations;
        }

        public TimeSpan Run(Action test)
        {
            Stopwatch watch = null;
            for (int i = 0; i < _outerIterations; i++)
            {
                watch = Stopwatch.StartNew();

                for (int j = 0; j < _innerIterations; j++)
                {
                    test();
                }
            }

            return watch.Elapsed;

        }
    }
}
