using HarmonyLib;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LimitHydroponicMod
{

    [HarmonyPatch(typeof(ConduitConsumer), "Consume")]
    public class ConduitConsumerPatch
    {

        public static  bool Prefix(ConduitConsumer __instance )
        {
            //  ConduitUpdate 这个方法上moc
            //  ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
            // conduitConsumer.alwaysConsume = false;
            // if (__instance.conduitType ==ConduitType.Liquid && __instance.capacityKG==5f )
            if (__instance.name== "HydroponicFarmComplete"|| __instance.storage.name== "HydroponicFarmComplete")
            {
                //__instance.storage.ToString();
                //__instance.gameObject.i
                // __instance.storage.

                // Debug.LogWarning($"------>.storage A -----{__instance.name} {__instance.storage} {__instance.storage.name} ");
                // Debug.LogWarning($"------>.storage mass -----{__instance.storage.capacityKg} {__instance.storage.MassStored()}"); 
               // __instance.storage.MassStored;

                //__instance.storage.items.
                //__instance.storage;//如果存量还很多.
                if (calcLiquidMass(__instance) > 1) //固体,液体都要计算.
                {
                   // Debug.LogWarning("------>修改了kg.B  -----"+__instance.name);
                    return false;
                }
                //判定.
            }
            else
            {

            }
        
            return true;
        }
        public static float calcLiquidMass( ConduitConsumer __instance)
        {
            //__instance.storage.MassStored()
            var items = __instance.storage.items;
            float num = 0f;
            for (int i = 0; i < items.Count; i++)
            {
                if (!(items[i] == null))
                {
                    PrimaryElement component = items[i].GetComponent<PrimaryElement>();

                    if (component.HasTag(GameTags.Liquid)||
                        component.ElementID == SimHashes.Water
                      || component.ElementID == SimHashes.DirtyWater
                      || component.ElementID == SimHashes.SaltWater)
                    {
                        num += component.Units;
                    };
                }
            }
            return (float)Mathf.RoundToInt(num * 1000f) / 1000f;
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
