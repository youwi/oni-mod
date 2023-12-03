using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ForceMuate
{
    // [HarmonyPatch]
    class Mutate_Patch_PlanA
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            //EntityTemplates.ExtendEntityToBasicPlant
            //GameObject
            yield return AccessTools.Method(typeof(EntityTemplates), nameof(EntityTemplates.ExtendEntityToBasicPlant));
        }

        static void Postfix(GameObject template) => template.AddComponent<ForceMutateButton>();
    }


    [HarmonyPatch(typeof(MutantPlant), "OnSpawn")]
    public static class MutatePatchPlanB
    {
        /*
         * 直接给给种子和植物添加变异
         * 
         * https://steamcommunity.com/sharedfiles/filedetails/?id=2493249462
         * 这有A方案和B方案。
         * 
         * B方案可以作用到种子上。.
         * 
         */
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
