using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ForceMuate
{
    [HarmonyPatch]
    class Mutate_Patch
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            //EntityTemplates.ExtendEntityToBasicPlant
            //GameObject
            yield return AccessTools.Method(typeof(EntityTemplates), nameof(EntityTemplates.ExtendEntityToBasicPlant));
        }

        static void Postfix(GameObject   template) => template.AddComponent<ForceMutateButton>();
    }
    
  
        [HarmonyPatch(typeof(MutantPlant), "OnSpawn")]
        public static class MutatePatchPlanB
        {
            public static void Postfix(MutantPlant __instance)
            {
                __instance.Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenuDelegate);
            }
            private static readonly EventSystem.IntraObjectHandler<MutantPlant> OnRefreshUserMenuDelegate 
            = new EventSystem.IntraObjectHandler<MutantPlant>(delegate (MutantPlant component, object data)
            {
                ForceMutateButton.OnRefreshUserMenuPlanB(component);
            });
    }
    
}
