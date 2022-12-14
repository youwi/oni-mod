using HarmonyLib;
using Klei;
using KMod;
using ProcGenGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static STRINGS.DUPLICANTS.ATTRIBUTES;
using static STRINGS.DUPLICANTS.PERSONALITIES;
using static System.Net.Mime.MediaTypeNames;

namespace SingleStarWorldMod
{
  public class GenWorldFix : KMod.UserMod2
  {
    public static string ModPath;
    public override void OnLoad(Harmony harmony)
    {
      ModPath = mod.ContentPath;
      //mod.
      base.OnLoad(harmony);
    }
    public static bool WorldReplaceFix(WorldGen gen, ref Sim.Cell[] cells, ref Sim.DiseaseCell[] dc, int baseId)
    {
   
      Console.WriteLine("插入了模板中: " + ModPath);
      int width = gen.data.world.size.x;
      int height = gen.data.world.size.y;

      string path = ModPath + "/templates/poi/bottom_geysers.yaml";
      var claimedCells = new Dictionary<int, int>();
      // TemplateContainer bottomTemplate = TemplateCache.GetTemplate("bottom_geysers");
      // TemplateContainer bottomTemplate = YamlIO.LoadFile<TemplateContainer>(path);
      // bottomTemplate.
      // SaveGame.Instance.S
      //
      //TemplateLoader.Stamp(bottomTemplate, new Vector2I(10, 10), delegate
      //{
      //  Console.WriteLine("插入了模板成功");
      //});
      //Vector2I bottomPos = new Vector2I(2, 2);//在地底插入模板
      //gen.data.gameSpawnData.AddTemplate(bottomTemplate, bottomPos, ref claimedCells);
      //bottomTemplate = YamlIO.LoadFile<TemplateContainer>(path);

      gen.data.gameSpawnData.AddTemplate(
          YamlIO.LoadFile<TemplateContainer>(ModPath + "/templates/poi/bottom_geysers2v.yaml"),
          new Vector2I(width-3-2, 11),//竖着放水泉,靠右
          ref claimedCells);

      gen.data.gameSpawnData.AddTemplate(
         YamlIO.LoadFile<TemplateContainer>(ModPath + "/templates/poi/bottom_geysers3.yaml"),
         new Vector2I(width - 31-2, 2),//靠右 火山
         ref claimedCells);

      gen.data.gameSpawnData.AddTemplate(
         YamlIO.LoadFile<TemplateContainer>(ModPath + "/templates/poi/bottom_geysers4.yaml"),
         new Vector2I(2, 2),//最左边下角放气泉
         ref claimedCells);
      gen.data.gameSpawnData.AddTemplate(
         YamlIO.LoadFile<TemplateContainer>(ModPath + "/templates/poi/other_poi_a.yaml"),
         new Vector2I(6, 11),//左下角放POI
         ref claimedCells);
      //gen.data.gameSpawnData.AddTemplate(
      //   YamlIO.LoadFile<TemplateContainer>(ModPath + "/templates/poi/poi_bunker_skyblock.yaml"),
      //   new Vector2I(width / 2-14, height/2-14),//中心下2格放必要的动物和种子
      //   ref claimedCells);
      return true;

    }
  }

}
