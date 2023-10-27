﻿using HarmonyLib;
using Klei;
using ProcGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DeleteAsteroid
{
    [HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.Save),
         new Type[] { typeof(string), typeof(bool), typeof(bool) })]
    public class DeleteOnSavePatch
    {
        public static void Prefix()
        {
            //SaveLoader.Instance.Save(filename, false, true);

            try
            {
                DeleteMarkedAsteroid();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
          
        }
        /*
         * 
         * 删除星球mod
         * 当殖民地改名为 "delete" 时
         * 保存再重新加载就可以 删除 殖民地.
         * 需要改名mod的支持.
         * 优点: 删除之后可以提升游戏帧数.
         * 
         * 删除之后不能还原,请做好备份.
         * 删除之后还是有隐藏垃圾存在,这些隐藏垃圾不影响.
         * 
         * 功能测试中.
         */

        public static void DeleteMarkedAsteroid()
        {
            //  SaveGame.Instance. //存档格式
            //  WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
            // CreateAsteroidWorldContainer(() 这个方法很重要
            // Assets.GetPrefab("Asteroid") //取全局数据
            // SaveGame.Instance.gameObject.as
            //SaveLoader.Instance.GetMyWorldDetail(clusterDetailSave);
            // SaveLoader.Instance.gameObject.get
            //SaveLoader.Instance.GetComponent<AsteroidGridEntity>();
            //AsteroidConfig ac =SaveLoader.Instance.GetComponent<AsteroidConfig>();

            //SaveLoader.Instance.getW;
            //ClusterManager.Instance.GetWorld(world_id);
            //ClusterManager.Instance.GetDiscoveredAsteroidIDsSorted();

            // var world = ClusterManager.Instance.GetWorld(row.Key);


            //ClusterManager.Instance.WorldContainers. Clear();//删除所有
            // ClusterManager.Instance.GetDiscoveredAsteroidIDsSorted;
            //SaveLoader.Instance.
            //ClusterGrid.Instance.;
            // ClusterGridEntity.in
            //if (m_worldContainers[i].IsDiscovered && !m_worldContainers[i].IsModuleInterior)
            // AsteroidGridEntity.ins
            //ClusterManager.Instance.;
            //AsteroidConfig s;
            //s.get
            //var world in ClusterManager.Instance.WorldContainers)
            //int max = ClusterManager.Instance.WorldContainers.Count;

            var worlds = ClusterManager.Instance.WorldContainers;
            //worlds 属于数据分区.  steroid属于存档.
            //ClusterManager.Instance.GetAllWorldsAccessibleAmounts
            //WorldContainer markDeleteWorld = null;
           
            Console.WriteLine("查找delete:" + worlds.Count);
            for (int i = 0; i < worlds.Count; i++)
            {
                if (worlds.Count > 1)
                {
                    var world = worlds[i];
                    if(world == null)
                    {
                        continue;
                    }
                    if (world.IsStartWorld || world.IsModuleInterior)
                    {
                        //初始星不能删除. 火箭不能删除
                        continue;
                    };
                    var gridEntity = world.GetComponent<ClusterGridEntity>();
                    var parentComp = world.GetComponentsInParent<AsteroidGridEntity>( );
                    if (gridEntity != null)
                    {
                        if (gridEntity.Name == "delete" || gridEntity.Name.Trim().ToLower() == "delete" )  // 获取右上角底层的实体名.
                        {
                            ClusterManager.Instance.UnregisterWorldContainer(world);
                            // remove 方法被隐藏了,只能使用自带的方法删除
                            //worlds.Remove(world);// markDeleteWorld = world;
                            // worlds.RemoveAt(i);
                            //Collection is read-only. 这里有只读问题.
                            Console.WriteLine("删除了星名:" + gridEntity.Name);
                            Console.WriteLine("parentComp List:" + parentComp);
                        };
                    }

                   
                }
            }
            GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("Asteroid"), null, null);
            GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab("Asteroid"), null, null);

            Console.WriteLine("KInstantiate:gameObject:" + gameObject);
            Console.WriteLine("KInstantiate:gameObject:对象相等：" + gameObject.Equals(gameObject2));

            GameObject go=Assets.GetPrefab("Asteroid");
            Console.WriteLine("Asteroid GameObject :" + go);

            // var asteroidList = SaveGame.Instance.GetComponents<AsteroidConfig>( );
            //看看这是什么东西. Asteroid为8个或1个.
            // 这个找法错了.
            // SaveGame.Instance.GetComponentInChildren("Asteroid");

            //SaveLoadRoot.
            //var asc = Game.Instance.GetComponent<AsteroidConfig>();
            //if (asc != null)
            //{
            //    Console.WriteLine("Game.Instance.GetComponent<AsteroidConfig>:" + asc.ToString());
            //}else
            //{ Console.WriteLine("Game.Instance.GetComponent<AsteroidConfig>:空");
            //}
            //var tmp= Game.Instance.FindComponent("Asteroid");
            //if (tmp != null)
            //{
            //    Console.WriteLine("Game.Instance.GetComponent<AsteroidConfig>:" + tmp.ToString());
            //}
            //else
            //{
            //    Console.WriteLine(" Game.Instance.FindComponent> Asteroid:空");
            //}
            //tmp = ClusterManager.Instance.FindComponent("Asteroid");
            //Console.WriteLine("ClusterManager.Instance.FindComponent  Asteroid(:" +tmp);
            //tmp = ClusterManager.Instance.FindComponent("AsteroidConfig");
            //Console.WriteLine("ClusterManager.Instance.FindComponent AsteroidConfig(:" +tmp );
            //tmp= SaveLoader.Instance.FindComponent("AsteroidConfig");
            //Console.WriteLine("find AsteroidConfig(:" + tmp);
            //tmp = SaveLoader.Instance.FindComponent("Asteroid");
            //Console.WriteLine("find Asteroid(:" + tmp);


            //if (tmp == null)
            //{
            //    Console.WriteLine("tmp为空");
            //}
            //var sse=SaveLoader.Instance.GetComponentInParent<AsteroidConfig>();
            //Console.WriteLine("SaveLoader.GetComponentInParent<AsteroidConfig>:" + sse );

            //sse=ClusterManager.Instance.gameObject.GetComponent<AsteroidConfig>();
            //Console.WriteLine("gameObject.GetComponent<AsteroidConfig>:" + sse);
            ////Game.Instance.gameObject.GetComponents
            //sse=Global.Instance.GetComponent<AsteroidConfig>();

            //Console.WriteLine("Global.GetComponent<AsteroidConfig> :" + sse);

            //tmp = Global.Instance.FindComponent("AsteroidConfig");
            //Console.WriteLine("Global.FindComponent<AsteroidConfig> :" + tmp);
            //tmp = Global.Instance.FindComponent("AsteroidGridEntity");
            //Console.WriteLine("Global.FindComponent<AsteroidGridEntity> :" + tmp);
            //if (tmp == null)
            //{
            //    Console.WriteLine("tmp为空");
            //}
            //if (sse == null)
            //{
            //    Console.WriteLine("sse为空");
            //}
            //  Game.Instance 29个功能.
            //  SaveLoader.Instance 3个功能
            //  ClusterManager.Instance 54个功能
            //  ClusterManager

            //int nextWorldId = this.GetNextWorldId();
            //GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("Asteroid"), null, null);
            //WorldContainer component = gameObject.GetComponent<WorldContainer>();
            // CreateAsteroidWorldContainer(() 这个方法很重要
            // Assets.GetPrefab("Asteroid") //取全局数据

            //ClusterManager.Instance ;
            //KMonoBehaviour[] components = SaveGame.Instance. GetComponents<KMonoBehaviour>();
            //if (components == null)
            //{
            //    Console.WriteLine("components为空");
            //}
            //else
            //{
            //    int num = components.Length;
            //    foreach (KMonoBehaviour kMonoBehaviour in components)
            //    {
            //        if( kMonoBehaviour.name == "Asteroid")
            //        {
            //            Console.WriteLine("找到一个KMonoBehaviour/Asteroid: " );
            //        }
            //        if (num < 100)
            //        {
            //            Console.WriteLine(kMonoBehaviour.ToString());
            //        }
            //    }
            //    Console.WriteLine("SaveGame:num: "+num);
            //}

            // 
            //components = ClusterManager.Instance.GetComponents<KMonoBehaviour>();
            //if (components == null)
            //{
            //    Console.WriteLine("components为空");
            //}
            //else
            //{
            //    int num = components.Length;
            //    foreach (KMonoBehaviour kMonoBehaviour in components)
            //    {
            //        if (kMonoBehaviour.name == "Asteroid")
            //        {
            //            Console.WriteLine("找到一个KMonoBehaviour/Asteroid: ");
            //        }
            //        if (num < 100)
            //        {
            //            Console.WriteLine(kMonoBehaviour.ToString());
            //        }
            //    }
            //// 
            //    Console.WriteLine("ClusterManager: num: " + num);
            //}

            //var tt = ClusterManager.Instance.GetComponentsInChildren<Behaviour>();
            //if (components == null)
            //{
            //    Console.WriteLine("tt 为空");
            //}
            //else
            //{
            //    int num = tt.Length;
            //    foreach (Behaviour kMonoBehaviour in tt)
            //    {
            //        if (kMonoBehaviour.name == "Asteroid")
            //        {
            //            Console.WriteLine("找到一个KMonoBehaviour/Asteroid: ");
            //        }
            //        if (num < 100)
            //        {
            //            Console.WriteLine(kMonoBehaviour.ToString());
            //        }
            //    }
            //    // 
            //    Console.WriteLine("Behaviour.GetComponentsInChildren: num: " + num);
            //}
            // WorldGenSpawner 

            // KMonoBehaviour[] components = GetComponents<KMonoBehaviour>();
            // SaveLoadRoot;

            //for (int i = 0; i < asteroidList.Count(); i++)
            //{
            //    var asteroid = asteroidList[i];
            //    // asteroid.get
            //    var gridEntity = asteroid.GetComponent<AsteroidGridEntity>();
            //    if (gridEntity != null)
            //    {
            //        if (gridEntity.Name == "delete" || gridEntity.Name.Trim().ToLower() == "delete")  // 获取右上角底层的实体名.
            //        {
            //            asteroidList[i] = null;//删除元素
            //            Console.WriteLine("删除了星名:" + gridEntity.Name);
            //        };
            //    }
            //}
        }

    }
}
