using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SingleStarWorldMod.Tests
{
    [TestClass()]
    public class SingleStarWorldModPatchTests
    {
        [TestMethod()]
        public void rand1Test()
        {
            global::Debug.Log(SingleStarWorldModPatch.rand1());
            global::Debug.Log(SingleStarWorldModPatch.rand1());
            global::Debug.Log(SingleStarWorldModPatch.rand1());
            global::Debug.Log(SingleStarWorldModPatch.rand1());
            global::Debug.Log(SingleStarWorldModPatch.rand1());
            global::Debug.Log(SingleStarWorldModPatch.rand1());

            global::Debug.Log(new System.Random().Next(1, 10000));
        }
    }
}