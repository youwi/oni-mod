using HarmonyLib;

using System.Collections.Generic;

using static HarvestablePOIConfig;
using static HarvestablePOIConfigurator;

namespace PoiAddResource
{

    // [HarmonyPatch(typeof(HarvestablePOIConfig), "GenerateConfigs", new Type[] { })]
    //public class PoiAddResourcePatch
    //{
    //    public static List<HarvestablePOIParams> patchList(List<HarvestablePOIParams> list)
    //    {
    //        foreach (var tmp in list)
    //        {
    //            if (tmp.anim == "radioactive_gas_cloud")
    //            {
    //                //  tmp.HarvestablePOIType
    //                tmp.poiType.harvestableElements.Add(SimHashes.Katairite, 1f);//深渊晶石
    //                tmp.poiType.harvestableElements.Add(SimHashes.Fossil, 0.5f);//化石
    //                tmp.poiType.harvestableElements.Add(SimHashes.Radium, 0.5f);//镭
    //                tmp.poiType.harvestableElements.Add(SimHashes.SolidMercury, 0.5f);//汞固体
    //                tmp.poiType.harvestableElements.Add(SimHashes.SolidNaphtha, 0.5f);//石脑油 固体

    //                tmp.poiType.harvestableElements.Add(SimHashes.SolidResin, 0.1f);//树脂 固体
    //                tmp.poiType.harvestableElements.Add(SimHashes.GoldAmalgam, 1f);//金汞矿 固体
    //                tmp.poiType.harvestableElements.Add(SimHashes.Niobium, 0.1f);//铌 固体

    //                break;
    //            }
    //            //List<HarvestablePOIConfig.HarvestablePOIParams> list = new List<HarvestablePOIConfig.HarvestablePOIParams>();

    //            //list.Add(new HarvestablePOIConfig.HarvestablePOIParams(
    //            //    "satellite_field",
    //            //    new HarvestablePOIConfigurator.HarvestablePOIType(
    //            //        "SatelliteField",
    //            //        new Dictionary<SimHashes, float>{
    //            //            { SimHashes.Sand,3f},
    //            //            { SimHashes.IronOre,3f },
    //            //            { SimHashes.MoltenCopper,2.67f },
    //            //            { SimHashes.Glass, 1.33f}
    //            //        },
    //            //        30000f,
    //            //        45000f,
    //            //        30000f,
    //            //        60000f,
    //            //        true,
    //            //        HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));

    //        }
    //        return list;
    //    }
        //public static List<HarvestablePOIParams> Postfix(HarvestablePOIConfig __instance)
        //{
        //    List<HarvestablePOIParams> list = __instance;
        //    list = patchList(list);
        //    return list;
        //}
        //public static void Postfix(HarvestablePOIConfig __instance, ref List<HarvestablePOIParams> __result)
        //{

        //    // IEnumerable
        //    // HarvestablePOIConfig.GenerateConfigs()
        //    if (__result == null)
        //    {
        //        return;
        //    }
        //    Debug.Log("HarvestablePOIConfig.GenerateConfigs()---->");
        //    patchList(__result);
        //}
   

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
                    //SimHashes;
                    //HarvestablePOIConfig.gen
                    if (tt.id == "RadioactiveGasCloud") //辐射星
                    {
                        tt.harvestableElements.Add(SimHashes.Katairite, 1f);//深渊晶石
                        tt.harvestableElements.Add(SimHashes.Fossil, 0.5f);//化石
                       // tt.harvestableElements.Add(SimHashes.Radium, 0.5f);//镭
                        tt.harvestableElements.Add(SimHashes.GoldAmalgam, 1f);//金汞齐 固体
                        tt.harvestableElements.Add(SimHashes.Niobium, 0.1f);//铌 固体
                        tt.harvestableElements.Add(SimHashes.Resin, 0.1f);//树脂  solid
                         //  Debug.Log("HarvestablePOIInstanceConfiguration O00000");
                     
                    }
                    if (tt.id == "OilyAsteroidField") //油星
                    {
                    
                        tt.harvestableElements.Add(SimHashes.Naphtha, 0.5f);//石脑油 固体

                    }
                    if (tt.id == "GildedAsteroidField") //金质小行星
                    {
                        tt.harvestableElements.Add(SimHashes.Mercury, 0.5f);//液体汞   solid
                        tt.harvestableElements.Add(SimHashes.Lead, 0.5f);//铅 固体
                        tt.harvestableElements.Add(SimHashes.DepletedUranium, 0.5f);//贫铀 
                    }
                    if (tt.id == "ForestyOreField") //森林小行星
                    {
                        tt.harvestableElements.Add(SimHashes.Steel, 0.5f);//钢 固体
                        tt.harvestableElements.Add(SimHashes.Phosphorite, 0.05f);// 磷矿 固体
                        tt.harvestableElements.Add(SimHashes.Phosphorus, 0.1f);// 精炼磷 固体
                    }
                    if (tt.id == "SwampyOreField") //沼泽矿带
                    {
                        tt.harvestableElements.Add(SimHashes.Fertilizer, 0.5f);//肥料
                        tt.harvestableElements.Add(SimHashes.Ethanol, 0.5f);//乙醇
                        tt.harvestableElements.Add(SimHashes.Clay, 0.5f);//粘土
                    }
                    if (tt.id == "RockyAsteroidField") //岩石小行星
                    {
                        tt.harvestableElements.Add(SimHashes.Obsidian, 0.5f);//黑曜石
                        tt.harvestableElements.Add(SimHashes.Granite, 0.5f);// 花岗岩
                        tt.harvestableElements.Add(SimHashes.Lime, 0.01f);// 石灰 
                        tt.harvestableElements.Add(SimHashes.MaficRock, 1f);// 镁铁岩
                    }
                    if(tt.id== "HeliumCloud")  //氦气云
                    {
                        tt.harvestableElements.Add(SimHashes.Lime, 0.5f);// 添加石灰 
                    }
                    if (tt.id == "CarbonAsteroidField")  //碳质小行星
                    {
                        tt.harvestableElements.Add(SimHashes.Ceramic, 0.5f);// 添加陶瓷 
                    }
                    if (tt.id == "IceAsteroidField")  //爆炸的冰巨星
                    {
                        tt.harvestableElements.Remove(SimHashes.SolidMethane);// 需要删除先.
                        tt.harvestableElements.Add(SimHashes.SolidMethane, 2f);// 添加大量天然气
                    }
                    //Debug.Log("HarvestablePOIInstanceConfiguration 11111000000"+ tt.id);
                }
                inited = true;
            }
          
            // Debug.Log("HarvestablePOIInstanceConfiguration ---->");

        }
    }

}

