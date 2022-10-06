using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;

namespace VacuumSpaceMod
{
    public class VacuumSpaceModBuilding : IBuildingConfig
    {
  
        public override BuildingDef CreateBuildingDef()
        {
            string id = "VacuumSpaceMod";

            int width = 3;
            int height = 3;
            string anim = "bomb_build_s_kanim";// 动画ID要以这个结尾 _kanim
            int hitpoints = 100;
            float construction_time = 120f;
            float[] tais1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
            EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
            string[] raw_METALS = MATERIALS.RAW_METALS;
            float melting_point = 2400f;
            BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tais1, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER2, tier2, 0.2f);
            buildingDef.DefaultAnimState = "off";
            buildingDef.Entombable = false;
            buildingDef.Invincible = true;

            buildingDef.ShowInBuildMenu = true;
            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.GetComponent<Deconstructable>().allowDeconstruction = true;
            
        }
        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
            PrimaryElement component = go.GetComponent<PrimaryElement>();
            component.SetElement(SimHashes.Copper, true);
            component.Temperature = 294.15f;
        }

    }
   
     
    
}
