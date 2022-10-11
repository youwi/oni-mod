using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static SingleStarWorldMod.SingleStarWorldModPatch;
using UnityEngine;
using ProcGen;
using static STRINGS.ELEMENTS;
using Newtonsoft.Json;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace SingleStarWorldMod
{

  [HarmonyPatch(typeof(ClusterManager))]
  [HarmonyPatch(nameof(ClusterManager.CreateRocketInteriorWorld))]
  public class ClusterManager_CreateRocketInteriorWorld_Patch2
  {

    //   直接修改这个值估计没什么用....
    //   https://github.com/Sgt-Imalas/Sgt_Imalas-Oni-Mods/blob/master/Robo%20Rockets/RoboRocketPatches.cs
    //   这个哥们使用编译器重写.

    public static void Prefix(string interiorTemplateName)
    {
      switch (interiorTemplateName)
      {
        case "interiors/habitat_medium_fix":

          TUNING.ROCKETRY.ROCKET_INTERIOR_SIZE = new Vector2I(16, 15);
          break;
        case "interiors/habitat_small_fix":
          TUNING.ROCKETRY.ROCKET_INTERIOR_SIZE = new Vector2I(12, 12);
          break;

      }

      //        TUNING.ROCKETRY.ROCKET_INTERIOR_SIZE = new Vector2I(32, 32);
      Console.WriteLine("修改了:模板名为: " + interiorTemplateName);
    }

    // [HarmonyPatch(typeof(ClusterManager))]
    //  [HarmonyPatch("CreateRocketInteriorWorld")]
    public class ClusterManager_CreateRocketInteriorWorld_Patch
    {



      public static Vector2I ConditionForSize(Vector2I original, string templateString)
      {
        return new Vector2I(8, 8);
      }

      private static readonly MethodInfo InteriorSizeHelper = AccessTools.Method(
         typeof(ClusterManager_CreateRocketInteriorWorld_Patch),
         nameof(ConditionForSize)
      );


      private static readonly FieldInfo SizeFieldInfo = AccessTools.Field(
          typeof(TUNING.ROCKETRY),
          "ROCKET_INTERIOR_SIZE"
      );

      static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
      {
        var code = instructions.ToList();
        var insertionIndex = code.FindIndex(ci => ci.operand is FieldInfo f && f == SizeFieldInfo);

        if (insertionIndex != -1)
        {
          code.Insert(++insertionIndex, new CodeInstruction(OpCodes.Ldarg_2));
          code.Insert(++insertionIndex, new CodeInstruction(OpCodes.Call, InteriorSizeHelper));
        }
        return code;
      }
    }

  }

  [HarmonyPatch(typeof(HabitatModuleMediumConfig))]
  [HarmonyPatch(nameof(HabitatModuleMediumConfig.ConfigureBuildingTemplate))]
  public static class SaveSpace_HabitatM_Patch
  {

    public static void Postfix(GameObject go)
    {
      go.AddOrGet<ClustercraftExteriorDoor>().interiorTemplateName = "interiors/habitat_medium_fix";
    }

  }

  /// <summary>
  /// Compact interior template for Small Habitat
  /// </summary>
  [HarmonyPatch(typeof(HabitatModuleSmallConfig))]
  [HarmonyPatch(nameof(HabitatModuleSmallConfig.ConfigureBuildingTemplate))]
  public static class SaveSpace_HabitatSmall_Patch
  {
    public static void Postfix(GameObject go)
    {
      go.AddOrGet<ClustercraftExteriorDoor>().interiorTemplateName = "interiors/habitat_small_fix";
    }
  }
  //..
  [HarmonyPatch(typeof(ProcGen.MutatedWorldData))]
  [HarmonyPatch("ApplyTrait")]
  public static class PatchInClass
  {
    public static void Prefix(WorldTrait trait)
    {
      //additionalSubworldFiles)
      if (trait == null
        || trait.additionalSubworldFiles == null
        || trait.additionalUnknownCellFilters == null
        || trait.additionalWorldTemplateRules == null
        || trait.filePath == null)
      {
        Console.WriteLine("ApplyTrait为空" + trait);
        return;
      }
      // var json = new JsonSerializer.Serialize(trait);
      // var json = JsonSerializer.Serialize(trait);
      // var json = JsonConvert.SerializeObject(trait);
      // string jsonString = JsonSerializer.Serialize(weatherForecast);

      // jsetting.TypeNameHandling =;
      var serializer = new SerializerBuilder()
        .WithNamingConvention(new CamelCaseNamingConvention())
        .Build();
      //worldTraitRules:  []  #关闭特性设置. 很容易崩溃
      var yaml = serializer.Serialize(trait);
      Console.WriteLine("trait: " + yaml);
      //DistanceFromTag

      // Sim/Cell.SetValues(byte,single,single) 只是在加水bug
      //.DrawWorldBorder_Patch1(ProcGenGame.WorldGen,Sim/Cell[],Chunk,SeededRandom,System.Collections.Generic.HashSet`1<int>&,System.Collections.Generic.List`1<UnityEngine.RectInt>&,ProcGenGame.WorldGen/OfflineCallbackFunction)
    }
  }
}

