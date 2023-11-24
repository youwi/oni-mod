using Database;
using HarmonyLib;
using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Timers;
using UnityEngine;

namespace TemporalTearOpenerPatch
{


    [HarmonyPatch(typeof(TemporalTearOpener.Instance), "OpenTemporalTear")]
    public class OpenTemporalTearPatchPlanA
    {
        public static bool Prefix(TemporalTearOpener.Instance __instance)
        {
            // 默认情况下,进入动画就触发陨石,需要判断是不是建造和加载游戏.
            ClusterManager.Instance.GetClusterPOIManager().RevealTemporalTear();
            if (ClusterManager.Instance.GetClusterPOIManager().IsTemporalTearOpen())
                return false;
            //判断建造

            //判断加载

            //判断不了

            return true;
        }
        public static void AAPostfix(TemporalTearOpener.Instance __instance)
        {
            int openerWorldId = __instance.GetComponent<StateMachineController>().GetMyWorldId();//这个好像只能获取主星的ID

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
            //2个方法都可行.
            //ClusterManager.Instance.GetClusterPOIManager().OpenTemporalTear(openerWorldId);
            ClusterManager.Instance.GetWorld(openerWorldId).GetSMI<GameplaySeasonManager.Instance>().StartNewSeason(Db.Get().GameplaySeasons.TemporalTearMeteorShowers);
            //清理粒子
            HighEnergyParticleStorage highEnergyParticleStorage = __instance.GetComponent<HighEnergyParticleStorage>();
            if (highEnergyParticleStorage != null)
                highEnergyParticleStorage.ConsumeAll();
            // highEnergyParticleStorage.IsFull
        }
    }

    // [HarmonyPatch(typeof(TemporalTearOpener),"InitializeStates")]
    public class TemporalTearOpenerInitPatch
    {
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

    [HarmonyPatch(typeof(TemporalTearOpener.Instance), "SidescreenEnabled")]
    public class SidescreenEnabledPatch
    {
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

        public static void Postfix(TemporalTearOpener.Instance __instance)
        {
            //内部自动延时.
            // var sm=Traverse.Create(__instance).Field("charging").GetValue() as TemporalTearOpener.ChargingState;
            var hps = __instance.GetComponent<HighEnergyParticleStorage>();
            // GameHashes.OnParticleStorageChanged= - 1837862626
            hps.Subscribe((int)GameHashes.OnParticleStorageChanged  , new Action<object>((o) =>
            {
                if (hps.IsFull()
                    && ClusterManager.Instance.GetClusterPOIManager().IsTemporalTearOpen())
                { //防误触发
                    Debug.Log("<<<<<触发陨石>>>>>>>>");
                    SidescreenButtonInteractablePatch.autoFire(__instance);
                }
            }));
            // 充能会无限触发.
            // 加载时会触发一次.
            // Debug.Log("<<<<<UpdateMeter 充能>>>>>>>>>>");
        }
    }

    [HarmonyPatch(typeof(TemporalTearOpener.Instance), "SidescreenButtonInteractable")]
    public class SidescreenButtonInteractablePatch
    {

        public static bool Postfix(bool __result, TemporalTearOpener.Instance __instance)
        {
            //  __instance.m
            if (__instance.GetComponent<HighEnergyParticleStorage>().IsFull())
            {
                __result = true;
                // SidescreenButtonInteractablePatch.autoFire(__instance);
            }
            // __instance.GoTo(__instance.sm.opening_tear_beam_pre);
            // TemporalTearOpener.Instance.FireTemporalTearOpener();
            //强制显示菜单,强制让按钮可点击.
            return __result;
        }
        static bool fireing = false; //正在操作
        public static void autoFire(TemporalTearOpener.Instance __instance)
        {
            if (fireing == true) { return; };//已经开始了.防重进
            fireing = true;
            int worldId = __instance.GetMyWorldId();
            var st = new System.Timers.Timer(3000); //延迟
            st.AutoReset = false;
            st.Enabled = true;
            st.Elapsed += (object data2, ElapsedEventArgs ss) =>
            {
                fireing = false;//后面容易出异常
                //方案一:内置启动
                //  Traverse.Create(__instance) .Method("OpenTemporalTear");
                //方案二:手动启动 
                __instance.GetComponent<HighEnergyParticleStorage>().ConsumeAll();//重置
                __instance.CreateBeamFX();//冲天特别动画,对的.
                //方案D:
                randomMeterPlanD(worldId);
                //方案ABC
                //var gameplaySeason = randomMeterPlanC(); //PlanA好像出错了. PlanB太慢了.
                //var gsi = ClusterManager.Instance.GetWorld(worldId).GetSMI<GameplaySeasonManager.Instance>();
                //gsi.StartNewSeason(gameplaySeason);
                //var dlcFlag=DlcManager.IsContentActive(gameplaySeason.dlcId);


                //不为空:
                //var ct = gsi.activeSeasons.Count;
                // Debug.Log($"<<<<<AutoFire---on-> Count:{ct}>>>");//tt.Id和Name一样
                //Debug.Log($"<<<<<AutoFire--end-on->world:{worldId}>>>{gameplaySeason.Name}:{dlcFlag}");//tt.Id和Name一样

            };
            st.Start();
            Debug.Log("<<<<<autoFire--10s-will-->>>>>>>>>>");
            //默认为标准陨石.

            // Db.Get().GameplaySeasons  
            // GameScreenManager.Instance.Scr

        }

