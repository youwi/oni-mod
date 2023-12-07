﻿

using HarmonyLib;
using Klei.AI;
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
    [HarmonyPatch(typeof(AsteroidGridEntity), "OnSpawn")]
    public class CoverAsteroidMod_Patch
    {
     
        public static void Postfix(AsteroidGridEntity __instance)
        {
            // UserMenuScreen;
            // AsteroidGridEntity 星图--星球的实体.
            // 右上角菜单
            __instance.Subscribe<AsteroidGridEntity>((int)GameHashes.RefreshUserMenu, OnRefreshUserMenuDelegate);
        }
    
        public static readonly EventSystem.IntraObjectHandler<AsteroidGridEntity> OnRefreshUserMenuDelegate =
            new EventSystem.IntraObjectHandler<AsteroidGridEntity>(
                delegate (AsteroidGridEntity component, object data)
                {
                    KIconButtonMenu.ButtonInfo button = new KIconButtonMenu.ButtonInfo(
                        "status_item_toilet_needs_emptying",
                        "Hide",
                        null,
                        global::Action.SwitchActiveWorld8,
                        null, null, null,
                        "Hide this Asteroid",
                        true);

                    button.onClick = delegate ()
                    {
                        if (button.text == "Hide")
                        {
                            button.text = "un Hide";
                            UnRegMarkedAsteroid(component,true);
                        }
                        else
                        {
                            button.text = "Hide";
                            UnRegMarkedAsteroid(component, false);
                        }
                            
                        Game.Instance.userMenu.Refresh(component.gameObject);
                    };
                    Game.Instance.userMenu.AddButton(component.gameObject, button, 1f);
                   
                });
        public static void UnRegMarkedAsteroid(AsteroidGridEntity age,bool flag)
        {
            var m_worldContainer=age.GetComponent<WorldContainer>();
            //  var worlds = ClusterManager.Instance.WorldContainers;
            int worldId = m_worldContainer.id;

            if(m_worldContainer.IsStartWorld)
            {
                Debug.Log($"---> 隐藏 IsStartWorld skiped ");
                return;
            }
            if (flag)
            {
                //GameplaySeasonInstance.ReferenceEquals();
                //GameplaySeasonManager.Instance.UpdateTableEntry();
                //删除null
                //foreach (GameplaySeasonInstance activeSeason in GameplaySeasonManager.Instance.activeSeasons)
                //{
                   // age.GetMyWorld();
                   // activeSeason.worldId;//删除
                   // ClusterManager.Instance.GetWorld(worldId);
                // }
                m_worldContainer.SetDiscovered(false);
                // age.IsVisible = false;
                //m_worldContainer.SetDupeVisited(false);ClusterManager.Instance.GetWorld(age.GetMyWorldId())
                //var gsmi2=ClusterManager.Instance.activeWorld.GetSMI<GameplaySeasonManager.Instance>();
                //gsmi2?.activeSeasons.RemoveAll(x => x.worldId == worldId);

                var gsmi3=m_worldContainer.GetSMI<GameplaySeasonManager.Instance>();
                gsmi3?.activeSeasons.RemoveAll(x => x.worldId == m_worldContainer.id);
                //.StartNewSeason(Db.Get().GameplaySeasons.TemporalTearMeteorShowers);
                // GameplaySeasonManager.Instance.activeSeasons.RemoveAll(x => x.worldId == age.GetMyWorldId());
                // GameplaySeasonManager.Instance. activeSeasons.Remove(where );
                ClusterManager.Instance.UnregisterWorldContainer(m_worldContainer);
            }
            else
            {
                // SetActiveWorld(age);
                m_worldContainer.SetDiscovered(true);
                ClusterManager.Instance.RegisterWorldContainer(m_worldContainer);
            }
            Debug.Log($"--->Cover 隐藏小行星:{age.Name} {flag} ");   
        }

    }
}
