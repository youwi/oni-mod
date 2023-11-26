using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace ShowDestinationPOIFixMod
{

    [HarmonyPatch(typeof(ClusterGrid), "GetLocationDescription")]
    public static class ClusterGrid_GetLocationDescription_Patch
    {
        // Token: 0x06000004 RID: 4 RVA: 0x00002074 File Offset: 0x00000274
        [HarmonyPrefix]
        public static bool Prefix(ClusterGrid __instance, AxialI location, ref Sprite sprite, ref string label, ref string sublabel)
        {

            ClusterGridEntity cg = __instance.GetVisibleEntityOfLayerAtCell(location, EntityLayer.POI);

            if (null != cg)
            {
                label = cg.Name;
                sublabel = "";

                //方案A:
                //sprite = cg.GetUISprite();
                // 方案B
                //2种类型:
                // ArtifactPOIClusterGridEntity.m_Anim
                // HarvestablePOIClusterGridEntity.m_Anim
                // 都有属性 m_Anim
                List<ClusterGridEntity.AnimConfig> animConfigs = cg.AnimConfigs;
                if (cg is HarvestablePOIClusterGridEntity || cg is ArtifactPOIClusterGridEntity) //时空裂口直接得到UI
                {
                    var m_Anim = Traverse.Create(cg).Field("m_Anim").GetValue().ToString();
                    // sprite = Assets.GetSprite(m_Anim);//获取不到的
                    //Assets.GetAnim("harvestable_space_poi_kanim");
                    sprite = Def.GetUISpriteFromMultiObjectAnim(animConfigs[0].animFile, m_Anim, false, "");
                    // Debug.Log(m_Anim + " ---ICON---" + sprite);
                }
                if (cg is TemporalTear)
                {
                    // open_loop, closed_loop 他有多个动画.
                    sprite = Def.GetUISpriteFromMultiObjectAnim(animConfigs[0].animFile, "open_loop", false, "");
                }

                //验证结果是所有小行星共用一个动画,
                //而且共用的动画没有 "UI"项目,
                //harvestable_space_poi_kanim ui placeSymbol [ ui ] is missing
                //这个UI是一个
                // ARTIFACT_POI.GRAVITASSPACESTATION7.NAME
                // Assets.GetSprite(cg.anim);  //m_Anim
                // Assets.GetSprite("hex_unknown");
                // 方案C
                //HarvestablePOIClusterGridEntity component = cg.GetComponent<HarvestablePOIClusterGridEntity>();
                //if(component != null)
                //{
                //    sprite = Assets.GetSprite(component.m_Anim);
                //}
                //测试:
                //sprite = cg.GetUISprite();
                //Debug.Log("-----GetUISprite>:   " + sprite);

                //sprite = Assets.GetSprite("space_race");
                ////Assets.GetAnim();
                //Debug.Log("-----space_race>:   "+sprite);
                return false;
            }
            return true;
        }
        public static void buildSpriteInMem(Sprite sprite, string name)
        {
            //内存中重置UI
            //sprite.sym
            // Def.GetUISprite( ).first ;
            //sprite.first
            Assets.GetSprite("ui_elements_classes");

            //radioactive_asteroid_field
        }
    }
}
