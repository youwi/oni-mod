﻿using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using static HarvestablePOIConfigurator;
using Formatting = Newtonsoft.Json.Formatting;

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
        public static void initConfigFile(string configFileName, List<HarvestablePOIType> list)
        {

            SortedDictionary<string, SortedDictionary<string, float>> configEntity = new SortedDictionary<string, SortedDictionary<string, float>>();

            if (!File.Exists(configFileName))
            {
                foreach (var tt in list)
                {
                    //SimHashes;
                    //HarvestablePOIConfig.gen
                    if (tt.id == "RadioactiveGasCloud") //辐射星
                    {
                        tt.harvestableElements.Remove(SimHashes.Katairite);
                        tt.harvestableElements.Remove(SimHashes.Fossil);
                        tt.harvestableElements.Remove(SimHashes.GoldAmalgam);
                        tt.harvestableElements.Remove(SimHashes.Niobium);
                        tt.harvestableElements.Remove(SimHashes.Resin);

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
                        tt.harvestableElements.Remove(SimHashes.SolidCarbonDioxide);//删除CO2.

                        tt.harvestableElements.Remove(SimHashes.CrudeOil); //原油
                        tt.harvestableElements.Remove(SimHashes.SolidNaphtha);// 石脑油固体
                        tt.harvestableElements.Remove(SimHashes.SolidCrudeOil); //固体原油.

                        tt.harvestableElements.Add(SimHashes.CrudeOil, 7.5f); //原油
                        tt.harvestableElements.Add(SimHashes.SolidNaphtha, 0.5f);//石脑油 固体
                        tt.harvestableElements.Add(SimHashes.SolidCrudeOil, 2f);// 固体原油

                    }
                    if (tt.id == "GildedAsteroidField") //金质小行星
                    {
                        tt.harvestableElements.Remove(SimHashes.Mercury);
                        tt.harvestableElements.Remove(SimHashes.Lead);
                        tt.harvestableElements.Remove(SimHashes.DepletedUranium);

                        tt.harvestableElements.Add(SimHashes.Mercury, 0.5f);//液体汞   solid
                        tt.harvestableElements.Add(SimHashes.Lead, 0.5f);//铅 固体
                        tt.harvestableElements.Add(SimHashes.DepletedUranium, 0.5f);//贫铀 
                    }
                    if (tt.id == "ForestyOreField") //森林小行星
                    {
                        tt.harvestableElements.Remove(SimHashes.Steel);
                        tt.harvestableElements.Remove(SimHashes.Phosphorite);
                        tt.harvestableElements.Remove(SimHashes.Phosphorus);

                        tt.harvestableElements.Add(SimHashes.Steel, 0.5f);//钢 固体
                        tt.harvestableElements.Add(SimHashes.Phosphorite, 0.05f);// 磷矿 固体
                        tt.harvestableElements.Add(SimHashes.Phosphorus, 0.1f);// 精炼磷 固体
                    }
                    if (tt.id == "SwampyOreField") //沼泽矿带
                    {
                        tt.harvestableElements.Remove(SimHashes.Fertilizer);
                        tt.harvestableElements.Remove(SimHashes.Ethanol);
                        tt.harvestableElements.Remove(SimHashes.Clay);

                        tt.harvestableElements.Add(SimHashes.Fertilizer, 0.5f);//肥料
                        tt.harvestableElements.Add(SimHashes.Ethanol, 0.5f);//乙醇
                        tt.harvestableElements.Add(SimHashes.Clay, 0.5f);//粘土
                    }
                    if (tt.id == "RockyAsteroidField") //岩石小行星
                    {
                        tt.harvestableElements.Remove(SimHashes.Katairite);
                        tt.harvestableElements.Remove(SimHashes.Obsidian);
                        tt.harvestableElements.Remove(SimHashes.Granite);
                        tt.harvestableElements.Remove(SimHashes.Lime);
                        tt.harvestableElements.Remove(SimHashes.MaficRock);

                        tt.harvestableElements.Add(SimHashes.Katairite, 0.1f);//深渊晶石
                        tt.harvestableElements.Add(SimHashes.Obsidian, 0.5f);//黑曜石
                        tt.harvestableElements.Add(SimHashes.Granite, 0.5f);// 花岗岩
                        tt.harvestableElements.Add(SimHashes.Lime, 0.01f);// 石灰 
                        tt.harvestableElements.Add(SimHashes.MaficRock, 1f);// 镁铁岩
                    }
                    if (tt.id == "HeliumCloud")  //氦气云
                    {
                        tt.harvestableElements.Remove(SimHashes.Lime);
                        tt.harvestableElements.Add(SimHashes.Lime, 0.5f);// 添加石灰 
                    }
                    if (tt.id == "ChlorineCloud")  //氯气云
                    {
                        tt.harvestableElements.Remove(SimHashes.SolidChlorine);
                        // tt.harvestableElements.Add(SimHashes.SolidChlorine, 0.5f);// 添加固态氯 
                    }
                    if (tt.id == "MetallicAsteroidField") //金属小行星带
                    {
                        tt.harvestableElements.Remove(SimHashes.GoldAmalgam);
                        tt.harvestableElements.Add(SimHashes.GoldAmalgam, 2f);//
                    }
                    if (tt.id == "SaltyAsteroidField")
                    {
                        tt.harvestableElements.Remove(SimHashes.Salt);
                        tt.harvestableElements.Add(SimHashes.Salt, 4f);//
                    }
                    if (tt.id == "OrganicMassField") //有机质带
                    {
                        tt.harvestableElements.Remove(SimHashes.Fertilizer);
                        tt.harvestableElements.Remove(SimHashes.Clay);
                        tt.harvestableElements.Add(SimHashes.Fertilizer, 0.5f);//肥料
                        tt.harvestableElements.Add(SimHashes.Clay, 0.5f);//粘土
                    }
                    if (tt.id == "CarbonAsteroidField")  //碳质小行星
                    {
                        tt.harvestableElements.Remove(SimHashes.Ceramic);
                        tt.harvestableElements.Add(SimHashes.Ceramic, 0.5f);// 添加陶瓷 
                    }
                    if (tt.id == "IceAsteroidField")  //爆炸的冰巨星
                    {
                        tt.harvestableElements.Remove(SimHashes.SolidMethane);// 需要删除先.
                        tt.harvestableElements.Add(SimHashes.SolidMethane, 2f);// 添加大量天然气
                    }
                    //Debug.Log("HarvestablePOIInstanceConfiguration 11111000000"+ tt.id);
                }
                foreach (var tt in list)
                {
                    SortedDictionary<string, float> keyValuePairs = new SortedDictionary<string, float>();

                    foreach (var kv in tt.harvestableElements)
                    {
                        keyValuePairs.Add(kv.Key.ToString(), kv.Value);
                    }
                    configEntity.Add(tt.id, keyValuePairs);
                }
                Debug.Log(" Dump:HarvestablePOIInstanceConfiguration ---->:" + configFileName);
                File.WriteAllText(configFileName, JsonConvert.SerializeObject(configEntity, Formatting.Indented));
            }
        }
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

                var configFileName = KMod.Manager.GetDirectory() + "/HarvestablePOIConfig.json";

                if (!File.Exists(configFileName))
                    initConfigFile(configFileName, list);

                SortedDictionary<string, SortedDictionary<string, float>> configEntityMy
                    = Newtonsoft.Json.JsonConvert.DeserializeObject<SortedDictionary<string, SortedDictionary<string, float>>>(File.ReadAllText(configFileName));

                try
                {
                    foreach (var oriPoi in list)
                    {
                        var REFC = Traverse.CreateWithType("SimHashes");
                        // REFC.Field("Katairite").GetValue<SimHashes>();
                        if (!configEntityMy.ContainsKey(oriPoi.id))
                        {  // 有的mod会添加POI.
                            Debug.LogWarning(" ---->>>PoiAddResource: Key not exist:" + oriPoi.id);
                            continue;
                        }
                        var kv = configEntityMy[oriPoi.id];//配置文件可以直接读取.
                        if (kv != null)
                        {
                            foreach (var nameInt in kv)
                            {
                                var field = REFC.Field(nameInt.Key);
                                if (field != null) //防止乱写配置文件
                                {
                                    var hash = field.GetValue<SimHashes>();
                                    if (oriPoi.harvestableElements.ContainsKey(hash))
                                        oriPoi.harvestableElements.Remove(hash);
                                    oriPoi.harvestableElements.Add(hash, nameInt.Value);

                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("--->HarvestablePOIInstanceConfigurationConPatchA:>>>" + ex.Message);
                }

                // 现在设置为初始代码.

                inited = true;
            }




        }
        public static int GetLineNumber(Exception ex)
        {
            var lineNumber = 0;
            const string lineSearch = ":line ";
            var index = ex.StackTrace.LastIndexOf(lineSearch);
            if (index != -1)
            {
                var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                if (int.TryParse(lineNumberText, out lineNumber))
                {
                }
            }
            return lineNumber;
        }
    }


}
