using HarmonyLib;

using System.Collections.Generic;

using static HarvestablePOIConfig;
using static HarvestablePOIConfigurator;

namespace PoiAddResource
{

    // [HarmonyPatch(typeof(HarvestablePOIConfig), "GenerateConfigs", new Type[] { })]
    public class PoiAddResourcePatch
    {
        public static List<HarvestablePOIParams> patchList(List<HarvestablePOIParams> list)
        {
            foreach (var tmp in list)
            {
                if (tmp.anim == "radioactive_gas_cloud")
                {
                    //  tmp.HarvestablePOIType
                    tmp.poiType.harvestableElements.Add(SimHashes.Katairite, 1f);//深渊晶石
                    tmp.poiType.harvestableElements.Add(SimHashes.Fossil, 0.5f);//化石
                    tmp.poiType.harvestableElements.Add(SimHashes.Radium, 0.5f);//镭
                    tmp.poiType.harvestableElements.Add(SimHashes.SolidMercury, 0.5f);//汞固体
                    tmp.poiType.harvestableElements.Add(SimHashes.SolidNaphtha, 0.5f);//石脑油 固体

                    tmp.poiType.harvestableElements.Add(SimHashes.SolidResin, 0.1f);//树脂 固体
                    tmp.poiType.harvestableElements.Add(SimHashes.GoldAmalgam, 1f);//金汞矿 固体
                    tmp.poiType.harvestableElements.Add(SimHashes.Niobium, 0.1f);//铌 固体

                    break;
                }
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

            }
            return list;
        }
        //public static List<HarvestablePOIParams> Postfix(HarvestablePOIConfig __instance)
        //{
        //    List<HarvestablePOIParams> list = __instance;
        //    list = patchList(list);
        //    return list;
        //}
        public static void Postfix(HarvestablePOIConfig __instance, ref List<HarvestablePOIParams> __result)
        {

            // IEnumerable
            // HarvestablePOIConfig.GenerateConfigs()
            if (__result == null)
            {
                return;
            }
            Debug.LogWarning("HarvestablePOIConfig.GenerateConfigs()---->");
            patchList(__result);
        }
    }

    [HarmonyPatch(typeof(HarvestablePOIInstanceConfiguration), "GetElementsWithWeights")]
    public class HarvestablePOIInstanceConfigurationConPatch
    {
        static bool inited = false;
        public static void Postfix(HarvestablePOIConfigurator __instance)
        {
            if (!inited)
            {
                //HarvestablePOIConfigurator._poiTypes;
                var list = Traverse.CreateWithType("HarvestablePOIConfigurator")
                  .Field("_poiTypes")
                  .GetValue<List<HarvestablePOIType>>();
                if (list == null)
                    return;

                foreach (var tt in list)
                {

                    if (tt.id == "RadioactiveGasCloud") //辐射星
                    {
                        tt.harvestableElements.Add(SimHashes.Katairite, 1f);//深渊晶石
                        tt.harvestableElements.Add(SimHashes.Fossil, 0.5f);//化石
                        tt.harvestableElements.Add(SimHashes.Radium, 0.5f);//镭
                        tt.harvestableElements.Add(SimHashes.GoldAmalgam, 1f);//金汞矿 固体
                        tt.harvestableElements.Add(SimHashes.Niobium, 0.1f);//铌 固体
                        tt.harvestableElements.Add(SimHashes.Resin, 0.1f);//树脂  solid
                                                                          //  Debug.LogWarning("HarvestablePOIInstanceConfiguration O00000");
                        inited = true;
                    }
                    if (tt.id == "OilyAsteroidField") //油星
                    {
                        tt.harvestableElements.Add(SimHashes.Mercury, 0.5f);//液体汞   solid
                        tt.harvestableElements.Add(SimHashes.Naphtha, 0.5f);//石脑油 固体

                    }
                    if (tt.id == "GildedAsteroidField") //金质小行星
                    {
                        tt.harvestableElements.Add(SimHashes.Lead, 0.5f);//铅 固体
                        tt.harvestableElements.Add(SimHashes.DepletedUranium, 0.5f);//贫铀 
                    }
                    if (tt.id == "ForestyOreField") //森林小行星
                    {
                        tt.harvestableElements.Add(SimHashes.Steel, 0.5f);//钢 固体
                        tt.harvestableElements.Add(SimHashes.Phosphorite, 0.05f);// 磷矿 固体
                        tt.harvestableElements.Add(SimHashes.Phosphorus, 0.1f);// 精炼磷 固体
                    }

                    //Debug.LogWarning("HarvestablePOIInstanceConfiguration 11111000000"+ tt.id);
                }

            }
            // Debug.LogWarning("HarvestablePOIInstanceConfiguration ---->");

        }
    }


}
