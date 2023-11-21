using HarmonyLib;
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

        public  bool Prefix(ConduitConsumer __instance )
        {
            //  ConduitUpdate 这个方法上moc
            //  ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
            // conduitConsumer.alwaysConsume = false;
            if (__instance.conduitType ==ConduitType.Liquid
                && __instance.capacityKG==5f
                )
            {
                //__instance.storage;//如果存量还很多.
               if(__instance.storage.capacityKg < 2.5)
                {
                    Debug.LogWarning("------>修改了kg.  -----"+__instance.name);
                    return true;
                }
                //判定.
            }
            else
            {

            }
            return false;
        }
    }
    //ConfigureBuildingTemplate

    [HarmonyPatch(typeof(HydroponicFarmConfig), "ConfigureBuildingTemplate")]
    public class HydroponicFarmConfigPatch
    {
        public void   Postfix(GameObject go)
        {
            ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
            conduitConsumer.consumptionRate = 100f;
        }
    }

}
