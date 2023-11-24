using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
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
                global::Debug.Log("-->DeleteOnSavePatch.Prefix--<");
                UnRegMarkedAsteroid();
                DeleteAstoidLoop();
            }
            catch (Exception ex)
            {
                global::Debug.LogWarning(ex.Message);
            }

        }
        public static void DeleteWorldBuildings(WorldContainer world)
        {

            world.CancelChores();
            HashSet<int> noRefundTiles;
            world.DestroyWorldBuildings(out noRefundTiles);

            OrbitalMechanics component = world.GetComponent<OrbitalMechanics>();
            if (!component.IsNullOrDestroyed())
            {
                UnityEngine.Object.Destroy(component);
            }

        }
        public static void DeleteWorldObjects(WorldContainer world)
        {

            //这是火箭删除的代码
            Grid.FreeGridSpace(world.WorldSize, world.WorldOffset);
            WorldInventory worldInventory = null;
            if (world != null)
            {
                worldInventory = world.GetComponent<WorldInventory>();
            }
            if (worldInventory != null)
            {
                UnityEngine.Object.Destroy(worldInventory);
            }
            if (world != null)
            {
                UnityEngine.Object.Destroy(world);
            }
        }
        public static void ResizeMapAfterDelete(List<SaveLoadRoot> list)
        {
            //找到world的最大offset。
            var maxWidthOff = 0;
            var maxOffsetx = 0;

            for (int i = 0; i < list.Count; i++)
            {
                var tt = list[i].GetComponent<WorldContainer>();
                var age = list[i].GetComponent<AsteroidGridEntity>();
                if (age == null) continue;//防止火箭空间写入在这里.

                if (tt.IsModuleInterior) continue;// 火箭的world绕过去.
                                                  //HabitatModuleMedium 火箭
                if (age.Name == null || age.Name == "") continue;
                if (tt != null)
                {
                    var tmp = tt.WorldSize.x + tt.WorldOffset.x;
                    if (tmp > maxWidthOff)
                    {
                        maxWidthOff = tmp;
                        maxOffsetx = tt.WorldOffset.x;
                    }
                }
            }

            var newwidth = maxWidthOff + 37;//设置新大小.不改高度.
            global::Debug.Log($">>>> 星数:{list.Count} 最大位置:<{maxOffsetx},{maxWidthOff}>, Grid.WidthInCells :{Grid.WidthInCells},新大小: {newwidth},{Grid.HeightInCells}");
            //星数:8 最大位置:<404,500>, Grid.WidthInCells :636,新大小: 537,404
            //星数:8 最大位置:<404,500>, Grid.WidthInCells :636,新大小: 537,404
            if (Grid.WidthInCells > newwidth + 99 || Grid.WidthInCells < newwidth)
            {   //大于136才需要改大小.
                //GridSettings.ClearGrid();//

                //World.Camera 可能也需要重置.
                //PropertyTextures.UpdateFogOfWar
                Camera.main.transform.parent.GetComponent<CameraController>().CameraGoHome();
                GridSettings.Reset(newwidth, Grid.HeightInCells);
                // Grid.WidthInCells=newwidth;
                // Game.Instance.gasConduitFlow.
                // Game.Instance.gasConduitFlow.
                global::Debug.Log($">>>>设置新大小: {newwidth},{Grid.HeightInCells}");
            }
            // GridSettings.Reset(worldGen.GetSize().x, worldGen.GetSize().y);

        }
        public static void DeleteAstoidLoop()
        {
            Dictionary<Tag, List<SaveLoadRoot>> sceneObjects = SaveLoader.Instance.saveManager.GetLists();
            List<Tag> orderedKeys = new List<Tag>();

            orderedKeys.Clear();
            orderedKeys.AddRange(sceneObjects.Keys);
            orderedKeys.Remove(SaveGame.Instance.PrefabID());
            orderedKeys = orderedKeys.OrderBy((Tag a) => a.Name == "Asteroid").ToList();

            foreach (Tag orderedKey in orderedKeys)
            {
                List<SaveLoadRoot> list = sceneObjects[orderedKey];
                if (list.Count <= 0)
                {
                    continue;
                }
                //  global::Debug.Log(orderedKey.ToString()+" -->"+list.Count);

                if (orderedKey.Name == "Asteroid")
                {
                    // global::Debug.Log(" 搜索Asteroid: ");
                    for (int i = 0; i < list.Count; i++)
                    {
                        SaveLoadRoot item = list[i];
                        if (!(item == null))
                        {

                            var age = item.GetComponent<AsteroidGridEntity>();
                            if (age.Name == "delete" || age.Name.Trim().ToLower() == "delete")
                            {

                                //var tt = item.GetComponent<WorldContainer>();
                                //Grid.FreeGridSpace(tt.WorldSize, tt.WorldOffset);
                                // SaveloadRoot需要删除.
                                list[i] = null;//这里直接删除没

                                UnityEngine.Object.Destroy(item);
                                list.RemoveAt(i);
                                i--;
                                global::Debug.Log("--->Delete Asteroid +1  删除+1");
                            }


                        }
                    }

                    // global::Debug.Log(" 搜索Asteroid:结束 ");
                    // ResizeMapAfterDelete(list);
                }
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

        public static void UnRegMarkedAsteroid()
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

            //global::Debug.Log($"Worlds.Count:{worlds.Count} 包括火箭" );
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

                    //var age = world.GetComponentInParent<AsteroidGridEntity>();
                    //var wi = world.GetComponentInParent<WorldInventory>();
                    //var wom = world.GetComponentInParent<OrbitalMechanics>();
                    //var wsmc = world.GetComponentInParent<StateMachineController>();
                    //global::Debug.Log("WorldInventory  ---> :" + wi);
                    //global::Debug.Log("OrbitalMechanics  ---> :" + wom);
                    //global::Debug.Log("StateMachineController  ---> :" + wsmc);
                    //global::Debug.Log("AsteroidGridEntity-->:" + age);

                    if (gridEntity != null)
                    {
                        if (gridEntity.Name == "delete" || gridEntity.Name.Trim().ToLower() == "delete")  // 获取右上角底层的实体名.
                        {
                            global::Debug.Log("Unregister删除小行星:" + gridEntity.Name);

                            ClusterManager.Instance.UnregisterWorldContainer(world);
                            //DeleteWorldBuildings(world);
                            //DeleteWorldObjects(world);
                            //Sim.Shutdown();
                            // remove 方法被隐藏了,只能使用自带的方法删除
                            //worlds.Remove(world);// markDeleteWorld = world;
                            // worlds.RemoveAt(i);
                            //Collection is read-only. 这里有只读问题.
                            // ClusterManager.Instance.DestoryRocketInteriorWorld
                        };
                    }
                }
            }


            //以下代码好像是创建星球的代码.
            //GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("Asteroid"), null, null);
            //GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab("Asteroid"), null, null);

            // global::Debug.Log("KInstantiate:gameObject:" + gameObject);
            // global::Debug.Log("KInstantiate:gameObject:对象相等：" + gameObject.Equals(gameObject2));
            // global::Debug.Log("Asteroid:gameObject:对象相等：" + Assets.GetPrefab("Asteroid").Equals(Assets.GetPrefab("Asteroid")));
            // GameObject go = Assets.GetPrefab("Asteroid");//好像只初始化一个星球.
                                                         //好像是空对象.
                                                         //===> Asteroid(KPrefabID)
                                                         //===> Asteroid(KSelectable)
                                                         //===> Asteroid(SaveLoadRoot)
                                                         //===> Asteroid(WorldInventory)
                                                         //===> Asteroid(WorldContainer)
                                                         //===> Asteroid(AsteroidGridEntity)
                                                         //===> Asteroid(OrbitalMechanics)
                                                         //===> Asteroid(StateMachineController)
            //global::Debug.Log("Asteroid GameObject :" + go);
            ////go.RemoveTag("");
            //var s = go.GetComponents<KMonoBehaviour>();
            //foreach (var tmp in s)
            //{
            //    global::Debug.Log("===>  " + tmp.ToString());
            //    if (tmp is AsteroidGridEntity tt)
            //    {

            //        global::Debug.Log("AsteroidGridEntity.Name:  " + tt.Name);
            //        // YamlIO.Save(tmp, "D:/GameObject3.yaml");
            //    }
            //    if (tmp is KPrefabID iD)
            //    {
            //        global::Debug.Log("KPrefabID" + iD.InstanceID);
            //    }
            //}
            // global::Debug.Log("Asteroid  ->AsteroidGridEntity.length :" + go.GetComponents<AsteroidGridEntity>().Length);
            // YamlIO.Save(go, "D:/GameObject.yaml");
            // YamlIO.Save(gameObject, "D:/GameObjectIn.yaml");
            // go.DeleteObject();

            //go.IsNullOrDestroyed();
            //go.re
            // go.
            //  global::Debug.Log(" a - config :" + go.GetComponent<AsteroidConfig>());
            //go.gameObjects
            //string json = JsonConvert.SerializeObject(go);
            // global::Debug.Log("Asteroid GameObject: "+json);



            // var asteroidList = SaveGame.Instance.GetComponents<AsteroidConfig>( );
            //看看这是什么东西. Asteroid为8个或1个.
            // 这个找法错了.
            // SaveGame.Instance.GetComponentInChildren("Asteroid");

            //SaveLoadRoot.
            //var asc = Game.Instance.GetComponent<AsteroidConfig>();
            //if (asc != null)
            //{
            //     global::Debug.Log("Game.Instance.GetComponent<AsteroidConfig>:" + asc.ToString());
            //}else
            //{  global::Debug.Log("Game.Instance.GetComponent<AsteroidConfig>:空");
            //}
            //var tmp= Game.Instance.FindComponent("Asteroid");
            //if (tmp != null)
            //{
            //     global::Debug.Log("Game.Instance.GetComponent<AsteroidConfig>:" + tmp.ToString());
            //}
            //else
            //{
            //     global::Debug.Log(" Game.Instance.FindComponent> Asteroid:空");
            //}
            //tmp = ClusterManager.Instance.FindComponent("Asteroid");
            // global::Debug.Log("ClusterManager.Instance.FindComponent  Asteroid(:" +tmp);
            //tmp = ClusterManager.Instance.FindComponent("AsteroidConfig");
            // global::Debug.Log("ClusterManager.Instance.FindComponent AsteroidConfig(:" +tmp );
            //tmp= SaveLoader.Instance.FindComponent("AsteroidConfig");
            // global::Debug.Log("find AsteroidConfig(:" + tmp);
            //tmp = SaveLoader.Instance.FindComponent("Asteroid");
            // global::Debug.Log("find Asteroid(:" + tmp);


            //if (tmp == null)
            //{
            //     global::Debug.Log("tmp为空");
            //}
            //var sse=SaveLoader.Instance.GetComponentInParent<AsteroidConfig>();
            // global::Debug.Log("SaveLoader.GetComponentInParent<AsteroidConfig>:" + sse );

            //sse=ClusterManager.Instance.gameObject.GetComponent<AsteroidConfig>();
            // global::Debug.Log("gameObject.GetComponent<AsteroidConfig>:" + sse);
            ////Game.Instance.gameObject.GetComponents
            //sse=Global.Instance.GetComponent<AsteroidConfig>();

            // global::Debug.Log("Global.GetComponent<AsteroidConfig> :" + sse);

            //tmp = Global.Instance.FindComponent("AsteroidConfig");
            // global::Debug.Log("Global.FindComponent<AsteroidConfig> :" + tmp);
            //tmp = Global.Instance.FindComponent("AsteroidGridEntity");
            // global::Debug.Log("Global.FindComponent<AsteroidGridEntity> :" + tmp);
            //if (tmp == null)
            //{
            //     global::Debug.Log("tmp为空");
            //}
            //if (sse == null)
            //{
            //     global::Debug.Log("sse为空");
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
            //     global::Debug.Log("components为空");
            //}
            //else
            //{
            //    int num = components.Length;
            //    foreach (KMonoBehaviour kMonoBehaviour in components)
            //    {
            //        if( kMonoBehaviour.name == "Asteroid")
            //        {
            //             global::Debug.Log("找到一个KMonoBehaviour/Asteroid: " );
            //        }
            //        if (num < 100)
            //        {
            //             global::Debug.Log(kMonoBehaviour.ToString());
            //        }
            //    }
            //     global::Debug.Log("SaveGame:num: "+num);
            //}

            // 
            //components = ClusterManager.Instance.GetComponents<KMonoBehaviour>();
            //if (components == null)
            //{
            //     global::Debug.Log("components为空");
            //}
            //else
            //{
            //    int num = components.Length;
            //    foreach (KMonoBehaviour kMonoBehaviour in components)
            //    {
            //        if (kMonoBehaviour.name == "Asteroid")
            //        {
            //             global::Debug.Log("找到一个KMonoBehaviour/Asteroid: ");
            //        }
            //        if (num < 100)
            //        {
            //             global::Debug.Log(kMonoBehaviour.ToString());
            //        }
            //    }
            //// 
            //     global::Debug.Log("ClusterManager: num: " + num);
            //}

            //var tt = ClusterManager.Instance.GetComponentsInChildren<Behaviour>();
            //if (components == null)
            //{
            //     global::Debug.Log("tt 为空");
            //}
            //else
            //{
            //    int num = tt.Length;
            //    foreach (Behaviour kMonoBehaviour in tt)
            //    {
            //        if (kMonoBehaviour.name == "Asteroid")
            //        {
            //             global::Debug.Log("找到一个KMonoBehaviour/Asteroid: ");
            //        }
            //        if (num < 100)
            //        {
            //             global::Debug.Log(kMonoBehaviour.ToString());
            //        }
            //    }
            //    // 
            //     global::Debug.Log("Behaviour.GetComponentsInChildren: num: " + num);
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
            //             global::Debug.Log("删除了星名:" + gridEntity.Name);
            //        };
            //    }
            //}
        }

    }
}
