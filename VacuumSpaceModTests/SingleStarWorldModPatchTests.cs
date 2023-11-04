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
             global::Debug.LogWarning(SingleStarWorldModPatch.rand1());
             global::Debug.LogWarning(SingleStarWorldModPatch.rand1());
             global::Debug.LogWarning(SingleStarWorldModPatch.rand1());
             global::Debug.LogWarning(SingleStarWorldModPatch.rand1());
             global::Debug.LogWarning(SingleStarWorldModPatch.rand1());
             global::Debug.LogWarning(SingleStarWorldModPatch.rand1());

            global::Debug.LogWarning(new System.Random().Next(1, 10000)) ;
        }
    }
}