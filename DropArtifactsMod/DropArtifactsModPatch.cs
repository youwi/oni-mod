using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DropArtifactsMod
{
    [HarmonyPatch(typeof(ArtifactModule), "OnSpawn")]
    public class DropArtifactsModPatch  
    {
        //ArtifactModule
        //ArtifactCargoBayConfig
        public static void Postfix(ArtifactModule __instance )
		{
           // GameObject go = __instance;
            ArtifactModule artifactModule = __instance.GetComponent<ArtifactModule>();
            Console.WriteLine("配置火箭自动.Artifact..");
            World.Instance.Subscribe((int)GameHashes.RocketLanded, delegate (object data)
            {
                Console.WriteLine("火箭着陆了A...");
            });

            __instance.Subscribe((int)GameHashes.RocketLanded, delegate (object data) {
                if (__instance != null)
                {
                    ItemPedestal itemPedestal = __instance.GetComponent<ItemPedestal>();
                    // itemPedestal.drop();//
                    // itemPedestal.o
                    SingleEntityReceptacle ser = __instance.GetComponent<SingleEntityReceptacle>();
                    if(ser!=null)
                    {
                        ser.OrderRemoveOccupant();
                    }
                    DecorProvider dp= __instance.GetComponent<DecorProvider>();
                    if (dp != null)
                    {
                        dp.Clear();
                    }
                }
               // GameHashes.Rocket
                Console.WriteLine("火箭着陆了...");
            });
        }

        /**
         * 
 
         */
    }
}
