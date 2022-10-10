using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using HarmonyLib;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;
using static STRINGS.UI.CLUSTERMAP;

namespace TeleporterBuildingMod
{
  //复制人传送器有多个类：WarpPortal,WarpReceiver，Teleporter,TeleportalPad
  //分不明白这几个类..
  //传送发射器.Teleporter Transmitter ,WarpPortal
  //传送接收器 Teleporter Receiver ,WarpReceiver

  //传送台(很旧的东西) TeleportalPad
  /*    [HarmonyPatch(typeof(TeleportalPadConfig))]

      public class DeconstructionPatch2
      {
          [HarmonyPatch("DoPostConfigureComplete")]
          public static void  Postfix(GameObject go)
          {
              go.GetComponent<Deconstructable>().allowDeconstruction = true;
          }   //TeleporterBuildingMod.DeconstructionPatch2::Postfix

      }*/
  /*    [HarmonyPatch(typeof(WarpPortalConfig))]
      public class WarpPortalConfigShowPatch
      {
          [HarmonyPatch("CreateBuildingDef")]
          public static BuildingDef Postfix(BuildingDef __result)
          {
              __result.ShowInBuildMenu = true;
              return __result;

          }

      }*/
  //可拆可建

  [HarmonyPatch(typeof(WarpReceiverConfig), "OnSpawn")]
  public class WarpReceiverConfigDeconstructionPatch
  {
    public static void Postfix(GameObject inst)
    {
      inst.AddOrGet<Deconstructable>();
      PrimaryElement component = inst.GetComponent<PrimaryElement>();
      component.SetElement(SimHashes.Lead);
      var obj = inst.GetComponent<Deconstructable>();
      if (obj != null)
      {
        obj.allowDeconstruction = true;
      }
      // inst.FindOrAddComponent<Deconstructable>();
    }
  }
  [HarmonyPatch(typeof(WarpPortalConfig), "OnSpawn")]
  public class WarpPortalConfigDeconstructionPatch
  {
    public static void Postfix(GameObject inst)
    {
      PrimaryElement component = inst.GetComponent<PrimaryElement>();
      component.SetElement(SimHashes.Lead);
      inst.AddOrGet<Deconstructable>();
      var obj = inst.GetComponent<Deconstructable>();
      if (obj != null)
        obj.allowDeconstruction = true;
    }
  }

  //添加建筑到菜单中
  [HarmonyPatch(typeof(GeneratedBuildings))]
  [HarmonyPatch("LoadGeneratedBuildings")]
  public class GeneratedBuildingsPatch
  {
    private static void Prefix()
    {
      Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.WARPPORTAL1.DESC", "WarpPortal" });
      Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.WARPPORTAL1.EFFECT", "WarpPortal" });
      Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.WARPPORTAL1.NAME", "WarpPortal" });

      Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.WARPRECEIVER1.DESC", "WarpReceiver1" });
      Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.WARPRECEIVER1.EFFECT", "WarpReceiver1" });
      Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.WARPRECEIVER1.NAME", "WarpReceiver1" });
      //    ModUtil.AddBuildingToPlanScreen("Base", "TeleportalPad"); //传送台没什么用.
      ModUtil.AddBuildingToPlanScreen("Base", "WarpPortal1");
      ModUtil.AddBuildingToPlanScreen("Base", "WarpReceiver1");

      //    ModUtil.AddBuildingToPlanScreen("Base", "WarpReceiver");//原生不支持
      //    ModUtil.AddBuildingToPlanScreen("Base", "TeleporterTransmitter");//可能的翻译
      //    ModUtil.AddBuildingToPlanScreen("Base", "TeleporterReceiver");//可能的翻译
    }

  }

  [HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
  public static class BuildingComplete_OnSpawn_Patch
  {

    public static void Postfix(BuildingComplete __instance)
    {
      GameObject go = __instance.gameObject;
      if (__instance.name == "WarpPortal1"
          || __instance.name == "WarpPortal1Complete"
          || __instance.name == "WarpReceiver1Complete" //这里和配置的模板名一样.
          || __instance.name == "WarpReceiver1"

          )
      {
        Vector3 pos = go.transform.position;
        PrimaryElement element = go.GetComponent<PrimaryElement>();
        float temperature = element.Temperature;
        float mass = 50f;
        int cell = Grid.PosToCell(pos);

        //替换成石块
        SimMessages.ReplaceAndDisplaceElement(cell, element.ElementID,
            null, mass, temperature, byte.MaxValue, 0, -1); // spawn Natural Block
                                                            //猜太空背景为
        replaceBuilding(__instance.name, cell);
        // SimMessages.ModifyCellWorldZone(cell, Sim.SpaceZoneID);
        // DestroyCellWithBackground(cell);

        go.DeleteObject(); // remove Natural Tile
      }
    }
    /**
     * 使用模板来替换建筑.模板名放在模板目录下.
     */
    public static void replaceBuilding(string templateName, int cell)
    {
      //TemplateCache.
      TemplateContainer template = TemplateCache.GetTemplate(templateName);
      TemplateLoader.Stamp(template, Grid.CellToPos(cell), delegate
      {
        Console.WriteLine("替换打印了...");
      });
    }

  }
}
