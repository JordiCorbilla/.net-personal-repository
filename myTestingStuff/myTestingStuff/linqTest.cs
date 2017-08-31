using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace thundax.myTestingStuff
{
    [TestClass]
    public class linqTest
    {
        [TestMethod]
        public void TestExtensionMethod()
        {
            double[] numbers1 = { 1.9, 2, 8, 4, 5.7, 6, 7.2, 0 };
            double value = numbers1.Median();
            Assert.IsTrue(Math.Abs(value - 4.85) < 0.00001);
        }
        
    }

    public static class LINQExtension
    {
        public static double Median(this IEnumerable<double> source)
        {
            if (source.Count() == 0)
            {
                throw new InvalidOperationException("Cannot compute median for an empty set.");
            }

            var sortedList = from number in source
                             orderby number
                             select number;

            int itemIndex = (int)sortedList.Count() / 2;

            if (sortedList.Count() % 2 == 0)
            {
                // Even number of items.  
                return (sortedList.ElementAt(itemIndex) + sortedList.ElementAt(itemIndex - 1)) / 2;
            }
            else
            {
                // Odd number of items.  
                return sortedList.ElementAt(itemIndex);
            }
        }
    }
}
