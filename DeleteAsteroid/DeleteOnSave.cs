using HarmonyLib;
using Klei;
using ProcGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DeleteAsteroid
{
  
    public class DeleteOnSave
    {

        [HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.Save))]
        public static void Prefix()
        {
            //SaveLoader.Instance.Save(filename, false, true);
        
            DeleteMarkedAsteroid();
        }
        /*
         * 
         * 删除星球mod
         * 当殖民地改名为 "delete" 时
         * 保存再重新加载就可以 删除 殖民地.
         * 需要改名的支持.
         * 优点: 删除之后可以提升游戏帧数.
         * 
         * 删除之后不能还原,请做好备份.
         * 删除之后还是有隐藏垃圾存在,这些隐藏垃圾不影响.
         * 
         * 功能测试中.
         */

        public static void DeleteMarkedAsteroid()
        {
            //  WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
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
                    if (gridEntity.Name == "delete"
                      || gridEntity.Name.ToLower() == "delete")  // 获取右上角底层的实体名.
                    {
                        worlds.Remove(world);// markDeleteWorld = world;
                        Console.WriteLine("删除了星名:"+ gridEntity.Name);
                    };
                    //循环删除
                }
            }
        }

    }
}
