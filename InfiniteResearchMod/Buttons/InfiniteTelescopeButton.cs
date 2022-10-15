using HarmonyLib;
using UnityEngine;

namespace InfiniteResearch
{
    class InfiniteTelescopeButton : InfiniteModeToggleButton
    {
        [MyCmpGet] Telescope telescope;

        protected override void UpdateState()
        {
            // telescope.UpdateWorkingState();
           var fun= AccessTools.Method(typeof(Telescope),"UpdateWorkingState");
            if (fun != null)
            {
                fun.Invoke(telescope, null);
            }
           // telescope.GetType().GetMethod("UpdateWorkingState").Invoke(telescope,null);
            //telescope.UpdateWorkingState(null);
        }
    }

    [HarmonyPatch(typeof(TelescopeConfig), nameof(TelescopeConfig.ConfigureBuildingTemplate))]
    class TelescopeConfig_Patch { static void Postfix(GameObject go) => go.AddComponent<InfiniteTelescopeButton>(); }
}
