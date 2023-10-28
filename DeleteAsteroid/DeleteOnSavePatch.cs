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
                Console.WriteLine(ex.Message);
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
               // Console.WriteLine(orderedKey.ToString()+" -->"+list.Count);
          
                if (orderedKey.Name == "Asteroid")
                {
                    Console.WriteLine(" 搜索Asteroid: ");
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
                                Console.WriteLine("删除+1");
                            }
                        }
                    }
                    Console.WriteLine(" 搜索Asteroid:结束 ");
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
                    
                    var age = world.GetComponentInParent<AsteroidGridEntity>();
                    var wi= world.GetComponentInParent<WorldInventory>();
                    var wom=world.GetComponentInParent<OrbitalMechanics>();
                    var wsmc=world.GetComponentInParent<StateMachineController>();
                    Console.WriteLine("WorldInventory  ---> :" + wi);
                    Console.WriteLine("OrbitalMechanics  ---> :" + wom);
                    Console.WriteLine("StateMachineController  ---> :" + wsmc);
                    Console.WriteLine("AsteroidGridEntity-->:" + age);

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
                            Console.WriteLine("掩盖星名:" + gridEntity.Name);
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

            Console.WriteLine("KInstantiate:gameObject:" + gameObject);
            Console.WriteLine("KInstantiate:gameObject:对象相等：" + gameObject.Equals(gameObject2));
            Console.WriteLine("Asteroid:gameObject:对象相等：" +
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
            Console.WriteLine("Asteroid GameObject :" + go);
            //go.RemoveTag("");
            var s = go.GetComponents<KMonoBehaviour>();
            foreach(var tmp in s)
            {
                Console.WriteLine("===>  "+tmp.ToString());
                if(tmp is AsteroidGridEntity tt)
                {
                     
                    Console.WriteLine("AsteroidGridEntity.Name:  " + tt.Name);
                    YamlIO.Save(tmp, "D:/GameObject3.yaml");
                }
                if(tmp is KPrefabID iD)
                {
                   Console.WriteLine("KPrefabID" +  iD.InstanceID);
                }
            }
            Console.WriteLine("Asteroid  ->AsteroidGridEntity.length :" + go.GetComponents<AsteroidGridEntity>().Length);
           // YamlIO.Save(go, "D:/GameObject.yaml");
           // YamlIO.Save(gameObject, "D:/GameObjectIn.yaml");
           // go.DeleteObject();

            //go.IsNullOrDestroyed();
            //go.re
           // go.
           // Console.WriteLine(" a - config :" + go.GetComponent<AsteroidConfig>());
           //go.gameObjects
           //string json = JsonConvert.SerializeObject(go);
           //Console.WriteLine("Asteroid GameObject: "+json);



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
