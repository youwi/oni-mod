using HarmonyLib;
using System.Collections.Generic;

namespace PoiAddResource
{
    // HarvestablePOIConfig.GenerateConfigs()


    [HarmonyPatch(typeof(HarvestablePOIConfig), "GenerateConfigs")]
    public static class PoiAddResourcePatch
    {
        public static void Postfix(List<HarvestablePOIConfig.HarvestablePOIParams> __result)
        {
            //List<HarvestablePOIConfig.HarvestablePOIParams> list = new List<HarvestablePOIConfig.HarvestablePOIParams>();

            //list.Add(new HarvestablePOIConfig.HarvestablePOIParams(
            //    "satellite_field",
            //    new HarvestablePOIConfigurator.HarvestablePOIType(
            //        "SatelliteField",
            //        new Dictionary<SimHashes, float>{
            //            { SimHashes.Sand,3f},
            //            { SimHashes.IronOre,3f },
            //            { SimHashes.MoltenCopper,2.67f },
            //            { SimHashes.Glass, 1.33f}
            //        },
            //        30000f,
            //        45000f,
            //        30000f,
            //        60000f,
            //        true,
            //        HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));

            foreach(var tmp in __result)
            {
                if (tmp.anim == "radioactive_gas_cloud")
                {
                    //  tmp.HarvestablePOIType
                    tmp.poiType.harvestableElements.Add(SimHashes.Katairite, 1f);//深渊晶石
                    tmp.poiType.harvestableElements.Add(SimHashes.Fossil, 0.5f);//化石
                    tmp.poiType.harvestableElements.Add(SimHashes.Radium, 0.5f);//镭
                    tmp.poiType.harvestableElements.Add(SimHashes.SolidMercury, 0.5f);//汞固体
                    tmp.poiType.harvestableElements.Add(SimHashes.SolidNaphtha, 0.5f);//石脑油 固体
                }
            }

        }
    }
}
