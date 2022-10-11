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
        //添加一轮变异,按20%概率添加.
        List<string> list = (List<string>)__instance.GetType().GetField("mutationIDs").GetValue(__instance);
        if (list == null || list.Count > 1   || list.Count==0 || rand10() >3)
        {//已经双重变异就不需要了.
          return;
        }
        string newMutionId = Db.Get().PlantMutations.GetRandomMutation(__instance.PrefabID().Name).Id;

        if (list[0]== newMutionId)
        {
          return;

        }
        list.Add(newMutionId);
       
        __instance.SetSubSpecies(list);
      }catch(Exception ex)
      {
        Debug.LogException(ex);
      }

    }
    public static int rand10()
    {
      Random rand = new System.Random();
      return rand.Next(1,10);
    }
  }
}
