using HarmonyLib;
using UnityEngine;

namespace LimitHydroponicMod
{

    [HarmonyPatch(typeof(ConduitConsumer), "Consume")]
    public class ConduitConsumerPatch
    {

        public static bool Prefix(ConduitConsumer __instance)
        {
            //  ConduitUpdate 这个方法上moc
            //  ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
            // conduitConsumer.alwaysConsume = false;
            // if (__instance.conduitType ==ConduitType.Liquid && __instance.capacityKG==5f )
            if (__instance.name == "HydroponicFarmComplete" || __instance.storage.name == "HydroponicFarmComplete")
            {
                //__instance.storage.ToString();
                //__instance.gameObject.i
                // __instance.storage.

                // Debug.Log($"------>.storage A -----{__instance.name} {__instance.storage} {__instance.storage.name} ");
                // Debug.Log($"------>.storage mass -----{__instance.storage.capacityKg} {__instance.storage.MassStored()}"); 
                // __instance.storage.MassStored;

                //__instance.storage.items.
                //__instance.storage;//如果存量还很多.
                if (calcLiquidMass(__instance) < 1    // 手动计算液体  1kg
                   || __instance.storage.MassStored() < 1) //固体,液体都会计算.
                {
                    // Debug.Log("------>修改了kg.B  -----"+__instance.name);
                    // __instance.storage.UnitsStored();
                    return true;
                }
                else
                {
                    return false;
                }
                //判定.
            }
            else
            {

            }

            return true;
        }
        public static float calcLiquidMass(ConduitConsumer __instance)
        {
            //__instance.storage.MassStored()
            var items = __instance.storage.items;
            float num = 0f;
            for (int i = 0; i < items.Count; i++)
            {
                if (!(items[i] == null))
                {
                    PrimaryElement component = items[i].GetComponent<PrimaryElement>();
                    //  Debug.Log($"------->>>{__instance}{component.HasTag(GameTags.Liquid)}--<<<<<");
                    if (component.HasTag(GameTags.Liquid)
                      || component.ElementID == SimHashes.Water
                      || component.ElementID == SimHashes.DirtyWater
                      || component.ElementID == SimHashes.SaltWater)
                    {
                        num += component.Units * component.MassPerUnit;
                        // Debug.Log($"------->>>{__instance}{component.HasTag(GameTags.Liquid)} {num}--<<<<<");
                    };
                }
            }
            return num;
        }
    }
    //ConfigureBuildingTemplate

    [HarmonyPatch(typeof(HydroponicFarmConfig), "ConfigureBuildingTemplate")]
    public class HydroponicFarmConfigPatch
    {
        public static void Postfix(GameObject go)
        {
            ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
            conduitConsumer.consumptionRate = 5f;
        }
    }



}
