using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static InfiniteResearch.BUILDING.STATUSITEMS;
using static LogicGateBase;
using static Operational;

namespace InfiniteResearch
{
    class InfiniteResearchCenterButtonPatch : InfiniteModeToggleButton
    {
        [MyCmpGet] ResearchCenter researchCenter;
        static bool IsEndlessWorking(Workable instance)
        {
            var ins = instance.GetComponent<InfiniteModeToggleButton>();
            if (ins != null)
                return ins.isInfiniteMode;
            return false;
        }
        protected override void UpdateState()
        {
           
            if (researchCenter != null)
            {
                var fun = researchCenter.GetType().GetMethod("UpdateWorkingState",
                    BindingFlags.Instance |
                    BindingFlags.NonPublic | BindingFlags.Public);
                // var fun = AccessTools.Method(typeof(ResearchCenter), "UpdateWorkingState" );
                /*   if (fun != null)
                       fun.Invoke(researchCenter, null);*/
               
                 global::Debug.LogWarning("UpdateState: "+ fun);
                var par=fun.GetParameters();

                
                fun.Invoke(researchCenter, par );
                // isInfiniteMode = true;  
               Chore ___chore= (Chore)AccessTools.Field(typeof(ResearchCenter), "chore").GetValue(researchCenter);
                var __instance = researchCenter;
                if (___chore == null && !IsEndlessWorking(__instance))
                {
                    //什么也不做
                }
                if (___chore == null && IsEndlessWorking(__instance))
                {
                    // var fun = AccessTools.Method(typeof(ResearchCenter), "CreateChore");
                    // ___chore = (Chore)fun.Invoke(__instance,null);
                    //      global::Debug.LogWarning("RemoveDupe ....chore:"+chore);
                    ___chore = CreateChoreAccess(__instance);
                    return;
                }
                if (___chore != null && IsEndlessWorking(__instance))
                {
                    //要判是不是初始化的
                    ___chore = CreateChoreAccess(__instance);
                    return;
                }
                if (___chore != null && !IsEndlessWorking(__instance))
                {
                    ___chore.Cancel("disable");//取消任务
                    
                    if (___chore.driver != null)
                        ___chore.driver.StopChore();
                }
            }

            // researchCenter.UpdateWorkingState(null);
        }
        private static bool CanPreemptCB(Chore.Precondition.Context context)
        {
            // global::Debug.LogWarning("判断任务是不是可以抢占");
            return false;
        }

        public static Chore CreateChoreAccess(ResearchCenter __instance)
        {
             global::Debug.LogWarning("CreateChoreAccess 创建了新任务");
            var chore = new WorkChore<ResearchCenter>(Db.Get().ChoreTypes.PowerTinker, __instance, null, true, null, null, null, true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true)
            {
                preemption_cb = new Func<Chore.Precondition.Context, bool>(CanPreemptCB)
            };
            var precondition = new Chore.Precondition()
            {
                id = "RequireAttributeRange",
                fn = delegate (ref Chore.Precondition.Context context, object data) {
                   // Console.Write("Precondition....." + data);
                    return IsEndlessWorking(__instance);
                },
                description = DUPLICANTS.CHORES.PRECONDITIONS.REQUIRES_ATTRIBUTE_RANGE.DESCRIPTION
            };
            chore.AddPrecondition(precondition);
          
            //修改内部变量
            AccessTools.Field(typeof(ResearchCenter), "chore").SetValue(__instance, chore);
            //  var op = new Operational();
            //  op.IsOperational = true;
            // op.IsActive = true;
            //op.SetActive(true);
            
             Operational op = (Operational)AccessTools.Field(typeof(ResearchCenter), "operational").GetValue(__instance);
             
             
            op.SetFlag(ResearchCenter.ResearchSelectedFlag, true);//不生效.
            op.Flags[ResearchCenter.ResearchSelectedFlag] = true;
            AccessTools.Method(typeof(Operational), "UpdateOperational").Invoke(op, null);//需要更新状态.
       

            Dictionary<Flag, bool>.Enumerator enumerator = op.Flags.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Flag tmp = enumerator.Current.Key;
                 global::Debug.LogWarning("-->.Operational.Flags  :  " + tmp.Name+ enumerator.Current);
            }
        
            var opP = AccessTools.Field(typeof(Operational), "IsOperational");
             global::Debug.LogWarning("Operational.IsOperational(反射)  :  " + opP);
            var opM = AccessTools.Method(typeof(Operational), "IsOperational");
             global::Debug.LogWarning("Operational.IsOperational(方法)  :  " + opM);
             global::Debug.LogWarning("Operational.IsOperational  :  " + op.IsOperational);

             

            // RequestDelivery();
            //this.NotifyResearchCenters(GameHashes.ActiveResearchChanged, this.queuedTech);
            __instance.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected, false);
            //__instance.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoApplicableResearchSelected, null);
             var requiresAttributeRange = new StatusItem("RequiresAttributeRange", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID)
            {
                resolveStringCallback = (string str, object obj) =>
                {
                    var minMax = ((int, int))obj;
                    return str.Replace("{Attributes}", minMax.Item1 + " - " + minMax.Item2);
                }
            };
            __instance.GetComponent<KSelectable>().AddStatusItem(requiresAttributeRange);
            var mkg = __instance.GetComponent<ManualDeliveryKG>();
         
           // mkg.choreTypeIDHash = Db.Get().ChoreTypes.Build.IdHash;//修改任务类型
            mkg.UpdateDeliveryState();
            __instance.SetWorkTime(float.PositiveInfinity);
            return chore;
        }
    }

    [HarmonyPatch]
    class ResearchCenterConfig_Patch
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(ResearchCenterConfig), nameof(ResearchCenterConfig.ConfigureBuildingTemplate));
            yield return AccessTools.Method(typeof(AdvancedResearchCenterConfig), nameof(AdvancedResearchCenterConfig.ConfigureBuildingTemplate));
            yield return AccessTools.Method(typeof(CosmicResearchCenterConfig), nameof(CosmicResearchCenterConfig.ConfigureBuildingTemplate));
        }

        static void Postfix(GameObject go) => go.AddComponent<InfiniteResearchCenterButtonPatch>();
    }
}