        static List<string> nameList = null;
        static List<MeteorShowerEvent> msList = null;
        public static void buildMsList()
        {
            if (nameList == null)
            {
                nameList = new List<string>();
                msList = new List<MeteorShowerEvent>();
                var fieldList = typeof(Database.GameplayEvents).GetFields();
                //var fieldListB = Traverse.Create(Db.Get().GameplayEvents).Fields();
                int countA = 0;
                foreach (var field in fieldList)
                {
                    if (field.DeclaringType == typeof(MeteorShowerEvent)
                        ||field.Name.EndsWith("Shower")
                        ||field.Name.StartsWith("MeteorShower")
                       // ||field.Name== "GassyMooteorEvent" //海牛
                        )
                    {
                        nameList.Add(field.Name);
                        MeteorShowerEvent ev = (MeteorShowerEvent)field.GetValue(Db.Get().GameplayEvents);
                        msList.Add(ev);
                        countA++;
                    };
                };
                Debug.Log($"--------->buildMsList:{countA}");
            }
        }
        public static GameplaySeason randomMeterPlanA()
        {
            // 获取  MeteorShowerEvent 列表
            //手动制作一个陨石.
            buildMsList();
            GameplaySeason rndMeteorShowerSeason = new MeteorShowerSeason(
              "TemporalTearMeteorShowers",  //ID
              GameplaySeason.Type.World,
              "EXPANSION1_ID", //DLC 
              1f,     //period 自带周期循环 
              false,  //synchronizedToPeriod
              0f,     //randomizedEventStartTime
              false,  //startActive
              -1,     //finishAfterNumEvents
              0f,     //minCycle
              float.PositiveInfinity,  //maxCycle
              1,      // numEventsToStartEachPeriod
              false,  //affectedByDifficultySettings
              -1f);  //延时:clusterTravelDuration
                     //rndMeteorShowerSeason.Disabled = true;
                     // GameplaySeasonInstance 为创建主方法
                     //  逻辑绑定 WillNeverRunAgain() ==> numTimesAllowed != -1 ,
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

        static bool initedPlanC = false;
        public static GameplaySeason randomMeterPlanC()
        {
            // Db.Get().GameplaySeasons.TemporalTearMeteorShowers
            var ss = Db.Get().GameplaySeasons.TemporalTearMeteorShowers;
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
            //  ss.period = 1;
            return Db.Get().GameplaySeasons.TemporalTearMeteorShowers;
        }
        public static GameplaySeason randomMeterPlanC2()
        { //有bug测试有问题.直接用原版看看.
            return Db.Get().GameplaySeasons.TemporalTearMeteorShowers;
        }
        public static GameplaySeason randomMeterPlanD(int worldId)
        {  // D方案直接用调用event ID,不用GameplaySeason
            //GameplaySeasonManager
            buildMsList();
            for(int i = 0; i < 3; i++)
            {  //循环3次
                var ttm = GameplayEventManager.Instance.StartNewEvent(
                     msList[UnityEngine.Random.Range(0, msList.Count)], worldId,
                     new Action<StateMachine.Instance>(callbackInfo));
                 ttm.StartEvent();
               // GameplayEventManager.Instance
               // ttm.disa
            }
             
            return null;
        }
        public static void callbackInfo(StateMachine.Instance st)
        {
            Debug.Log($"randomMeterPlanD回调事件触发");
        }
    }


    [HarmonyPatch(typeof(GameplaySeasonManager.Instance), "Update")]
    public class GameplaySeasonManagerPatch
    {
        public static void Prefix(GameplaySeasonManager.Instance __instance)
        {
            //清理多余的陨石,以前的陨石要删除.
            if (__instance.activeSeasons.Count() > 1)
            {
                Debug.Log("--GameplaySeasonManager.Instance.Update()--前置删除前:activeSeasons:Count>>>>>" + __instance.activeSeasons.Count);

                var obj = __instance.activeSeasons.Last();
                // __instance.activeSeasons.Clear();
                Debug.Log("--GameplaySeasonManager.Instance.Update()----前置删除后:activeSeasons:Count>>>>>" + __instance.activeSeasons.Count);
                // __instance.activeSeasons.Add(obj);  //前置删除试试.
            }
        }
    }


    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]  //添加建筑到菜单中
    public class GeneratedBuildingsPatch
    {
        private static void Postfix()
        {
            ModUtil.AddBuildingToPlanScreen("Base", "TemporalTearOpener");
        }
    }
    [HarmonyPatch(typeof(TemporalTearOpenerConfig), "CreateBuildingDef")]  //出现在菜单
    public class TemporalTearOpenerConfig_Patch
    {

        public static BuildingDef Postfix(BuildingDef __result)
        {
            __result.ShowInBuildMenu = true;
            return __result;
        }

    }
    //补丁和前面的重复了
    //  [HarmonyPatch(typeof(TemporalTearOpenerConfig), "ConfigureBuildingTemplate")] //重复了
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
    //可拆可建
    [HarmonyPatch(typeof(TemporalTearOpenerConfig), "DoPostConfigureComplete")] //可拆
    public class DeconstructionPatch
    {
        [HarmonyPatch()]
        public static void Postfix(GameObject go)
        {
            go.GetComponent<Deconstructable>().allowDeconstruction = true;
            EstablishColonies.BASE_COUNT = 1;//强制修改数量为0;
        }

    }
}
