using Database;
using HarmonyLib;
using Klei.AI;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Timers;
using UnityEngine;

namespace TemporalTearOpenerPatch
{
    //可拆可建
    [HarmonyPatch(typeof(TemporalTearOpenerConfig))]
    public class DeconstructionPatch
    {
        [HarmonyPatch("DoPostConfigureComplete")]
        public static void Postfix(GameObject go)
        {
            go.GetComponent<Deconstructable>().allowDeconstruction = true;
            EstablishColonies.BASE_COUNT = 1;//强制修改数量为0;
        }

    }
    [HarmonyPatch(typeof(TemporalTearOpener.Instance))]
    public class OpenTemporalTearPatch
    {
        [HarmonyPatch("OpenTemporalTear")]
        public static void Postfix(TemporalTearOpener.Instance __instance)

        {
            int openerWorldId = __instance.GetComponent<StateMachineController>().GetMyWorldId();//这个好像只能获取主星的ID
            ClusterManager.Instance.GetClusterPOIManager().OpenTemporalTear(openerWorldId);

            FieldInfo fld = typeof(TemporalTearOpener.Instance).GetField("charging"); //初始化
            if (fld != null)
            {
                var charging = fld.GetValue(__instance);
                __instance.GoTo((StateMachine.BaseState)charging);
            }


            //    FieldInfo fld_op = typeof(TemporalTearOpener.Instance).GetField("opening_tear_beam"); //opening_tear_beam
            //   var opening_tear_beam = fld_op.GetValue(__instance);
            //   __instance.GoTo((StateMachine.BaseState)opening_tear_beam);

            FieldInfo fld_mc = typeof(TemporalTearOpener.Instance).GetField("m_particlesConsumed");
            if (fld_mc != null)
            {
                fld_mc.SetValue(__instance, 0f);//点击开火,数据清0;
            }

            ClusterManager.Instance.GetWorld(openerWorldId).GetSMI<GameplaySeasonManager.Instance>().StartNewSeason(Db.Get().GameplaySeasons.TemporalTearMeteorShowers);
            //清理粒子
            HighEnergyParticleStorage highEnergyParticleStorage = __instance.GetComponent<HighEnergyParticleStorage>();
            if (highEnergyParticleStorage != null)
                highEnergyParticleStorage.ConsumeAll();
            // highEnergyParticleStorage.IsFull


        }
    }
    // [HarmonyPatch(typeof(TemporalTearOpener))]
    public class TemporalTearOpenerInitPatch
    {
        //     [HarmonyPatch("InitializeStates")]

        public static void Postfix(TemporalTearOpener __instance)
        {

            __instance.root.Enter(delegate (TemporalTearOpener.Instance smi)
            {

                //  smi.UpdateMeter();
                FieldInfo fld = typeof(TemporalTearOpener).GetField("charging"); //初始化
                FieldInfo ready_fld = typeof(TemporalTearOpener).GetField("ready"); //初始化
                FieldInfo check_fld = typeof(TemporalTearOpener).GetField("check_requirements"); //初始化

                //check_requirements
                if (fld != null)
                {

                    var charging = (GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State)fld.GetValue(__instance);

                    //   smi.GoTo((StateMachine.BaseState)check_fld.GetValue(__instance));

                    //添加触发器.满了进入ready状态.
                    var ready = (GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State)ready_fld.GetValue(__instance);
                    /*  ready.EventTransition(GameHashes.OnParticleStorageChanged,
                         ready,
                           delegate (TemporalTearOpener.Instance smi2)  {
                               //直接触发陨石,如果满的话.
                               HighEnergyParticleStorage highEnergyParticleStorage = smi2.GetComponent<HighEnergyParticleStorage>();
                               int openerWorldId = smi2.GetComponent<StateMachineController>().GetMyWorldId();//这个好像只能获取主星的ID
                               if (smi2.GetComponent<HighEnergyParticleStorage>().IsFull())
                               {
                                   highEnergyParticleStorage.ConsumeAll();
                                   ClusterManager.Instance.GetWorld(openerWorldId).GetSMI<GameplaySeasonManager.Instance>().StartNewSeason(Db.Get().GameplaySeasons.TemporalTearMeteorShowers);
                                   return true;
                               }
                               //清理粒子

                               return false;
                          });
                       */
                }
                /*                fld = typeof(TemporalTearOpener).GetField("check_requirements"); //转检查器.
                                if (fld != null)
                                {
                                    var check_requirements = fld.GetValue(__instance);
                                    smi.GoTo((StateMachine.BaseState)check_requirements);//进入初始状态.
                                }*/
            }).PlayAnim("off");//重新重置动画.

        }

    }

