using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SingleStarWorldMod.Tests
{
    [TestClass()]
    public class SingleStarWorldModPatchTests
    {
        [TestMethod()]
        public void rand1Test()
        {
            Debug.Log(SingleStarWorldModPatch.rand1());
            Debug.Log(SingleStarWorldModPatch.rand1());
            Debug.Log(SingleStarWorldModPatch.rand1());
            Debug.Log(SingleStarWorldModPatch.rand1());
            Debug.Log(SingleStarWorldModPatch.rand1());
            Debug.Log(SingleStarWorldModPatch.rand1());

            Debug.Log(new System.Random().Next(1, 10000));
        }
    }
}