using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using static KAnim;

namespace GoodDoorMod
{
    //[HarmonyPatch("PressureDoorConfig", "CreateBuildingDef")]
    //public class PressureDoorConfigPatch
    //{

    //    public static void Postfix(BuildingDef __result)
    //    {
    //        __result.AnimFiles = new KAnimFile[]
    //        {
    //            Assets.GetAnim("door_external_beauty_kanim")
    //        };
    //        Debug.Log("---->PressureDoorConfigPatch: door_external_beauty_kanim");

    //    }
    //}

    [HarmonyPatch("ManualPressureDoorConfig", "DoPostConfigureComplete")]
    public class ManualPressureDoorConfig_DoPostConfigureCompletePatch
    {

        public static void Postfix(GameObject go)
        {
            go.GetComponent<KBatchedAnimController>().AnimFiles = new KAnimFile[]
            {
                Assets.GetAnim("door_manual_beauty_kanim")
            };
            go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
            Debug.Log("---->DoPostConfigureComplete: door_manual_beauty_kanim");

        }
    }

    [HarmonyPatch("PressureDoorConfig", "DoPostConfigureComplete")]
    public class PressureDoorConfig_DoPostConfigureCompletePatch
    {

        public static void Postfix(GameObject go)
        {
      
            go.GetComponent<KBatchedAnimController>().AnimFiles = new KAnimFile[]
            {
                Assets.GetAnim("door_external_beauty_kanim")
            };
            go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
            Debug.Log("---->DoPostConfigureComplete: door_external_beauty_kanim");

        }
    }

    [HarmonyPatch("DoorConfig", "DoPostConfigureComplete")]
    public class DoorConfig_DoPostConfigureCompletePatch
    {

        public static void Postfix(GameObject go)
        {

            go.GetComponent<KBatchedAnimController>().AnimFiles = new KAnimFile[]
            {
                Assets.GetAnim("door_internal_beauty_kanim")
            };
            go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
            Debug.Log("---->DoPostConfigureComplete: door_internal_beauty_kanim");
        }
    }

}

 