    [HarmonyPatch(typeof(TemporalTearOpener.Instance))]
    public class SidescreenEnabledPatch
    {
        [HarmonyPatch("SidescreenEnabled")]
        public static bool Postfix(bool __result, TemporalTearOpener.Instance __instance)
        {
            //  __instance.m
            if (__instance.GetComponent<HighEnergyParticleStorage>().IsFull())
                __result = true;
            //强制显示菜单
            return __result;
        }

    }
    [HarmonyPatch(typeof(TemporalTearOpener.Instance), "UpdateMeter")]
    public class TemporalTearOpener_UpdateMeter_Patch
    {
        public static void Postfix( TemporalTearOpener.Instance __instance)
        {
            //内部自动延时.
            if (__instance.GetComponent<HighEnergyParticleStorage>().IsFull())
                SidescreenButtonInteractablePatch.autoFire(__instance);
        }
    }

    [HarmonyPatch(typeof(TemporalTearOpener.Instance))]
    public class SidescreenButtonInteractablePatch
    {
        [HarmonyPatch("SidescreenButtonInteractable")]
        public static bool Postfix(bool __result, TemporalTearOpener.Instance __instance)
        {
            //  __instance.m
            if (__instance.GetComponent<HighEnergyParticleStorage>().IsFull())
            {
                __result = true;
          
            }
            // __instance.GoTo(__instance.sm.opening_tear_beam_pre);
            // TemporalTearOpener.Instance.FireTemporalTearOpener();
            //强制显示菜单,强制让按钮可点击.
            return __result;
        }
        static bool fireing=false; //正在操作
        public static void autoFire(TemporalTearOpener.Instance __instance)
        {
            if(fireing == true) { return; };//已经开始了.防重进
            fireing = true;
            int worldId = __instance.GetMyWorldId();
            var st = new System.Timers.Timer(10000);
            st.AutoReset = false;
            st.Enabled = true;
            st.Elapsed += (object data2, ElapsedEventArgs ss) =>
            {
                fireing = false;//后面容易出异常
                //方案一:内置启动
                //  Traverse.Create(__instance) .Method("OpenTemporalTear");
                //方案二:手动启动 
                var tt = randomMeterPlanC(); //PlanA好像出错了. PlanB太慢了.
                
                ClusterManager.Instance.GetWorld(worldId).GetSMI<GameplaySeasonManager.Instance>() .StartNewSeason(tt);
               
                Debug.LogWarning("<<<<<autoFire--end-on--->>>>>>>>>>"+tt.Name+tt.Id);
            };
            st.Start();
            Debug.LogWarning("<<<<<autoFire--10s-will-->>>>>>>>>>");
            //默认为标准陨石.

            // Db.Get().GameplaySeasons  
            // GameScreenManager.Instance.Scr

        }
        
        static List<string> nameList =null;
        static List<MeteorShowerEvent> msList =null;
        public static GameplaySeason randomMeterPlanA()
        {
            // 获取  MeteorShowerEvent 列表
            //手动制作一个陨石.
           if(nameList==null)
            {
                nameList=new List<string>();
                msList=new List<MeteorShowerEvent>();
                var fieldList = typeof(Database.GameplayEvents).GetFields();
                foreach (var field in fieldList)
                {
                    if (field.DeclaringType == typeof(MeteorShowerEvent))
                    {
                        nameList.Add(field.Name);
                        MeteorShowerEvent ev =(MeteorShowerEvent)field.GetValue(Db.Get().GameplayEvents);
                        msList.Add(ev);
                    };
                };
            }
            GameplaySeason rndMeteorShowerSeason = new MeteorShowerSeason(
              "TemporalTearMeteorShowers",
              GameplaySeason.Type.World,
              "EXPANSION1_ID",
              1f, //1周期立即落下
              false, 0f, false, -1, 0f,
              float.PositiveInfinity,
              1, false, -1f);//延时:clusterTravelDuration

            rndMeteorShowerSeason.events.Clear();
            // 方案一:随机3个效果
            for (int i = 0; i < 3; i++)
            {
                var ridc = UnityEngine.Random.Range(0, nameList.Count);
                rndMeteorShowerSeason.AddEvent(msList[ridc]);
            }

            //方案二,直接写,
            //rndMeteorShowerSeason
            //   .AddEvent(Db.Get().GameplayEvents.MeteorShowerDustEvent)
            //   .AddEvent(Db.Get().GameplayEvents.MeteorShowerGoldEvent)
            //   .AddEvent(Db.Get().GameplayEvents.MeteorShowerIronEvent)
            //   .AddEvent(Db.Get().GameplayEvents.MeteorShowerCopperEvent)
            //   .AddEvent(Db.Get().GameplayEvents.ClusterIceShower)
            //   .AddEvent(Db.Get().GameplayEvents.MeteorShowerFullereneEvent);
           
            return rndMeteorShowerSeason;
        }
        public static GameplaySeason randomMeterPlanB()
        {
            //随机的只有一个TemporalTearMeteor是立即产生的,其它要其它陨石雨要20周期.
            //GameplaySeasons 随机找一个...
            var tmp = Db.Get().GameplaySeasons;

            var rid = UnityEngine.Random.Range(0, tmp.Count);
            var tt = Db.Get().GameplaySeasons.resources[rid];
            if (tt.Id == "MeteorShowers")
            {
                return Db.Get().GameplaySeasons.TemporalTearMeteorShowers;
            }
            if (tt.Id.EndsWith("MeteorShowers"))
            {
                return tt;
            }
           return Db.Get().GameplaySeasons.TemporalTearMeteorShowers;
        }

