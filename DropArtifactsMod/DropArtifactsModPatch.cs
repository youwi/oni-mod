using HarmonyLib;

namespace DropArtifactsMod
{
    [HarmonyPatch(typeof(ArtifactModule), "OnSpawn")]
    public class DropArtifactsModPatch
    {
        //ArtifactModule
        //ArtifactCargoBayConfig
        public static void Postfix(ArtifactModule __instance)
        {
            // GameObject go = __instance;
            ArtifactModule artifactModule = __instance.GetComponent<ArtifactModule>();
            global::Debug.LogWarning("配置火箭自动.Artifact..");
            World.Instance.Subscribe((int)GameHashes.RocketLanded, delegate (object data)
            {
                global::Debug.LogWarning("火箭着陆了A...");
            });

            __instance.Subscribe((int)GameHashes.RocketLanded, delegate (object data)
            {
                if (__instance != null)
                {
                    ItemPedestal itemPedestal = __instance.GetComponent<ItemPedestal>();
                    // itemPedestal.drop();//
                    // itemPedestal.o
                    SingleEntityReceptacle ser = __instance.GetComponent<SingleEntityReceptacle>();
                    if (ser != null)
                    {
                        ser.OrderRemoveOccupant();
                    }
                    DecorProvider dp = __instance.GetComponent<DecorProvider>();
                    if (dp != null)
                    {
                        dp.Clear();
                    }
                }
                // GameHashes.Rocket
                global::Debug.LogWarning("火箭着陆了...");
            });
        }

        /**
         * 
 
         */
    }
}
