using HarmonyLib;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using static KAnim;
using static STRINGS.BUILDINGS.PREFABS;
using static STRINGS.UI.CLUSTERMAP;

namespace GoodDoorMod
{
    

    //[HarmonyPatch(typeof(Door), "SetWorldState")]
    public class DoorPatch
    {

        public static void Postfix(Door __instance)
        {

            var anim=__instance.GetAnimController();
            Debug.Log("----->Door.layer: "+anim.fgLayer+ " "+__instance.gameObject.transform.position.z);
            //anim.SetLayer(1);
           // anim.
            // anim.onLayerChanged(); 
        }
     }
    [HarmonyPatch(typeof(PressureDoorConfig), "CreateBuildingDef")]
    public class PressureDoorConfigPatch
    {

        public static void Postfix(BuildingDef __result)
        {
            //__result.AnimFiles = new KAnimFile[]
            //{
            //    Assets.GetAnim("door_external_beauty_kanim")
            //};
            __result.AnimFiles[0] = Assets.GetAnim("door_external_beauty_kanim");
            __result.SceneLayer = Grid.SceneLayer.BuildingFront;//这个影响层级.
            //__result.TileLayer= ObjectLayer.FoundationTile;
            Debug.Log("---->CreateBuildingDefPatch: door_external_beauty_kanim");
            //obj.TileLayer = ObjectLayer.FoundationTile;
            //obj.AudioCategory = "Metal";
            //obj.PermittedRotations = PermittedRotations.R90;
            //obj.SceneLayer = Grid.SceneLayer.TileMain;
            //obj.ForegroundLayer = Grid.SceneLayer.InteriorWall;
            //obj.LogicInputPorts = DoorConfig.CreateSingleInputPortList(new CellOffset(0, 0));
 
        }

        public static void Postfix_backup(GameObject go)
        {
            var aim = go.GetComponent<KBatchedAnimController>();
            //aim.AddAnimOverrides(Assets.GetAnim("door_external_beauty_kanim"), 0);
            aim.AnimFiles = new KAnimFile[]
            {
               // Assets.GetAnim("door_external_beauty_kanim")
                Assets.GetAnim("door_internal_beauty_kanim")
            };
            //aim.SetSceneLayer(Grid.SceneLayer.Backwall);
            //aim.SetFGLayer(Grid.SceneLayer.Backwall);
            aim.initialAnim = "closed";
            // aim.SetLayer(1);
            // go.SetLayerRecursively(1);
            // go.transform.position.z = 0;
            // var pos = Grid.PosToXY(go.transform.position);
            // go.transform.position = new Vector3(pos.X ,pos.Y  ,Grid.GetLayerZ(Grid.SceneLayer.Backwall));
            // Grid.SceneLayer.Backwall;
            Debug.Log("---->DoPostConfigureComplete: door_external_beauty_kanim " + aim.GetLayer() + " " + aim.GetLayering());
        }
    }
    
    [HarmonyPatch(typeof(ManualPressureDoorConfig), "CreateBuildingDef")] //DoPostConfigureComplete
    public class ManualPressureDoorConfig_DoPostConfigureCompletePatch
    {
        public static void Postfix(BuildingDef __result)
        {
            __result.AnimFiles[0] = Assets.GetAnim("door_manual_beauty_kanim");
            __result.SceneLayer = Grid.SceneLayer.BuildingFront;//这个影响层级.
        }

        public static void Postfix_BackUp(GameObject go)  // DoPostConfigureComplete
        {
            var aim = go.GetComponent<KBatchedAnimController>();
           aim.AnimFiles = new KAnimFile[]
            {
                Assets.GetAnim("door_manual_beauty_kanim")
            };
           aim.initialAnim = "closed";
           
          //  go.transform.
            Debug.Log("---->DoPostConfigureComplete: door_manual_beauty_kanim " +aim.GetLayer());
        }
    }

  

    [HarmonyPatch(typeof(DoorConfig), "CreateBuildingDef")] //DoPostConfigureComplete
    public class DoorConfig_DoPostConfigureCompletePatch
    {
        public static void Postfix(BuildingDef __result)
        {
            __result.AnimFiles[0] = Assets.GetAnim("door_internal_beauty_kanim");
           // __result.SceneLayer = Grid.SceneLayer.BuildingFront;//这个影响层级. 
        }

        public static void Postfix_backup(GameObject go)
        {
            var aim = go.GetComponent<KBatchedAnimController>();

            aim.AnimFiles = new KAnimFile[]
            {
                Assets.GetAnim("door_internal_beauty_kanim")
            };
            aim.initialAnim = "closed";
            Debug.Log("---->DoPostConfigureComplete: door_internal_beauty_kanim "+ aim.GetLayer());
        }
    }

}

 