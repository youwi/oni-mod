using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleMutantMod
{
  [HarmonyPatch(typeof(MutantPlant), nameof(MutantPlant.Mutate))]
  public class DoubleMutantModPatch
  {
    public static void Postfix(MutantPlant __instance)
    {
      try
      {
       
        var field = __instance.GetType().GetField("mutationIDs");
        List<string> list = null;
        if (field != null)
        {
          list = (List<string>)field.GetValue(__instance);
        }
        if (list == null)
        {
          Console.WriteLine("变异listNull:" + list);
          list= new List<string>();
        }
        if (list.Count > 2 )
        {//已经双重变异就不需要了.
          Console.WriteLine("变异已经大于2:"+list);
          return;
        }
        //添加一轮变异,按20%概率添加.
        if ( rand10() > 3)
        {
          Console.WriteLine("二次变异率为20%,变异未触发,:" + list);
          return;
        }
        string name = Db.Get().PlantMutations.GetRandomMutation(__instance.PrefabID().Name).Id;
        list.Add(name);
        string name2 = Db.Get().PlantMutations.GetRandomMutation(__instance.PrefabID().Name).Id;
        if(name!=name2)
          list.Add(name2);//防止重复添加,如果重复了就当成一次变异
        __instance.SetSubSpecies(list);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

    }
    public static int rand10()
    {
      Random rand = new System.Random();
      return rand.Next(1, 10);
    }
  }
}
