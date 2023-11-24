using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoubleMutantMod
{
    [HarmonyPatch(typeof(MutantPlant), nameof(MutantPlant.Mutate))]
    public class DoubleMutantModPatch
    {
        //使用名单机制. 算了,这个表太大
        public static string[] blickList =
        {
                    "moderatelyLoose_moderatelyTight",
                    "moderatelyLoose_extremelyTight",
               //     "moderatelyLoose_bonusLice",
                    "moderatelyLoose_sunnySpeed",
               //     "moderatelyLoose_slowBurn",
                //    "moderatelyLoose_blooms",
                    "moderatelyLoose_loadedWithFruit",
                    "moderatelyLoose_heavyFruit",
                    "moderatelyLoose_rottenHeaps",
               //     "moderatelyTight_extremelyTight",
               //     "moderatelyTight_bonusLice",
                    "moderatelyTight_sunnySpeed",
               //     "moderatelyTight_slowBurn",
               //     "moderatelyTight_blooms",
                    "moderatelyTight_loadedWithFruit",
                    "moderatelyTight_heavyFruit",
                    "moderatelyTight_rottenHeaps",
               //     "extremelyTight_bonusLice",
                    "extremelyTight_sunnySpeed",
               //     "extremelyTight_slowBurn",
               //     "extremelyTight_blooms",
                    "extremelyTight_loadedWithFruit",
                    "extremelyTight_heavyFruit",
                    "extremelyTight_rottenHeaps",
              //      "bonusLice_sunnySpeed",
              //      "bonusLice_slowBurn",
              //      "bonusLice_blooms",
              //      "bonusLice_loadedWithFruit",
              //      "bonusLice_heavyFruit",
              //      "bonusLice_rottenHeaps",
               //     "sunnySpeed_slowBurn",
               //     "sunnySpeed_blooms",
                    "sunnySpeed_loadedWithFruit",
                    "sunnySpeed_heavyFruit",
              //      "sunnySpeed_rottenHeaps", //bug级别
                //    "slowBurn_blooms",
                //    "slowBurn_loadedWithFruit",
                //    "slowBurn_heavyFruit",
                //    "slowBurn_rottenHeaps",
               //     "blooms_loadedWithFruit",
               //     "blooms_heavyFruit",
               //     "blooms_rottenHeaps",
                    "loadedWithFruit_heavyFruit",
                    "loadedWithFruit_rottenHeaps",
                    "heavyFruit_rottenHeaps"
        };
        public static void Postfix(MutantPlant __instance)
        {
            //		plant.AddOrGet<SeedProducer>().Configure(id, productionType, numberOfSeeds);
            try
            {

                var field = __instance.GetType().GetField("mutationIDs");
                List<string> list = null;
                if (field != null)
                {
                    list = (List<string>)field.GetValue(__instance);
                }
                if (list == null)
                {
                    // global::Debug.Log("变异listNull:" + list);
                    list = new List<string>();
                }
                if (list.Count > 2)
                {   //已经双重变异就不需要了.
                    global::Debug.Log("变异已经大于2:" + list);
                    return;
                }
                if (rand100() > 80) //注意双重随机数会重叠.
                {
                    //注释看看能不能加性能

                  //  global::Debug.Log("二次变异率为80%,变异未触发");
                    return;
                }
                //添加一轮变异,按25%概率添加.
                //黑名单本身机率减一半,
                /* 
                 * // 这里是A方案.

                string name = Db.Get().PlantMutations.GetRandomMutation(__instance.PrefabID().Name).Id;
                string name2 = Db.Get().PlantMutations.GetRandomMutation(__instance.PrefabID().Name).Id;
                string nameTmp = name + "_" + name2;//

                global::Debug.Log("变异目标:" + nameTmp + ",匹配:" + blickList.Contains(nameTmp));

                list.Add(name);//一级变异,
                if (blickList.Contains(nameTmp)) //机率再减50%
                {
                    if (name == "sunnySpeed" && name2 == "rottenHeaps") return;//bug级
                    if (name == "rottenHeaps" && name2 == "sunnySpeed") return;//bug级

                    //if (name == "slowBurn" || name2 == "slowBurn")
                    //    return;//野化 为垃圾变异,不需要了.
                    //if (name == "blooms" || name2 == "blooms")
                    //    return;//盛开 为垃圾变异,不需要了.
                    //if (name == "bonusLice" || name2 == "bonusLice")
                    //    return;//米虱  为垃圾变异,不需要了.
                    //if (name == "moderatelyTight" || name2 == "moderatelyTight")
                    //    return;//专化 ,不需要了.

                    //if (name == "rottenHeaps" && name2 == "loadedWithFruit") { return; }//无用的 

                    ////无视顺序的删除一半.按字母顺序.
                    //if (name == "rottenHeaps" ) return;//旺盛减一半,只后置.

                    //if ( name== "heavyFruit") return;// 硕果减一半. 只后置.

                    //if (name == "extremelyTight") return;//超专化减一半.

                    //if (name == "heavyFruit") return;// 硕果减一半.

                    // moderatelyLoose; 温和
                    //-- moderatelyTight; 专化
                    // extremelyTight; 超专化
                    //-- bonusLice; 米虱   垃圾
                    // sunnySpeed; 绿叶  要光,-50周期,不能和旺盛一起出.
                    //-- slowBurn; 野化
                    //-- blooms; 盛开 加20装饰
                    // loadedWithFruit; 富饶 要光
                    // heavyFruit; 硕果
                    // rottenHeaps;旺盛

                    list.Add(name2);//防止重复添加,如果重复了就当成一次变异
                }
                __instance.SetSubSpecies(list);
                */
                // 这里是B方案
                __instance.SetSubSpecies(randInList().Split('_').ToList());

            }
            catch (Exception ex)
            {
                global::Debug.Log(ex.Message);
            }

        }
        static Random randA = new System.Random();
        static Random randB = new System.Random();

        public static int rand100()
        {

            randB.Next();
            randB.Next(1000);
            return randB.Next(1, 100);
        }
        public static string randInList()
        {

            string ot = blickList[randA.Next(0, blickList.Length)];
          //  global::Debug.Log("双重变异为:" + ot);
            return ot;
        }
    }
}
