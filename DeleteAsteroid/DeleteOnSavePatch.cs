using HarmonyLib;
using Klei;
using Newtonsoft.Json;
using ProcGen;
using System;
using System.Collections.Generic;
using System.IO;
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
                 global::Debug.LogWarning(ex.Message);
            }
          
        }
        public static void DeleteWorldObjects(WorldContainer world)
        {
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
        public static void loopList(Dictionary<Tag, List<SaveLoadRoot>> sceneObjects)
        {
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
               //  global::Debug.LogWarning(orderedKey.ToString()+" -->"+list.Count);
          
                if (orderedKey.Name == "Asteroid")
                {
                     global::Debug.LogWarning(" 搜索Asteroid: ");
                    for(int i=0; i<list.Count; i++)
                    {
                        SaveLoadRoot item = list[i];
                        if (!(item == null))
                        {
                            //  if(item.)
                            var age = item.GetComponent<AsteroidGridEntity>();
                            // YamlIO.
                            if (age.Name == "delete" || age.Name.Trim().ToLower() == "delete")
                            {
                                // SaveloadRoot需要删除.
                                list[i]=null;//这里直接删除没
                                UnityEngine.Object.Destroy(item);
                                list.RemoveAt(i);
                              
                                i--;
                                 global::Debug.LogWarning("删除+1");
                            }
                        }
                    }
                     global::Debug.LogWarning(" 搜索Asteroid:结束 ");
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
           
             global::Debug.LogWarning("查找delete:" + worlds.Count);
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
                    
                    var age = world.GetComponentInParent<AsteroidGridEntity>();
                    var wi= world.GetComponentInParent<WorldInventory>();
                    var wom=world.GetComponentInParent<OrbitalMechanics>();
                    var wsmc=world.GetComponentInParent<StateMachineController>();
                     global::Debug.LogWarning("WorldInventory  ---> :" + wi);
                     global::Debug.LogWarning("OrbitalMechanics  ---> :" + wom);
                     global::Debug.LogWarning("StateMachineController  ---> :" + wsmc);
                     global::Debug.LogWarning("AsteroidGridEntity-->:" + age);

                    if (gridEntity != null)
                    {
                        if (gridEntity.Name == "delete" || gridEntity.Name.Trim().ToLower() == "delete" )  // 获取右上角底层的实体名.
                        {
                            ClusterManager.Instance.UnregisterWorldContainer(world);
                            // remove 方法被隐藏了,只能使用自带的方法删除
                            //worlds.Remove(world);// markDeleteWorld = world;
                            // worlds.RemoveAt(i);
                            //Collection is read-only. 这里有只读问题.
                            // ClusterManager.Instance.DestoryRocketInteriorWorld
                             global::Debug.LogWarning("掩盖星名:" + gridEntity.Name);
                            //DeleteWorldObjects(world);
                        };
                    }
                }
            }
            var allObjmaybe= SaveLoader.Instance.saveManager.GetLists();
            loopList(allObjmaybe);
            //allObjmaybe[]
            // SaveManager



            //以下代码好像是创建星球的代码.
            GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("Asteroid"), null, null);
            GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab("Asteroid"), null, null);

             global::Debug.LogWarning("KInstantiate:gameObject:" + gameObject);
             global::Debug.LogWarning("KInstantiate:gameObject:对象相等：" + gameObject.Equals(gameObject2));
             global::Debug.LogWarning("Asteroid:gameObject:对象相等：" +
            Assets.GetPrefab("Asteroid").Equals(Assets.GetPrefab("Asteroid")));
            GameObject go=Assets.GetPrefab("Asteroid");//好像只初始化一个星球.
            //好像是空对象.
            //===> Asteroid(KPrefabID)
            //===> Asteroid(KSelectable)
            //===> Asteroid(SaveLoadRoot)
            //===> Asteroid(WorldInventory)
            //===> Asteroid(WorldContainer)
            //===> Asteroid(AsteroidGridEntity)
            //===> Asteroid(OrbitalMechanics)
            //===> Asteroid(StateMachineController)
             global::Debug.LogWarning("Asteroid GameObject :" + go);
            //go.RemoveTag("");
            var s = go.GetComponents<KMonoBehaviour>();
            foreach(var tmp in s)
            {
                 global::Debug.LogWarning("===>  "+tmp.ToString());
                if(tmp is AsteroidGridEntity tt)
                {
                     
                     global::Debug.LogWarning("AsteroidGridEntity.Name:  " + tt.Name);
                    YamlIO.Save(tmp, "D:/GameObject3.yaml");
                }
                if(tmp is KPrefabID iD)
                {
                    global::Debug.LogWarning("KPrefabID" +  iD.InstanceID);
                }
            }
             global::Debug.LogWarning("Asteroid  ->AsteroidGridEntity.length :" + go.GetComponents<AsteroidGridEntity>().Length);
           // YamlIO.Save(go, "D:/GameObject.yaml");
           // YamlIO.Save(gameObject, "D:/GameObjectIn.yaml");
           // go.DeleteObject();

            //go.IsNullOrDestroyed();
            //go.re
           // go.
           //  global::Debug.LogWarning(" a - config :" + go.GetComponent<AsteroidConfig>());
           //go.gameObjects
           //string json = JsonConvert.SerializeObject(go);
           // global::Debug.LogWarning("Asteroid GameObject: "+json);



            // var asteroidList = SaveGame.Instance.GetComponents<AsteroidConfig>( );
            //看看这是什么东西. Asteroid为8个或1个.
            // 这个找法错了.
            // SaveGame.Instance.GetComponentInChildren("Asteroid");

            //SaveLoadRoot.
            //var asc = Game.Instance.GetComponent<AsteroidConfig>();
            //if (asc != null)
            //{
            //     global::Debug.LogWarning("Game.Instance.GetComponent<AsteroidConfig>:" + asc.ToString());
            //}else
            //{  global::Debug.LogWarning("Game.Instance.GetComponent<AsteroidConfig>:空");
            //}
            //var tmp= Game.Instance.FindComponent("Asteroid");
            //if (tmp != null)
            //{
            //     global::Debug.LogWarning("Game.Instance.GetComponent<AsteroidConfig>:" + tmp.ToString());
            //}
            //else
            //{
            //     global::Debug.LogWarning(" Game.Instance.FindComponent> Asteroid:空");
            //}
            //tmp = ClusterManager.Instance.FindComponent("Asteroid");
            // global::Debug.LogWarning("ClusterManager.Instance.FindComponent  Asteroid(:" +tmp);
            //tmp = ClusterManager.Instance.FindComponent("AsteroidConfig");
            // global::Debug.LogWarning("ClusterManager.Instance.FindComponent AsteroidConfig(:" +tmp );
            //tmp= SaveLoader.Instance.FindComponent("AsteroidConfig");
            // global::Debug.LogWarning("find AsteroidConfig(:" + tmp);
            //tmp = SaveLoader.Instance.FindComponent("Asteroid");
            // global::Debug.LogWarning("find Asteroid(:" + tmp);


            //if (tmp == null)
            //{
            //     global::Debug.LogWarning("tmp为空");
            //}
            //var sse=SaveLoader.Instance.GetComponentInParent<AsteroidConfig>();
            // global::Debug.LogWarning("SaveLoader.GetComponentInParent<AsteroidConfig>:" + sse );

            //sse=ClusterManager.Instance.gameObject.GetComponent<AsteroidConfig>();
            // global::Debug.LogWarning("gameObject.GetComponent<AsteroidConfig>:" + sse);
            ////Game.Instance.gameObject.GetComponents
            //sse=Global.Instance.GetComponent<AsteroidConfig>();

            // global::Debug.LogWarning("Global.GetComponent<AsteroidConfig> :" + sse);

            //tmp = Global.Instance.FindComponent("AsteroidConfig");
            // global::Debug.LogWarning("Global.FindComponent<AsteroidConfig> :" + tmp);
            //tmp = Global.Instance.FindComponent("AsteroidGridEntity");
            // global::Debug.LogWarning("Global.FindComponent<AsteroidGridEntity> :" + tmp);
            //if (tmp == null)
            //{
            //     global::Debug.LogWarning("tmp为空");
            //}
            //if (sse == null)
            //{
            //     global::Debug.LogWarning("sse为空");
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
            //     global::Debug.LogWarning("components为空");
            //}
            //else
            //{
            //    int num = components.Length;
            //    foreach (KMonoBehaviour kMonoBehaviour in components)
            //    {
            //        if( kMonoBehaviour.name == "Asteroid")
            //        {
            //             global::Debug.LogWarning("找到一个KMonoBehaviour/Asteroid: " );
            //        }
            //        if (num < 100)
            //        {
            //             global::Debug.LogWarning(kMonoBehaviour.ToString());
            //        }
            //    }
            //     global::Debug.LogWarning("SaveGame:num: "+num);
            //}

            // 
            //components = ClusterManager.Instance.GetComponents<KMonoBehaviour>();
            //if (components == null)
            //{
            //     global::Debug.LogWarning("components为空");
            //}
            //else
            //{
            //    int num = components.Length;
            //    foreach (KMonoBehaviour kMonoBehaviour in components)
            //    {
            //        if (kMonoBehaviour.name == "Asteroid")
            //        {
            //             global::Debug.LogWarning("找到一个KMonoBehaviour/Asteroid: ");
            //        }
            //        if (num < 100)
            //        {
            //             global::Debug.LogWarning(kMonoBehaviour.ToString());
            //        }
            //    }
            //// 
            //     global::Debug.LogWarning("ClusterManager: num: " + num);
            //}

            //var tt = ClusterManager.Instance.GetComponentsInChildren<Behaviour>();
            //if (components == null)
            //{
            //     global::Debug.LogWarning("tt 为空");
            //}
            //else
            //{
            //    int num = tt.Length;
            //    foreach (Behaviour kMonoBehaviour in tt)
            //    {
            //        if (kMonoBehaviour.name == "Asteroid")
            //        {
            //             global::Debug.LogWarning("找到一个KMonoBehaviour/Asteroid: ");
            //        }
            //        if (num < 100)
            //        {
            //             global::Debug.LogWarning(kMonoBehaviour.ToString());
            //        }
            //    }
            //    // 
            //     global::Debug.LogWarning("Behaviour.GetComponentsInChildren: num: " + num);
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
            //             global::Debug.LogWarning("删除了星名:" + gridEntity.Name);
            //        };
            //    }
            //}
        }

    }
}
