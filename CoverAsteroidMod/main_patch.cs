

using HarmonyLib;
using Klei.CustomSettings;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Timers;
using UnityEngine;

namespace CoverAsteroidMod
{
    ////创建游戏时就加载
    [HarmonyPatch(typeof(PressureDoorConfig), "CreateBuildingDef")]
    public class CoverAsteroidMod_Patch
    {
     
        public static void Postfix(Geyser __instance)
        {
            __instance.Subscribe<Geyser>((int)GameHashes.RefreshUserMenu, OnRefreshUserMenuDelegate);
        }
  
        private static readonly EventSystem.IntraObjectHandler<Geyser> OnRefreshUserMenuDelegate =
            new EventSystem.IntraObjectHandler<Geyser>(
                delegate (Geyser component, object data)
                {
                    KIconButtonMenu.ButtonInfo button = new KIconButtonMenu.ButtonInfo("status_item_toilet_needs_emptying",
                        "Cover", null, global::Action.SwitchActiveWorld8, null, null, null, "Start the eruption immediately in (10 seconds)", true);

                    button.onClick = delegate ()
                    {
                        if (button.text == "Cover")
                            button.text = "Un Cover";
                        button.text = "Cover";
                        UnRegMarkedAsteroid();
                        Game.Instance.userMenu.Refresh(component.gameObject);
                    };
                    Game.Instance.userMenu.AddButton(component.gameObject, button, 1f);
                   
                });
        public static void UnRegMarkedAsteroid()
        {
            var worlds = ClusterManager.Instance.WorldContainers;

            for (int i = 0; i < worlds.Count; i++)
            {
                if (worlds.Count > 1)
                {
                    var world = worlds[i];
                    if (world == null)
                    {
                        continue;
                    }
                    if (world.IsStartWorld || world.IsModuleInterior)
                    {
                        //初始星不能删除. 火箭不能删除
                        continue;
                    };
                    var gridEntity = world.GetComponent<ClusterGridEntity>();



                    if (gridEntity != null)
                    {
                        if (gridEntity.Name == "delete" || gridEntity.Name.Trim().ToLower() == "delete")  // 获取右上角底层的实体名.
                        {
                            global::Debug.Log("Unregister删除小行星:" + gridEntity.Name);

                            ClusterManager.Instance.UnregisterWorldContainer(world);

                        };
                    }
                }
            }
        }

    }
}
