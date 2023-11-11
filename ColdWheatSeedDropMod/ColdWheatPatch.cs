using HarmonyLib;
using Klei.AI;
using System.Reflection;

namespace ColdWheatSeedDropMod
{
    /*
        [HarmonyPatch(typeof(SeedProducer))]
        [HarmonyPatch("ProduceSeed")]
        public static class SeedProducer_RollForMutation_Patch
        {
            public static void Postfix(SeedProducer __instance, string seedId, bool canMutate, ref GameObject __result)
            {
                // string seedId, int units = 1, bool canMutate = true
                //检查小麦是否存在?
                // 已经确认：收获时不触发SeedProducer.ProduceSeed方法.
                // 挖开时触发SeedProducer.ProduceSeed方法
                if (seedId == "ColdWheatSeed" || seedId.Contains("ColdWheat"))
                {
                 //   Debug.LogWarning(new System.Diagnostics.StackTrace().ToString());
                    Debug.LogWarning(">>>>>>>>小麦变异出来了:" + seedId + " ,canMutate:" + canMutate);

                }

            }
        }
    */
    [HarmonyPatch(typeof(SeedProducer))]
    [HarmonyPatch("Configure")]
    public static class SeedProducer_Configure_Patch
    {
        public static void Prefix(SeedProducer __instance, string SeedID, ref SeedProducer.ProductionType productionType, ref int newSeedsProduced)
        {

            if (productionType == SeedProducer.ProductionType.DigOnly)
            {    //经过测试改变类型有效.
                Debug.LogWarning(">>>>>>>>小麦,小吃豆类型改变");
                productionType = SeedProducer.ProductionType.Harvest;
            }
            if (SeedID.Contains("ColdWheat"))
            {
                newSeedsProduced = 18;
            }
            if (SeedID.Contains("BeanPlant"))
            {
                newSeedsProduced = 12;
            }

        }
    }
    static class AccessExtensions
    {
        public static object call(this object o, string methodName, params object[] args)
        {
            var mi = o.GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (mi != null)
            {
                return mi.Invoke(o, args);
            }
            return null;
        }
    }
    [HarmonyPatch(typeof(SeedProducer))]
    [HarmonyPatch("CropPicked")]
    public static class ProduceMoreSeed_Patch
    {

        //强制18个种子
        public static void Postfix(SeedProducer __instance)
        {
            if (__instance.seedInfo.seedId.Contains("ColdWheat")
                || __instance.seedInfo.seedId.Contains("BeanPlant"))
            {
                Worker completed_by = __instance.GetComponent<Harvestable>().completed_by;
                float num = 0.1f;
                if (completed_by != null)
                {
                    num += completed_by.GetComponent<AttributeConverters>().Get(Db.Get().AttributeConverters.SeedHarvestChance).Evaluate();
                }
                int num2 = (UnityEngine.Random.Range(0f, 1f) <= num) ? 1 : 0;
                if (num2 > 0)
                {
                    var fun = __instance.GetType().GetMethod("ProduceSeed", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                    var rdmax = UnityEngine.Random.Range(0f, __instance.seedInfo.newSeedsProduced / 3);
                    for (int i = 0; i < rdmax; i++)
                    {

                        //var fun = AccessTools.Method(typeof(ResearchCenter), "UpdateWorkingState");
                        //fun.Invoke(__instance.seedInfo, 1, true);

                        fun.Invoke(__instance, new object[] { __instance.seedInfo.seedId, num2, true });

                        //__instance.ProduceSeed(__instance.seedInfo.seedId, num2) .Trigger(580035959, completed_by);
                    }

                }
            }
        }
    }

}