        static bool initedPlanC=false;
        public static GameplaySeason randomMeterPlanC()
        {
            // Db.Get().GameplaySeasons.TemporalTearMeteorShowers
            if (initedPlanC == false)
            {
                Db.Get().GameplaySeasons.TemporalTearMeteorShowers
                 .AddEvent(Db.Get().GameplayEvents.MeteorShowerDustEvent)
                 .AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower)
                 .AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower)
                 .AddEvent(Db.Get().GameplayEvents.ClusterRegolithShower)
                 .AddEvent(Db.Get().GameplayEvents.ClusterBleachStoneShower)
                 .AddEvent(Db.Get().GameplayEvents.ClusterUraniumShower)
                 .AddEvent(Db.Get().GameplayEvents.ClusterGoldShower)
                 .AddEvent(Db.Get().GameplayEvents.MeteorShowerFullereneEvent);
                initedPlanC = true;
            }
            return Db.Get().GameplaySeasons.TemporalTearMeteorShowers;


        }
    }

      

    //清理多余的陨石
    [HarmonyPatch(typeof(GameplaySeasonManager.Instance))]
    public class GameplaySeasonManagerPatch
    {
        [HarmonyPatch("Update")]
        public static void Prefix(GameplaySeasonManager.Instance __instance)
        {
            if (__instance.activeSeasons.Count() > 0)
            {
                var obj = __instance.activeSeasons.Last();
                __instance.activeSeasons.Clear();
                __instance.activeSeasons.Add(obj);
            }
        }
    }


    // copy /Y "C:/Users/amd/source/repos/oni-mod-test/bin/Debug/oni-mod-test.dll"   "D:/Doc/Klei/OxygenNotIncluded\mods/local/OpenTest/oni-mod-test.dll" 

    //添加建筑到菜单中
    [HarmonyPatch(typeof(GeneratedBuildings))]
    [HarmonyPatch("LoadGeneratedBuildings")]
    public class GeneratedBuildingsPatch
    {
        private static void Postfix()
        {
            ModUtil.AddBuildingToPlanScreen("Base", "TemporalTearOpener");
            
        }
    }
    [HarmonyPatch(typeof(TemporalTearOpenerConfig))]
    public class TemporalTearOpenerConfig_Patch
    {
        [HarmonyPatch("CreateBuildingDef")]
        public static BuildingDef Postfix(BuildingDef __result)
        {
            __result.ShowInBuildMenu = true;
            return __result;
        }

    }
    [HarmonyPatch(typeof(TemporalTearOpenerConfig), "ConfigureBuildingTemplate")]
    public class TemporalTearOpenerConfigDeconstruction_Patch
    {
        public static void Postfix(GameObject go)
        {
            go.AddOrGet<Deconstructable>();
            PrimaryElement component = go.GetComponent<PrimaryElement>();
            component.SetElement(SimHashes.Steel);//钢
            var obj = go.GetComponent<Deconstructable>();
            if (obj != null)
            {
                obj.allowDeconstruction = true;
            }
            // inst.FindOrAddComponent<Deconstructable>();
        }
    }
}
