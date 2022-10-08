using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleStarWorldMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleStarWorldMod.Tests
{
    [TestClass()]
    public class SingleStarWorldModPatchTests
    {
        [TestMethod()]
        public void rand1Test()
        {
            Console.WriteLine(SingleStarWorldModPatch.rand1());
            Console.WriteLine(SingleStarWorldModPatch.rand1());
            Console.WriteLine(SingleStarWorldModPatch.rand1());
            Console.WriteLine(SingleStarWorldModPatch.rand1());
            Console.WriteLine(SingleStarWorldModPatch.rand1());
            Console.WriteLine(SingleStarWorldModPatch.rand1());
        }
    }
}